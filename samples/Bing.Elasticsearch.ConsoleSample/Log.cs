using System;

namespace Bing.Elasticsearch.ConsoleSample
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(string msg) => Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {msg}");
    }
}
