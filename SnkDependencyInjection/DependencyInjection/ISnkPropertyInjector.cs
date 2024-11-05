namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// 定义属性注入器接口。
    /// </summary>
    public interface ISnkPropertyInjector
    {
        /// <summary>
        /// 向目标对象注入属性。
        /// </summary>
        /// <param name="target">目标对象，注入将应用于该对象的属性。</param>
        /// <param name="options">可选的属性注入器选项，用于配置注入行为。</param>
        void Inject(object target, ISnkPropertyInjectorOptions options = null);
    }
}