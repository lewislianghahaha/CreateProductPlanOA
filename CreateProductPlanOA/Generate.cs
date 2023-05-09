using System;
using System.Data;
using System.Data.SqlClient;
using CreateProductPlanOA.WebReference;

namespace CreateProductPlanOA
{
    public class Generate
    {
        SearchDt searchDt = new SearchDt();
        Tempdt tempdt = new Tempdt();
        ConnDb conDb = new ConnDb();

        public string mainid = "";
        public DataTable detaildt = new DataTable();

        /// <summary>
        /// 获取相关信息,并将K3信息通过OA接口传输至OA,最后达到创建新流程目的
        /// </summary>
        /// <param name="primaryKeyid">获取前端收集的主表主键列表</param>
        /// <param name="entryKeyid">获取前端收集的明细表主键列表</param>
        /// <param name="username">当前操作用户名称</param>
        /// <returns></returns>
        public string GetMessageIntoOa(string primaryKeyid,string entryKeyid,string username)
        {
            var result = "Finish";

            try
            {
                //获取临时表-新增OA流程更新使用
                var oauptempdt = tempdt.UpdateDtTemp();
                //获取临时表-新增OA流程插入明细表使用
                var oainserttempdt = tempdt.InsertOaDetailTemp();

                //根据username获取OA-人员ID 及 部门ID
                var oaDt = searchDt.SearchOaUserInfo(username).Copy();

                //获取K3销售订单明细记录
                detaildt = searchDt.SearchK3OrderDetailRecord(entryKeyid).Copy();

                //通过primaryKeyid 及 entryKeyid获取对应K3记录,并将记录分别插入至oauptempdt 及 oainserttempdt临时表内
                oauptempdt.Merge(InsertRecordToUpTempDt(oauptempdt,primaryKeyid));

                //对oauptempdt表进行数据处理,便于在最后更新时使用
                var updatelist = GetUpdateList(oauptempdt);

                //将oauptempdt,oainserttempdt 数据作为OA接口进行输出,并最后执行OA API方法
                var resultvalue = CreateOaWorkFlow(Convert.ToInt32(oaDt.Rows[0][0]),updatelist,oainserttempdt,username);

                result = resultvalue == "Finish" ? "Finish" : $"生成OA-订制产品生产计划流程导常,请联系管理员";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据获取的临时表记录,并利用OA API创建流程接口,创建流程
        /// </summary>
        /// <param name="createid"></param>
        /// <param name="updatelist"></param>
        /// <param name="oainserttempdt"></param>
        /// <param name="customername"></param>
        /// <returns></returns>
        private string CreateOaWorkFlow(int createid, string updatelist,DataTable oainserttempdt, string customername)
        {
            var result = string.Empty;

            try
            {
                WorkflowService workflow = new WorkflowService();

                WorkflowRequestInfo workflowRequestInfo = new WorkflowRequestInfo();
                WorkflowBaseInfo baseInfo = new WorkflowBaseInfo();

                //设置工作流ID_必须添加(重)
                baseInfo.workflowId = "13";  //"129";
                //设置工作流名称_必须添加(重)
                baseInfo.workflowName = "SC-002订制产品生产计划表-"+customername+"-"+DateTime.Now.ToString("yyyy-MM-dd");

                //设置如能否修改 查询等基础信息
                workflowRequestInfo.canView = true;
                workflowRequestInfo.canEdit = true;
                workflowRequestInfo.requestName = baseInfo.workflowName;   //设置标题_此项必须添加(重)
                workflowRequestInfo.requestLevel = "0";
                workflowRequestInfo.creatorId = Convert.ToString(createid);  //设置创建者ID(重要:创建流程时必须填)

                workflowRequestInfo.workflowBaseInfo = baseInfo;

                //主表设置
                WorkflowMainTableInfo workflowMainTableInfo = new WorkflowMainTableInfo();
                WorkflowRequestTableRecord[] workflowRequestTableRecords = new WorkflowRequestTableRecord[1]; //设置主表字段有一条记录
                WorkflowRequestTableField[] workflowtabFields = new WorkflowRequestTableField[1];  //设置主表有多少个字段

                //将workflowtableFields所设置的字段加载到workflowRequestTableRecords内
                workflowRequestTableRecords[0] = new WorkflowRequestTableRecord();
                workflowRequestTableRecords[0].workflowRequestTableFields = workflowtabFields;

                //然后将workflowRequestTableRecords加载到workflowMainTableInfo.requestRecords内
                workflowMainTableInfo.requestRecords = workflowRequestTableRecords;

                //最后将workflowMainTableInfo加载到workflowRequestInfo.workflowMainTableInfo内
                workflowRequestInfo.workflowMainTableInfo = workflowMainTableInfo;

                //执行doCreateWorkflowRequest()方法,若返回值>0 就成功;反之,出现异常
                var requestid = workflow.doCreateWorkflowRequest(workflowRequestInfo, createid);

                //在获取requestid后,1. 对相关值进行更新 2. 收集相关信息并完成对明细表的插入
                if (Convert.ToInt32(requestid) > 0)
                {
                    //对formtable_main_16表进行更新
                    searchDt.UpdateRecord(requestid, updatelist);
                    //根据获取的requestid查找对应的mainid值
                    mainid = searchDt.SearchOaRecord(requestid);
                    //整合明细数据
                    oainserttempdt.Merge(InsertRecordToTempDt(oainserttempdt));

                    //插入明细数据至formtable_main_16_dt1 内
                    ImportDtToDb("formtable_main_16_dt1", oainserttempdt);

                    result = "Finish";
                }
                else
                {
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 根据整合后的temp,循环获取相关值,便于在最后获取节点后更新使用
        /// </summary>
        /// <param name="sourcedt"></param>
        /// <returns></returns>
        private string GetUpdateList(DataTable sourcedt)
        {
            var flistid = string.Empty;

            for (var i = 0; i < sourcedt.Columns.Count; i++)
            {
                if (string.IsNullOrEmpty(flistid))
                {
                    flistid = sourcedt.Columns[i].ColumnName + "=" + "'" + Convert.ToString(sourcedt.Rows[0][i]) + "'";
                }
                else
                {
                    flistid += "," + sourcedt.Columns[i].ColumnName + "=" + "'" + Convert.ToString(sourcedt.Rows[0][i]) + "'";
                }
            }
            return flistid;
        }

        /// <summary>
        /// 将数据整合至临时表内-OA表头更新时使用
        /// </summary>
        /// <param name="resultdt"></param>
        /// <param name="primaryKeyid">获取前端收集的主表主键列表</param>
        /// <returns></returns>
        private DataTable InsertRecordToUpTempDt(DataTable resultdt, string primaryKeyid)
        {
            //获取K3相关客户信息
            var k3Custdt = searchDt.SearchK3CustomerCode(primaryKeyid).Copy();
            //根据从K3获取的客户信息,获取对应的OA 对应客户ID记录
            var oacustid = searchDt.SearchOaCustomerRecord(Convert.ToString(k3Custdt.Rows[0][0]));

            var newrow = resultdt.NewRow();
            newrow[0] = "1";      //是否为新客户
            newrow[1] = oacustid; //客户名称
            newrow[2] = Convert.ToString(k3Custdt.Rows[0][0]); //客户代码
            newrow[3] = "2";      //产品性质
            newrow[4] = "1";      //是否为工业涂料事业部客户
            resultdt.Rows.Add(newrow);

            return resultdt;
        }

        /// <summary>
        /// 将数据整合至临时表内-OA表体插入时使用
        /// </summary>
        /// <param name="resultdt"></param>
        /// <returns></returns>
        private DataTable InsertRecordToTempDt(DataTable resultdt)
        {
            //循环将销售订单明细记录插入至临时表内
            foreach (DataRow rows in detaildt.Rows)
            {
                var newrow = resultdt.NewRow();
                newrow[1] = mainid;     //主表主键
                newrow[2] = rows[1];    //产品名称
                newrow[3] = rows[2];    //品牌
                newrow[4] = rows[3];    //规格型号
                newrow[5] = rows[4];    //数量(罐)
                newrow[6] = rows[5];    //包装要求
                newrow[7] = rows[6];    //喷码/帖标
                newrow[8] = rows[0];    //产品代码
                newrow[9] = rows[7];    //工业项目料号
                resultdt.Rows.Add(newrow);
            }

            return resultdt;
        }

        /// <summary>
        /// 针对指定表进行数据插入
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">包含数据的临时表</param>
        private void ImportDtToDb(string tableName, DataTable dt)
        {
            var sqlcon = conDb.GetK3CloudConn(1);
            sqlcon.Open(); //若返回一个SqlConnection的话,必须要显式打开 
            //注:1)要插入的DataTable内的字段数据类型必须要与数据库内的一致;并且要按数据表内的字段顺序 2)SqlBulkCopy类只提供将数据写入到数据库内
            using (var sqlBulkCopy = new SqlBulkCopy(sqlcon))
            {
                sqlBulkCopy.BatchSize = 1000;                    //表示以1000行 为一个批次进行插入
                sqlBulkCopy.DestinationTableName = tableName;   //数据库中对应的表名
                sqlBulkCopy.NotifyAfter = dt.Rows.Count;       //赋值DataTable的行数
                sqlBulkCopy.WriteToServer(dt);                //数据导入数据库
                sqlBulkCopy.Close();                         //关闭连接 
            }
        }
    }
}
