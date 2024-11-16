using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkLogging;

namespace SnkDependencyInjection
{
    /// <summary>
    /// <para>用于依赖注入的容器类。</para>
    /// <para>实现接口 <see cref="ISnkDIProvider"/> </para>
    /// </summary>
    public sealed partial class SnkDIContainer : ISnkDIProvider
    {
        private static readonly ResolverType? ResolverTypeNoneSpecified = null;

        /// <summary>
        /// 存储类型与其对应的解析器的字典。
        /// </summary>
        private readonly Dictionary<Type, List<IResolver>> _resolvers = new Dictionary<Type, List<IResolver>>();

        /// <summary>
        /// 用于检测循环依赖的字典。
        /// </summary>
        private readonly Dictionary<Type, bool> _circularTypeDetection = new Dictionary<Type, bool>();

        /// <summary>
        /// 用于线程同步的锁对象。
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 配置选项。
        /// </summary>
        private readonly ISnkDIOptions _options;

        /// <summary>
        /// 属性注入器。
        /// </summary>
        private readonly ISnkPropertyInjector _propertyInjector;

        /// <summary>
        /// 父级依赖注入提供者。
        /// </summary>
        private readonly ISnkDIProvider _parentProvider;

        /// <summary>
        /// 获取配置选项。
        /// </summary>
        private ISnkDIOptions Options => _options;

        /// <summary>
        /// 使用指定的选项和父级提供者创建一个新的 DI 容器实例。
        /// </summary>
        /// <param name="options">依赖注入配置选项。</param>
        /// <param name="parentProvider">父级 DI 提供者。</param>
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
        /// 使用父级提供者创建 DI 容器实例。
        /// </summary>
        /// <param name="parentProvider">父级 DI 提供者。</param>
        /// <exception cref="ArgumentNullException">如果 parentProvider 为 null，则抛出异常。</exception>
        public SnkDIContainer(ISnkDIProvider parentProvider) : this(null, parentProvider)
        {
            if (parentProvider == null)
            {
                throw new ArgumentNullException(nameof(parentProvider), "Provide a parent DI provider to this constructor");
            }
        }
 
        /// <summary>
        /// 检查容器是否可以解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要检查的类型。</typeparam>
        /// <returns>如果可以解析，返回 true；否则返回 false。</returns>
        public bool CanResolve<T>() where T : class
        {
            return CanResolve(typeof(T));
        }

        /// <summary>
        /// 检查容器是否可以解析指定类型的实例。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <returns>如果可以解析，返回 true；否则返回 false。</returns>
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
        /// 尝试解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <param name="resolved">解析成功时返回的实例；否则返回类型的默认实例。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
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
        /// 尝试解析指定类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolved">解析成功时返回的实例对象；否则为 null。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        public bool TryResolve(Type type, out object resolved)
        {
            lock (_locker)
            {
                return InternalTryResolve(type, out resolved);
            }
        }

        /// <summary>
        /// 尝试解析指定类型的实例集合。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <param name="resolved">解析后的实例集合。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
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
        /// 尝试解析指定类型的实例集合。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolved">解析后的实例集合。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
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
        /// 解析指定类型的实例，当解析失败时抛出异常。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <returns>解析后的实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法解析类型时抛出。</exception>
        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        /// <summary>
        /// 解析指定类型的实例，当解析失败时抛出异常。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <returns>解析后的实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法解析类型时抛出。</exception>
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
        /// 解析指定类型的实例集合。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <returns>解析后的实例集合。</returns>
        public IEnumerable<T> Resolves<T>() where T : class
        {
            if (this._resolvers.TryGetValue(typeof(T), out var list) == false)
                return Enumerable.Empty<T>();
            return list.Cast<T>();
        }

        /// <summary>
        /// 解析指定类型的实例集合。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <returns>解析后的实例集合。</returns>
        public IEnumerable<object> Resolves(Type type)
        {
            if (this._resolvers.TryGetValue(type, out var list) == false)
                return Enumerable.Empty<object>();
            return list;
        }

