using System;
using System.Collections.Generic;

namespace SnkDependencyInjection
{
    /// <summary>
    /// <para>依赖注入提供者。</para>
    /// <para>委托给 <see cref="SnkDIContainer"/> 实现</para>
    /// </summary>
    public sealed class SnkDIProvider : ISnkDIProvider
    {
        internal static ISnkDIProvider Instance { get; private set; }
        /// <summary>
        /// 初始化依赖注入提供者
        /// </summary>
        /// <param name="options">依赖注入选项的接口对象</param>
        /// <returns>依赖注入提供者实例接口对象</returns>
        public static ISnkDIProvider Initialize(ISnkDIOptions options = null)
        {
            if (Instance != null)
                return Instance;

            // create a new DI container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            var instance = new SnkDIProvider(options);
            
            // ReSharper restore ObjectCreationAsStatement
            return instance;
        }

        private readonly SnkDIContainer _provider;

        private SnkDIProvider(ISnkDIOptions options)
        {
            _provider = new SnkDIContainer(options);
        }

        /// <summary>
        /// 检查是否可以解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要检查的类型。</typeparam>
        /// <returns>如果可以解析，返回 true；否则返回 false。</returns>
        public bool CanResolve<T>() where T : class
        {
            return _provider.CanResolve<T>();
        }

        /// <summary>
        /// 检查是否可以解析指定类型的实例。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <returns>如果可以解析，返回 true；否则返回 false。</returns>
        public bool CanResolve(Type type)
        {
            return _provider.CanResolve(type);
        }

        /// <summary>
        /// 尝试解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <param name="resolved">解析后的实例。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        public bool TryResolve<T>(out T resolved) where T : class
        {
            return _provider.TryResolve(out resolved);
        }

        /// <summary>
        /// 尝试解析指定类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolved">解析后的实例。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        public bool TryResolve(Type type, out object resolved)
        {
            return _provider.TryResolve(type, out resolved);
        }

        /// <summary>
        /// 尝试解析指定类型的实例集合。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <param name="resolved">解析后的实例集合。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        public bool TryResolves<T>(out IEnumerable<T> resolved) where T : class
        {
            return _provider.TryResolves(out resolved);
        }

        /// <summary>
        /// 尝试解析指定类型的实例集合。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolved">解析后的实例集合。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        public bool TryResolves(Type type, out IEnumerable<object> resolved)
        {
            return _provider.TryResolves(out resolved);
        }
        /// <summary>
        /// 解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <returns>解析后的实例。</returns>
        public T Resolve<T>() where T : class
        {
            return _provider.Resolve<T>();
        }

        /// <summary>
        /// 解析指定类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <returns>解析后的实例。</returns>
        public object Resolve(Type type)
        {
            return _provider.Resolve(type);
        }


        /// <summary>
        /// 解析指定类型的实例集合。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <returns>解析后的实例集合。</returns>
        public IEnumerable<T> Resolves<T>() where T : class
        {
            return _provider.Resolves<T>();
        }

        /// <summary>
        /// 解析指定类型的实例集合。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <returns>解析后的实例集合。</returns>
        public IEnumerable<object> Resolves(Type type)
        {
            return _provider.Resolves(type);
        }

        /// <summary>
        /// 获取指定类型的单例实例。
        /// </summary>
        /// <typeparam name="T">要获取的类型。</typeparam>
        /// <returns>单例实例。</returns>
        public T GetSingleton<T>() where T : class
        {
            return _provider.GetSingleton<T>();
        }

        /// <summary>
        /// 获取指定类型的单例实例。
        /// </summary>
        /// <param name="type">要获取的类型。</param>
        /// <returns>单例实例。</returns>
        public object GetSingleton(Type type)
        {
            return _provider.GetSingleton(type);
        }

        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要创建的类型。</typeparam>
        /// <returns>创建的实例。</returns>
        public T Create<T>() where T : class
        {
            return _provider.Create<T>();
        }

        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <param name="type">要创建的类型。</param>
        /// <returns>创建的实例。</returns>
        public object Create(Type type)
        {
            return _provider.Create(type);
        }

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <typeparam name="TInterface">源类型。</typeparam>
        /// <typeparam name="TToConstruct">目标类型。</typeparam>
        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            _provider.RegisterType<TInterface, TToConstruct>();
        }

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="constructor">构造函数委托。</param>
        public void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class
        {
            _provider.RegisterType(constructor);
        }

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <param name="t">类型。</param>
        /// <param name="constructor">构造函数委托。</param>
        public void RegisterType(Type t, Func<object> constructor)
        {
            _provider.RegisterType(t, constructor);
        }

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <param name="tFrom">源类型。</param>
        /// <param name="tTo">目标类型。</param>
        public void RegisterType(Type tFrom, Type tTo)
        {
            _provider.RegisterType(tFrom, tTo);
        }

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="theObject">单例实例。</param>
        public void RegisterSingleton<TInterface>(TInterface theObject) where TInterface : class
        {
            _provider.RegisterSingleton(theObject);
        }

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <param name="tInterface">接口类型。</param>
        /// <param name="theObject">单例实例。</param>
        public void RegisterSingleton(Type tInterface, object theObject)
        {
            _provider.RegisterSingleton(tInterface, theObject);
        }

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="theConstructor">构造函数委托。</param>
        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor) where TInterface : class
        {
            _provider.RegisterSingleton(theConstructor);
        }

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <param name="tInterface">接口类型。</param>
        /// <param name="theConstructor">构造函数委托。</param>
        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            _provider.RegisterSingleton(tInterface, theConstructor);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>() where T : class
        {
            return _provider.DIConstruct<T>((IDictionary<string, object>)null);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            return _provider.DIConstruct<T>(arguments);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>(params object[] arguments) where T : class
        {
            return _provider.DIConstruct<T>(arguments);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        public T DIConstruct<T>(object arguments) where T : class
        {
            return _provider.DIConstruct<T>(arguments);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <returns>构造的实例。</returns>
        public object DIConstruct(Type type)
        {
            return _provider.DIConstruct(type, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        public object DIConstruct(Type type, IDictionary<string, object> arguments)
        {
            return _provider.DIConstruct(type, arguments);
        }
        
        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        public object DIConstruct(Type type, object arguments)
        {
            return _provider.DIConstruct(type, arguments);
        }

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        public object DIConstruct(Type type, params object[] arguments)
        {
            return _provider.DIConstruct(type, arguments);
        }

        /// <summary>
        /// 清空所有解析器
        /// </summary>
        public void CleanAllResolvers()
        {
            _provider.CleanAllResolvers();
        }

        /// <summary>
        /// 创建子容器。
        /// </summary>
        /// <returns>子容器实例。</returns>
        public ISnkDIProvider CreateChildContainer()
        {
            return _provider.CreateChildContainer();
        }
    }
}