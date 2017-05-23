using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Mvc;
using GoldMantis.Entity;
using GoldMantis.Entity.ValidateAttribute;

 
namespace GoldMantis.Entity {
    
    
    [DataContract]
    public class OAKPILeaves {
        
        [DataMember()]
        public virtual Int32 ID {
            get;
            set;
        }
        
        [DataMember()]
        public virtual String Code {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Int32 DeptId {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Int32 EmpId {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "��ٿ�ʼʱ��")]
        public virtual DateTime BeginDate {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "��ٽ���ʱ��")]
        [DateTimeNotLessThan("BeginDate", "��ʼʱ��")]
        public virtual DateTime EndDate {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "�������")]
        public virtual Decimal? Days {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "�������")]
        public virtual Int32? LeaveType {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "���ԭ��")]
        public virtual String Reason {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Boolean IsPost {
            get;
            set;
        }
        
        [DataMember()]
        public virtual DateTime? PostDate {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Int32 CheckStatus {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Int32 MakerID {
            get;
            set;
        }
        
        [DataMember()]
        public virtual DateTime MakeDate {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Int32? WorkAgentID {
            get;
            set;
        }

        [DataMember()]
        public virtual String WorkAgentName
        {
            get;
            set;
        }
    }
}
