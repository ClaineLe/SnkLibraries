namespace SnkConnection
{
    public class SnkConnectionDefines
    {
        /// <summary>
        /// 连接默认超时时间(ms)
        /// </summary>
        public const int CONNECT_TIMEOUT_MILLISECONDS = 5000;

        /// <summary>
        /// 重连间隔时间
        /// </summary>
        public const int RECONNECT_INTERVAL_TIME = 3000;

        /// <summary>
        /// 重连退避时间
        /// </summary>
        public const int RECONNECT_INTERVAL_TIME_BACKOFF = 1000;

        /// <summary>
        /// 最大重连次数
        /// </summary>
        public const int RECONNECT_MAX_TIMES = 7;
    }
}
