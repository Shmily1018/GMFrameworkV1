using System;
using System.Collections.Generic;
using System.Linq;
using GoldMantis.Common;
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
    public class VW_OAOvertimeBiz : BaseTableBiz<VW_OAOvertime>, IVW_OAOvertimeService
    {
        private VW_OAOvertimeDal _dal;

        protected override IRepository<VW_OAOvertime> Repository
        {
            get { return _dal; }
            set { _dal = ObjectExtensions.As<VW_OAOvertimeDal>(value); }
        }

        public ModelVW_OAOvertimeIndex GetModelVW_OAOvertimeIndex(ModelVW_OAOvertimeIndex model)
        {
            var condition = ExpressionConditionHelper.ExpressionCondition<VW_OAOvertime>.GetInstance();
            if (!String.IsNullOrEmpty(model.SearchEntity.CommonSearchCondition))
            {
                condition.Or(x => x.OvertimeUser, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.UserDept, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.Maker, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                condition.Or(x => x.OvertimeReason, model.SearchEntity.CommonSearchCondition,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.OvertimeUser))
                {
                    condition.And(x => x.OvertimeUser, model.SearchEntity.OvertimeUser,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.UserDept))
                {
                    condition.And(x => x.UserDept, model.SearchEntity.UserDept,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.Maker))
                {
                    condition.And(x => x.Maker, model.SearchEntity.Maker,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }
                if (!String.IsNullOrWhiteSpace(model.SearchEntity.OverTimeReason))
                {
                    condition.And(x => x.OvertimeReason, model.SearchEntity.OverTimeReason,
                    ExpressionConditionHelper.ExpressionValueRelation.Like);
                }

                if (model.SearchEntity.OverTimeDateBegin.HasValue)
                {
                    condition.And(x => x.OvertimerDate, model.SearchEntity.OverTimeDateBegin,
                    ExpressionConditionHelper.ExpressionValueRelation.GreaterThanOrEqual);
                }

                if (model.SearchEntity.OverTimeDateEnd.HasValue)
                {
                    condition.And(x => x.OvertimerDate, model.SearchEntity.OverTimeDateEnd.Value.AddDays(1),
                    ExpressionConditionHelper.ExpressionValueRelation.LessThan);
                }

            }
            //if (string.IsNullOrEmpty(model.SearchEntity.OrderName))
            //{
            //    model.SearchEntity.OrderName = "KeyID";
            //    model.SearchEntity.OrderDirection = ((int)OrderBy.DESC).ToString();
            //}

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
