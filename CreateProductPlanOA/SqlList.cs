namespace CreateProductPlanOA
{
    public class SqlList
    {
        private string _result;

        /// <summary>
        /// 根据用户名称获取OA-用户ID及部门ID信息
        /// </summary>
        /// <param name="username">K3登录用户名称</param>
        /// <returns></returns>
        public string SearchOaUserInfo(string username)
        {
            _result = $@"
                            SELECT A.ID 用户ID,A.lastname 名称,B.id 部门ID,C.ID 岗位ID,CONVERT(VARCHAR(10),GETDATE(),23) 申请日期
                            --,B.departmentmark 部门 
                            FROM dbo.HrmResource A
                            INNER JOIN dbo.HrmDepartment B ON A.departmentid=B.id
                            INNER JOIN HrmJobTitles C ON A.jobtitle=C.ID
                            WHERE A.lastname='{username}' --'梁嘉杰'--ID='249'
                        ";
            return _result;
        }

        //todo:根据主表主键查询出K3‘销售订单’-表头相关信息 primaryKeyid
        public string SearchSalesOrderHeadRecord(string primaryKeyid)
        {
            _result = $@"";

            return _result;
        }

        //todo:根据明细表主键查询出K3‘销售订单’-明细表相关信息 entryKeyid
        public string SearchSalesOderDeatilRecord(string entryKeyid)
        {
            _result = $@"";

            return _result;
        }
    }
}
