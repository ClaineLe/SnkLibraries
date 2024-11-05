namespace SnkLogging
{
    /// <summary>
    /// 定义日志记录的级别枚举。
    /// </summary>
    public enum SnkLogLevel
    {
        /// <summary>
        /// 调试级别，用于记录调试信息。
        /// </summary>
        Debug,

        /// <summary>
        /// 信息级别，用于记录一般信息。
        /// </summary>
        Info,

        /// <summary>
        /// 警告级别，用于记录需要注意的问题。
        /// </summary>
        Warning,

        /// <summary>
        /// 错误级别，用于记录发生错误的事件。
        /// </summary>
        Error,

        /// <summary>
        /// 关键级别，用于记录严重错误或重大事件。
        /// </summary>
        Critical
    }
}