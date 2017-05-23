using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;
using GoldMantis.Entity;

namespace GoldMantis.Web.ViewModel.OA
{
    /// <summary>
    /// 列表页模型
    /// </summary>
    [DataContract]
    public class ModelVW_OAKPILeavesIndex : BaseModel
    {
        private SearchOAKPILeaves _searchEntity = new SearchOAKPILeaves();

        /// <summary>
        /// 列表数据源
        /// </summary>
        [DataMember]
        public IList<VW_OAKPILeaves> GridDataSources
        { get; set; }

        /// <summary>
        /// 查询实体
        /// </summary>
        [DataMember]
        public SearchOAKPILeaves SearchEntity
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

        [DataMember]
        public IList<SelectListItem> LeavesTypeList{ get; set; }
    }

    /// <summary>
    /// 查询面板模型
    /// </summary>
    [DataContract]
    public class SearchOAKPILeaves : BaseSearch
    {
        [DataMember(Order = 1)]
        public virtual string UserName { get; set; }

        [DataMember(Order = 2)]
        public virtual string Code { get; set; }

        [DataMember(Order = 3)]
        public virtual string Reason { get; set; }

        [DataMember(Order = 4)]
        public virtual int? LeaveType { get; set; }

        [DataMember(Order = 5)]
        public virtual int? Days { get; set; }

        [DataMember(Order = 6)]
        public virtual string DeptName { get; set; }
    }


    [DataContract]
    public class ModelOAKPILeavesCreate : WFModel
    {
        /// <summary>
        /// 用户实体
        /// </summary>
        [DataMember]
        public OAKPILeaves OAKPILeaves { get; set; }
       

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public String DeptName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String PositionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String MakerName { get; set; }


        [DataMember]
        public String EmployeeName { get; set; } 

        [DataMember]
        public IList<SelectListItem> LeavesTypeList { get; set; }

    }
}