# Bing.Elasticsearch 发行说明

## [1.0.0](https://www.nuget.org/packages/Bing.Elasticsearch/1.0.0)

### ✨ 新功能

* 🧩 初始化 ES 查询子句结构（`And`, `Or`, `Like`, `NotEqual`, `Range` 等）
* 🆕 支持分页查询、ScrollAll、WhereIfNotEmpty 等扩展方法
* ➕ 支持 `OrderBy`、模糊查询 `Keyword` 追加、泛型查询能力
* 🔧 新增 `ConnectionSettings` 外部配置访问接口

---

### 🎨 代码重构

* 重构范围查询方法（`refactor: 范围方法`）
* 重构条件构造逻辑，支持多种组合条件链式调用
* 重构项目结构，调整测试项目引用、移除 `dependency.props`，对齐统一测试配置

---

### 🛠️ 修复 & 改进

* 修复默认走 `bing_es` 索引的问题（`FindByIdsAsync`）
* 修复模糊查询关键词追加逻辑
* 修复仓储分页兼容性问题

---

### ✅ 单元测试 & 文档

* 完善测试项目结构，初始化统一测试基类与配置
* 添加项目 README.md、自述文档
* 发布多个 Preview 版本用于验证（共 6 个 Preview）
