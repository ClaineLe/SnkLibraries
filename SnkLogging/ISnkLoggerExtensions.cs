using System;

namespace SnkFramework.Logging
{
    /// <summary>
    /// 为 <see cref="ISnkLogger"/> 接口提供扩展方法，用于简化日志记录的操作。
    /// </summary>
    public static class ISnkLoggerExtensions
    {
        /// <summary>
        /// 记录调试级别的日志。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Debug(this ISnkLogger logger, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Debug, null, message, parameters);

        /// <summary>
        /// 记录信息级别的日志。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Info(this ISnkLogger logger, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Info, null, message, parameters);

        /// <summary>
        /// 记录警告级别的日志，包含异常信息。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="exception">与日志相关的异常信息。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Warning(this ISnkLogger logger, Exception exception, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Warning, exception, message, parameters);

        /// <summary>
        /// 记录警告级别的日志，不包含异常信息。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Warning(this ISnkLogger logger, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Warning, null, message, parameters);

        /// <summary>
        /// 记录错误级别的日志，包含异常信息。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="exception">与日志相关的异常信息。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Error(this ISnkLogger logger, Exception exception, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Error, exception, message, parameters);

        /// <summary>
        /// 记录错误级别的日志，不包含异常信息。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Error(this ISnkLogger logger, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Error, null, message, parameters);

        /// <summary>
        /// 记录严重级别的日志，包含异常信息。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="exception">与日志相关的异常信息。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Critical(this ISnkLogger logger, Exception exception, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Critical, exception, message, parameters);

        /// <summary>
        /// 记录严重级别的日志，不包含异常信息。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        public static void Critical(this ISnkLogger logger, string message, params object[] parameters)
            => logger.Log(SnkLogLevel.Critical, null, message, parameters);

        /// <summary>
        /// 记录异常信息的日志。
        /// </summary>
        /// <param name="logger">日志记录器接口。</param>
        /// <param name="exception">要记录的异常信息。</param>
        public static void Exception(this ISnkLogger logger, Exception exception)
            => logger.Log(SnkLogLevel.Critical, exception, exception?.Message);
    }
}