using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GoldMantis.Common;
using GoldMantis.Common.Const;
using GoldMantis.Common.Data;
using GoldMantis.DI;
using GoldMantis.Entity;
using GoldMantis.Framework.CryptogramService;
using GoldMantis.Service.Biz.Redis;
using GoldMantis.Service.Contract.Contract;
using GoldMantis.Service.Dal.Dal;
using UserInfo = GoldMantis.Common.UserInfo;

namespace GoldMantis.Service.Biz.Biz
{
    public class PermissionBiz : IPermissionService
    {
        private readonly PermissionDal permissionDal=new PermissionDal();
        //private static ILoginPermissionService service ;
        //private static ChannelFactory<ILoginPermissionService> channelFactory;

        //public PermissionBiz()
        //{
        //    if (channelFactory != null)
        //    {
        //        channelFactory.Close();
        //    }
        //    string remoteAddress = ConfigurationManager.AppSettings["PermissionUrl"];
        //    var binding = new WSHttpBinding()
        //    {
        //        MaxBufferPoolSize = int.MaxValue,
        //        MaxReceivedMessageSize = int.MaxValue,
        //        OpenTimeout = new TimeSpan(0, 1, 20),
        //        SendTimeout = new TimeSpan(0, 30, 0),

        //        ReceiveTimeout = new TimeSpan(0, 1, 20),
        //        CloseTimeout = new TimeSpan(0, 1, 20),
        //        ReaderQuotas = new XmlDictionaryReaderQuotas()
        //        {
        //            MaxStringContentLength = int.MaxValue,
        //            MaxArrayLength = int.MaxValue
        //        }
        //    };
        //    channelFactory =
        //        new ChannelFactory<ILoginPermissionService>(binding, remoteAddress);


        //    foreach (OperationDescription operationDescription in channelFactory.Endpoint.Contract.Operations)
        //    {
        //        DataContractSerializerOperationBehavior operationBehavior =
        //            operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
        //        if (operationBehavior != null)
        //        {
        //            operationBehavior.MaxItemsInObjectGraph = int.MaxValue;
        //        }
        //    }
        //    service = channelFactory.CreateChannel();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public UserInfo GetLoginInfo(string loginID, string password, bool isImitation, out string message)
        {
            message = String.Empty;
            UserInfo user=permissionDal.GetLoginInfo(loginID);
            user.IsImitation = isImitation;

            string pwd = TripleDESEncrypt.Encrypt(password, loginID.ToLower());

            if (user == null || user.Password != pwd)
            {
                message = "用户帐号或者密码错误！";
            }
            else
            {
                if (!user.CanLogin)
                {
                    message = "用户帐号已禁止登录系统！";
                }

                if (!user.IsOnJob)
                {
                    message = "用户帐号处于异常状态，请联系统系管理员！";
                }

                if (!user.IsOn)
                {
                    message = "用户帐号被禁用，请联系统系管理员！";
                }
            }

            if (user != null && message == String.Empty)
            {
                user.LoginResult = true;
            }
            return user;
        }

        public UserInfo ImitationAccount(int userID, int currentUserID, out string message)
        {
            var userInfo = new UserInfo();
            message=String.Empty;
            
            if (!permissionDal.GetAgentPermission(userID, currentUserID))
            {
                message = "模拟失败，失败原因：您无权模拟该用户！";
                userInfo.LoginResult = false;
            }
            else
            {
                var user = GetSAUserByID(userID);
                if (user != null)
                {
                    userInfo = permissionDal.GetLoginInfo(user.LoginID);
                    userInfo.IsImitation = true;
                    userInfo.LoginResult = true;
                }
            }
            return userInfo;
        }

        public IList<SAMenu> GetUserMenuInfo(int userID, int sourceAppId)
        {
            return permissionDal.GetUserMenuInfo(userID, sourceAppId);
        }

        public IList<SAUser> GetSAUserList()
        {
            return permissionDal.GetSAUserList();
        }

        public SAUser GetSAUserByID(int id)
        {
            return permissionDal.GetSAUserByID(id);
        }

        public IList<SAUser> GetSAUserByIDs(int[] ids)
        {
            return permissionDal.GetSAUserByIDs(ids);
        }

        public IList<SAMenu> GetSAMenuList()
        {
            return permissionDal.GetSAMenuList();
        }

        public SAMenu GetSAMenuByID(int id)
        {
            return permissionDal.GetSAMenuByID(id);
        }





    }
}
