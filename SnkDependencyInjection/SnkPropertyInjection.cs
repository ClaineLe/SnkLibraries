namespace SnkDependencyInjection
{
    /// <summary>
    /// 枚举类型，表示属性注入的策略。
    /// </summary>
    public enum SnkPropertyInjection
    {
        /// <summary>
        /// 不注入任何属性。
        /// </summary>
        None,

        /// <summary>
        /// 仅注入带有接口类型的属性。
        /// </summary>
        InjectInterfaceProperties,

        /// <summary>
        /// 注入所有接口类型的属性。
        /// </summary>
        AllInterfaceProperties
    }
}
