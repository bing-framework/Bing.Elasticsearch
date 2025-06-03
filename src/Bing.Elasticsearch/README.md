### Bing.Elasticsearch

- 一个 `Elasticsearch` 驱动的服务包，方便使用 `Elasticsearch` 文档数据库。

#### 使用

- Nuget 安装 `Bing.Elasticsearch` 。
- 推荐同时安装 `Bing.Extensions.Elasticsearch` 包，添加了对 `分页查询`、`滚动查询` 的支持。

##### 方法 1. 使用默认依赖注入方式
```csharp
var builder = WebApplication.CreateBuilder(args);

var provider = builder.Services.BuildServiceProviderFromFactory();
// 添加 Ealsticsearch 文档数据库服务
builder.Services.AddElasticsearch(x => { });
```