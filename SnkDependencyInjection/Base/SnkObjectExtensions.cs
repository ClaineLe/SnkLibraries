using System;

namespace SnkFramework.Base
{
    /// <summary>
    /// 提供了对对象进行操作的扩展方法。
    /// </summary>
    public static class SnkObjectExtensions
    {
        /// <summary>
        /// 检查对象是否实现了 <see cref="IDisposable"/> 接口，如果是，则调用其 <c>Dispose</c> 方法。
        /// </summary>
        /// <param name="thing">需要检查并可能释放的对象。</param>
        public static void DisposeIfDisposable(this object thing)
        {
            // 类型转换检查，如果成功则调用 Dispose 方法
            if (thing is IDisposable disposable)
                disposable.Dispose();
        }
    }
}