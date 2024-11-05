using System;
using System.Collections.Generic;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 定义依赖注入提供者的接口。
    /// </summary>
    public interface ISnkDIProvider
    {
        /// <summary>
        /// 检查是否可以解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要检查的类型。</typeparam>
        /// <returns>如果可以解析，返回 true；否则返回 false。</returns>
        bool CanResolve<T>() where T : class;

        /// <summary>
        /// 检查是否可以解析指定类型的实例。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <returns>如果可以解析，返回 true；否则返回 false。</returns>
        bool CanResolve(Type type);

        /// <summary>
        /// 解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <returns>解析后的实例。</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// 解析指定类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <returns>解析后的实例。</returns>
        object Resolve(Type type);

        /// <summary>
        /// 尝试解析指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要解析的类型。</typeparam>
        /// <param name="resolved">解析后的实例。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        bool TryResolve<T>(out T resolved) where T : class;

        /// <summary>
        /// 尝试解析指定类型的实例。
        /// </summary>
        /// <param name="type">要解析的类型。</param>
        /// <param name="resolved">解析后的实例。</param>
        /// <returns>如果解析成功，返回 true；否则返回 false。</returns>
        bool TryResolve(Type type, out object resolved);

        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要创建的类型。</typeparam>
        /// <returns>创建的实例。</returns>
        T Create<T>() where T : class;

        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <param name="type">要创建的类型。</param>
        /// <returns>创建的实例。</returns>
        object Create(Type type);

        /// <summary>
        /// 获取指定类型的单例实例。
        /// </summary>
        /// <typeparam name="T">要获取的类型。</typeparam>
        /// <returns>单例实例。</returns>
        T GetSingleton<T>() where T : class;

        /// <summary>
        /// 获取指定类型的单例实例。
        /// </summary>
        /// <param name="type">要获取的类型。</param>
        /// <returns>单例实例。</returns>
        object GetSingleton(Type type);

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <typeparam name="TFrom">源类型。</typeparam>
        /// <typeparam name="TTo">目标类型。</typeparam>
        void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="constructor">构造函数委托。</param>
        void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class;

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <param name="t">类型。</param>
        /// <param name="constructor">构造函数委托。</param>
        void RegisterType(Type t, Func<object> constructor);

        /// <summary>
        /// 注册类型映射。
        /// </summary>
        /// <param name="tFrom">源类型。</param>
        /// <param name="tTo">目标类型。</param>
        void RegisterType(Type tFrom, Type tTo);

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="theObject">单例实例。</param>
        void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class;

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <param name="tInterface">接口类型。</param>
        /// <param name="theObject">单例实例。</param>
        void RegisterSingleton(Type tInterface, object theObject);

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <param name="theConstructor">构造函数委托。</param>
        void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;

        /// <summary>
        /// 注册单例实例。
        /// </summary>
        /// <param name="tInterface">接口类型。</param>
        /// <param name="theConstructor">构造函数委托。</param>
        void RegisterSingleton(Type tInterface, Func<object> theConstructor);

        /// <summary>
        /// 使用依赖注入构造指定类型的实例。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <returns>构造的实例。</returns>
        T DIConstruct<T>()
            where T : class;

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        T DIConstruct<T>(IDictionary<string, object> arguments)
            where T : class;

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        T DIConstruct<T>(object arguments)
            where T : class;

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <typeparam name="T">要构造的类型。</typeparam>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        T DIConstruct<T>(params object[] arguments)
            where T : class;

        /// <summary>
        /// 使用依赖注入构造指定类型的实例。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <returns>构造的实例。</returns>
        object DIConstruct(Type type);

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        object DIConstruct(Type type, IDictionary<string, object> arguments);

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        object DIConstruct(Type type, object arguments);

        /// <summary>
        /// 使用依赖注入构造指定类型的实例，并传递参数。
        /// </summary>
        /// <param name="type">要构造的类型。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>构造的实例。</returns>
        object DIConstruct(Type type, params object[] arguments);

        /// <summary>
        /// 创建子容器。
        /// </summary>
        /// <returns>子容器实例。</returns>
        ISnkDIProvider CreateChildContainer();
    }
}