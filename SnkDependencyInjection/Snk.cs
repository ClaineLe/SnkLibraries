using SnkFramework.Base;
using SnkFramework.DependencyInjection;

namespace SnkFramework
{
    /// <summary>
    /// 提供框架级别的常用静态成员和功能。
    /// </summary>
    public static class Snk
    {
        /// <summary>
        /// 获取依赖注入提供者实例。
        /// </summary>
        public static ISnkDIProvider DIProvider => SnkSingleton<ISnkDIProvider>.Instance;
    }
}