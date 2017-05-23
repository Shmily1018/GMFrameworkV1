using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text; 
using GoldMantis.Entity;


namespace GoldMantis.Entity {

   [DataContract]
    public class OAOvertimeDetail {
        
        [DataMember()]
        public virtual Int32 ID {
            get;
            set;
        }

        [DataMember()]
        [Required]
        public virtual int OvertimeID
        {
            get;
            set;
        }

        [DataMember()]
        [Required]
        [Display(Name = "Ա������")]
        public virtual int MakerID
        {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "��ɹ������")]
        public virtual String JobID {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Decimal OvertimeStartProgress {
            get;
            set;
        }
        
        [DataMember()]
        public virtual Decimal OvertimeEndProgress {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        public virtual Decimal OvertimeDate {
            get;
            set;
        }
        
        [DataMember()]
        [Required]
        [Display(Name = "�°�ʱ��")]
        public virtual DateTime GetOffWorkDate {
            get;
            set;
        }
        
        [DataMember()]
        public virtual String Memo {
            get;
            set;
        }

        [DataMember()]
        public virtual String MakerName
        {
            get;
            set;
        }
    }
}
