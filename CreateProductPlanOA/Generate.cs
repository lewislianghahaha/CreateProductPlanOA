using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateProductPlanOA
{
    public class Generate
    {
        /// <summary>
        /// 获取相关信息,并将K3信息通过OA接口传输至OA,最后达到创建新流程目的
        /// </summary>
        /// <param name="primaryKeyid">获取前端收集的主表主键列表</param>
        /// <param name="entryKeyid">获取前端收集的明细表主键列表</param>
        /// <param name="username">当前操作用户名称</param>
        /// <returns></returns>
        public String GetMessageIntoOa(string primaryKeyid,string entryKeyid,string username)
        {
            var result = "Finish";

            try
            {
                //todo:目的:收集从‘销售订单’列表


            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }


    }
}
