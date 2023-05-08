using System;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.List.PlugIn;

namespace CreateProductPlanOA
{
    public class ButtonEvents : AbstractListPlugIn
    {


        public override void BarItemClick(BarItemClickEventArgs e)
        {
            //定义主键变量(用与收集所选中的行主键值)
            var primaryKeyid = string.Empty;
            //定义明细行主键变量(用与收集所选中的行主键值)
            var entryKeyid = string.Empty;
            //中转判断值
            var tempstring = string.Empty;
            //返回信息记录
            var mesage = string.Empty;

            base.BarItemClick(e);

            if (e.BarItemKey == "tbCreateProductPlan")
            {
                //获取当前登录用户名称
                var username = this.Context.UserName;

                //获取列表上通过复选框勾选的记录
                var selectedrows = this.ListView.SelectedRowsInfo;
                //判断需要有选择记录时才继续
                if (selectedrows.Count > 0)
                {
                    //todo:通过循环将选中行的主键进行收集(注:去除重复的选项,只保留不重复的主键记录)
                    foreach (var row in selectedrows)
                    {
                        if (string.IsNullOrEmpty(primaryKeyid))
                        {
                            primaryKeyid = "'" + Convert.ToString(row.PrimaryKeyValue) + "'";
                            tempstring = Convert.ToString(row.PrimaryKeyValue);
                        }
                        else
                        {
                            if (tempstring != Convert.ToString(row.PrimaryKeyValue))
                            {
                                primaryKeyid += "," + "'" + Convert.ToString(row.PrimaryKeyValue) + "'";
                                tempstring = Convert.ToString(row.PrimaryKeyValue);
                            }
                        }
                    }

                    //todo:初始化中间变量
                    tempstring = "";

                    //todo:通过循环将选中行的明细行主键进行收集(注:去除重复的选项,只保留不重复的明细行主键记录) 
                    foreach (var row in selectedrows)
                    {
                        if (string.IsNullOrEmpty(entryKeyid))
                        {
                            entryKeyid = "'" + Convert.ToString(row.EntryPrimaryKeyValue) + "'";
                            tempstring = Convert.ToString(row.EntryPrimaryKeyValue);
                        }
                        else
                        {
                            if (tempstring != Convert.ToString(row.EntryPrimaryKeyValue))
                            {
                                entryKeyid += "," + "'" + Convert.ToString(row.EntryPrimaryKeyValue) + "'";
                                tempstring = Convert.ToString(row.EntryPrimaryKeyValue);
                            }
                        }
                    }

                    //todo:执行运算并返回相关结果
                    mesage = "";
                    View.ShowMessage(mesage != "Finish" ? $"新增超额出货异常,原因:'{mesage}'" : "新增成功,请打开OA,并留意右下角的OA信息提示");
                }
                else
                {
                    View.ShowErrMessage("请选择‘物料’后继续.");
                }
            }
        }
    }
}
