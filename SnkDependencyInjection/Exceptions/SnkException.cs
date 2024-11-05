using System;
using System.Runtime.Serialization;

namespace SnkFramework.Exceptions
{
    /// <summary>
    /// 表示 SnkFramework 框架中的基础异常类。
    /// </summary>
    [Serializable]
    public class SnkException : Exception
    {
        /// <summary>
        /// 初始化 SnkException 类的新实例。
        /// </summary>
        public SnkException()
        {
        }

        /// <summary>
        /// 使用指定错误消息初始化 SnkException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public SnkException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 使用格式化字符串消息初始化 SnkException 类的新实例。
        /// </summary>
        /// <param name="messageFormat">错误消息的格式字符串。</param>
        /// <param name="messageFormatArguments">格式字符串的参数。</param>
        public SnkException(string messageFormat, params object[] messageFormatArguments)
            : base(string.Format(messageFormat, messageFormatArguments))
        {
        }

        /// <summary>
        /// 使用内部异常和格式化字符串消息初始化 SnkException 类的新实例。
        /// </summary>
        /// <param name="innerException">导致当前异常的内部异常。</param>
        /// <param name="messageFormat">错误消息的格式字符串。</param>
        /// <param name="formatArguments">格式字符串的参数。</param>
        public SnkException(Exception innerException, string messageFormat, params object[] formatArguments)
            : base(string.Format(messageFormat, formatArguments), innerException)
        {
        }

        /// <summary>
        /// 使用指定错误消息和内部异常初始化 SnkException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的内部异常。</param>
        public SnkException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用序列化数据初始化 SnkException 类的新实例。
        /// </summary>
        /// <param name="info">保存序列化对象数据的对象。</param>
        /// <param name="context">有关序列化的源或目标的上下文信息。</param>
        protected SnkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}