        /// <summary>
        /// 获取指定类型的单例实例，若不存在将抛出异常。
        /// </summary>
        /// <typeparam name="T">要获取的类型。</typeparam>
        /// <returns>单例实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法获取单例时抛出。</exception>
        public T GetSingleton<T>() where T : class
        {
            return GetSingleton(typeof(T)) as T;
        }

        /// <summary>
        /// 获取指定类型的单例实例，若不存在将抛出异常。
        /// </summary>
        /// <param name="type">要获取的类型。</param>
        /// <returns>单例实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法获取单例时抛出。</exception>
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
        /// 创建指定类型的新实例，每次调用返回不同的对象。
        /// </summary>
        /// <typeparam name="T">要创建的类型。</typeparam>
        /// <returns>创建的实例。</returns>
        public T Create<T>() where T : class
        {
            return Create(typeof(T)) as T;
        }

        /// <summary>
        /// 创建指定类型的新实例，每次调用返回不同的对象。
        /// </summary>
        /// <param name="type">要创建的类型。</param>
        /// <returns>创建的实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法创建实例时抛出。</exception>
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
        /// 注册类型映射，将接口类型与具体实现或构造函数关联。
        /// </summary>
        /// <typeparam name="TInterface">源接口类型。</typeparam>
        /// <typeparam name="TToConstruct">目标实现类型。</typeparam>
        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            RegisterType(typeof(TInterface), typeof(TToConstruct));
        }

        /// <summary>
        /// 注册类型映射，使用委托构造接口类型的实例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="constructor">用于创建实例的构造函数委托。</param>
        public void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class
        {
            var resolver = new FuncConstructingResolver(constructor);
            InternalSetResolver(typeof(TInterface), resolver);
        }

