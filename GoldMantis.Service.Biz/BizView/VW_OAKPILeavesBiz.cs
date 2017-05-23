using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoldMantis.Common;
using GoldMantis.Common.CustomEnum;
using GoldMantis.Entity;
using GoldMantis.Entity;
using GoldMantis.Service.Contract.Contract;
using GoldMantis.Service.Contract.ContractView;
using GoldMantis.Service.Dal;
using GoldMantis.Service.Dal.Dal;
using GoldMantis.Service.Dal.DalView;
using GoldMantis.Web.ViewModel;
using GoldMantis.Web.ViewModel.OA;
using GoldMantis.Web.ViewModel.User;
using NHibernate;
using NHibernate.Linq;

namespace GoldMantis.Service.Biz.Biz
{
    public class VW_OAKPILeavesBiz : BaseTableBiz<VW_OAKPILeaves>, IVW_OAKPILeavesService
    {
        private VW_OAKPILeavesDal _dal;

        protected override IRepository<VW_OAKPILeaves> Repository
        {
            get { return _dal; }
            set { _dal = ObjectExtensions.As<VW_OAKPILeavesDal>(value); }
        }

        public ModelVW_OAKPILeavesIndex GetModelVW_OAKPILeavesIndex(ModelVW_OAKPILeavesIndex model)
        {

            //var x2= DataAccess.Session.Query<PRODUCTINFORMATION>().ToList();


            model.LeavesTypeList = EnumHelper.GetSelectListWithFirstOption(typeof(EnumLeaveType), "--请选择--", "");

            var condition = ExpressionConditionHelper.ExpressionCondition<VW_OAKPILeaves>.GetInstance();
            if (!String.IsNullOrEmpty(model.SearchEntity.CommonSearchCondition))
            {
                condition.Or(x => x.Code, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.UserName, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.WorkAgentName, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.Reason, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.DeptName, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.UserName))
                {
                    condition.And(x => x.UserName, model.SearchEntity.UserName,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.Code))
                {
                    condition.And(x => x.Code, model.SearchEntity.Code,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.Reason))
                {
                    condition.And(x => x.Reason, model.SearchEntity.Reason,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.DeptName))
                {
                    condition.And(x => x.DeptName, model.SearchEntity.DeptName,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (model.SearchEntity.LeaveType.HasValue)
                {
                    condition.And(x => x.LeaveType, model.SearchEntity.LeaveType,
                    ExpressionConditionHelper.ExpressionValueRelation.Equal);
                }

                if (model.SearchEntity.Days.HasValue)
                {
                    condition.And(x => x.Days, model.SearchEntity.Days,
                    ExpressionConditionHelper.ExpressionValueRelation.Equal);
                }

               
            }
            if (string.IsNullOrEmpty(model.SearchEntity.OrderName))
            {
                model.SearchEntity.OrderName = "KeyID";
                model.SearchEntity.OrderDirection = ((int)OrderBy.DESC).ToString();
            }

            int count = 0;
            ////导出全部数据
            //if (model.ExportObject != null && model.ExportObject.ExportType == ExportType.导出全部)
            //{
            //    model.GridDataSources = this.ListBy(condition.ConditionExpression,
            //       model.SearchEntity._OrderName,
            //       model.SearchEntity.EnumOrderDirection);
            //    return model;
            //}

            //导出本页数据或者不导出逻辑

            model.GridDataSources = PaginateList(model.SearchEntity.PageSize, model.SearchEntity.PageIndex, ref count, condition.ConditionExpression, model.SearchEntity.OrderName, model.SearchEntity.EnumOrderDirection);
            //分页加载
            model.PaginateHelperObject = PaginateAttribute.ConstructPaginate(model.SearchEntity.PageSize, count, model.SearchEntity.PageIndex, model.SearchEntity, "SearchEntity");
            return model;
        }


        public void DeleteByKeys(int[] ids)
        {
            base.DeleteByKeys(ids);
        }
    }
}
