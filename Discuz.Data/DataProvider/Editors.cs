using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
{
    public class Editors
    {
        /// <summary>
        /// 以CustomEditorButtonInfo数组
        /// </summary>
        /// <returns></returns>
        public static CustomEditorButtonInfo[] GetCustomEditButtonListWithInfo()
        {
            IDataReader iDataReader = DatabaseProvider.GetInstance().GetCustomEditButtonList();
            List<CustomEditorButtonInfo> buttonList = new List<CustomEditorButtonInfo>();
            while(iDataReader.Read())
            {         
                CustomEditorButtonInfo buttonInfo = new CustomEditorButtonInfo();
                buttonInfo.Id = TypeConverter.ObjectToInt(iDataReader["id"]);
                buttonInfo.Tag = iDataReader["Tag"].ToString();
                buttonInfo.Icon = iDataReader["Icon"].ToString();
                buttonInfo.Available = TypeConverter.ObjectToInt(iDataReader["Available"]);
                buttonInfo.Example = iDataReader["Example"].ToString();
                buttonInfo.Explanation = iDataReader["Explanation"].ToString();
                buttonInfo.Params = TypeConverter.ObjectToInt(iDataReader["Params"]);
                buttonInfo.Nest = TypeConverter.ObjectToInt(iDataReader["Nest"]);
                buttonInfo.Paramsdefvalue = iDataReader["Paramsdefvalue"].ToString();
                buttonInfo.Paramsdescript = iDataReader["Paramsdescript"].ToString();
                buttonInfo.Replacement = iDataReader["Replacement"].ToString();
                buttonList.Add(buttonInfo);
            }
            iDataReader.Close();

            return buttonList.ToArray();
        }


    }
}
