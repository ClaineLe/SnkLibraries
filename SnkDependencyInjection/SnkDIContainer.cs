using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkLogging;

namespace SnkDependencyInjection
{
    /// <summary>
    /// <para>��������ע��������ࡣ</para>
    /// <para>ʵ�ֽӿ� <see cref="ISnkDIProvider"/> </para>
    /// </summary>
    public sealed partial class SnkDIContainer : ISnkDIProvider
    {
        private static readonly ResolverType? ResolverTypeNoneSpecified = null;

        /// <summary>
        /// �洢���������Ӧ�Ľ��������ֵ䡣
        /// </summary>
        private readonly Dictionary<Type, List<IResolver>> _resolvers = new Dictionary<Type, List<IResolver>>();

        /// <summary>
        /// ���ڼ��ѭ���������ֵ䡣
        /// </summary>
        private readonly Dictionary<Type, bool> _circularTypeDetection = new Dictionary<Type, bool>();

        /// <summary>
        /// �����߳�ͬ����������
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// ����ѡ�
        /// </summary>
        private readonly ISnkDIOptions _options;

        /// <summary>
        /// ����ע������
        /// </summary>
        private readonly ISnkPropertyInjector _propertyInjector;

        /// <summary>
        /// ��������ע���ṩ�ߡ�
        /// </summary>
        private readonly ISnkDIProvider _parentProvider;

        /// <summary>
        /// ��ȡ����ѡ�
        /// </summary>
        private ISnkDIOptions Options => _options;

        /// <summary>
        /// ʹ��ָ����ѡ��͸����ṩ�ߴ���һ���µ� DI ����ʵ����
        /// </summary>
        /// <param name="options">����ע������ѡ�</param>
        /// <param name="parentProvider">���� DI �ṩ�ߡ�</param>
        public SnkDIContainer(ISnkDIOptions options, ISnkDIProvider parentProvider = null)
        {
            _options = options ?? new SnkDIOptions();
            if (_options.PropertyInjectorType != null)
            {
                _propertyInjector = Activator.CreateInstance(_options.PropertyInjectorType) as ISnkPropertyInjector;
            }
            if (_propertyInjector != null)
            {
                RegisterSingleton(typeof(ISnkPropertyInjector), _propertyInjector);
            }
            if (parentProvider != null)
            {
                _parentProvider = parentProvider;
            }
        }

        /// <summary>
        /// ʹ�ø����ṩ�ߴ��� DI ����ʵ����
        /// </summary>
        /// <param name="parentProvider">���� DI �ṩ�ߡ�</param>
        /// <exception cref="ArgumentNullException">��� parentProvider Ϊ null�����׳��쳣��</exception>
        public SnkDIContainer(ISnkDIProvider parentProvider) : this(null, parentProvider)
        {
            if (parentProvider == null)
            {
                throw new ArgumentNullException(nameof(parentProvider), "Provide a parent DI provider to this constructor");
            }
        }
 
        /// <summary>
        /// ��������Ƿ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ�������͡�</typeparam>
        /// <returns>������Խ��������� true�����򷵻� false��</returns>
        public bool CanResolve<T>() where T : class
        {
            return CanResolve(typeof(T));
        }

