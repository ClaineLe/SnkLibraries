using System;

namespace SnkFramework.Logging
{
    /// <summary>
    /// 提供日志记录器的主机类，用于获取默认日志记录器和设置日志记录器工厂。
    /// </summary>
    public static class SnkLogHost
    {
        private static readonly object _locker = new object();
        private static ISnkLogger _defaultLogger;
        private static ISnkLoggerFactory _loggerFactory;

        /// <summary>
        /// 获取或设置默认的日志记录器。
        /// </summary>
        public static ISnkLogger Default
        {
            get
            {
                if (_defaultLogger == null)
                {
                    lock (_locker)
                    {
                        if (_defaultLogger == null)
                        {
                            _defaultLogger = GetLog("Default");
                        }
                    }
                }
                return _defaultLogger;
            }
        }

        /// <summary>
        /// 设置日志记录器工厂。
        /// </summary>
        /// <param name="loggerFactory">要设置的日志记录器工厂。</param>
        public static void SetupLoggerFactory(ISnkLoggerFactory loggerFactory) => _loggerFactory = loggerFactory;

        /// <summary>
        /// 通过类型设置日志记录器工厂。
        /// </summary>
        /// <param name="loggerFactoryType">要设置的日志记录器工厂类型。</param>
        /// <exception cref="ArgumentException">如果提供的类型未实现 <see cref="ISnkLoggerFactory"/>，则抛出此异常。</exception>
        public static void SetupLoggerFactory(Type loggerFactoryType)
        {
            if (!typeof(ISnkLoggerFactory).IsAssignableFrom(loggerFactoryType))
                throw new ArgumentException("Type must implement ISnkLoggerFactory", nameof(loggerFactoryType));
            _loggerFactory = Activator.CreateInstance(loggerFactoryType) as ISnkLoggerFactory;
        }

        /// <summary>
        /// 通过泛型类型设置日志记录器工厂。
        /// </summary>
        /// <typeparam name="TLoggerFactory">要设置的日志记录器工厂的泛型类型。</typeparam>
        public static void SetupLoggerFactory<TLoggerFactory>() => SetupLoggerFactory(typeof(TLoggerFactory));

        /// <summary>
        /// 获取泛型类型的日志记录器。
        /// </summary>
        /// <typeparam name="T">日志记录的泛型类型。</typeparam>
        /// <returns>返回一个 <see cref="ISnkLogger{T}"/> 实例。</returns>
        /// <exception cref="InvalidOperationException">如果日志记录器工厂未初始化，则抛出此异常。</exception>
        public static ISnkLogger<T> GetLog<T>()
        {
            if (_loggerFactory == null)
                throw new InvalidOperationException("Logger factory not initialized.");
            return _loggerFactory.CreateLogger<T>();
        }

        /// <summary>
        /// 获取指定类别名称的日志记录器。
        /// </summary>
        /// <param name="categoryName">日志记录的类别名称。</param>
        /// <returns>返回一个 <see cref="ISnkLogger"/> 实例。</returns>
        /// <exception cref="InvalidOperationException">如果日志记录器工厂未初始化，则抛出此异常。</exception>
        public static ISnkLogger GetLog(string categoryName)
        {
            if (_loggerFactory == null)
                throw new InvalidOperationException("Logger factory not initialized.");
            return _loggerFactory.CreateLogger(categoryName);
        }
    }
}