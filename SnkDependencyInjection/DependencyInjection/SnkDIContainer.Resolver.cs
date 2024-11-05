using SnkFramework.Exceptions;
using System;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// 用于依赖注入的容器类。
    /// </summary>
    public sealed partial class SnkDIContainer
    {
        /// <summary>
        /// 用于表示解析器类型的枚举。
        /// </summary>
        private enum ResolverType
        {
            /// <summary>
            /// 每次解析时动态生成实例
            /// </summary>
            DynamicPerResolve,

            /// <summary>
            /// 单例模式，只生成一个实例
            /// </summary>
            Singleton,

            /// <summary>
            /// 未知的解析类型
            /// </summary>
            Unknown
        }

        /// <summary>
        /// 定义用于解析依赖项的接口。
        /// </summary>
        private interface IResolver
        {
            /// <summary>
            /// 解析并返回对象实例。
            /// </summary>
            /// <returns>解析的对象实例。</returns>
            object Resolve();

            /// <summary>
            /// 获取解析器的类型。
            /// </summary>
            ResolverType ResolveType { get; }

            /// <summary>
            /// 设置通用类型参数（对于不支持的解析器会抛出异常）。
            /// </summary>
            /// <param name="genericTypeParameters">泛型类型参数数组。</param>
            void SetGenericTypeParameters(Type[] genericTypeParameters);
        }

        /// <summary>
        /// 用于按需动态构造实例的解析器。
        /// </summary>
        private sealed class ConstructingResolver : IResolver
        {
            private readonly Type _type;
            private readonly ISnkDIProvider _parent;

            /// <summary>
            /// 初始化 <see cref="ConstructingResolver"/> 类的新实例。
            /// </summary>
            /// <param name="type">要解析的类型。</param>
            /// <param name="parent">依赖注入提供者接口。</param>
            public ConstructingResolver(Type type, ISnkDIProvider parent)
            {
                _type = type;
                _parent = parent;
            }

            /// <summary>
            /// 解析并返回类型的实例。
            /// </summary>
            /// <returns>解析的对象实例。</returns>
            public object Resolve()
            {
                return _parent.DIConstruct(_type, (object)null);
            }

            /// <summary>
            /// 不支持设置泛型类型参数的方法，会抛出异常。
            /// </summary>
            /// <param name="genericTypeParameters">泛型类型参数数组。</param>
            /// <exception cref="InvalidOperationException">总是抛出此异常。</exception>
            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            /// <summary>
            /// 获取解析器类型为 <see cref="ResolverType.DynamicPerResolve"/>。
            /// </summary>
            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        /// <summary>
        /// 使用提供的委托函数生成对象的解析器。
        /// </summary>
        private sealed class FuncConstructingResolver : IResolver
        {
            private readonly Func<object> _constructor;

            /// <summary>
            /// 初始化 <see cref="FuncConstructingResolver"/> 类的新实例。
            /// </summary>
            /// <param name="constructor">创建对象的委托函数。</param>
            public FuncConstructingResolver(Func<object> constructor)
            {
                _constructor = constructor;
            }

            /// <summary>
            /// 解析并通过委托返回对象实例。
            /// </summary>
            /// <returns>解析的对象实例。</returns>
            public object Resolve()
            {
                return _constructor();
            }

            /// <summary>
            /// 不支持设置泛型类型参数的方法，会抛出异常。
            /// </summary>
            /// <param name="genericTypeParameters">泛型类型参数数组。</param>
            /// <exception cref="InvalidOperationException">总是抛出此异常。</exception>
            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            /// <summary>
            /// 获取解析器类型为 <see cref="ResolverType.DynamicPerResolve"/>。
            /// </summary>
            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        /// <summary>
        /// 单例解析器，用于返回已经存在的对象。
        /// </summary>
        private sealed class SingletonResolver : IResolver
        {
            private readonly object _theObject;

            /// <summary>
            /// 初始化 <see cref="SingletonResolver"/> 类的新实例。
            /// </summary>
            /// <param name="theObject">本次解析中返回的单例对象。</param>
            public SingletonResolver(object theObject)
            {
                _theObject = theObject;
            }

            /// <summary>
            /// 解析并返回单例对象实例。
            /// </summary>
            /// <returns>已存在的对象实例。</returns>
            public object Resolve()
            {
                return _theObject;
            }

            /// <summary>
            /// 不支持设置泛型类型参数的方法，会抛出异常。
            /// </summary>
            /// <param name="genericTypeParameters">泛型类型参数数组。</param>
            /// <exception cref="InvalidOperationException">总是抛出此异常。</exception>
            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            /// <summary>
            /// 获取解析器类型为 <see cref="ResolverType.Singleton"/>。
            /// </summary>
            public ResolverType ResolveType => ResolverType.Singleton;
        }

        /// <summary>
        /// 构造单例对象的解析器，只有在首次请求对象时才会进行构造。
        /// </summary>
        private sealed class ConstructingSingletonResolver : IResolver
        {
            private readonly object _syncObject = new object();
            private readonly Func<object> _constructor;
            private object _theObject;

            /// <summary>
            /// 初始化 <see cref="ConstructingSingletonResolver"/> 类的新实例。
            /// </summary>
            /// <param name="theConstructor">用于实例化对象的委托函数。</param>
            public ConstructingSingletonResolver(Func<object> theConstructor)
            {
                _constructor = theConstructor;
            }

            /// <summary>
            /// 解析并返回单例对象实例，使用双重检查锁定确保线程安全。
            /// </summary>
            /// <returns>解析的对象实例。</returns>
            public object Resolve()
            {
                if (_theObject != null)
                    return _theObject;

                object constructed;
                lock (_syncObject)
                {
                    if (_theObject != null)
                        return _theObject;

                    constructed = _constructor();
                }
                _theObject = constructed;

                return _theObject;
            }

            /// <summary>
            /// 不支持设置泛型类型参数的方法，会抛出异常。
            /// </summary>
            /// <param name="genericTypeParameters">泛型类型参数数组。</param>
            /// <exception cref="InvalidOperationException">总是抛出此异常。</exception>
            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            /// <summary>
            /// 获取解析器类型为 <see cref="ResolverType.Singleton"/>。
            /// </summary>
            public ResolverType ResolveType => ResolverType.Singleton;
        }

        /// <summary>
        /// 用于构造开放泛型类型实例的解析器。
        /// </summary>
        private sealed class ConstructingOpenGenericResolver : IResolver
        {
            private readonly Type _genericTypeDefinition;
            private readonly ISnkDIProvider _parent;
            private Type[] _genericTypeParameters;

            /// <summary>
            /// 初始化 <see cref="ConstructingOpenGenericResolver"/> 类的新实例。
            /// </summary>
            /// <param name="genericTypeDefinition">要实例化的泛型类型定义。</param>
            /// <param name="parent">依赖注入提供者接口。</param>
            public ConstructingOpenGenericResolver(Type genericTypeDefinition, ISnkDIProvider parent)
            {
                _genericTypeDefinition = genericTypeDefinition;
                _parent = parent;
            }

            /// <summary>
            /// 设置开放泛型类型的具体类型参数。
            /// </summary>
            /// <param name="genericTypeParameters">泛型类型参数数组。</param>
            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                _genericTypeParameters = genericTypeParameters;
            }

            /// <summary>
            /// 解析并返回开放泛型类型的实例。
            /// </summary>
            /// <returns>解析的对象实例。</returns>
            /// <exception cref="SnkDIResolveException">如果未提供泛型类型参数则抛出此异常。</exception>
            public object Resolve()
            {
                if (_genericTypeParameters == null)
                {
                    throw new SnkDIResolveException("No Generic Type Parameters provided for Type: {0}", _genericTypeDefinition.FullName);
                }

                return _parent.DIConstruct(_genericTypeDefinition.MakeGenericType(_genericTypeParameters), (object)null);
            }

            /// <summary>
            /// 获取解析器类型为 <see cref="ResolverType.DynamicPerResolve"/>。
            /// </summary>
            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }
    }
}