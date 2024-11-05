using SnkFramework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// һ���������ͻ����࣬���ڻ����ض����͵����ࡣ
    /// </summary>
    /// <typeparam name="TType">Ҫ��������͵Ļ����ӿڡ�</typeparam>
    public class SnkTypeCache<TType> : ISnkTypeCache
    {
        /// <summary>
        /// ���������͵��������Ƶ�Сд��ʽΪ���������ֵ䡣
        /// </summary>
        public Dictionary<string, Type> LowerCaseFullNameCache { get; } = new Dictionary<string, Type>();

        /// <summary>
        /// ���������͵���������Ϊ���������ֵ䡣
        /// </summary>
        public Dictionary<string, Type> FullNameCache { get; } = new Dictionary<string, Type>();

        /// <summary>
        /// ���������͵�����Ϊ���������ֵ䡣
        /// </summary>
        public Dictionary<string, Type> NameCache { get; } = new Dictionary<string, Type>();

        /// <summary>
        /// ��¼�Ѿ�������ĳ��򼯣������ظ����档
        /// </summary>
        public Dictionary<Assembly, bool> CachedAssemblies { get; } = new Dictionary<Assembly, bool>();

        /// <summary>
        /// ��ָ���ĳ����е�������ӵ������С�
        /// </summary>
        /// <param name="assembly">Ҫɨ��ĳ��򼯡�</param>
        public void AddAssembly(Assembly assembly)
        {
            try
            {
                // ��������Ѿ����棬ֱ�ӷ��ء�
                if (CachedAssemblies.ContainsKey(assembly))
                    return;

                var viewType = typeof(TType);
                // ���ҳ��򼯵����������У��̳����ض������ʵ���ض��ӿڵ����͡�
                var query = assembly.DefinedTypes.Where(ti => ti.IsSubclassOf(viewType)).Select(ti => ti.AsType());

                // ���ҵ���������Ϣ��ӵ������С�
                foreach (var type in query)
                {
                    var fullName = type.FullName;
                    if (!string.IsNullOrEmpty(fullName))
                    {
                        FullNameCache[fullName] = type;
                        LowerCaseFullNameCache[fullName.ToLowerInvariant()] = type;
                    }

                    var name = type.Name;
                    if (!string.IsNullOrEmpty(name))
                        NameCache[name] = type;
                }

                // �����򼯱��Ϊ�ѻ��档
                CachedAssemblies[assembly] = true;
            }
            catch (ReflectionTypeLoadException e)
            {
                // ��������쳣����¼������־������Ӱ��������ִ�С�
                SnkLogHost.Default?.Warning(e, $"ReflectionTypeLoadException masked during loading of {assembly.FullName}");
            }
        }
    }
}