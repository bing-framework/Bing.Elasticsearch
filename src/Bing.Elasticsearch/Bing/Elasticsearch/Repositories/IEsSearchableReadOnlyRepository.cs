namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// ES可搜索的只读仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEsSearchableReadOnlyRepository<TEntity> : IEsReadOnlyRepository<TEntity>
        where TEntity : class
    {
    }
}
