/*********************************************************
** Copyright (c) 2008 Gold Mantis Co., Ltd. 
** FileName:         BaseController.cs
** Creator:          
** CreateDate:       2015-08-13
** Changer:
** LastChangeDate:
** Description:      Controller基类
** VersionInfo:
*********************************************************/

using System;
using System.Web.Mvc;
using GoldMantis.Common;
using GoldMantis.Common.CustomAttribute;
using GoldMantis.Common.CustomEnum;
using GoldMantis.Log;
using GoldMantis.Web.Core.Session;
using log4net;
using log4net.Repository.Hierarchy;

namespace GoldMantis.Web.Core
{
    public class BaseController : Controller
    {
        public string Error
        {
            get
            {
                if (this.TempData["Error"] == null)
                    return string.Empty;
                return this.TempData["Error"].ToString();
            }
            set
            {
                this.TempData["Error"] = (object)value;
            }
        }

        public string Message
        {
            get
            {
                if (this.TempData["Message"] == null)
                    return string.Empty;
                return this.TempData["Message"].ToString();
            }
            set
            {
                this.TempData["Message"] = (object)value;
            }
        }

        public string RetUrl
        {
            get
            {
                return this.Request.QueryString["RetUrl"];
            }
        }

        //public EnumPageState? PageState
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(this.Request.QueryString["pagestate"]))
        //            return new EnumPageState?();
        //        return new EnumPageState?((EnumPageState)int.Parse(this.Request.QueryString["pagestate"]));
        //    }
        //}

        public EnumPageMode GetPageState(object obj, int? checkStatus)
        {
            if (obj == null || String.IsNullOrWhiteSpace(obj.ToString()))
            {
                return EnumPageMode.Add;
            }
            else if (checkStatus == EnumCheckStatus.Init.GetHashCode())
            {
                return EnumPageMode.Edit;
            }
            else
            {
                return EnumPageMode.View;
            }
        }

        /// <summary>
        /// 获取页面状态中文名称
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="checkStatus"></param>
        /// <returns></returns>
        public String GetPageStateName(object obj, int? checkStatus)
        {
            EnumPageMode result = GetPageState(obj, checkStatus);
            return EnumHelper.GetDescription(result);
        }

        public string IsPostDecoration(int checkStatus)
        {
            string tem=@"<button type='button' class='btn {2} btn-xs'><i class='fa {1}'></i>{0}</button>";

            return String.Format(tem, EnumCode.GetFieldCode(Enum.Parse(typeof(EnumCheckStatus), checkStatus.ToString())), "fa-check", "blue");
            
            
        }

        protected ActionResult View()
        {
            if (AjaxRequestExtensions.IsAjaxRequest(this.Request))
                return (ActionResult)base.View((string)this.Request.RequestContext.RouteData.Values["action"] + (object)"_Partial");
            return (ActionResult)base.View();
        }

        protected new ViewResult View(string viewName)
        {
            return base.View(viewName);
        }

        protected new ViewResult View(object model)
        {
            if (AjaxRequestExtensions.IsAjaxRequest(this.Request))
                return this.View((string)this.Request.RequestContext.RouteData.Values["action"] + (object)"_Partial", model);
            return base.View(model);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected override HttpNotFoundResult HttpNotFound(string statusDescription)
        {
            return base.HttpNotFound(statusDescription);
        }

        protected virtual void WriteLoginLog(string message)
        {
            string userName = "", account = "";
            if (SessionManager.UserInfo != null)
            {
                account = SessionManager.UserInfo.Account;
                userName = SessionManager.UserInfo.UserName;
            }
            LogHelper.WriteLog(account, userName, string.Empty, "登录/注销", message);

        }
        

        protected virtual void WriteOperaLog(string title, OperaType operaType, string message, string keyId, bool result, string projectName, params object[] list)
        {
            message = string.Format("{0} ID:{1}", message, keyId);
            message += result ? "    操作成功" : "    操作失败";
            LogHelper.WriteLog(SessionManager.UserInfo.Account,
                        SessionManager.UserInfo.UserName,
                        projectName,
                        title + EnumCode.GetFieldCode(operaType), message, list);
        }

        protected virtual void WriteOperaLog(string title, string message)
        {
            string userName = "", account = "";
            if (SessionManager.UserInfo != null)
            {
                account = SessionManager.UserInfo.Account;
                userName = SessionManager.UserInfo.UserName;
            }
            LogHelper.WriteLog(account, userName, string.Empty, title, message);

        }


       
    }
}