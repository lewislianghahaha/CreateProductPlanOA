using System;
using System.Data;

namespace CreateProductPlanOA
{
    //临时表
    public class Tempdt
    {
        /// <summary>
        /// 更新formtable_main_16表各字段时使用
        /// </summary>
        /// <returns></returns>
        public DataTable UpdateDtTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 9; i++)
            {
                var dc = new DataColumn();
                switch (i)
                {
                    //是否为新客户
                    case 0:
                        dc.ColumnName = "sfwxkh";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //客户名称
                    case 1:
                        dc.ColumnName = "khdm";
                        dc.DataType = Type.GetType("System.String"); 
                        break;
                    //客户代码
                    case 2:
                        dc.ColumnName = "yykhdm";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //产品性质
                    case 3:
                        dc.ColumnName = "cpxz";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //是否为工业涂料事业部客户
                    case 4:
                        dc.ColumnName = "sfwgytlsybkh";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //申请人
                    case 5:
                        dc.ColumnName = "sqr";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //申部日期
                    case 6:
                        dc.ColumnName = "sqrq";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //部门
                    case 7:
                        dc.ColumnName = "bm";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //职务
                    case 8:
                        dc.ColumnName = "zw";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 插入formtable_main_16_dt1表使用
        /// </summary>
        /// <returns></returns>
        public DataTable InsertOaDetailTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 10; i++)
            {
                var dc = new DataColumn();
                switch (i)
                {
                    //自增主键
                    case 0:
                        dc.ColumnName = "id";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //主表主键
                    case 1:
                        dc.ColumnName = "mainid";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //产品名称
                    case 2:
                        dc.ColumnName = "cpmc";
                        dc.DataType = Type.GetType("System.String"); 
                        break;
                    //品牌
                    case 3:
                        dc.ColumnName = "pp";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //规格型号
                    case 4:
                        dc.ColumnName = "gg";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //[数量(罐)]
                    case 5:
                        dc.ColumnName = "slg";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //包装要求
                    case 6:
                        dc.ColumnName = "bzyq";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //喷码/帖标
                    case 7:
                        dc.ColumnName = "pmtb";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //产品代码
                    case 8:
                        dc.ColumnName = "cpdm";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //工业项目料号
                    case 9:
                        dc.ColumnName = "gyxmlh";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }


    }
}
