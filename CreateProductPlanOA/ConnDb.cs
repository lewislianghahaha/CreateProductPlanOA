﻿using System.Data.SqlClient;

namespace CreateProductPlanOA
{
    public class ConnDb
    {
        /// <summary>
        /// 获取连接返回信息
        /// </summary>
        /// <param name="conid">0:连接K3 1:连接OA</param>
        /// <returns></returns>
        public SqlConnection GetK3CloudConn(int conid)
        {
            var sqlcon = new SqlConnection(GetConnectionString(conid));
            return sqlcon;
        }

        /// <summary>
        /// 0:连接K3 1:连接OA
        /// </summary>
        /// <param name="conid"></param>
        /// <returns></returns>
        private string GetConnectionString(int conid)
        {
            var strcon = conid == 0
                 ? @"Data Source='192.168.1.228';Initial Catalog='AIS20220817082811';Persist Security Info=True;User ID='sa'; Password='kingdee';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0"
                 : @"Data Source='192.168.1.237';Initial Catalog='ecology';Persist Security Info=True;User ID='sa'; Password='Yatu8773866';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0";

            return strcon;
        }
    }
}
