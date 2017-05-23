/*********************************************************
** Copyright (c) 2008 Gold Mantis Co., Ltd. 
** FileName:         ServiceFactory.cs
** Creator:          
** CreateDate:       2015-08-13
** Changer:
** LastChangeDate:
** Description:      服务创建工厂
** VersionInfo:
*********************************************************/
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace GoldMantis.DI
{
    public static class ServiceFactory
    {
        private static ServicesConfig _servicesConfig = ServicesConfig.GetServicesConfig();
        private static string _contractPrefix = "GoldMantis.Service.Contract.Contract";

        public static T GetService<T>()
        {
            Type type = typeof (T);
            string name = type.Assembly.GetName().Name;
            return GetWcfService<T>(name, _servicesConfig.ServiceConfigItems[name].serverURL);
        }

        private static T GetWcfService<T>(string wcfServer, string serviceUrl)
        {
            if (!serviceUrl.EndsWith("/"))
            {
                serviceUrl = string.Format("{0}/", serviceUrl);
            }

            string fullName = typeof (T).FullName;
            string partPath = fullName.Substring(_contractPrefix.Length);

            serviceUrl = string.Format("{0}Service{1}/{2}.svc", serviceUrl,
                partPath.Substring(0, partPath.LastIndexOf('.')),
                partPath.Substring(partPath.LastIndexOf('.') + 2));

            EndpointAddress remoteAddress = new EndpointAddress(serviceUrl);
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding
            {
                Security = {Mode = BasicHttpSecurityMode.None},
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                OpenTimeout = new TimeSpan(0, 1, 20),
                SendTimeout = new TimeSpan(0, 30, 0),
                ReceiveTimeout = new TimeSpan(0, 1, 20),
                CloseTimeout = new TimeSpan(0, 1, 20),
                ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxStringContentLength = int.MaxValue,
                    MaxArrayLength = int.MaxValue
                }
            };

            

            ChannelFactory<T> channelFactory = new ChannelFactory<T>(basicHttpBinding, remoteAddress);

            foreach (OperationDescription operationDescription in channelFactory.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior operationBehavior =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (operationBehavior != null)
                {
                    operationBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }


            return channelFactory.CreateChannel();
        }
    }
}