namespace SnkDependencyInjection
{
    /// <summary>
    /// 表示属性注入选项的类。
    /// </summary>
    public class SnkPropertyInjectorOptions : ISnkPropertyInjectorOptions
    {
        /// <summary>
        /// 初始化 <see cref="SnkPropertyInjectorOptions"/> 类的新实例，设置默认选项。
        /// </summary>
        public SnkPropertyInjectorOptions()
        {
            // 默认为不进行任何属性注入
            InjectIntoProperties = SnkPropertyInjection.None;
            // 默认为属性注入失败时不抛出异常
            ThrowIfPropertyInjectionFails = false;
        }

        /// <summary>
        /// 获取或设置属性注入的类型。
        /// </summary>
        public SnkPropertyInjection InjectIntoProperties { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在属性注入失败时抛出异常。
        /// </summary>
        public bool ThrowIfPropertyInjectionFails { get; set; }

        /// <summary>
        /// 静态字段，用于存储注入属性选项的实例。
        /// </summary>
        private static ISnkPropertyInjectorOptions _injectProperties;

        /// <summary>
        /// 获取用于注入接口属性的选项实例。
        /// </summary>
        public static ISnkPropertyInjectorOptions Inject
        {
            get
            {
                // 如果尚未初始化，则创建一个新实例并设置为注入接口属性
                _injectProperties = _injectProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.InjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _injectProperties;
            }
        }

        /// <summary>
        /// 静态字段，用于存储全部属性注入选项的实例。
        /// </summary>
        private static ISnkPropertyInjectorOptions _allProperties;

        /// <summary>
        /// 获取用于注入所有接口属性的选项实例。
        /// </summary>
        public static ISnkPropertyInjectorOptions All
        {
            get
            {
                // 如果尚未初始化，则创建一个新实例并设置为注入所有接口属性
                _allProperties = _allProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _allProperties;
            }
        }
    }
}