        /// <summary>
        /// ��������Ƿ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ�������͡�</param>
        /// <returns>������Խ��������� true�����򷵻� false��</returns>
        public bool CanResolve(Type type)
        {
            lock (_locker)
            {
                if (_resolvers.ContainsKey(type))
                {
                    return true;
                }
                if (_parentProvider != null && _parentProvider.CanResolve(type))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <param name="resolved">�����ɹ�ʱ���ص�ʵ�������򷵻����͵�Ĭ��ʵ����</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        public bool TryResolve<T>(out T resolved) where T : class
        {
            try
            {
                var toReturn = TryResolve(typeof(T), out var item);
                resolved = item as T;
                return toReturn;
            }
            catch (SnkDIResolveException)
            {
                resolved = typeof(T).CreateDefault() as T;
                return false;
            }
        }

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="resolved">�����ɹ�ʱ���ص�ʵ�����󣻷���Ϊ null��</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        public bool TryResolve(Type type, out object resolved)
        {
            lock (_locker)
            {
                return InternalTryResolve(type, out resolved);
            }
        }

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ�����ϡ�
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <param name="resolved">�������ʵ�����ϡ�</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        public bool TryResolves<T>(out IEnumerable<T> resolved) where T : class
        {
            if (this._resolvers.TryGetValue(typeof(T), out var list) == false)
            {
                resolved = Enumerable.Empty<T>();
                return false;
            }
            resolved = list.Cast<T>();
            return true;
        }

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ�����ϡ�
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="resolved">�������ʵ�����ϡ�</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        public bool TryResolves(Type type, out IEnumerable<object> resolved)
        {
            if (this._resolvers.TryGetValue(type, out var list) == false)
            {
                resolved = Enumerable.Empty<object>();
                return false;
            }
            resolved = list;
            return true;
        }

        /// <summary>
        /// ����ָ�����͵�ʵ����������ʧ��ʱ�׳��쳣��
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>�������ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷���������ʱ�׳���</exception>
        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        /// <summary>
        /// ����ָ�����͵�ʵ����������ʧ��ʱ�׳��쳣��
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>�������ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷���������ʱ�׳���</exception>
        public object Resolve(Type type)
        {
            lock (_locker)
            {
                if (!InternalTryResolve(type, out var resolved))
                {
                    throw new SnkDIResolveException("Failed to resolve type {0}", type.FullName);
                }
                return resolved;
            }
        }

        /// <summary>
        /// ����ָ�����͵�ʵ�����ϡ�
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>�������ʵ�����ϡ�</returns>
        public IEnumerable<T> Resolves<T>() where T : class
        {
            if (this._resolvers.TryGetValue(typeof(T), out var list) == false)
                return Enumerable.Empty<T>();
            return list.Cast<T>();
        }

        /// <summary>
        /// ����ָ�����͵�ʵ�����ϡ�
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>�������ʵ�����ϡ�</returns>
        public IEnumerable<object> Resolves(Type type)
        {
            if (this._resolvers.TryGetValue(type, out var list) == false)
                return Enumerable.Empty<object>();
            return list;
        }

        /// <summary>
        /// ��ȡָ�����͵ĵ���ʵ�����������ڽ��׳��쳣��
        /// </summary>
        /// <typeparam name="T">Ҫ��ȡ�����͡�</typeparam>
        /// <returns>����ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷���ȡ����ʱ�׳���</exception>
        public T GetSingleton<T>() where T : class
        {
            return GetSingleton(typeof(T)) as T;
        }

        /// <summary>
        /// ��ȡָ�����͵ĵ���ʵ�����������ڽ��׳��쳣��
        /// </summary>
        /// <param name="type">Ҫ��ȡ�����͡�</param>
        /// <returns>����ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷���ȡ����ʱ�׳���</exception>
        public object GetSingleton(Type type)
        {
            lock (_locker)
            {
                if (!InternalTryResolve(type, ResolverType.Singleton, out var resolved))
                {
                    throw new SnkDIResolveException("Failed to resolve type {0}", type.FullName);
                }
                return resolved;
            }
        }

        /// <summary>
        /// ����ָ�����͵���ʵ����ÿ�ε��÷��ز�ͬ�Ķ���
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>������ʵ����</returns>
        public T Create<T>() where T : class
        {
            return Create(typeof(T)) as T;
        }

        /// <summary>
        /// ����ָ�����͵���ʵ����ÿ�ε��÷��ز�ͬ�Ķ���
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>������ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷�����ʵ��ʱ�׳���</exception>
        public object Create(Type type)
        {
            lock (_locker)
            {
                if (!InternalTryResolve(type, ResolverType.DynamicPerResolve, out var resolved))
                {
                    throw new SnkDIResolveException("Failed to resolve type {0}", type.FullName);
                }
                return resolved;
            }
        }

        /// <summary>
        /// ע������ӳ�䣬���ӿ����������ʵ�ֻ��캯��������
        /// </summary>
        /// <typeparam name="TInterface">Դ�ӿ����͡�</typeparam>
        /// <typeparam name="TToConstruct">Ŀ��ʵ�����͡�</typeparam>
        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            RegisterType(typeof(TInterface), typeof(TToConstruct));
        }

        /// <summary>
        /// ע������ӳ�䣬ʹ��ί�й���ӿ����͵�ʵ����
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="constructor">���ڴ���ʵ���Ĺ��캯��ί�С�</param>
        public void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class
        {
            var resolver = new FuncConstructingResolver(constructor);
            InternalSetResolver(typeof(TInterface), resolver);
        }

        /// <summary>
        /// ע������ӳ�䣬ʹ��ί�й���ʵ����
        /// </summary>
        /// <param name="t">�ӿڻ�������͡�</param>
        /// <param name="constructor">���캯��ί�С�</param>
        /// <exception cref="SnkDIResolveException">�����캯�����صĶ��������Ͳ�����ʱ�׳���</exception>
        public void RegisterType(Type t, Func<object> constructor)
        {
            var resolver = new FuncConstructingResolver(() =>
            {
                var ret = constructor();
                if (ret != null && !t.IsInstanceOfType(ret))
                {
                    throw new SnkDIResolveException("Constructor failed to return a compatibly object for type {0}", t.FullName);
                }
                return ret;
            });

            InternalSetResolver(t, resolver);
        }

        /// <summary>
        /// ע������ӳ�䣬��һ������ӳ�䵽��һ�����ͣ����������Դ����ʱ����Ŀ�����͵�ʵ����
        /// </summary>
        /// <param name="tFrom">Դ���͡�</param>
        /// <param name="tTo">Ŀ�����͡�</param>
        public void RegisterType(Type tFrom, Type tTo)
        {
            IResolver resolver;
            if (tFrom.GetTypeInfo().IsGenericTypeDefinition)
                resolver = new ConstructingOpenGenericResolver(tTo, this);
            else
                resolver = new ConstructingResolver(tTo, this);
            InternalSetResolver(tFrom, resolver);
        }

        /// <summary>
        /// ע��һ������ʵ�����ӿ����������ʵ��������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="theObject">����ʵ����</param>
        public void RegisterSingleton<TInterface>(TInterface theObject) where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theObject);
        }

