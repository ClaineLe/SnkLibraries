using System;

namespace SnkFramework.Logging
{
    /// <summary>
    /// 定义日志记录器的接口，用于记录日志信息。
    /// </summary>
    public interface ISnkLogger
    {
        /// <summary>
        /// 记录日志信息。
        /// </summary>
        /// <param name="logLevel">日志级别，用于指示日志的重要性。</param>
        /// <param name="exception">与日志相关的异常信息。</param>
        /// <param name="message">要记录的日志消息。</param>
        /// <param name="parameters">消息中的格式化参数，可变数量参数。</param>
        void Log(SnkLogLevel logLevel, Exception exception, string message, params object[] parameters);
    }

    /// <summary>
    /// 泛型日志记录器接口，表示特定类型的日志记录器。
    /// </summary>
    /// <typeparam name="T">用于标识日志记录器的特定类型。</typeparam>
    public interface ISnkLogger<T> : ISnkLogger { }
}