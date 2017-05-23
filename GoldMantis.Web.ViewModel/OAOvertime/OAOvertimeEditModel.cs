using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldMantis.Web.ViewModel
{
    public class OAOvertimeEditModel
    {
        [Required]
        [StringLength(20,MinimumLength = 10,ErrorMessage = "字符长度必须在10到20之间")]
        public string TextBox { get; set; }

        public int DropDown { get; set; }

        public bool CheckBox { get; set; }

        public bool RadioButton { get; set; }

        public List<int> CheckBoxList { get; set; }

        public int RadioButtonList { get; set; }

        public string TextArea { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateTime { get; set; }

        public double Numeric { get; set; }

        [Required(ErrorMessage = "项目必填")]
        public string SelectText { get; set; }

        public string SelectValue { get; set; }

        public List<OAOvertimeDetailModel> Detail { get; set; }
    }

    public class OAOvertimeDetailModel
    {
        public string TextBox { get; set; }

        public int DropDown { get; set; }

        public string TextArea { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "项目必填")]
        public string SelectText { get; set; }

        public string SelectValue { get; set; }

        public List<int> CheckBoxList { get; set; }
    }
}
