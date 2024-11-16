using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 用于延迟创建单例实例的类。
    /// </summary>
    public class SnkLazySingletonCreator
    {
        // 用来同步线程锁的对象
        private readonly object _locker = new object();

        // 需要创建单例实例的类型
        private readonly Type _type;

        // 保存单例实例的字段
        private object _instance;

        /// <summary>
        /// 获取单例实例，如果实例尚未创建，则延迟创建它。
        /// </summary>
        public object Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                // 确保线程安全的实例化
                lock (_locker)
                {
                    // 如果_instance为null，则调用依赖注入提供的构造方法创建实例
                    _instance = _instance ?? SnkDIProvider.Instance.DIConstruct(_type);
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="SnkLazySingletonCreator"/> 类的新实例。
        /// </summary>
        /// <param name="type">需要创建单例实例的类型。</param>
        public SnkLazySingletonCreator(Type type)
        {
            _type = type;
        }
    }
}