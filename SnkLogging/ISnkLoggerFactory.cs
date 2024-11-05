namespace SnkLogging
{
    /// <summary>
    /// 定义日志记录器工厂的接口。
    /// </summary>
    public interface ISnkLoggerFactory
    {
        /// <summary>
        /// 创建指定类别名称的日志记录器。
        /// </summary>
        /// <param name="categoryName">日志记录的类别名称。</param>
        /// <returns>返回一个 <see cref="ISnkLogger"/> 实例。</returns>
        ISnkLogger CreateLogger(string categoryName);

        /// <summary>
        /// 创建泛型类型的日志记录器。
        /// </summary>
        /// <typeparam name="T">日志记录的泛型类型。</typeparam>
        /// <returns>返回一个 <see cref="ISnkLogger{T}"/> 实例。</returns>
        ISnkLogger<T> CreateLogger<T>();
    }
}