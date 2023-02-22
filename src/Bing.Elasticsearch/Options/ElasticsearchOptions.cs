﻿using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;

namespace Bing.Elasticsearch.Options;

/// <summary>
/// ES选项配置
/// </summary>
public class ElasticsearchOptions
{
    /// <summary>
    /// ES服务地址
    /// </summary>
    public List<string> Urls { get; set; }

    /// <summary>
    /// 默认索引名称
    /// </summary>
    public string DefaultIndex { get; set; } = "bing_es";

    /// <summary>
    /// ES版本(默认>=7.0)
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 是否开启链接调试信息。用于显示请求体
    /// </summary>
    public bool EnableDebugInfo { get; set; } = true;

    /// <summary>
    /// 抛出异常。默认：false，错误信息在每个操作的response中
    /// </summary>
    public bool ThrowExceptions { get; set; } = false;

    /// <summary>
    /// 是否禁用ping。禁用ping，第一次使用节点或使用被标记死亡的节点进行ping
    /// </summary>
    public bool DisablePing { get; set; } = true;

    /// <summary>
    /// 连接池类型
    /// </summary>
    public ElasticsearchConnectionPoolType PoolType { get; set; } = ElasticsearchConnectionPoolType.Static;

    /// <summary>
    /// 主分片数量
    /// </summary>
    public int NumberOfShards { get; set; } = 1;

    /// <summary>
    /// 每个主分片的副分片数量
    /// </summary>
    public int NumberOfReplicas { get; set; } = 1;

    /// <summary>
    /// 索引前缀（防止索引名称重复）
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// 是否检查索引
    /// </summary>
    public bool CheckIndex { get; set; } = true;

    /// <summary>
    /// 请求完成操作
    /// </summary>
    public Action<IApiCallDetails, ILogger> RequestCompletedAct { get; set; } = (apiCallDetails, logger) =>
    {
        // 请求完成 返回apiCallDetails
        if (apiCallDetails.Success)
            logger.LogRequest(apiCallDetails);
        else
            logger.LogErrorRequest(apiCallDetails, "RequestCompletedAct");
    };

    /// <summary>
    /// NEST连接设置操作
    /// </summary>
    public Action<ConnectionSettings> ConnectionSettingsAct { get; set; }

    /// <summary>
    /// 兼容版本(>=7.0)
    /// </summary>
    public bool IsCompatibleVersion()
    {
        if (string.IsNullOrEmpty(Version))
            return true;
        try
        {
            var major = Version.Split('.').FirstOrDefault();
            return major != null && int.Parse(major) >= 7;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 验证配置是否有效
    /// </summary>
    /// <param name="options">ES选项配置</param>
    public static void Validate(ElasticsearchOptions options)
    {
        if (options == null)
            throw new ArgumentException("ElasticsearchOptions不能为null", nameof(options));
        if (options.Urls == null || !options.Urls.Any())
            throw new ArgumentException("未指定ElasticSearch的Urls", nameof(options.Urls));

        try
        {
            options.Urls.Select(x => new Uri(x));
        }
        catch (UriFormatException uriEx)
        {
            throw new ArgumentException("无效的Urls", uriEx);
        }

        if (string.IsNullOrEmpty(options.DefaultIndex))
            throw new ArgumentException("未指定ElasticSearch的DefaultIndex", nameof(options.DefaultIndex));
    }
}