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

        /// <summary>
        /// 根据主表主键查询出K3‘销售订单’-表头相关信息 primaryKeyid
        /// </summary>
        /// <param name="primaryKeyid">主表主键（注:只有一个值）</param>
        /// <returns></returns>
        public string SearchK3SalesOrderHeadRecord(string primaryKeyid)
        {
            _result = $@"SELECT B.FNUMBER 客户代码
                         FROM dbo.T_SAL_ORDER A
                         INNER JOIN dbo.T_BD_CUSTOMER B ON A.FCUSTID=B.FCUSTID
                         WHERE A.FID={primaryKeyid}";

            return _result;
        }

        /// <summary>
        /// 根据明细表主键查询出K3‘销售订单’-明细表相关信息 entryKeyid
        /// </summary>
        /// <param name="entryKeyid">明细表主键-(注:会有多个,并用逗号分隔)</param>
        /// <returns></returns>
        public string SearchK3SalesOderDeatilRecord(string entryKeyid)
        {
            _result = $@"SELECT B.FNUMBER 产品代码,C.FNAME 产品名称,D.FDATAVALUE 品牌,C.FSPECIFICATION 规格型号,A.FQTY 数量,ISNULL(E.F_YTC_TEXT1,'') 包装要求
                                ,'0' 喷码,NULL 工业项目料号
                         FROM dbo.T_SAL_ORDERENTRY A
                         INNER JOIN dbo.T_BD_MATERIAL B ON A.FMATERIALID=B.FMATERIALID
                         INNER JOIN dbo.T_BD_MATERIAL_L C ON B.FMATERIALID=C.FMATERIALID AND C.FLOCALEID=2052
                         INNER JOIN dbo.T_BAS_ASSISTANTDATAENTRY_L D ON B.F_YTC_ASSISTANT7=D.FENTRYID AND D.FLOCALEID=2052
                         LEFT JOIN dbo.ytc_t_Cust100010 E ON B.F_YTC_BASE2=E.FID
                         WHERE A.FENTRYID IN ({entryKeyid})";

            return _result;
        }

        /// <summary>
        /// 根据K3客户编码,查询相关OA客户档案里面的ID值
        /// </summary>
        /// <param name="k3Customercode">K3客户编码</param>
        /// <returns></returns>
        public string SearchOaCustomerRecord(string k3Customercode)
        {
            _result = $@"SELECT A.id
                         FROM dbo.uf_khda A
                         WHERE A.khbm='{k3Customercode}'";
            return _result;
        }

        /// <summary>
        /// 根据API成功返回的requestid查询对应的主表ID值,在最后的明细表更新时使用
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        public string SearchOaRecord(string requestid)
        {
            _result = $@"SELECT a.id mainid 
                         FROM dbo.formtable_main_16 a
                         WHERE a.requestId='{requestid}'";

            return _result;
        }

        /// <summary>
        /// 插入成功后,更新OA相关信息
        /// </summary>
        /// <param name="requestid"></param>
        /// <param name="updatelistvalue"></param>
        /// <returns></returns>
        public string UpdateRecord(string requestid, string updatelistvalue)
        {
            _result =
                $@"
                    update formtable_main_16 set {updatelistvalue} where requestid='{requestid}'
                  ";

            return _result;
        }

    }
}
