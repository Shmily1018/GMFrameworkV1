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
    public class ModelVW_OAOvertimeIndex : BaseModel
    {
        private SearchOAOvertime _searchEntity = new SearchOAOvertime();

        /// <summary>
        /// 列表数据源
        /// </summary>
        [DataMember]
        public IList<VW_OAOvertime> GridDataSources
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
}