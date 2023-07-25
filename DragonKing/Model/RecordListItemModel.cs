using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.Model
{
    public class RecordListItemModel
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public string Index { get; set; }

        public string Channel { get; set; }
        /// <summary>
        /// 加样孔
        /// </summary>
        public string SampleIndex { get; set; }
        public string SampleBarCode { get; set; }
        public string ExperimentName { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        public DateTime DetectionTime { get; set; }

        /// <summary>
        /// 结果,是否有效
        /// </summary>

        public string Result { get; set; }
        public string OperatorName { get; set; }//操作人员

        //public CommonLibrary.SampleType SampleType { get; set; }//样本类型
        public string SampleType { get; set; }//样本类型
    }
}
