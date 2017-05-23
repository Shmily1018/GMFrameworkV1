using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoldMantis.Common;
using GoldMantis.Common.Const;
using GoldMantis.Common.CustomEnum;
using GoldMantis.Entity;
using GoldMantis.Entity;
using GoldMantis.Service.Contract.Contract;
using GoldMantis.Service.Dal;
using GoldMantis.Service.Dal.Dal;
using GoldMantis.Web.ViewModel;
using GoldMantis.Web.ViewModel.OA;
using GoldMantis.Web.ViewModel.User;
using NHibernate;
using NHibernate.Linq;

namespace GoldMantis.Service.Biz.Biz
{
    public class OAKPILeavesBiz : BaseTableBiz<OAKPILeaves>, IOAKPILeavesService
    {
        private OAKPILeavesDal _dal;
        private SCFlowMapingBiz _flowMapingBiz;


        protected override IRepository<OAKPILeaves> Repository
        {
            get { return _dal; }
            set { _dal = ObjectExtensions.As<OAKPILeavesDal>(value); }
        }

        public ModelOAKPILeavesCreate GetModelOAKPILeavesCreate(ModelOAKPILeavesCreate model)
        {

            model.LeavesTypeList = EnumHelper.GetSelectListWithFirstOption(typeof (EnumLeaveType),"--请选择--","");
                

            if (model.OAKPILeaves != null && model.OAKPILeaves.ID != 0)
            {
                model.OAKPILeaves = this.GetByKey<Int32>(model.OAKPILeaves.ID);
            }

            return model;
        }

        public ModelOAKPILeavesCreate PostModelOAKPILeavesCreate(ModelOAKPILeavesCreate model)
        {
            ITransaction tx = DataAccess.Session.BeginTransaction();

            if (model.OAKPILeaves.Code == Consts.SAVE_AND_GENERATE)
            {
                var overtime = DataAccess.Session.Query<OAKPILeaves>().OrderByDescending(x => x.ID).FirstOrDefault();
                int num = 0;
                if (overtime != null)
                {
                    num = Int32.Parse(overtime.Code.Substring(10));
                }
                model.OAKPILeaves.Code = String.Format("{0}-{1}-{2}", "YGQJ", DateTime.Now.ToString("yyMM"), (num + 1).ToString().PadLeft(4, '0'));
            }
            using (tx)
            {
                DataAccess.SaveOrUpdate(model.OAKPILeaves);

              

                tx.Commit();
            }
            return model;
        }

        public void DeleteByKeys(int[] ids)
        {
            base.DeleteByKeys(ids);
        }

        /// <summary>
        /// 根据ID设置审批状态
        /// </summary>
        /// <param name="keyID"></param>
        /// <param name="checkStatus"></param>
        /// <returns></returns>
        public bool Checked(int keyID, int checkStatus)
        {
            var session = DataAccess.Session;
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var demo = session.Get<OAKPILeaves>(keyID);
                    demo.CheckStatus = checkStatus;
                    session.Update(demo);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return true;
        }


        /// <summary>
        /// 审核结果
        /// </summary>
        /// <param name="keyID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Post(int keyID, out string message)
        {
            message = string.Empty;
            var session = DataAccess.Session;
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var demo = session.Get<OAKPILeaves>(keyID);
                    if (demo.IsPost)
                    {
                        message = "已经审核，不可操作！";
                        return false;
                    }

                    demo.PostDate = DateTime.Now;
                    demo.IsPost = true;
                    session.Update(demo);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return true;
        }


        public bool UnSubmit(int keyID, out string message)
        {
            message = string.Empty;
            var session = DataAccess.Session;
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var demo = session.Get<OAKPILeaves>(keyID);
                    if (!demo.IsPost)
                    {
                        message = "未审核，不可反审核操作！";
                        return false;
                    }

                    demo.PostDate = DateTime.Now;
                    demo.IsPost = false;
                    //demo.CheckStatus = 0;

                    session.Update(demo);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return true;
        }
    }
}
