using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace CreateProductPlanOA
{
    public class SearchDt
    {
        ConnDb conDb = new ConnDb();
        SqlList sqlList = new SqlList();

        /// <summary>
        /// 根据SQL语句查询得出对应的DT
        /// </summary>
        /// <param name="sqlscript">各SQL语句</param>
        /// <param name="conid">0:连接K3 1:连接OA</param>
        /// <returns></returns>
        private DataTable UseSqlSearchIntoDt(int conid, string sqlscript)
        {
            var resultdt = new DataTable();

            try
            {
                var sqlDataAdapter = new SqlDataAdapter(sqlscript, conDb.GetK3CloudConn(conid));
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
                var sqlcon = conDb.GetK3CloudConn(1);
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

        /// <summary>
        /// 根据用户名称获取OA-用户ID及部门ID信息
        /// OA使用
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataTable SearchOaUserInfo(string username)
        {
            var dt = UseSqlSearchIntoDt(1, sqlList.SearchOaUserInfo(username));
            return dt;
        }

        /// <summary>
        /// 获取K3相关销售订单对应客户信息
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public DataTable SearchK3CustomerCode(string pid)
        {
            var dt = UseSqlSearchIntoDt(0, sqlList.SearchK3SalesOrderHeadRecord(pid));
            return dt;
        }

        /// <summary>
        /// 获取K3相关销售订单明细信息
        /// </summary>
        /// <param name="entryid"></param>
        /// <returns></returns>
        public DataTable SearchK3OrderDetailRecord(string entryid)
        {
            var dt = UseSqlSearchIntoDt(0, sqlList.SearchK3SalesOderDeatilRecord(entryid));
            return dt;
        }

        /// <summary>
        /// 根据K3客户编码查找OA-客户档案表对应的ID值
        /// </summary>
        /// <returns></returns>
        public string SearchOaCustomerRecord(string k3Customercode)
        {
            var result = Convert.ToString(UseSqlSearchIntoDt(1,sqlList.SearchOaCustomerRecord(k3Customercode)).Rows[0][0]);
            return result;
        }

        /// <summary>
        ///  根据API成功返回的requestid查询对应的主表ID值,在最后的明细表更新时使用
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        public string SearchOaRecord(string requestid)
        {
            var mainid = Convert.ToString(UseSqlSearchIntoDt(1, sqlList.SearchOaRecord(requestid)).Rows[0][0]);
            return mainid;
        }

        /// <summary>
        /// 最后在创建requestid后,对指定记录进行更新记录
        /// </summary>
        /// <param name="requestid"></param>
        /// <param name="valuelist"></param>
        public void UpdateRecord(string requestid, string valuelist)
        {
            var sqllist = sqlList.UpdateRecord(requestid, valuelist);
            Generdt(sqllist);
        }

    }
}
