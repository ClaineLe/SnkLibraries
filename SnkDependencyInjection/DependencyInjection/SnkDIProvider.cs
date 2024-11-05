using System;
using System.Collections.Generic;

namespace SnkDependencyInjection
{
    /// <summary>
    /// <para>����ע���ṩ�ߡ�</para>
    /// <para>ί�и� <see cref="SnkDIContainer"/> ʵ��</para>
    /// </summary>
    public sealed class SnkDIProvider : ISnkDIProvider
    {
        internal static ISnkDIProvider Instance { get; private set; }
        /// <summary>
        /// ��ʼ������ע���ṩ��
        /// </summary>
        /// <param name="options">����ע��ѡ��Ľӿڶ���</param>
        /// <returns>����ע���ṩ��ʵ���ӿڶ���</returns>
        public static ISnkDIProvider Initialize(ISnkDIOptions options = null)
        {
            if (Instance != null)
                return Instance;

            // create a new DI container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            var instance = new SnkDIProvider(options);
            
            // ReSharper restore ObjectCreationAsStatement
            return instance;
        }

        private readonly SnkDIContainer _provider;

        private SnkDIProvider(ISnkDIOptions options)
        {
            _provider = new SnkDIContainer(options);
        }

        /// <summary>
        /// ����Ƿ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ�������͡�</typeparam>
        /// <returns>������Խ��������� true�����򷵻� false��</returns>
        public bool CanResolve<T>() where T : class
        {
            return _provider.CanResolve<T>();
        }

        /// <summary>
        /// ����Ƿ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ�������͡�</param>
        /// <returns>������Խ��������� true�����򷵻� false��</returns>
        public bool CanResolve(Type type)
        {
            return _provider.CanResolve(type);
        }

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <param name="resolved">�������ʵ����</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        public bool TryResolve<T>(out T resolved) where T : class
        {
            return _provider.TryResolve(out resolved);
        }

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="resolved">�������ʵ����</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        public bool TryResolve(Type type, out object resolved)
        {
            return _provider.TryResolve(type, out resolved);
        }

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>�������ʵ����</returns>
        public T Resolve<T>() where T : class
        {
            return _provider.Resolve<T>();
        }

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>�������ʵ����</returns>
        public object Resolve(Type type)
        {
            return _provider.Resolve(type);
        }

        /// <summary>
        /// ��ȡָ�����͵ĵ���ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ��ȡ�����͡�</typeparam>
        /// <returns>����ʵ����</returns>
        public T GetSingleton<T>() where T : class
        {
            return _provider.GetSingleton<T>();
        }

        /// <summary>
        /// ��ȡָ�����͵ĵ���ʵ����
        /// </summary>
        /// <param name="type">Ҫ��ȡ�����͡�</param>
        /// <returns>����ʵ����</returns>
        public object GetSingleton(Type type)
        {
            return _provider.GetSingleton(type);
        }

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>������ʵ����</returns>
        public T Create<T>() where T : class
        {
            return _provider.Create<T>();
        }

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>������ʵ����</returns>
        public object Create(Type type)
        {
            return _provider.Create(type);
        }

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <typeparam name="TInterface">Դ���͡�</typeparam>
        /// <typeparam name="TToConstruct">Ŀ�����͡�</typeparam>
        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            _provider.RegisterType<TInterface, TToConstruct>();
        }

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="constructor">���캯��ί�С�</param>
        public void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class
        {
            _provider.RegisterType(constructor);
        }

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <param name="t">���͡�</param>
        /// <param name="constructor">���캯��ί�С�</param>
        public void RegisterType(Type t, Func<object> constructor)
        {
            _provider.RegisterType(t, constructor);
        }

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <param name="tFrom">Դ���͡�</param>
        /// <param name="tTo">Ŀ�����͡�</param>
        public void RegisterType(Type tFrom, Type tTo)
        {
            _provider.RegisterType(tFrom, tTo);
        }

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="theObject">����ʵ����</param>
        public void RegisterSingleton<TInterface>(TInterface theObject) where TInterface : class
        {
            _provider.RegisterSingleton(theObject);
        }

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <param name="tInterface">�ӿ����͡�</param>
        /// <param name="theObject">����ʵ����</param>
        public void RegisterSingleton(Type tInterface, object theObject)
        {
            _provider.RegisterSingleton(tInterface, theObject);
        }

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="theConstructor">���캯��ί�С�</param>
        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor) where TInterface : class
        {
            _provider.RegisterSingleton(theConstructor);
        }

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <param name="tInterface">�ӿ����͡�</param>
        /// <param name="theConstructor">���캯��ί�С�</param>
        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            _provider.RegisterSingleton(tInterface, theConstructor);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>() where T : class
        {
            return _provider.DIConstruct<T>((IDictionary<string, object>)null);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            return _provider.DIConstruct<T>(arguments);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>(params object[] arguments) where T : class
        {
            return _provider.DIConstruct<T>(arguments);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        public T DIConstruct<T>(object arguments) where T : class
        {
            return _provider.DIConstruct<T>(arguments);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <returns>�����ʵ����</returns>
        public object DIConstruct(Type type)
        {
            return _provider.DIConstruct(type, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        public object DIConstruct(Type type, IDictionary<string, object> arguments)
        {
            return _provider.DIConstruct(type, arguments);
        }
        
        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        public object DIConstruct(Type type, object arguments)
        {
            return _provider.DIConstruct(type, arguments);
        }

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        public object DIConstruct(Type type, params object[] arguments)
        {
            return _provider.DIConstruct(type, arguments);
        }

        /// <summary>
        /// ������н�����
        /// </summary>
        public void CleanAllResolvers()
        {
            _provider.CleanAllResolvers();
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns>������ʵ����</returns>
        public ISnkDIProvider CreateChildContainer()
        {
            return _provider.CreateChildContainer();
        }
    }
}