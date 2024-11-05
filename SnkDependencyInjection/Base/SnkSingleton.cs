using SnkFramework.Exceptions;
using System;
using System.Collections.Generic;

namespace SnkFramework.Base
{
    /// <summary>
    /// 提供用于实现单例模式的抽象基类，支持 <see cref="IDisposable"/> 接口。
    /// </summary>
    public abstract class SnkSingleton : IDisposable
    {
        /// <summary>
        /// 析构函数用于释放非托管资源。
        /// </summary>
        ~SnkSingleton()
        {
            Dispose(false);
        }

        /// <summary>
        /// 实现IDisposable接口，用于释放资源。
        /// </summary>
        public void Dispose()
        {
            // 释放托管资源
            Dispose(true);
            // 阻止析构函数被调用
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 子类必须实现用于释放资源的逻辑。
        /// </summary>
        /// <param name="isDisposing">如果是 true，释放托管和非托管资源；否则仅释放非托管资源。</param>
        protected abstract void Dispose(bool isDisposing);

        /// <summary>
        /// 存储所有 SnkSingleton 实例的集合。
        /// </summary>
        private static readonly List<SnkSingleton> Singletons = new List<SnkSingleton>();

        /// <summary>
        /// 初始化 SnkSingleton 实例，并将其实例添加到集合中。
        /// </summary>
        protected SnkSingleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        /// <summary>
        /// 释放所有单例实例并清空集合。
        /// </summary>
        public static void ClearAllSingletons()
        {
            lock (Singletons)
            {
                foreach (var s in Singletons)
                {
                    s.Dispose();
                }

                Singletons.Clear();
            }
        }
    }

    /// <summary>
    /// 泛型单例基类，限制只能有一个实例。
    /// </summary>
    /// <typeparam name="TInterface">单例所实现的接口类型，必须为引用类型。</typeparam>
    public abstract class SnkSingleton<TInterface> : SnkSingleton where TInterface : class
    {
        /// <summary>
        /// 初始化 SnkSingleton 的新实例，并确保该实例为单例。
        /// </summary>
        /// <exception cref="SnkException">如果尝试创建多个实例，则抛出异常。</exception>
        protected SnkSingleton()
        {
            if (Instance != null)
                throw new SnkException("You cannot create more than one instance of SnkSingleton");

            Instance = this as TInterface;
        }

        /// <summary>
        /// 当前单例实例。
        /// </summary>
        public static TInterface Instance { get; private set; }

        /// <summary>
        /// 覆盖释放资源的方法，确保单例实例被清除。
        /// </summary>
        /// <param name="isDisposing">如果为 true，则释放托管资源。</param>
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // 清除当前单例实例
                Instance = null;
            }
        }
    }
}