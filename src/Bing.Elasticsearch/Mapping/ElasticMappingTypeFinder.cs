using System;
using System.Linq;
using Bing.Extensions;
using Bing.Finders;
using Bing.Reflection;

namespace Bing.Elasticsearch.Mapping;

/// <summary>
/// ES映射 类型查找器
/// </summary>
public class ElasticMappingTypeFinder : FinderBase<Type>, IElasticMappingTypeFinder
{
    /// <summary>
    /// 所有程序集查找器
    /// </summary>
    private readonly IAllAssemblyFinder _allAssemblyFinder;

    /// <summary>
    /// 初始化一个<see cref="ElasticMappingTypeFinder"/>类型的实例
    /// </summary>
    /// <param name="allAssemblyFinder">所有程序集查找器</param>
    public ElasticMappingTypeFinder(IAllAssemblyFinder allAssemblyFinder) => _allAssemblyFinder =
        allAssemblyFinder ?? throw new ArgumentNullException(nameof(allAssemblyFinder));

    /// <inheritdoc />
    protected override Type[] FindAllItems()
    {
        var assemblies = _allAssemblyFinder.FindAll(true);
        var types = assemblies
            .SelectMany(assembly => assembly.GetTypes().Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                !type.IsInterface &&
                typeof(IElasticMapping<>).IsGenericAssignableFrom(type)))
            .ToArray();
        return types;
    }
}