﻿using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Bing.Reflection;
using Nest;

namespace Bing.Elasticsearch.WinformSample
{
    public partial class Form1 : Form
    {
        private readonly IElasticsearchContext _context;

        private readonly IElasticClient _client;

        /// <summary>
        /// 自适应窗体
        /// </summary>
        private AutoSizeForm _asc = new AutoSizeForm();

        public Form1(IElasticsearchContext context)
        {
            _context = context;
            _client = _context.GetClient();
            InitializeComponent();
            DoubleBufferedDataGirdView(dgvTable, true);
            dgvTable.ReadOnly = true;
            dgvTable.VirtualMode = true;
            //dgvTable.RowCount = 10;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            var sql = tbSql.Text;
            var querySw = Stopwatch.StartNew();
            var result = await _client.Sql.QueryAsync(x => x.Query(sql).Format("json"));
            querySw.Stop();
            if (!result.IsValid)
            {
                MessageBox.Show(result.ServerError.Error.Reason);
                return;
            }
            var renderSw = Stopwatch.StartNew();
            var table = CreateDataTable(result);
            dgvTable.DataSource = table;
            renderSw.Stop();

            sw.Stop();
            lblQueryTime.Text = $"查询耗时：{querySw.Elapsed.TotalMilliseconds,7:n0}ms";
            lblRenderTime.Text = $"渲染耗时：{renderSw.Elapsed.TotalMilliseconds,7:n0}ms";
            lblTotalTime.Text = $"总耗时：{sw.Elapsed.TotalMilliseconds,7:n0}ms";
        }

        private static DataTable CreateDataTable(QuerySqlResponse resp)
        {
            DataTable dt = new DataTable();
            // 初始化列名
            dt.Columns.AddRange(resp.Columns.Select(x => new DataColumn(x.Name, GetType(x.Type))).ToArray());
            foreach (var rowItem in resp.Rows)
            {
                var row = dt.NewRow();
                var columnIndex = 0;
                foreach (var columnItem in rowItem)
                {
                    row[columnIndex] = columnItem == null ? null : columnItem.As(dt.Columns[columnIndex].DataType);
                    columnIndex++;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        private static Type GetType(string type)
        {
            switch (type)
            {
                case "boolean":
                    return TypeClass.BooleanClazz;
                case "byte":
                    return TypeClass.ByteClazz;
                case "short":
                    return TypeClass.ShortClazz;
                case "integer":
                    return TypeClass.Int32Clazz;
                case "long":
                    return TypeClass.LongClazz;
                case "double":
                case "scaled_float":
                    return TypeClass.DoubleClazz;
                case "float":
                case "half_float":
                    return TypeClass.FloatClazz;
                case "string":
                case "keyword":
                case "text":
                case "ip":
                    return TypeClass.StringClazz;
                case "datetime":
                    return TypeClass.DateTimeClazz;
                case "binary":
                    return TypeClass.ByteArrayClazz;
                case "object":
                case "nested":
                    return TypeClass.ObjectClazz;
                default:
                    return TypeClass.StringClazz;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 为窗体添加Load事件，并在其方法Form1_Load中，调用类的初始化方法，记录窗体和其控件的初始位置和大小  
            _asc.ControllInitializeSize(this);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // 窗体添加SizeChanged事件，并在其方法Form1_SizeChanged中，调用类的自适应方法，完成自适应
            _asc.ControlAutoSize(this);
        }

        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedDataGirdView(DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
    }
}
