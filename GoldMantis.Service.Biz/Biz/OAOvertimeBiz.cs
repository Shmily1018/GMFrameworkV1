using System;
using System.Collections.Generic;
using System.Linq;
using GoldMantis.Common;
using GoldMantis.Common.Const;
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
    public class OAOvertimeBiz : BaseTableBiz<OAOvertime>, IOAOvertimeService
    {
        private OAOvertimeDal _dal;
        private VW_UserProfileBiz userProfileBiz=new VW_UserProfileBiz();

        protected override IRepository<OAOvertime> Repository
        {
            get { return _dal; }
            set { _dal = ObjectExtensions.As<OAOvertimeDal>(value); }
        }

        public ModelOAOvertimeCreate GetModelOAOvertimeCreate(ModelOAOvertimeCreate model)
        {

            if (model.OAOvertime != null && model.OAOvertime.ID != 0)
            {
                model.OAOvertime = this.GetByKey<Int32>(model.OAOvertime.ID);
            }


            //明细表赋值
            if (model.OAOvertime != null)
            {
                model.Detail =
                    DataAccess.Session.Query<OAOvertimeDetail>()
                        .Where(x => x.OvertimeID == model.OAOvertime.ID)
                        .ToList();

                foreach (var item in model.Detail)
                {
                    var vwUserProfile = userProfileBiz.ListByKeyID(item.MakerID).FirstOrDefault();
                    if (vwUserProfile != null)
                        item.MakerName = vwUserProfile.UserName;
                }
            }
            return model;
        }

        public ModelOAOvertimeCreate PostModelOAOvertimeCreate(ModelOAOvertimeCreate model)
        {
            ITransaction tx = DataAccess.Session.BeginTransaction();

            if (model.OAOvertime.Code == Consts.SAVE_AND_GENERATE)
            {
                var overtime = DataAccess.Session.Query<OAOvertime>().OrderByDescending(x=>x.ID).FirstOrDefault();
                int num = 0;
                if (overtime != null)
                {
                    num = Int32.Parse(overtime.Code.Substring(10));
                }
                model.OAOvertime.Code=String.Format("{0}-{1}-{2}","YGJB",DateTime.Now.ToString("yyMM"),(num+1).ToString().PadLeft(4,'0'));
            }
            using (tx)
            {
                DataAccess.SaveOrUpdate(model.OAOvertime);

                foreach (var detail in model.Detail)
                {
                    detail.OvertimeID = model.OAOvertime.ID;
                    DataAccess.Session.SaveOrUpdate(detail);
                }

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
                    var demo = session.Get<OAOvertime>(keyID);
                    demo.Checkstatus = checkStatus;
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
                    var demo = session.Get<OAOvertime>(keyID);
                    if (demo.Ispost)
                    {
                        message = "已经审核，不可操作！";
                        return false;
                    }

                    demo.Postdate = DateTime.Now;
                    demo.Ispost = true;
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
