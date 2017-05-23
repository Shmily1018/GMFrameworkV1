/*********************************************************
** Copyright (c)     2015 Gold Mantis Co., Ltd. 
** FileName:         OrderBy.cs
** Creator:          Joe
** CreateDate:       2015-03-27
** Changer:
** LastChangeDate:
** Description:      数据库排序
** VersionInfo:
*********************************************************/
using System.Runtime.Serialization;

namespace GoldMantis.Common
{
    public enum OrderBy
    {
        [EnumMember]
        ASC = 0,

        [EnumMember]
        DESC = 1
    }
}
