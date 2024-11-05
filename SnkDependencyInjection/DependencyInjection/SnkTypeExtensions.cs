using SnkFramework.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// 提供了对类型进行操作的扩展方法。
    /// </summary>
    public static class SnkTypeExtensions
    {
        /// <summary>
        /// 安全地获取程序集中所有类型，即使在类型加载时出现异常。
        /// </summary>
        /// <param name="assembly">目标程序集。</param>
        /// <returns>程序集中的类型集合。</returns>
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
        /// 获取程序集中的可创建类型，排除抽象类和没有公共构造函数的类型。
        /// </summary>
        /// <param name="assembly">目标程序集。</param>
        /// <returns>可创建的类型集合。</returns>
        public static IEnumerable<Type> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .ExceptionSafeGetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => !t.IsAbstract && t.DeclaredConstructors.Any(constructor => !constructor.IsStatic && constructor.IsPublic))
                .Select(t => t.AsType());
        }

        /// <summary>
        /// 获取名称以指定字符串结尾的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="endingWith">目标结尾字符串。</param>
        /// <returns>匹配的类型集合。</returns>
        public static IEnumerable<Type> EndingWith(this IEnumerable<Type> types, string endingWith)
        {
            return types.Where(x => x.Name.EndsWith(endingWith));
        }

        /// <summary>
        /// 获取名称以指定字符串开头的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="endingWith">目标开头字符串。</param>
        /// <returns>匹配的类型集合。</returns>
        public static IEnumerable<Type> StartingWith(this IEnumerable<Type> types, string endingWith)
        {
            return types.Where(x => x.Name.StartsWith(endingWith));
        }

        /// <summary>
        /// 获取名称包含指定字符串的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="containing">目标包含字符串。</param>
        /// <returns>匹配的类型集合。</returns>
        public static IEnumerable<Type> Containing(this IEnumerable<Type> types, string containing)
        {
            return types.Where(x => x.Name.Contains(containing));
        }

        /// <summary>
        /// 获取命名空间以指定字符串开头的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="namespaceBase">目标命名空间字符串。</param>
        /// <returns>匹配的类型集合。</returns>
        public static IEnumerable<Type> InNamespace(this IEnumerable<Type> types, string namespaceBase)
        {
            return types.Where(x => x.Namespace?.StartsWith(namespaceBase) == true);
        }

        /// <summary>
        /// 获取带有指定属性的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="attributeType">目标属性类型。</param>
        /// <returns>匹配的类型集合。</returns>
        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType)
        {
            return types.Where(x => x.GetCustomAttributes(attributeType, true).Length > 0);
        }

        /// <summary>
        /// 获取带有指定属性的类型，泛型版本。
        /// </summary>
        /// <typeparam name="TAttribute">目标属性类型。</typeparam>
        /// <param name="types">类型集合。</param>
        /// <returns>匹配的类型集合。</returns>
        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            return types.WithAttribute(typeof(TAttribute));
        }

        /// <summary>
        /// 获取继承自指定基类的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="baseType">基类类型。</param>
        /// <returns>继承自基类的类型集合。</returns>
        public static IEnumerable<Type> Inherits(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(baseType.IsAssignableFrom);
        }

        /// <summary>
        /// 获取不继承自指定基类的类型。
        /// </summary>
        /// <typeparam name="TBase">基类类型。</typeparam>
        /// <param name="types">类型集合。</param>
        /// <returns>不继承自基类的类型集合。</returns>
        public static IEnumerable<Type> Inherits<TBase>(this IEnumerable<Type> types)
        {
            return types.Inherits(typeof(TBase));
        }

        /// <summary>
        /// 获取不继承自指定基类的类型。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="baseType">基类类型。</param>
        /// <returns>不继承自基类的类型集合。</returns>
        public static IEnumerable<Type> DoesNotInherit(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(x => !baseType.IsAssignableFrom(x));
        }

        /// <summary>
        /// 获取不继承自指定基类的类型，泛型版本。
        /// </summary>
        /// <typeparam name="TBase">基类类型。</typeparam>
        /// <param name="types">类型集合。</param>
        /// <returns>不继承自基类的类型集合。</returns>
        public static IEnumerable<Type> DoesNotInherit<TBase>(this IEnumerable<Type> types)
            where TBase : Attribute
        {
            return types.DoesNotInherit(typeof(TBase));
        }

        /// <summary>
        /// 排除指定类型的集合。
        /// </summary>
        /// <param name="types">原始类型集合。</param>
        /// <param name="except">要排除的类型数组。</param>
        /// <returns>排除后的类型集合。</returns>
        public static IEnumerable<Type> Except(this IEnumerable<Type> types, params Type[] except)
        {
            // 优化 - 如果排除的类型数大于等于3，则使用字典进行查找
            if (except.Length >= 3)
            {
                var lookup = except.ToDictionary(x => x, _ => true);
                return types.Where(x => !lookup.ContainsKey(x));
            }

            return types.Where(x => !except.Contains(x));
        }

        /// <summary>
        /// 检查类型是否是部分封闭的泛型类型。
        /// </summary>
        /// <param name="type">目标类型。</param>
        /// <returns>如果是部分封闭的泛型，返回 true；否则返回 false。</returns>
        public static bool IsGenericPartiallyClosed(this Type type) =>
            type.GetTypeInfo().IsGenericType &&
            type.GetTypeInfo().ContainsGenericParameters &&
            type.GetGenericTypeDefinition() != type;

        /// <summary>
        /// 表示服务类型和实现类型对的类。
        /// </summary>
        public class ServiceTypeAndImplementationTypePair
        {
            /// <summary>
            /// 服务类型链表。
            /// </summary>
            public List<Type> ServiceTypes { get; }

            /// <summary>
            /// 实现类型。
            /// </summary>
            public Type ImplementationType { get; }

            /// <summary>
            /// 构造方法
            /// </summary>
            /// <param name="serviceTypes">服务类型链表。</param>
            /// <param name="implementationType">实现类型。</param>

            public ServiceTypeAndImplementationTypePair(List<Type> serviceTypes, Type implementationType)
            {
                ImplementationType = implementationType;
                ServiceTypes = serviceTypes;
            }
        }

        /// <summary>
        /// 以类型自身作为服务类型的形式获取服务和实现类型对。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <returns>服务类型和实现类型对集合。</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
        {
            return types.Select(t => new ServiceTypeAndImplementationTypePair(new List<Type> { t }, t));
        }

        /// <summary>
        /// 以类型实现的接口作为服务类型的形式获取服务和实现类型对。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <returns>服务类型和实现类型对集合。</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types) =>
            types.Select(t => new ServiceTypeAndImplementationTypePair(t.GetInterfaces().ToList(), t));

        /// <summary>
        /// 以指定接口作为服务类型的形式获取服务和实现类型对。
        /// </summary>
        /// <param name="types">类型集合。</param>
        /// <param name="interfaces">接口类型数组。</param>
        /// <returns>服务类型和实现类型对集合。</returns>
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types, params Type[] interfaces)
        {
            // 优化 - 如果接口类型数大于等于3，则使用字典进行查找
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
        /// 排除指定接口类型的服务和实现类型对。
        /// </summary>
        /// <param name="pairs">服务和实现类型对集合。</param>
        /// <param name="toExclude">要排除的接口类型数组。</param>
        /// <returns>排除后的服务和实现类型对集合。</returns>
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
        /// 将服务和实现类型对注册为单例。
        /// </summary>
        /// <param name="pairs">服务和实现类型对集合。</param>
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
        /// 将服务和实现类型对注册为懒加载单例。
        /// </summary>
        /// <param name="pairs">服务和实现类型对集合。</param>
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
        /// 将服务和实现类型对注册为动态类型。
        /// </summary>
        /// <param name="pairs">服务和实现类型对集合。</param>
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
        /// 创建指定类型的默认实例。
        /// </summary>
        /// <param name="type">目标类型。</param>
        /// <returns>默认实例，如果无法创建则返回null。</returns>
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
        /// 根据给定的参数字典查找适用的构造函数。
        /// </summary>
        /// <param name="type">目标类型。</param>
        /// <param name="arguments">参数字典。</param>
        /// <returns>找到的构造函数信息。</returns>
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
        /// 根据给定的参数数组查找适用的构造函数。
        /// </summary>
        /// <param name="type">目标类型。</param>
        /// <param name="arguments">参数数组。</param>
        /// <returns>找到的构造函数信息。</returns>
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
        /// 检查构造函数的参数是否与给定的参数字典匹配。
        /// </summary>
        /// <param name="arguments">参数字典。</param>
        /// <param name="constructor">目标构造函数。</param>
        /// <param name="unusedKeys">尚未使用的键集合。</param>
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