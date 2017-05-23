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
    public class ModelVW_OAOvertimeDetailIndex : BaseModel
    {
       
        [DataMember]
        public OAOvertimeDetail OAOvertimeDetail { get; set; }


        [DataMember]
        public String UserName { get; set; }

        
    }
}