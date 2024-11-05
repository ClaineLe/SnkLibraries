using System;
using System.Collections.Generic;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// ��չ�����࣬���ڴ���������ע�������
    /// </summary>
    public static class SnkDIContainerExtensions
    {
        /// <summary>
        /// �������������������ڹ�����Ҫһ�������Ľ������ӿڶ���
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯���ĵ�һ�����������͡�</typeparam>
        /// <param name="provider">����ע�������ṩ�ߣ�������������</param>
        /// <param name="typedConstructor">һ���������������Ĺ��캯����</param>
        /// <returns>����һ���������ú������������ؽӿ����͵�ʵ����</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1>(
            this ISnkDIProvider provider,
            Func<TParameter1, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                return typedConstructor((TParameter1)parameter1);
            };
        }

        /// <summary>
        /// �������������������ڹ�����Ҫ���������Ľ������ӿڶ���
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯���ĵ�һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ĵڶ������������͡�</typeparam>
        /// <param name="provider">����ע�������ṩ�ߣ�������������</param>
        /// <param name="typedConstructor">һ���������������Ĺ��캯����</param>
        /// <returns>����һ���������ú������������ؽӿ����͵�ʵ����</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2);
            };
        }

        /// <summary>
        /// �������������������ڹ�����Ҫ���������Ľ������ӿڶ���
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯���ĵ�һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ĵڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯���ĵ��������������͡�</typeparam>
        /// <param name="provider">����ע�������ṩ�ߣ�������������</param>
        /// <param name="typedConstructor">һ���������������Ĺ��캯����</param>
        /// <returns>����һ���������ú������������ؽӿ����͵�ʵ����</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2, TParameter3>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TParameter3, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                provider.TryResolve(typeof(TParameter3), out var parameter3);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3);
            };
        }

        /// <summary>
        /// �������������������ڹ�����Ҫ�ĸ������Ľ������ӿڶ���
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯���ĵ�һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ĵڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯���ĵ��������������͡�</typeparam>
        /// <typeparam name="TParameter4">���캯���ĵ��ĸ����������͡�</typeparam>
        /// <param name="provider">����ע�������ṩ�ߣ�������������</param>
        /// <param name="typedConstructor">һ�������ĸ������Ĺ��캯����</param>
        /// <returns>����һ���������ú������������ؽӿ����͵�ʵ����</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2, TParameter3, TParameter4>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                provider.TryResolve(typeof(TParameter3), out var parameter3);
                provider.TryResolve(typeof(TParameter4), out var parameter4);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3, (TParameter4)parameter4);
            };
        }

        /// <summary>
        /// �������������������ڹ�����Ҫ��������Ľ������ӿڶ���
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯���ĵ�һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ĵڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯���ĵ��������������͡�</typeparam>
        /// <typeparam name="TParameter4">���캯���ĵ��ĸ����������͡�</typeparam>
        /// <typeparam name="TParameter5">���캯���ĵ�������������͡�</typeparam>
        /// <param name="provider">����ע�������ṩ�ߣ�������������</param>
        /// <param name="typedConstructor">һ��������������Ĺ��캯����</param>
        /// <returns>����һ���������ú������������ؽӿ����͵�ʵ����</returns>
        private static Func<TInterface> CreateResolver<TInterface, TParameter1, TParameter2, TParameter3, TParameter4, TParameter5>(
            this ISnkDIProvider provider,
            Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> typedConstructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
            where TParameter5 : class
        {
            return () =>
            {
                provider.TryResolve(typeof(TParameter1), out var parameter1);
                provider.TryResolve(typeof(TParameter2), out var parameter2);
                provider.TryResolve(typeof(TParameter3), out var parameter3);
                provider.TryResolve(typeof(TParameter4), out var parameter4);
                provider.TryResolve(typeof(TParameter5), out var parameter5);
                return typedConstructor((TParameter1)parameter1, (TParameter2)parameter2, (TParameter3)parameter3, (TParameter4)parameter4, (TParameter5)parameter5);
            };
        }

        /// <summary>
        /// ����һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TType">����ʵ�����͡�</typeparam>
        /// <param name="provider">����ע���ṩ�ߡ�</param>
        /// <returns>������ʵ����</returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>();
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// ʹ�ò�������һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TType">����ʵ�����͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="arguments">����ʵ�����������������</param>
        /// <returns></returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider, IDictionary<string, object> arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>(arguments);
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// ʹ�õ���������Ϊ��������һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="arguments">����ʵ������Ķ��������</param>
        /// <returns>������ʵ����</returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider, object arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>(arguments);
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// ʹ�ö����������һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="arguments">����ʵ������Ĳ������顣</param>
        /// <returns>������ʵ����</returns>
        public static TType ConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider, params object[] arguments)
            where TInterface : class
            where TType : class, TInterface
        {
            var instance = provider.DIConstruct<TType>(arguments);
            provider.RegisterSingleton<TInterface>(instance);
            return instance;
        }

        /// <summary>
        /// ����һ���������͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="type">Ҫʵ���������͡�</param>
        /// <returns>������ʵ����</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type)
        {
            var instance = provider.DIConstruct(type);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// ʹ�ò�������һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="type">Ҫʵ���������͡�</param>
        /// <param name="arguments">����ʵ�����������������</param>
        /// <returns>������ʵ����</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, IDictionary<string, object> arguments)
        {
            var instance = provider.DIConstruct(type, arguments);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// ʹ�õ���������Ϊ��������һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="type">Ҫʵ���������͡�</param>
        /// <param name="arguments">����ʵ������Ķ��������</param>
        /// <returns>������ʵ����</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, object arguments)
        {
            var instance = provider.DIConstruct(type, arguments);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// ʹ�ö����������һ�����͵�ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="type">Ҫʵ���������͡�</param>
        /// <param name="arguments">����ʵ������Ĳ������顣</param>
        /// <returns>������ʵ����</returns>
        public static object ConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, params object[] arguments)
        {
            var instance = provider.DIConstruct(type, arguments);
            provider.RegisterSingleton(type, instance);
            return instance;
        }

        /// <summary>
        /// �ӳٹ���һ�����͵�ʵ����ע��Ϊ����������һ�α�����ʱ�Ź��졣
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TType">����ʵ�����͡�</typeparam>
        /// <param name="provider">����ע���ṩ�ߡ�</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TType>(this ISnkDIProvider provider)
            where TInterface : class
            where TType : class, TInterface
        {
            provider.RegisterSingleton<TInterface>(() => provider.DIConstruct<TType>());
        }

        /// <summary>
        /// ʹ��ָ�����캯���ӳٹ�����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">����ʵ�����Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton<TInterface>(this ISnkDIProvider provider, Func<TInterface> constructor)
            where TInterface : class
        {
            provider.RegisterSingleton<TInterface>(constructor);
        }

        /// <summary>
        /// ʹ��ָ�����캯���ӳٹ���һ��ʵ����ע��Ϊ������
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="type">Ҫʵ���������͡�</param>
        /// <param name="constructor">����ʵ�����Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton(this ISnkDIProvider provider, Type type, Func<object> constructor)
        {
            provider.RegisterSingleton(type, constructor);
        }

        /// <summary>
        /// ʹ�ô�һ�������Ĺ��캯���ӳٹ�����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯�����������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">��һ�������Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1>(this ISnkDIProvider provider, Func<TParameter1, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// ʹ�ô����������Ĺ��캯���ӳٹ�����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">�����������Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// ʹ�ô����������Ĺ��캯���ӳٹ�����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯�����������������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">�����������Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2, TParameter3>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// ʹ�ô��ĸ������Ĺ��캯���ӳٹ�����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯�����������������͡�</typeparam>
        /// <typeparam name="TParameter4">���캯�����ĸ����������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">���ĸ������Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2, TParameter3, TParameter4>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// ʹ�ô���������Ĺ��캯���ӳٹ�����ע��Ϊ������
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯�����������������͡�</typeparam>
        /// <typeparam name="TParameter4">���캯�����ĸ����������͡�</typeparam>
        /// <typeparam name="TParameter5">���캯����������������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">����������Ĺ��캯����</param>
        public static void LazyConstructAndRegisterSingleton<TInterface, TParameter1, TParameter2, TParameter3, TParameter4, TParameter5>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
            where TParameter5 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterSingleton(resolver);
        }

        /// <summary>
        /// ע��һ�����ͣ�ʹ���������Ҫʱ�����졣
        /// </summary>
        /// <typeparam name="TType">Ҫע������͡�</typeparam>
        public static void RegisterType<TType>(this ISnkDIProvider provider)
            where TType : class
        {
            provider.RegisterType<TType, TType>();
        }

        /// <summary>
        /// ע��һ�����ͣ�ʹ���������Ҫʱ�����졣
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="tType">Ҫע������͡�</param>
        public static void RegisterType(this ISnkDIProvider provider, Type tType)
        {
            provider.RegisterType(tType, tType);
        }

        /// <summary>
        /// ʹ�ô�һ�������Ĺ��캯��ע��һ�����͡�
        /// </summary>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯�����������͡�</typeparam>
        /// <param name="constructor">�����������Ĺ��캯����</param>
        public static void RegisterType<TInterface, TParameter1>(this ISnkDIProvider provider, Func<TParameter1, TInterface> constructor)
           where TInterface : class
           where TParameter1 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// ʹ�ô����������Ĺ��캯��ע��һ�����͡�
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">�����������Ĺ��캯����</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// ʹ�ô����������Ĺ��캯��ע��һ�����͡�
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯�����������������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">�����������Ĺ��캯����</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2, TParameter3>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// ʹ�ô��ĸ������Ĺ��캯��ע��һ�����͡�
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯�����������������͡�</typeparam>
        /// <typeparam name="TParameter4">���캯�����ĸ����������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">���ĸ������Ĺ��캯����</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2, TParameter3, TParameter4>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }

        /// <summary>
        /// ʹ�ô���������Ĺ��캯��ע��һ�����͡�
        /// </summary>
        /// <typeparam name="TInterface">�ӿ����͡�</typeparam>
        /// <typeparam name="TParameter1">���캯����һ�����������͡�</typeparam>
        /// <typeparam name="TParameter2">���캯���ڶ������������͡�</typeparam>
        /// <typeparam name="TParameter3">���캯�����������������͡�</typeparam>
        /// <typeparam name="TParameter4">���캯�����ĸ����������͡�</typeparam>
        /// <typeparam name="TParameter5">���캯����������������͡�</typeparam>
        /// <param name="provider">����ע���ṩ����չ����</param>
        /// <param name="constructor">����������Ĺ��캯����</param>
        public static void RegisterType<TInterface, TParameter1, TParameter2, TParameter3, TParameter4, TParameter5>(this ISnkDIProvider provider, Func<TParameter1, TParameter2, TParameter3, TParameter4, TParameter5, TInterface> constructor)
            where TInterface : class
            where TParameter1 : class
            where TParameter2 : class
            where TParameter3 : class
            where TParameter4 : class
            where TParameter5 : class
        {
            var resolver = provider.CreateResolver(constructor);
            provider.RegisterType(resolver);
        }
    }
}
