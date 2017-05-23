using System;
using System.Collections.Generic;
using System.Linq;
using GoldMantis.Common;
using GoldMantis.Entity;
using GoldMantis.Entity;
using GoldMantis.Service.Contract.Contract;
using GoldMantis.Service.Dal;
using GoldMantis.Service.Dal.Dal;
using GoldMantis.Web.ViewModel;
using GoldMantis.Web.ViewModel.User;
using NHibernate.Linq;

namespace GoldMantis.Service.Biz.Biz
{
    public class OAOvertimeDetailBiz : BaseTableBiz<OAOvertimeDetail>, IOAOvertimeDetailService
    {
        private OAOvertimeDetailDal _dal;

        protected override IRepository<OAOvertimeDetail> Repository
        {
            get { return _dal; }
            set { _dal = ObjectExtensions.As<OAOvertimeDetailDal>(value); }
        }
    }
}
