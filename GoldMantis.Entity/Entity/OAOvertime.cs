using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using GoldMantis.Entity;


namespace GoldMantis.Entity
{

    [DataContract]
    public class OAOvertime
    {

        [DataMember()]
        public virtual Int32 ID
        {
            get;
            set;
        }

        [DataMember()]
        public virtual String Code
        {
            get;
            set;
        }

        [DataMember()]
        public virtual int EmployeeID
        {
            get;
            set;
        }

        [DataMember()]
        public virtual int DeptID
        {
            get;
            set;
        }

        [Required()]
        [Display(Name = "加班原因")]
        [DataMember()]
        public virtual String OvertimeReason
        {
            get;
            set;
        }

        [DataMember()]
        [Required()]
        [Display(Name = "加班时间")]
        public virtual DateTime OvertimerDate
        {
            get;
            set;
        }

        [DataMember()]
        public virtual Int32 Checkstatus
        {
            get;
            set;
        }

        [DataMember()]
        public virtual Boolean Ispost
        {
            get;
            set;
        }

        [DataMember()]
        public virtual DateTime Postdate
        {
            get;
            set;
        }

        [DataMember()]
        public virtual int MakerID 
        { 
            get; 
            set; 
        }

        [DataMember()]
        public virtual DateTime MakeDate
        {
            get;
            set;
        }
    }
}
