using SnkFramework.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// �ṩ�˶����ͽ��в�������չ������
    /// </summary>
    public static class SnkTypeExtensions
    {
        /// <summary>
        /// ��ȫ�ػ�ȡ�������������ͣ���ʹ�����ͼ���ʱ�����쳣��
        /// </summary>
        /// <param name="assembly">Ŀ����򼯡�</param>
        /// <returns>�����е����ͼ��ϡ�</returns>
        public static IEnumerable<Type> ExceptionSafeGetTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                SnkLogHost.Default?.Warning(e, $"ReflectionTypeLoadException masked during loading of {assembly.FullName}");

                foreach (var exception in e.LoaderExceptions)
                {
                    SnkLogHost.Default?.Warning(exception, "Failed to load type");
                }

                if (Debugger.IsAttached)
                    Debugger.Break();

                return Enumerable.Empty<Type>();
            }
        }

        /// <summary>
        /// ��ȡ�����еĿɴ������ͣ��ų��������û�й������캯�������͡�
        /// </summary>
        /// <param name="assembly">Ŀ����򼯡�</param>
        /// <returns>�ɴ��������ͼ��ϡ�</returns>
        public static IEnumerable<Type> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .ExceptionSafeGetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => !t.IsAbstract && t.DeclaredConstructors.Any(constructor => !constructor.IsStatic && constructor.IsPublic))
                .Select(t => t.AsType());
        }

        /// <summary>
        /// ��ȡ������ָ���ַ�����β�����͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="endingWith">Ŀ���β�ַ�����</param>
        /// <returns>ƥ������ͼ��ϡ�</returns>
        public static IEnumerable<Type> EndingWith(this IEnumerable<Type> types, string endingWith)
        {
            return types.Where(x => x.Name.EndsWith(endingWith));
        }

        /// <summary>
        /// ��ȡ������ָ���ַ�����ͷ�����͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="endingWith">Ŀ�꿪ͷ�ַ�����</param>
        /// <returns>ƥ������ͼ��ϡ�</returns>
        public static IEnumerable<Type> StartingWith(this IEnumerable<Type> types, string endingWith)
        {
            return types.Where(x => x.Name.StartsWith(endingWith));
        }

        /// <summary>
        /// ��ȡ���ư���ָ���ַ��������͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="containing">Ŀ������ַ�����</param>
        /// <returns>ƥ������ͼ��ϡ�</returns>
        public static IEnumerable<Type> Containing(this IEnumerable<Type> types, string containing)
        {
            return types.Where(x => x.Name.Contains(containing));
        }

        /// <summary>
        /// ��ȡ�����ռ���ָ���ַ�����ͷ�����͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="namespaceBase">Ŀ�������ռ��ַ�����</param>
        /// <returns>ƥ������ͼ��ϡ�</returns>
        public static IEnumerable<Type> InNamespace(this IEnumerable<Type> types, string namespaceBase)
        {
            return types.Where(x => x.Namespace?.StartsWith(namespaceBase) == true);
        }

        /// <summary>
        /// ��ȡ����ָ�����Ե����͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="attributeType">Ŀ���������͡�</param>
        /// <returns>ƥ������ͼ��ϡ�</returns>
        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType)
        {
            return types.Where(x => x.GetCustomAttributes(attributeType, true).Length > 0);
        }

        /// <summary>
        /// ��ȡ����ָ�����Ե����ͣ����Ͱ汾��
        /// </summary>
        /// <typeparam name="TAttribute">Ŀ���������͡�</typeparam>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <returns>ƥ������ͼ��ϡ�</returns>
        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return types.WithAttribute(typeof(TAttribute));
        }

        /// <summary>
        /// ��ȡ�̳���ָ����������͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="baseType">�������͡�</param>
        /// <returns>�̳��Ի�������ͼ��ϡ�</returns>
        public static IEnumerable<Type> Inherits(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(baseType.IsAssignableFrom);
        }

        /// <summary>
        /// ��ȡ���̳���ָ����������͡�
        /// </summary>
        /// <typeparam name="TBase">�������͡�</typeparam>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <returns>���̳��Ի�������ͼ��ϡ�</returns>
        public static IEnumerable<Type> Inherits<TBase>(this IEnumerable<Type> types)
        {
            return types.Inherits(typeof(TBase));
        }

        /// <summary>
        /// ��ȡ���̳���ָ����������͡�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="baseType">�������͡�</param>
        /// <returns>���̳��Ի�������ͼ��ϡ�</returns>
        public static IEnumerable<Type> DoesNotInherit(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(x => !baseType.IsAssignableFrom(x));
        }

        /// <summary>
        /// ��ȡ���̳���ָ����������ͣ����Ͱ汾��
        /// </summary>
        /// <typeparam name="TBase">�������͡�</typeparam>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <returns>���̳��Ի�������ͼ��ϡ�</returns>
        public static IEnumerable<Type> DoesNotInherit<TBase>(this IEnumerable<Type> types)
            where TBase : Attribute
        {
            return types.DoesNotInherit(typeof(TBase));
        }

        /// <summary>
        /// �ų�ָ�����͵ļ��ϡ�
        /// </summary>
        /// <param name="types">ԭʼ���ͼ��ϡ�</param>
        /// <param name="except">Ҫ�ų����������顣</param>
        /// <returns>�ų�������ͼ��ϡ�</returns>
        public static IEnumerable<Type> Except(this IEnumerable<Type> types, params Type[] except)
        {
            // �Ż� - ����ų������������ڵ���3����ʹ���ֵ���в���
            if (except.Length >= 3)
            {
                var lookup = except.ToDictionary(x => x, _ => true);
                return types.Where(x => !lookup.ContainsKey(x));
            }

            return types.Where(x => !except.Contains(x));
        }

        /// <summary>
        /// ��������Ƿ��ǲ��ַ�յķ������͡�
        /// </summary>
        /// <param name="type">Ŀ�����͡�</param>
        /// <returns>����ǲ��ַ�յķ��ͣ����� true�����򷵻� false��</returns>
        public static bool IsGenericPartiallyClosed(this Type type) =>
            type.GetTypeInfo().IsGenericType &&
            type.GetTypeInfo().ContainsGenericParameters &&
            type.GetGenericTypeDefinition() != type;

        /// <summary>
        /// ��ʾ�������ͺ�ʵ�����ͶԵ��ࡣ
        /// </summary>
        public class ServiceTypeAndImplementationTypePair
        {
            /// <summary>
            /// ������������
            /// </summary>
            public List<Type> ServiceTypes { get; }

            /// <summary>
            /// ʵ�����͡�
            /// </summary>
            public Type ImplementationType { get; }

            /// <summary>
            /// ���췽��
            /// </summary>
            /// <param name="serviceTypes">������������</param>
            /// <param name="implementationType">ʵ�����͡�</param>

            public ServiceTypeAndImplementationTypePair(List<Type> serviceTypes, Type implementationType)
            {
                ImplementationType = implementationType;
                ServiceTypes = serviceTypes;
            }
        }

        /// <summary>
        /// ������������Ϊ�������͵���ʽ��ȡ�����ʵ�����Ͷԡ�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <returns>�������ͺ�ʵ�����ͶԼ��ϡ�</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
        {
            return types.Select(t => new ServiceTypeAndImplementationTypePair(new List<Type> { t }, t));
        }

        /// <summary>
        /// ������ʵ�ֵĽӿ���Ϊ�������͵���ʽ��ȡ�����ʵ�����Ͷԡ�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <returns>�������ͺ�ʵ�����ͶԼ��ϡ�</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types) =>
            types.Select(t => new ServiceTypeAndImplementationTypePair(t.GetInterfaces().ToList(), t));

        /// <summary>
        /// ��ָ���ӿ���Ϊ�������͵���ʽ��ȡ�����ʵ�����Ͷԡ�
        /// </summary>
        /// <param name="types">���ͼ��ϡ�</param>
        /// <param name="interfaces">�ӿ��������顣</param>
        /// <returns>�������ͺ�ʵ�����ͶԼ��ϡ�</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types, params Type[] interfaces)
        {
            // �Ż� - ����ӿ����������ڵ���3����ʹ���ֵ���в���
            if (interfaces.Length >= 3)
            {
                var lookup = interfaces.ToDictionary(x => x, _ => true);
                return
                    types.Select(
                        t =>
                            new ServiceTypeAndImplementationTypePair(
                                t.GetInterfaces().Where(iface => lookup.ContainsKey(iface)).ToList(), t));
            }

            return
                types.Select(
                    t =>
                        new ServiceTypeAndImplementationTypePair(
                            t.GetInterfaces().Where(interfaces.Contains).ToList(), t));
        }

        /// <summary>
        /// �ų�ָ���ӿ����͵ķ����ʵ�����Ͷԡ�
        /// </summary>
        /// <param name="pairs">�����ʵ�����ͶԼ��ϡ�</param>
        /// <param name="toExclude">Ҫ�ų��Ľӿ��������顣</param>
        /// <returns>�ų���ķ����ʵ�����ͶԼ��ϡ�</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> ExcludeInterfaces(
            this IEnumerable<ServiceTypeAndImplementationTypePair> pairs, params Type[] toExclude)
        {
            return pairs
                .Select(pair =>
                    new { pair, excludedList = pair.ServiceTypes.Where(c => !toExclude.Contains(c)).ToList() })
                .Where(t => t.excludedList.Count > 0)
                .Select(t => new ServiceTypeAndImplementationTypePair(t.excludedList, t.pair.ImplementationType));
        }

        /// <summary>
        /// �������ʵ�����Ͷ�ע��Ϊ������
        /// </summary>
        /// <param name="pairs">�����ʵ�����ͶԼ��ϡ�</param>
        public static void RegisterAsSingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var pair in pairs)
            {
                if (pair.ServiceTypes.Count == 0)
                    continue;

                var instance = Snk.DIProvider?.DIConstruct(pair.ImplementationType, (object)null);
                if (instance == null)
                    continue;

                foreach (var serviceType in pair.ServiceTypes)
                {
                    Snk.DIProvider?.RegisterSingleton(serviceType, instance);
                }
            }
        }

        /// <summary>
        /// �������ʵ�����Ͷ�ע��Ϊ�����ص�����
        /// </summary>
        /// <param name="pairs">�����ʵ�����ͶԼ��ϡ�</param>
        public static void RegisterAsLazySingleton(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var pair in pairs)
            {
                if (!pair.ServiceTypes.Any())
                    continue;

                var typeToCreate = pair.ImplementationType;
                var creator = new SnkLazySingletonCreator(typeToCreate);
                var creationFunc = new Func<object>(() => creator.Instance);
                foreach (var serviceType in pair.ServiceTypes)
                {
                    Snk.DIProvider?.RegisterSingleton(serviceType, creationFunc);
                }
            }
        }

        /// <summary>
        /// �������ʵ�����Ͷ�ע��Ϊ��̬���͡�
        /// </summary>
        /// <param name="pairs">�����ʵ�����ͶԼ��ϡ�</param>
        public static void RegisterAsDynamic(this IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            foreach (var pair in pairs)
            {
                foreach (var serviceType in pair.ServiceTypes)
                {
                    Snk.DIProvider?.RegisterType(serviceType, pair.ImplementationType);
                }
            }
        }

        /// <summary>
        /// ����ָ�����͵�Ĭ��ʵ����
        /// </summary>
        /// <param name="type">Ŀ�����͡�</param>
        /// <returns>Ĭ��ʵ��������޷������򷵻�null��</returns>
        public static object CreateDefault(this Type type)
        {
            if (type == null)
                return null;

            if (!type.GetTypeInfo().IsValueType)
            {
                return null;
            }

            if (Nullable.GetUnderlyingType(type) != null)
                return null;

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// ���ݸ����Ĳ����ֵ�������õĹ��캯����
        /// </summary>
        /// <param name="type">Ŀ�����͡�</param>
        /// <param name="arguments">�����ֵ䡣</param>
        /// <returns>�ҵ��Ĺ��캯����Ϣ��</returns>
        public static ConstructorInfo FindApplicableConstructor(this Type type, IDictionary<string, object> arguments)
        {
            var constructors = type.GetConstructors();
            if (arguments == null || arguments.Count == 0)
            {
                return constructors.Aggregate((minC, c) => c.GetParameters().Length < minC.GetParameters().Length ? c : minC);
            }

            var unusedKeys = new List<string>(arguments.Keys);
            foreach (var constructor in constructors)
            {
                CheckConstructors(arguments, constructor, ref unusedKeys);

                if (unusedKeys.Count == 0)
                {
                    return constructor;
                }
            }

            return null;
        }

        /// <summary>
        /// ���ݸ����Ĳ�������������õĹ��캯����
        /// </summary>
        /// <param name="type">Ŀ�����͡�</param>
        /// <param name="arguments">�������顣</param>
        /// <returns>�ҵ��Ĺ��캯����Ϣ��</returns>
        public static ConstructorInfo FindApplicableConstructor(this Type type, object[] arguments)
        {
            var constructors = type.GetConstructors();
            if (arguments.Length == 0)
            {
                return constructors.Aggregate((minC, c) => c.GetParameters().Length < minC.GetParameters().Length ? c : minC);
            }

            foreach (var constructor in constructors)
            {
                var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType);
                var unusedArguments = arguments.ToList();

                foreach (var parameterType in parameterTypes)
                {
                    var argumentMatch = unusedArguments.Find(arg => parameterType.IsInstanceOfType(arg));
                    if (argumentMatch != null)
                    {
                        unusedArguments.Remove(argumentMatch);
                    }
                }

                if (unusedArguments.Count == 0)
                {
                    return constructor;
                }
            }

            return null;
        }

        /// <summary>
        /// ��鹹�캯���Ĳ����Ƿ�������Ĳ����ֵ�ƥ�䡣
        /// </summary>
        /// <param name="arguments">�����ֵ䡣</param>
        /// <param name="constructor">Ŀ�깹�캯����</param>
        /// <param name="unusedKeys">��δʹ�õļ����ϡ�</param>
        private static void CheckConstructors(IDictionary<string, object> arguments, MethodBase constructor, ref List<string> unusedKeys)
        {
            foreach (var parameter in constructor.GetParameters())
            {
                if (parameter.Name != null &&
                    unusedKeys.Contains(parameter.Name) &&
                    parameter.ParameterType.IsInstanceOfType(arguments[parameter.Name]))
                {
                    unusedKeys.Remove(parameter.Name);
                }
            }
        }
    }
}