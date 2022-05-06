using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bing.Elasticsearch.Internals;
using Bing.Elasticsearch.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace Bing.Elasticsearch.Repositories
{
    /// <summary>
    /// ES只读仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class EsReadOnlyRepositoryBase<TEntity> : IEsSearchableReadOnlyRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// 延迟加载的ES客户端
        /// </summary>
        protected readonly Lazy<IElasticClient> _lazyClient;

        /// <summary>
        /// ES客户端
        /// </summary>
        protected IElasticClient Client => _lazyClient.Value;

        /// <summary>
        /// ES上下文
        /// </summary>
        protected IElasticsearchContext Context { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        protected virtual string IndexName { get; set; }

        /// <summary>
        /// ES选项配置
        /// </summary>
        protected ElasticsearchOptions Options { get; set; }

        /// <summary>
        /// 初始化一个<see cref="EsReadOnlyRepositoryBase{TEntity}"/>类型的实例
        /// </summary>
        /// <param name="context">ES上下文</param>
        /// <param name="options">ES选项配置</param>
        protected EsReadOnlyRepositoryBase(IElasticsearchContext context, IOptions<ElasticsearchOptions> options)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Options = options.Value;
            IndexName = Helper.SafeIndexName<TEntity>(IndexName);
            _lazyClient = new Lazy<IElasticClient>(() => Context.GetClient());
            _logger = context.LoggerFactory.CreateLogger(GetType());
        }

        /// <summary>
        /// 通过标识查找
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            return await Context.FindByIdAsync<TEntity>(id, indexName, cancellationToken);
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        public virtual Task<IEnumerable<TEntity>> FindByIdsAsync(params string[] ids) => FindByIdsAsync((IEnumerable<string>)ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        public virtual Task<IEnumerable<TEntity>> FindByIdsAsync(params long[] ids) => FindByIdsAsync((IEnumerable<long>)ids);

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            return await Context.FindByIdsAsync<TEntity>(ids, indexName, cancellationToken);
        }

        /// <summary>
        /// 通过标识集合查找
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
        {
            var indexName = Helper.SafeIndexName<TEntity>(IndexName);
            return await Context.FindByIdsAsync<TEntity>(ids, indexName, cancellationToken);
        }
    }
}
