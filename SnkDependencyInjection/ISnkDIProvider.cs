using System;
using System.Collections.Generic;

namespace SnkDependencyInjection
{
    /// <summary>
    /// ��������ע���ṩ�ߵĽӿڡ�
    /// </summary>
    public interface ISnkDIProvider
    {
        /// <summary>
        /// ����Ƿ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ�������͡�</typeparam>
        /// <returns>������Խ��������� true�����򷵻� false��</returns>
        bool CanResolve<T>() where T : class;

        /// <summary>
        /// ����Ƿ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ�������͡�</param>
        /// <returns>������Խ��������� true�����򷵻� false��</returns>
        bool CanResolve(Type type);

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>�������ʵ����</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>�������ʵ����</returns>
        object Resolve(Type type);

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <param name="resolved">�������ʵ����</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        bool TryResolve<T>(out T resolved) where T : class;

        /// <summary>
        /// ���Խ���ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <param name="resolved">�������ʵ����</param>
        /// <returns>��������ɹ������� true�����򷵻� false��</returns>
        bool TryResolve(Type type, out object resolved);

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ���������͡�</typeparam>
        /// <returns>������ʵ����</returns>
        T Create<T>() where T : class;

        /// <summary>
        /// ����ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ���������͡�</param>
        /// <returns>������ʵ����</returns>
        object Create(Type type);

        /// <summary>
        /// ��ȡָ�����͵ĵ���ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ��ȡ�����͡�</typeparam>
        /// <returns>����ʵ����</returns>
        T GetSingleton<T>() where T : class;

        /// <summary>
        /// ��ȡָ�����͵ĵ���ʵ����
        /// </summary>
        /// <param name="type">Ҫ��ȡ�����͡�</param>
        /// <returns>����ʵ����</returns>
        object GetSingleton(Type type);

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <typeparam name="TFrom">Դ���͡�</typeparam>
        /// <typeparam name="TTo">Ŀ�����͡�</typeparam>
        void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="constructor">���캯��ί�С�</param>
        void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class;

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <param name="t">���͡�</param>
        /// <param name="constructor">���캯��ί�С�</param>
        void RegisterType(Type t, Func<object> constructor);

        /// <summary>
        /// ע������ӳ�䡣
        /// </summary>
        /// <param name="tFrom">Դ���͡�</param>
        /// <param name="tTo">Ŀ�����͡�</param>
        void RegisterType(Type tFrom, Type tTo);

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="theObject">����ʵ����</param>
        void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class;

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <param name="tInterface">�ӿ����͡�</param>
        /// <param name="theObject">����ʵ����</param>
        void RegisterSingleton(Type tInterface, object theObject);

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <param name="theConstructor">���캯��ί�С�</param>
        void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;

        /// <summary>
        /// ע�ᵥ��ʵ����
        /// </summary>
        /// <param name="tInterface">�ӿ����͡�</param>
        /// <param name="theConstructor">���캯��ί�С�</param>
        void RegisterSingleton(Type tInterface, Func<object> theConstructor);

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <returns>�����ʵ����</returns>
        T DIConstruct<T>()
            where T : class;

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        T DIConstruct<T>(IDictionary<string, object> arguments)
            where T : class;

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        T DIConstruct<T>(object arguments)
            where T : class;

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <typeparam name="T">Ҫ��������͡�</typeparam>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        T DIConstruct<T>(params object[] arguments)
            where T : class;

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <returns>�����ʵ����</returns>
        object DIConstruct(Type type);

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        object DIConstruct(Type type, IDictionary<string, object> arguments);

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        object DIConstruct(Type type, object arguments);

        /// <summary>
        /// ʹ������ע�빹��ָ�����͵�ʵ���������ݲ�����
        /// </summary>
        /// <param name="type">Ҫ��������͡�</param>
        /// <param name="arguments">������</param>
        /// <returns>�����ʵ����</returns>
        object DIConstruct(Type type, params object[] arguments);

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns>������ʵ����</returns>
        ISnkDIProvider CreateChildContainer();
    }
}