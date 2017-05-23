using System;
using System.Collections.Generic;
using GoldMantis.Entity;

namespace GoldMantis.Web.ViewModel
{
    public class OAOvertimeMaintainModel : BaseModel
    {
        private SearchModel _search= new SearchModel();

        public IList<OAOvertime> DataSource { get; set; }

        public SearchModel Search {
            get { return _search;}
            set { _search = value; }
        }
    }

    public class SearchModel : BaseSearch
    {
        public string OvertimeCode { get; set; }

        public string OvertimeUser { get; set; }

        public DateTime? OvertimerDate { get; set; }
    }
}