        /// <summary>
        /// ע��һ������ʵ�����ӿ����������ʵ��������
        /// </summary>
        /// <param name="tInterface">�ӿ����͡�</param>
        /// <param name="theObject">����ʵ����</param>
        public void RegisterSingleton(Type tInterface, object theObject)
        {
            InternalSetResolver(tInterface, new SingletonResolver(theObject));
        }

        /// <summary>
        /// ע�ᵥ��ʵ����ʹ�ù��캯��ί�С�
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="theConstructor">���ڴ�������ʵ���Ĺ��캯��ί�С�</param>
        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor) where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theConstructor);
        }

        /// <summary>
        /// ע�ᵥ��ʵ����ʹ�ù��캯��ί�С�
        /// </summary>
        /// <param name="tInterface">�ӿ����͡�</param>
        /// <param name="theConstructor">���캯��ί�С�</param>
        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            InternalSetResolver(tInterface, new ConstructingSingletonResolver(theConstructor));
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <returns>�����ʵ����</returns>
        public object DIConstruct(Type type)
        {
            return DIConstruct(type, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">���ڹ��캯���Ĳ�����</param>
        /// <returns>�����ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷��ҵ����õĹ��캯��ʱ�׳���</exception>
        public object DIConstruct(Type type, object arguments)
        {
            return DIConstruct(type, arguments?.ToPropertyDictionary());
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>() where T : class
        {
            return DIConstruct(typeof(T), (IDictionary<string, object>)null) as T;
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">���ڹ��캯���Ĳ�����</param>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            return DIConstruct(typeof(T), arguments) as T;
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">���ڹ��캯���Ĳ�����</param>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>(object arguments) where T : class
        {
            return DIConstruct(typeof(T), arguments?.ToPropertyDictionary()) as T;
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">���ڹ��캯���Ĳ�����</param>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>(params object[] arguments) where T : class
        {
            return DIConstruct(typeof(T), arguments) as T;
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">���ڹ��캯���Ĳ�����</param>
        /// <returns>�����ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷��ҵ����õĹ��캯��ʱ�׳���</exception>
        public object DIConstruct(Type type, params object[] arguments)
        {
            var selectedConstructor = type.FindApplicableConstructor(arguments);

            if (selectedConstructor == null)
            {
                throw new SnkDIResolveException("Failed to find constructor for type {0} with arguments: {1}",
                    type.FullName, arguments.Select(x => x?.GetType().Name));
            }

            var parameters = GetDIParameterValues(type, selectedConstructor, arguments);
            return DIConstruct(type, selectedConstructor, parameters.ToArray());
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">���ڹ��캯���Ĳ�����</param>
        /// <returns>�����ʵ����</returns>
        /// <exception cref="SnkDIResolveException">���޷��ҵ����õĹ��캯��ʱ�׳���</exception>
        public object DIConstruct(Type type, IDictionary<string, object> arguments)
        {
            var selectedConstructor = type.FindApplicableConstructor(arguments);

            if (selectedConstructor == null)
            {
                throw new SnkDIResolveException("Failed to find constructor for type {0}", type.FullName);
            }

            var parameters = GetDIParameterValues(type, selectedConstructor, arguments);
            return DIConstruct(type, selectedConstructor, parameters.ToArray());
        }

        /// <summary>
        /// ������н����������ͷŽ�������Ϊ����й¶�����ͷź���������ٽ�����ղ���
        /// </summary>
        public void CleanAllResolvers()
        {
            lock (_locker)
            {
                _resolvers.Clear();
                _circularTypeDetection.Clear();
            }
        }

        /// <summary>
        /// ����������һ���µ����������̳е�ǰ���������á�
        /// </summary>
        /// <returns>������ʵ����</returns>
        public ISnkDIProvider CreateChildContainer() => new SnkDIContainer(this);

        /// <summary>
        /// ����ָ�����͵�ʵ������ע�����ԡ�
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="constructor">ʹ�õĹ��캯����</param>
        /// <param name="arguments">���캯��������</param>
        /// <returns>����Ķ���</returns>
        /// <exception cref="SnkDIResolveException">�������ʧ�ܣ����׳��쳣��</exception>
        private object DIConstruct(Type type, ConstructorInfo constructor, object[] arguments)
        {
            object toReturn;
            try
            {
                toReturn = constructor.Invoke(arguments);
            }
            catch (TargetInvocationException invocation)
            {
                throw new SnkDIResolveException(invocation, "Failed to construct {0}", type.Name);
            }

            try
            {
                InjectProperties(toReturn);
            }
            catch (Exception)
            {
                if (!Options.CheckDisposeIfPropertyInjectionFails)
                    throw;

                // ����ת����飬����ɹ������ Dispose ����
                if (toReturn is IDisposable disposable)
                    disposable.Dispose();
                throw;
            }
            return toReturn;
        }

        /// <summary>
        /// ���������Ƿ�֧��ָ���Ľ������͡�
        /// </summary>
        /// <param name="resolver">����������</param>
        /// <param name="requiredResolverType">��Ҫ�Ľ������͡�</param>
        /// <returns>���֧�֣����� true�����򷵻� false��</returns>
        private static bool Supports(IResolver resolver, ResolverType? requiredResolverType)
        {
            if (resolver == null)
                return false;

            if (!requiredResolverType.HasValue)
                return true;

            return resolver.ResolveType == requiredResolverType.Value;
        }

        /// <summary>
        /// ���Խ������͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="resolved">������Ķ���</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        private bool InternalTryResolve(Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        /// <summary>
        /// ���Խ������͵�ʵ��������ָ���Ľ������͡�
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="requiredResolverType">��Ҫ�Ľ������͡�</param>
        /// <param name="resolved">������Ķ���</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        private bool InternalTryResolve(Type type, ResolverType? requiredResolverType, out object resolved)
        {
            if (!TryGetResolver(type, out var resolver))
            {
                if (_parentProvider != null && _parentProvider.TryResolve(type, out resolved))
                {
                    return true;
                }

                resolved = type.CreateDefault();
                return false;
            }

            if (!Supports(resolver, requiredResolverType))
            {
                resolved = type.CreateDefault();
                return false;
            }

            return InternalTryResolve(type, resolver, out resolved);
        }

        /// <summary>
        /// ����ʹ��ָ���������������͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="resolver">���ڽ����Ľ�������</param>
        /// <param name="resolved">������Ķ���</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        private bool InternalTryResolve(Type type, IResolver resolver, out object resolved)
        {
            var detectingCircular = ShouldDetectCircularReferencesFor(resolver);
            if (detectingCircular)
            {
                try
                {
                    _circularTypeDetection.Add(type, true);
                }
                catch (ArgumentException argumentException)
                {
                    SnkLogHost.Default?.Error(argumentException, $"DependencyInjection circular reference detected - cannot currently resolve {type.Name}");
                    resolved = type.CreateDefault();
                    return false;
                }
            }

            try
            {
                if (resolver is ConstructingOpenGenericResolver)
                {
                    resolver.SetGenericTypeParameters(type.GetTypeInfo().GenericTypeArguments);
                }

                var raw = resolver.Resolve();
                if (raw == null)
                {
                    throw new System.Exception("Resolver returned null");
                }
                if (!type.IsInstanceOfType(raw))
                {
                    throw new System.Exception($"Resolver returned object type {raw.GetType().FullName} which does not support interface {type.FullName}");
                }

                resolved = raw;
                return true;
            }
            finally
            {
                if (detectingCircular)
                {
                    _circularTypeDetection.Remove(type);
                }
            }
        }

        /// <summary>
        /// ���Ի�ȡָ�����͵Ľ�������
        /// </summary>
        /// <param name="type">��Ҫ���������͡�</param>
        /// <param name="resolver">���ҵ��Ľ�������</param>
        /// <returns>����ҵ������������� true�����򷵻� false��</returns>
        private bool TryGetResolver(Type type, out IResolver resolver)
        {
            if (_resolvers.TryGetValue(type, out var list))
            {
                resolver = list.FirstOrDefault();
                return resolver != null;
            }

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (_resolvers.TryGetValue(genericTypeDefinition, out list))
                {
                    resolver = list.FirstOrDefault();
                    return resolver != null;
                }
            }

            resolver = null;
            return false;
        }

        /// <summary>
        /// �жϽ������Ƿ���Ҫ���ѭ�����á�
        /// </summary>
        /// <param name="resolver">Ҫ���Ľ�������</param>
        /// <returns>�����Ҫ��⣬���� true�����򷵻� false��</returns>
        /// <exception cref="SnkException">�������������δ֪���׳��쳣��</exception>
        private bool ShouldDetectCircularReferencesFor(IResolver resolver)
        {
            switch (resolver.ResolveType)
            {
                case ResolverType.DynamicPerResolve:
                    return Options.TryToDetectDynamicCircularReferences;

                case ResolverType.Singleton:
                    return Options.TryToDetectSingletonCircularReferences;

                case ResolverType.Unknown:
                    throw new System.Exception($"A resolver must have a known type - error in {resolver.GetType().Name}");
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolver), "unknown resolveType of " + resolver.ResolveType);
            }
        }

        /// <summary>
        /// �������͵Ľ�������
        /// </summary>
        /// <param name="interfaceType">�ӿ����͡�</param>
        /// <param name="resolver">��������</param>
        private void InternalSetResolver(Type interfaceType, IResolver resolver)
        {
            lock (_locker)
            {
                if (_resolvers.TryGetValue(interfaceType, out var list) == false)
                {
                    list = new List<IResolver>();
                    _resolvers.Add(interfaceType, list);
                }
                list.Add(resolver);
            }
        }

        /// <summary>
        /// �������ע�����ԡ�
        /// </summary>
        /// <param name="toReturn">����������ע��Ķ���</param>
        private void InjectProperties(object toReturn)
        {
            _propertyInjector?.Inject(toReturn, _options.PropertyInjectorOptions);
        }

        /// <summary>
        /// ��ȡ����ע�빹�캯������ֵ��
        /// </summary>
        /// <param name="type">�������͡�</param>
        /// <param name="selectedConstructor">ѡ��Ĺ��췽����</param>
        /// <param name="arguments">�����ֵ䡣</param>
        /// <returns>����ֵ�б�</returns>
        private List<object> GetDIParameterValues(Type type, MethodBase selectedConstructor, IDictionary<string, object> arguments)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in selectedConstructor.GetParameters())
            {
                if (!string.IsNullOrEmpty(parameterInfo.Name) &&
                    arguments?.TryGetValue(parameterInfo.Name, out var argument) is true)
                {
                    parameters.Add(argument);
                }
                else if (TryResolveParameter(type, parameterInfo, out var parameterValue) && parameterValue != null)
                {
                    parameters.Add(parameterValue);
                }
            }
            return parameters;
        }

        /// <summary>
        /// ��ȡ����ע�빹�캯������ֵ��
        /// </summary>
        /// <param name="type">�������͡�</param>
        /// <param name="selectedConstructor">ѡ��Ĺ��췽����</param>
        /// <param name="arguments">�������顣</param>
        /// <returns>����ֵ�б�</returns>
        private List<object> GetDIParameterValues(Type type, MethodBase selectedConstructor, object[] arguments)
        {
            var parameters = new List<object>();
            if (arguments == null)
                return parameters;

            var unusedArguments = arguments.ToList();

            foreach (var parameterInfo in selectedConstructor.GetParameters())
            {
                var argumentMatch = unusedArguments.Find(arg => parameterInfo.ParameterType.IsInstanceOfType(arg));

                if (argumentMatch != null)
                {
                    parameters.Add(argumentMatch);
                    unusedArguments.Remove(argumentMatch);
                }
                else if (TryResolveParameter(type, parameterInfo, out var parameterValue) && parameterValue != null)
                {
                    parameters.Add(parameterValue);
                }
            }
            return parameters;
        }

        /// <summary>
        /// ���Խ������캯��������
        /// </summary>
        /// <param name="type">�������͡�</param>
        /// <param name="parameterInfo">������Ϣ��</param>
        /// <param name="parameterValue">������Ĳ���ֵ��</param>
        /// <returns>����������ɹ�Ϊ true��ʧ��Ϊ false��</returns>
        /// <exception cref="SnkDIResolveException">����ʧ��ʱ�׳��쳣��</exception>
        private bool TryResolveParameter(Type type, ParameterInfo parameterInfo, out object parameterValue)
        {
            if (!TryResolve(parameterInfo.ParameterType, out parameterValue))
            {
                if (parameterInfo.IsOptional)
                {
                    parameterValue = Type.Missing;
                }
                else
                {
                    throw new SnkDIResolveException(
                        "Failed to resolve parameter for parameter {0} of type {1} when creating {2}. You may pass it as an argument",
                        parameterInfo.Name,
                        parameterInfo.ParameterType.Name,
                        type.FullName);
                }
            }

            return true;
        }
    }
}