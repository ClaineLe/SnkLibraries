using SnkFramework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// 一个泛型类型缓存类，用于缓存特定类型的子类。
    /// </summary>
    /// <typeparam name="TType">要缓存的类型的基类或接口。</typeparam>
    public class SnkTypeCache<TType> : ISnkTypeCache
    {
        /// <summary>
        /// 缓存以类型的完整名称的小写形式为键的类型字典。
        /// </summary>
        public Dictionary<string, Type> LowerCaseFullNameCache { get; } = new Dictionary<string, Type>();

        /// <summary>
        /// 缓存以类型的完整名称为键的类型字典。
        /// </summary>
        public Dictionary<string, Type> FullNameCache { get; } = new Dictionary<string, Type>();

        /// <summary>
        /// 缓存以类型的名称为键的类型字典。
        /// </summary>
        public Dictionary<string, Type> NameCache { get; } = new Dictionary<string, Type>();

        /// <summary>
        /// 记录已经缓存过的程序集，避免重复缓存。
        /// </summary>
        public Dictionary<Assembly, bool> CachedAssemblies { get; } = new Dictionary<Assembly, bool>();

        /// <summary>
        /// 将指定的程序集中的类型添加到缓存中。
        /// </summary>
        /// <param name="assembly">要扫描的程序集。</param>
        public void AddAssembly(Assembly assembly)
        {
            try
            {
                // 如果程序集已经缓存，直接返回。
                if (CachedAssemblies.ContainsKey(assembly))
                    return;

                var viewType = typeof(TType);
                // 查找程序集的所有类型中，继承自特定基类或实现特定接口的类型。
                var query = assembly.DefinedTypes.Where(ti => ti.IsSubclassOf(viewType)).Select(ti => ti.AsType());

                // 将找到的类型信息添加到缓存中。
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

                // 将程序集标记为已缓存。
                CachedAssemblies[assembly] = true;
            }
            catch (ReflectionTypeLoadException e)
            {
                // 捕获加载异常并记录警告日志，而不影响程序继续执行。
                SnkLogHost.Default?.Warning(e, $"ReflectionTypeLoadException masked during loading of {assembly.FullName}");
            }
        }
    }
}