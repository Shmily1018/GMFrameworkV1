using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using GoldMantis.Entity;

namespace GoldMantis.Web.ViewModel.OA
{
    /// <summary>
    /// 列表页模型
    /// </summary>
    [DataContract]
    public class ModelOAOvertimeIndex : BaseModel
    {
        private SearchOAOvertime _searchEntity = new SearchOAOvertime();

        /// <summary>
        /// 列表数据源
        /// </summary>
        [DataMember]
        public IList<OAOvertime> GridDataSources
        { get; set; }

        /// <summary>
        /// 查询实体
        /// </summary>
        [DataMember]
        public SearchOAOvertime SearchEntity
        {
            get
            {
                return _searchEntity;
            }
            set
            {
                _searchEntity = value;
            }
        }

        ///// <summary>
        ///// 导出成Excel格式
        ///// </summary>
        ///// <param name="datasource">租赁户信息管理</param>
        //public void ExportExcel(IList<OAOvertime> datasource)
        //{
        //    if (ExportObject.ExportType == null)
        //        return;
        //    var exportInstance = ExportHelper.GetInstance();
        //    exportInstance.ExportExcel<OAOvertime>(datasource, ExportObject, "租赁户信息管理");
        //}
    }

    /// <summary>
    /// 查询面板模型
    /// </summary>
    [DataContract]
    public class SearchOAOvertime : BaseSearch
    {
        [DataMember(Order = 1)]
        public virtual string OvertimeUser { get; set; }

        [DataMember(Order = 2)]
        public virtual string UserDept { get; set; }

        [DataMember(Order = 3)]
        public virtual string OverTimeReason { get; set; }

        [DataMember(Order = 4)]
        public virtual string Maker { get; set; }

        [DataMember(Order = 4)]
        public virtual DateTime? OverTimeDateBegin { get; set; }

        [DataMember(Order = 4)]
        public virtual DateTime? OverTimeDateEnd { get; set; }
    }


    [DataContract]
    public class ModelOAOvertimeCreate : WFModel
    {
        /// <summary>
        /// 用户实体
        /// </summary>
        [DataMember]
        public OAOvertime OAOvertime { get; set; }
        /// <summary>
        /// 记录文件路径
        /// </summary>
        [DataMember]
        public string FinalPath { get; set; }

        /// <summary>
        /// 记录文件在服务器端的相对路径（供客户端下载用）
        /// </summary>
        [DataMember]
        public string ServerFinalPath { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public String DeptName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Maker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String EmployeeName { get; set; }

        [DataMember]
        public List<OAOvertimeDetail> Detail { get; set; }

    }
}