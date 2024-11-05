using System;
using System.Runtime.Serialization;

namespace SnkFramework.Exceptions
{
    /// <summary>
    /// 表示在依赖注入解析过程中出现的异常。
    /// </summary>
    [Serializable]
    public class SnkDIResolveException : SnkException
    {
        /// <summary>
        /// 初始化 SnkDIResolveException 类的新实例。
        /// </summary>
        public SnkDIResolveException()
        {
        }

        /// <summary>
        /// 使用指定错误消息初始化 SnkDIResolveException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public SnkDIResolveException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 使用格式化错误消息初始化 SnkDIResolveException 类的新实例。
        /// </summary>
        /// <param name="messageFormat">错误消息的格式字符串。</param>
        /// <param name="messageFormatArguments">格式字符串的参数。</param>
        public SnkDIResolveException(string messageFormat, params object[] messageFormatArguments)
            : base(messageFormat, messageFormatArguments)
        {
        }

        /// <summary>
        /// 使用指定错误消息和内部异常初始化 SnkDIResolveException 类的新实例。
        /// </summary>
        /// <param name="innerException">导致当前异常的异常。</param>
        /// <param name="messageFormat">错误消息的格式字符串。</param>
        /// <param name="formatArguments">格式字符串的参数。</param>
        public SnkDIResolveException(Exception innerException, string messageFormat, params object[] formatArguments)
            : base(innerException, messageFormat, formatArguments)
        {
        }

        /// <summary>
        /// 使用指定错误消息和内部异常初始化 SnkDIResolveException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常。</param>
        public SnkDIResolveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用序列化数据初始化 SnkDIResolveException 类的新实例。
        /// </summary>
        /// <param name="info">保存序列化对象数据的对象。</param>
        /// <param name="context">有关序列化的源或目标的上下文信息。</param>
        protected SnkDIResolveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}