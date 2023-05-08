using System;
using System.Data;

namespace CreateProductPlanOA
{
    //临时表
    public class Tempdt
    {
        /// <summary>
        /// 插入ytc_t_Cust100001_L时使用
        /// </summary>
        /// <returns></returns>
        public DataTable Insertdtytc_L()
        {
            var dt = new DataTable();
            for (var i = 0; i < 4; i++)
            {
                var dc = new DataColumn();
                switch (i)
                {
                    case 0:
                        dc.ColumnName = "FPKID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 1:
                        dc.ColumnName = "FID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 2:
                        dc.ColumnName = "FLocaleID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 3:
                        dc.ColumnName = "F_YTC_MULLANGTEXT";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }
    }
}
