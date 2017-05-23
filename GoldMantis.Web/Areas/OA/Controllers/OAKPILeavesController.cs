using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using GoldMantis.Common;
using GoldMantis.Common.Const;
using GoldMantis.Common.CustomAttribute;
using GoldMantis.Common.CustomEnum;
using GoldMantis.Entity;
using GoldMantis.Service.Contract.Contract;
using GoldMantis.Service.Contract.ContractView;
using GoldMantis.Web.Core;
using GoldMantis.Web.Core.Session;
using GoldMantis.Web.HtmlControl;
using GoldMantis.Web.ViewModel.OA;
using GoldMantis.WorkFlow.WCFClient;

namespace GoldMantis.Web.Areas.OA.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class OAKPILeavesController : WFController
    {
        //IBus bus;
        protected IOAKPILeavesService _OAKPILeavesService;
        protected IVW_OAKPILeavesService _VW_OAKPILeavesService;
        protected ISAUserService _SAUserService;
        protected IVW_UserProfileService userProfileService;

        public OAKPILeavesController()
        {
        }
        //public OAKPILeavesController(IBus bus)
        //{
        //    this.bus = bus;
        //}

        // GET: OA/OAKPILeaves
        public ActionResult Index(ModelVW_OAKPILeavesIndex model)
        {
            model = model ?? new ModelVW_OAKPILeavesIndex();
            model = _VW_OAKPILeavesService.GetModelVW_OAKPILeavesIndex(model);

            TableAttribute<VW_OAKPILeaves> table = new TableAttribute<VW_OAKPILeaves>();
            table.DataSource = model.GridDataSources;

            table.TableHeaderRow = new TableRowAttribute<VW_OAKPILeaves>();

            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "" });
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "编号" });

            //table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = ThOrderExtension.ThOrderHelper<VW_OAKPILeaves>(x=>x.UserName,"员工"), Order = true});
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "员工" });

            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "所在部门" });
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "请假类型" });
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "请假日期" });
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "请假天数" });
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "请假原因" });
            table.TableHeaderRow.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>() { InnerText = "审批状态" });


            table.TableBodyRowTemplete = new TableRowAttribute<VW_OAKPILeaves>()
            {
                HtmlAttributeDelegates = new List<Func<VW_OAKPILeaves, KeyValuePair<string, object>>>()
                    {
                        m => new KeyValuePair<string, object>("ondblclick", 
                            "window.top.menu.addTabs('"+GetPageStateName(m.KeyID,m.CheckStatus) +"员工请假信息', Maintain.editUrl+\'?id=" + m.KeyID + "&checkStatus=" + m.CheckStatus + "\')")
                    }
            };

            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 30,
                InnerHtmlDelegate =
                    o =>
                    string.Format("<input class='cb-maintain-selection' type='checkbox' name='selection' value='{0}' {1}>", o.KeyID, o.CheckStatus == EnumCheckStatus.Init.GetHashCode() && !o.IsPost ? "" : " disabled='disabled'")
            });
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 100,
                InnerHtmlDelegate =
                    o =>
                    o.Code,
                TipTextDelegate = o => o.Code
            });
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 100,
                InnerHtmlDelegate =
                    o =>
                    o.UserName,
                TipTextDelegate = o => o.UserName
            });
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 100,
                InnerHtmlDelegate =
                    o =>
                    o.DeptName,
                TipTextDelegate = o => o.DeptName
            });
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 100,
                InnerHtmlDelegate =
                    o =>
                    Enum.GetName(typeof(EnumLeaveType), o.LeaveType),
                TipTextDelegate = o => Enum.GetName(typeof(EnumLeaveType), o.LeaveType)
            });
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 200,
                InnerHtmlDelegate =
                    o =>
                    o.BeginDate.ToString("yyyy-MM-dd HH:mm") + "--" + o.EndDate.ToString("yyyy-MM-dd HH:mm")
            });
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 100,
                InnerHtmlDelegate =
                    o =>
                    o.Days,
                TipTextDelegate = o => o.Days
            });

            var attr = new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 300,
                InnerHtmlDelegate =
                o =>
                    o.Reason,
                TipTextDelegate = o => o.Reason
            };
            attr.Class.Add("text-left");

            table.TableBodyRowTemplete.TableCells.Add(attr);
            table.TableBodyRowTemplete.TableCells.Add(new TableCellAttribute<VW_OAKPILeaves>()
            {
                CellWidth = 100,
                InnerHtmlDelegate =
                    o =>
                    IsPostDecoration(o.CheckStatus),
                TipTextDelegate = o => EnumCode.GetFieldCode(Enum.Parse(typeof(EnumCheckStatus), o.CheckStatus.ToString()))
            });

            ViewBag.TableData = table;

            return View(model);
        }

        public ActionResult Create(Int32? id, int? checkStatus)
        {


            //ViewBag.PageState = GetPageState(id, checkStatus);

            var model = new ModelOAKPILeavesCreate
            {
                EnumPageMode = GetPageState(id, checkStatus)
            };
            try
            {
                if (id.HasValue)
                {
                    model.OAKPILeaves = new OAKPILeaves { ID = id.Value };
                }
                model = _OAKPILeavesService.GetModelOAKPILeavesCreate(model);

                if (model.OAKPILeaves == null)
                {
                    model.OAKPILeaves = new OAKPILeaves();
                    model.OAKPILeaves.Code = Consts.SAVE_AND_GENERATE;
                    model.OAKPILeaves.DeptId = SessionManager.UserInfo.HrDeptID;
                    model.OAKPILeaves.MakeDate = DateTime.Now;
                    model.OAKPILeaves.EmpId = SessionManager.UserInfo.UserID;
                    model.PositionName = SessionManager.UserInfo.DisplayPosition;

                    model.DeptName = SessionManager.UserInfo.DisplayDepartMent;
                    model.MakerName = SessionManager.UserInfo.DisplayName;
                    model.EmployeeName = SessionManager.UserInfo.DisplayName;
                }
                else
                {
                    var userProfile = userProfileService.ListByKeyID(model.OAKPILeaves.EmpId).FirstOrDefault();
                    model.MakerName = userProfile.UserName;
                    model.EmployeeName = userProfile.UserName;
                    model.DeptName = userProfile.DeptName;
                    model.PositionName = userProfile.PositionName;

                    if (model.OAKPILeaves.WorkAgentID != null)
                    {
                        var userProfileWorkAgent = userProfileService.ListByKeyID(model.OAKPILeaves.WorkAgentID.Value).FirstOrDefault();
                        if (userProfileWorkAgent != null)
                            model.OAKPILeaves.WorkAgentName = userProfileWorkAgent.UserName;
                    }
                }

                #region 绑定工作流

                if (model.OAKPILeaves != null)
                {
                    model.CheckStatus = model.OAKPILeaves.CheckStatus;
                    if (model.CheckStatus.HasValue && model.CheckStatus.Value > 0)
                    {
                        model.FormCheckStatus = EnumCheckStatus.Post;
                    }

                    //反审核按钮可见性根据是否审核判断
                    model.ButtonUnSubmitVisible = model.OAKPILeaves.IsPost;

                }

                model.MenuID = "1547";
                model.MenuURL = "OA/KPI/OAKPILeavesMaintain.aspx";  //审批时会用到。

                model.DeptID = SessionManager.UserInfo.HrDeptID;

                model.KeyID = model.OAKPILeaves != null ? model.OAKPILeaves.ID : 0;

                

                base.InitWFController(model);

                #endregion




                return View(model);
            }
            catch
            {
                return Content("读取信息失败!");
            }
        }


        [HttpPost]
        public ActionResult Create(ModelOAKPILeavesCreate model)
        {
            try
            {
                model.OAKPILeaves.MakeDate = DateTime.Now;
                model.OAKPILeaves.PostDate = DateTime.Now;
                model.OAKPILeaves.MakerID = SessionManager.UserInfo.UserID;


                model = _OAKPILeavesService.PostModelOAKPILeavesCreate(model);


                if (model.Comand == CommandCatalog.Commit.GetHashCode().ToString())
                {
                    //model.KeyID = model.OAKPILeaves.ID;
                    //ViewBag.PageState = EnumPageState.Edit;
                    //return View(model);
                }

                return Content("<script>window.top.menu.closeTabCallBack("+model.OAKPILeaves.ID+");</script>");
            }
            catch (Exception ex)
            {
                Error = "操作失败";
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            JsonResult jsresult = new JsonResult();
            try
            {
                _OAKPILeavesService.DeleteByKeys(ids);
                jsresult.Data = new { result = "操作成功!" };
            }
            catch
            {
                jsresult.Data = new { result = "操作失败!" };
            }
            return jsresult;
        }

        [HttpPost]
        public ActionResult UnSubmit(int keyID)
        {
            JsonResult jsresult = new JsonResult();
            try
            {
                string message;
                bool result = _OAKPILeavesService.UnSubmit(keyID, out message);
                message = result ? "反审核成功！" : string.Format("反审核失败！原因：{0}", message);

                return Content("window.top.bootstrapGM.alert({width: 450,title: '系统提示',msg: '" + message + "'});window.top.menu.closeTabCallBack();",
                    "application/x-javascript");
            }
            catch
            {
                jsresult.Data = new { result = "操作失败!" };
            }
            return jsresult;
        }

        
        public override bool OnWorkflowCompleted(out string message, WorkflowEventArgs e, int keyID)
        {
            int checkstatus = e.CheckStatus.GetHashCode();
            message = string.Empty;

            bool result = _OAKPILeavesService.Checked(keyID, checkstatus);

            if (checkstatus == EnumCheckStatus.Ok.GetHashCode()) //审批通过自动审核
            {
                result = result && _OAKPILeavesService.Post(keyID, out  message);
            }
            return result;
        }

        public JsonResult CheckDateTime(OAKPILeaves OAKPILeaves)
        {
            bool isValidate = false;

            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

    }
}