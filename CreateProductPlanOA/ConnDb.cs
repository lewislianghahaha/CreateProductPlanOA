using System.Data.SqlClient;

namespace CreateProductPlanOA
{
    public class ConnDb
    {
        /// <summary>
        /// 获取连接返回信息
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetK3CloudConn()
        {
            var sqlcon = new SqlConnection(GetConnectionString());
            return sqlcon;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            var strcon = @"Data Source='192.168.1.228';Initial Catalog='AIS20181204095717';Persist Security Info=True;User ID='sa'; Password='kingdee';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0";
            return strcon;
        }
    }
}
