namespace SnkConnection
{
    namespace Channel
    {
        public enum SnkChannelState
        {
            /// <summary>
            /// 无
            /// </summary>
            None,

            /// <summary>
            /// 正在打开
            /// </summary>
            Opening,

            /// <summary>
            /// 已经打开
            /// </summary>
            Opened,

            /// <summary>
            /// 正在关闭
            /// </summary>
            Closing,

            /// <summary>
            /// 已经关闭
            /// </summary>
            Closed,
        }
    }
}
