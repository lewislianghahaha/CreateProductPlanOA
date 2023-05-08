using System;
using System.Data;
using System.Data.SqlClient;

namespace CreateProductPlanOA
{
    public class SearchDt
    {
        ConnDb conDb = new ConnDb();
        SqlList sqlList = new SqlList();

        /// <summary>
        /// 根据SQL语句查询得出对应的DT
        /// </summary>
        /// <param name="sqlscript"></param>
        /// <returns></returns>
        private DataTable UseSqlSearchIntoDt(string sqlscript)
        {
            var resultdt = new DataTable();

            try
            {
                var sqlDataAdapter = new SqlDataAdapter(sqlscript, conDb.GetK3CloudConn());
                sqlDataAdapter.Fill(resultdt);
            }
            catch (Exception)
            {
                resultdt.Rows.Clear();
                resultdt.Columns.Clear();
            }

            return resultdt;
        }

        /// <summary>
        /// 按照指定的SQL语句执行记录并返回执行结果（true 或 false）
        /// 作用:更新及删除
        /// </summary>
        /// <param name="sqlscript"></param>
        /// <returns></returns>
        public bool Generdt(string sqlscript)
        {
            var result = true;
            try
            {
                var sqlcon = conDb.GetK3CloudConn();
                using (sqlcon)
                {
                    sqlcon.Open();
                    var sqlCommand = new SqlCommand(sqlscript, sqlcon);
                    sqlCommand.ExecuteNonQuery();
                    sqlcon.Close();
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }



    }
}
