using System;
using System.Collections.Generic;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// 扩展方法类，用于创建创建和注册解析器
    /// </summary>
    public static class SnkDIContainerExtensions
    {
        /// <summary>
        /// 创建解析器方法，用于构造需要一个参数的解析器接口对象。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数的第一个参数的类型。</typeparam>
        /// <param name="provider">依赖注入容器提供者，负责解析依赖项。</param>
        /// <param name="typedConstructor">一个包含单个参数的构造函数。</param>
        /// <returns>返回一个函数，该函数解析并返回接口类型的实例。</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1>(
            this ISnkDIProvider provider,
            Func<TParameter1, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                return typedConstructor((TParameter1)parameter1);
            };
        }

        /// <summary>
        /// 创建解析器方法，用于构造需要两个参数的解析器接口对象。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数的第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数的第二个参数的类型。</typeparam>
        /// <param name="provider">依赖注入容器提供者，负责解析依赖项。</param>
        /// <param name="typedConstructor">一个包含两个参数的构造函数。</param>
        /// <returns>返回一个函数，该函数解析并返回接口类型的实例。</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2);
            };
        }

        /// <summary>
        /// 创建解析器方法，用于构造需要三个参数的解析器接口对象。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数的第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数的第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数的第三个参数的类型。</typeparam>
        /// <param name="provider">依赖注入容器提供者，负责解析依赖项。</param>
        /// <param name="typedConstructor">一个包含三个参数的构造函数。</param>
        /// <returns>返回一个函数，该函数解析并返回接口类型的实例。</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2, TParameter3>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TParameter3, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                provider.TryResolve(typeof(TParameter3), out var parameter3);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3);
            };
        }

        /// <summary>
        /// 创建解析器方法，用于构造需要四个参数的解析器接口对象。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数的第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数的第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数的第三个参数的类型。</typeparam>
        /// <typeparam name="TParameter4">构造函数的第四个参数的类型。</typeparam>
        /// <param name="provider">依赖注入容器提供者，负责解析依赖项。</param>
        /// <param name="typedConstructor">一个包含四个参数的构造函数。</param>
        /// <returns>返回一个函数，该函数解析并返回接口类型的实例。</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2, TParameter3, TParameter4>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                provider.TryResolve(typeof(TParameter3), out var parameter3);
                provider.TryResolve(typeof(TParameter4), out var parameter4);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3, (TParameter4)parameter4);
            };
        }

        /// <summary>
        /// 创建解析器方法，用于构造需要五个参数的解析器接口对象。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数的第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数的第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数的第三个参数的类型。</typeparam>
        /// <typeparam name="TParameter4">构造函数的第四个参数的类型。</typeparam>
        /// <typeparam name="TParameter5">构造函数的第五个参数的类型。</typeparam>
        /// <param name="provider">依赖注入容器提供者，负责解析依赖项。</param>
        /// <param name="typedConstructor">一个包含五个参数的构造函数。</param>
        /// <returns>返回一个函数，该函数解析并返回接口类型的实例。</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2, TParameter3, TParameter4, TParameter5>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
            where TParameter5 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                provider.TryResolve(typeof(TParameter3), out var parameter3);
                provider.TryResolve(typeof(TParameter4), out var parameter4);
                provider.TryResolve(typeof(TParameter5), out var parameter5);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3, (TParameter4)parameter4, (TParameter5)parameter5);
            };
        }

        /// <summary>
        /// 构建一个类型的实例并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TType">具体实现类型。</typeparam>
        /// <param name="provider">依赖注入提供者。</param>
        /// <returns>构建的实例。</returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>();
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// 使用参数构建一个类型的实例并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TType">具体实现类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="arguments">用于实例构造的命名参数。</param>
        /// <returns></returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider, IDictionary<string, object> arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>(arguments);
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// 使用单个对象作为参数构建一个类型的实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="arguments">用于实例构造的对象参数。</param>
        /// <returns>构建的实例。</returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider, object arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>(arguments);
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// 使用多个参数构建一个类型的实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="arguments">用于实例构造的参数数组。</param>
        /// <returns>构建的实例。</returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider, params object[] arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>(arguments);
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// 构建一个给定类型的实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="type">要实例化的类型。</param>
        /// <returns>构建的实例。</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type)
        {
            var instance = provider.DIConstruct(type);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// 使用参数构建一个类型的实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="type">要实例化的类型。</param>
        /// <param name="arguments">用于实例构造的命名参数。</param>
        /// <returns>构建的实例。</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, IDictionary<string, object> arguments)
        {
            var instance = provider.DIConstruct(type, arguments);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// 使用单个对象作为参数构建一个类型的实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="type">要实例化的类型。</param>
        /// <param name="arguments">用于实例构造的对象参数。</param>
        /// <returns>构建的实例。</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, object arguments)
        {
            var instance = provider.DIConstruct(type, arguments);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// 使用多个参数构建一个类型的实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="type">要实例化的类型。</param>
        /// <param name="arguments">用于实例构造的参数数组。</param>
        /// <returns>构建的实例。</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, params object[] arguments)
        {
            var instance = provider.DIConstruct(type, arguments);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// 延迟构建一个类型的实例并注册为单例，当第一次被请求时才构造。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TType">具体实现类型。</typeparam>
        /// <param name="provider">依赖注入提供者。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider)
            where TInterface : class
            where TType : class, TInterface
        {
            provider.RegisterSingleton<TInterface>(() => provider.DIConstruct<TType>());
        }

        /// <summary>
        /// 使用指定构造函数延迟构建并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">用于实例化的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface>(this ISnkDIProvider provider, Func<TInterface> constructor)
            where TInterface : class
        {
            provider.RegisterSingleton<TInterface>(constructor);
        }

        /// <summary>
        /// 使用指定构造函数延迟构建一个实例并注册为单例。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="type">要实例化的类型。</param>
        /// <param name="constructor">用于实例化的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, Func<object> constructor)
        {
            provider.RegisterSingleton(type, constructor);
        }

        /// <summary>
        /// 使用带一个参数的构造函数延迟构建并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带一个参数的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1>(this ISnkDIProvider provider, Func<TParameter1, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// 使用带两个参数的构造函数延迟构建并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带两个参数的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// 使用带三个参数的构造函数延迟构建并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数第三个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带三个参数的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2, TParameter3>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// 使用带四个参数的构造函数延迟构建并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数第三个参数的类型。</typeparam>
        /// <typeparam name="TParameter4">构造函数第四个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带四个参数的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2, TParameter3, TParameter4>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// 使用带五个参数的构造函数延迟构建并注册为单例。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数第三个参数的类型。</typeparam>
        /// <typeparam name="TParameter4">构造函数第四个参数的类型。</typeparam>
        /// <typeparam name="TParameter5">构造函数第五个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带五个参数的构造函数。</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2, TParameter3, TParameter4, TParameter5>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
            where TParameter5 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// 注册一个类型，使其可以在需要时被构造。
        /// </summary>
        /// <typeparam name="TType">要注册的类型。</typeparam>
        public static void RegisterType<TType>(this ISnkDIProvider provider)
            where TType : class
        {
            provider.RegisterType<TType, TType>();
        }

        /// <summary>
        /// 注册一个类型，使其可以在需要时被构造。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="tType">要注册的类型。</param>
        public static void RegisterType(this ISnkDIProvider provider, Type tType)
        {
            provider.RegisterType(tType, tType);
        }

        /// <summary>
        /// 使用带一个参数的构造函数注册一个类型。
        /// </summary>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数参数的类型。</typeparam>
        /// <param name="constructor">带单个参数的构造函数。</param>
        public static void RegisterType<TInterface, TParameter1>(this ISnkDIProvider provider, Func<TParameter1, TInterface> constructor)
           where TInterface : class
           where TParameter1 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// 使用带两个参数的构造函数注册一个类型。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带两个参数的构造函数。</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// 使用带三个参数的构造函数注册一个类型。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数第三个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带三个参数的构造函数。</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2, TParameter3>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// 使用带四个参数的构造函数注册一个类型。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数第三个参数的类型。</typeparam>
        /// <typeparam name="TParameter4">构造函数第四个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带四个参数的构造函数。</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2, TParameter3, TParameter4>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// 使用带五个参数的构造函数注册一个类型。
        /// </summary>
        /// <typeparam name="TInterface">接口类型。</typeparam>
        /// <typeparam name="TParameter1">构造函数第一个参数的类型。</typeparam>
        /// <typeparam name="TParameter2">构造函数第二个参数的类型。</typeparam>
        /// <typeparam name="TParameter3">构造函数第三个参数的类型。</typeparam>
        /// <typeparam name="TParameter4">构造函数第四个参数的类型。</typeparam>
        /// <typeparam name="TParameter5">构造函数第五个参数的类型。</typeparam>
        /// <param name="provider">依赖注入提供者拓展对象。</param>
        /// <param name="constructor">带五个参数的构造函数。</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2, TParameter3, TParameter4, TParameter5>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
            where TParameter5 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }
    }
}