        /// <summary>
        /// 注册类型映射，使用委托构造实例。
        /// </summary>
        /// <param name="t">接口或基类类型。</param>
        /// <param name="constructor">构造函数委托。</param>
        /// <exception cref="SnkDIResolveException">当构造函数返回的对象与类型不兼容时抛出。</exception>
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
        /// 注册类型映射，将一个类型映射到另一个类型，当请求解析源类型时创建目标类型的实例。
        /// </summary>
        /// <param name="tFrom">源类型。</param>
        /// <param name="tTo">目标类型。</param>
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
        /// 注册一个单例实例，接口类型与具体实例关联。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="theObject">单例实例。</param>
        public void RegisterSingleton<TInterface>(TInterface theObject) where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theObject);
        }

        /// <summary>
        /// 注册一个单例实例，接口类型与具体实例关联。
        /// </summary>
        /// <param name="tInterface">接口类型。</param>
        /// <param name="theObject">单例实例。</param>
        public void RegisterSingleton(Type tInterface, object theObject)
        {
            InternalSetResolver(tInterface, new SingletonResolver(theObject));
        }

        /// <summary>
        /// 注册单例实例，使用构造函数委托。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="theConstructor">用于创建单例实例的构造函数委托。</param>
        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor) where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theConstructor);
        }

        /// <summary>
        /// 注册单例实例，使用构造函数委托。
        /// </summary>
        /// <param name="tInterface">接口类型。</param>
        /// <param name="theConstructor">构造函数委托。</param>
        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            InternalSetResolver(tInterface, new ConstructingSingletonResolver(theConstructor));
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <returns>构造的实例。</returns>
        public object DIConstruct(Type type)
        {
            return DIConstruct(type, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">用于构造函数的参数。</param>
        /// <returns>构造的实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法找到适用的构造函数时抛出。</exception>
        public object DIConstruct(Type type, object arguments)
        {
            return DIConstruct(type, arguments?.ToPropertyDictionary());
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>() where T : class
        {
            return DIConstruct(typeof(T), (IDictionary<string, object>)null) as T;
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">用于构造函数的参数。</param>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            return DIConstruct(typeof(T), arguments) as T;
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">用于构造函数的参数。</param>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>(object arguments) where T : class
        {
            return DIConstruct(typeof(T), arguments?.ToPropertyDictionary()) as T;
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">用于构造函数的参数。</param>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>(params object[] arguments) where T : class
        {
            return DIConstruct(typeof(T), arguments) as T;
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">用于构造函数的参数。</param>
        /// <returns>构造的实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法找到适用的构造函数时抛出。</exception>
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
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">用于构造函数的参数。</param>
        /// <returns>构造的实例。</returns>
        /// <exception cref="SnkDIResolveException">当无法找到适用的构造函数时抛出。</exception>
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
        /// 清空所有解析器，非释放解析器，为避免泄露，请释放后解析器后，再进行清空操作
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
        /// 创建并返回一个新的子容器，继承当前容器的配置。
        /// </summary>
        /// <returns>子容器实例。</returns>
        public ISnkDIProvider CreateChildContainer() => new SnkDIContainer(this);

        /// <summary>
        /// 构建指定类型的实例，并注入属性。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="constructor">使用的构造函数。</param>
        /// <param name="arguments">构造函数参数。</param>
        /// <returns>构造的对象。</returns>
        /// <exception cref="SnkDIResolveException">如果构造失败，则抛出异常。</exception>
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

                // 类型转换检查，如果成功则调用 Dispose 方法
                if (toReturn is IDisposable disposable)
                    disposable.Dispose();
                throw;
            }
            return toReturn;
        }

        /// <summary>
        /// 检查解析器是否支持指定的解析类型。
        /// </summary>
        /// <param name="resolver">解析器对象。</param>
        /// <param name="requiredResolverType">需要的解析类型。</param>
        /// <returns>如果支持，返回 true；否则返回 false。</returns>
        private static bool Supports(IResolver resolver, ResolverType? requiredResolverType)
        {
            if (resolver == null)
                return false;

            if (!requiredResolverType.HasValue)
                return true;

            return resolver.ResolveType == requiredResolverType.Value;
        }

        /// <summary>
        /// 尝试解析类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolved">解析后的对象。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        private bool InternalTryResolve(Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        /// <summary>
        /// 尝试解析类型的实例，考虑指定的解析类型。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="requiredResolverType">需要的解析类型。</param>
        /// <param name="resolved">解析后的对象。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
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
        /// 尝试使用指定解析器解析类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolver">用于解析的解析器。</param>
        /// <param name="resolved">解析后的对象。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
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
        /// 尝试获取指定类型的解析器。
        /// </summary>
        /// <param name="type">需要解析的类型。</param>
        /// <param name="resolver">查找到的解析器。</param>
        /// <returns>如果找到解析器，返回 true；否则返回 false。</returns>
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
        /// 判断解析器是否需要检测循环引用。
        /// </summary>
        /// <param name="resolver">要检查的解析器。</param>
        /// <returns>如果需要检测，返回 true；否则返回 false。</returns>
        /// <exception cref="SnkException">如果解析器类型未知，抛出异常。</exception>
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
        /// 设置类型的解析器。
        /// </summary>
        /// <param name="interfaceType">接口类型。</param>
        /// <param name="resolver">解析器。</param>
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
        /// 向对象中注入属性。
        /// </summary>
        /// <param name="toReturn">待进行属性注入的对象。</param>
        private void InjectProperties(object toReturn)
        {
            _propertyInjector?.Inject(toReturn, _options.PropertyInjectorOptions);
        }

        /// <summary>
        /// 获取依赖注入构造函数参数值。
        /// </summary>
        /// <param name="type">构造类型。</param>
        /// <param name="selectedConstructor">选择的构造方法。</param>
        /// <param name="arguments">参数字典。</param>
        /// <returns>参数值列表。</returns>
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
        /// 获取依赖注入构造函数参数值。
        /// </summary>
        /// <param name="type">构造类型。</param>
        /// <param name="selectedConstructor">选择的构造方法。</param>
        /// <param name="arguments">参数数组。</param>
        /// <returns>参数值列表。</returns>
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
        /// 尝试解析构造函数参数。
        /// </summary>
        /// <param name="type">构造类型。</param>
        /// <param name="parameterInfo">参数信息。</param>
        /// <param name="parameterValue">解析后的参数值。</param>
        /// <returns>解析结果，成功为 true，失败为 false。</returns>
        /// <exception cref="SnkDIResolveException">解析失败时抛出异常。</exception>
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