using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;

namespace Discuz.Forum
{
    /// <summary>
    /// �����������
    /// </summary>
    public class Smilies
    {
        public static Regex[] regexSmile = null;

        static Smilies()
        {
            InitRegexSmilies();
        }

        /// <summary>
        /// ��ʼ�����������������
        /// </summary>
        public static void InitRegexSmilies()
        {
            SmiliesInfo[] smiliesList = Smilies.GetSmiliesListWithInfo();
            //�Ա���������򣬽���ʶ�����ķŵ��ʼ�����������ó���ʶ�����Ƚ��ͣ��Է�ֹ:giggle :g ����Ϊ[:g]iggle [:g]
            for (int Outer = smiliesList.Length - 1; Outer >= 1; Outer--)
            {
                //һ��ð������Ƚ�0~ourter-1��Ԫ�صĴ�С  
                for (int Inner = 0; Inner <= Outer - 1; Inner++)
                {
                    //�������  
                    if (smiliesList[Inner].Code.Length < smiliesList[Inner + 1].Code.Length)
                    {
                        SmiliesInfo temp = smiliesList[Inner];
                        smiliesList[Inner] = smiliesList[Inner + 1];
                        smiliesList[Inner + 1] = temp;
                    }
                }
            }
            regexSmile = new Regex[smiliesList.Length];

            for (int i = 0; i < smiliesList.Length; i++)
            {
                regexSmile[i] = new Regex(@Regex.Escape(smiliesList[i].Code), RegexOptions.None);
            }
        }

        /// <summary>
        /// ���¼��ز���ʼ�����������������
        /// </summary>
        /// <param name="smiliesList">�����������</param>
        public static void ResetRegexSmilies(SmiliesInfo[] smiliesList)
        {
            int smiliesCount = smiliesList.Length;

            // �����Ŀ��ͬ�����´�������, ���ⷢ������Խ��
            if (regexSmile == null || regexSmile.Length != smiliesCount)
                regexSmile = new Regex[smiliesCount];

            for (int i = 0; i < smiliesCount; i++)
            {
                regexSmile[i] = new Regex(@Regex.Escape(smiliesList[i].Code), RegexOptions.None);
            }
        }

        /// <summary>
        /// �������еı�����Ϣ����ΪSmiliesInfo[],�������������
        /// </summary>
        /// <returns></returns>
        public static SmiliesInfo[] GetSmiliesListWithInfo()
        {
            DNTCache cache = DNTCache.GetCacheService();
            SmiliesInfo[] smiliesInfoList = cache.RetrieveObject("/Forum/UI/SmiliesListWithInfo") as SmiliesInfo[];

            if (smiliesInfoList == null)
            {
                smiliesInfoList = Discuz.Data.Smilies.GetSmiliesListWithoutType();
                cache.AddObject("/Forum/UI/SmiliesListWithInfo", smiliesInfoList);

                //���黺�����¼���ʱ���³�ʼ�����������������
                ResetRegexSmilies(smiliesInfoList);
            }
            return smiliesInfoList;
        }

        public static SmiliesInfo GetSmiliesTypeById(int smiliesId)
        {
            SmiliesInfo[] smiliesInfoList = Discuz.Data.Smilies.GetSmiliesTypesInfo();
            foreach (SmiliesInfo smiliesInfo in smiliesInfoList)
            {
                if (smiliesInfo.Id == smiliesId)
                    return smiliesInfo;
            }
            return null;
        }

        public static SmiliesInfo GetSmiliesById(int smiliesId)
        {
            SmiliesInfo[] smiliesInfoList = GetSmiliesListWithInfo();
            foreach (SmiliesInfo smiliesInfo in smiliesInfoList)
            {
                if (smiliesInfo.Id == smiliesId)
                    return smiliesInfo;
            }
            return null;
        }

        /// <summary>
        /// ��ñ�������б�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSmiliesTypes()
        {
            return Data.Smilies.GetSmiliesTypes();
        }


        public static DataTable GetSmilieByType(int typeId)
        {
            return typeId > 0 ? Data.Smilies.GetSmiliesInfoByType(typeId) : new DataTable();
        }

        /// <summary>
        /// ����յı������
        /// </summary>
        /// <returns>��������Ŀձ�������б�</returns>
        public static string ClearEmptySmiliesType()
        {
            string emptySmilieList = "";
            DataTable smilieType = Discuz.Data.Smilies.GetSmiliesTypes();
            foreach (DataRow dr in smilieType.Rows)
            {
                if (Discuz.Data.Smilies.GetSmiliesInfoByType(int.Parse(dr["id"].ToString())).Rows.Count == 0)
                {
                    emptySmilieList += dr["code"].ToString() + ",";
                    Discuz.Data.Smilies.DeleteSmilies(dr["id"].ToString());
                }
            }
            return emptySmilieList.TrimEnd(',');
        }

        /// <summary>
        /// ��ȡ�������Id
        /// </summary>
        /// <returns></returns>
        public static int GetMaxSmiliesId()
        {
            return Data.Smilies.GetMaxSmiliesId();
        }

        /// <summary>
        /// �õ����������
        /// </summary>
        /// <returns>���������</returns>
        public static DataTable GetSmilies()
        {
            return Data.Smilies.GetSmilies();
        }

        /// <summary>
        /// ����Ƿ�����ͬ����
        /// </summary>
        /// <param name="code">�������</param>
        /// <param name="currentid">����ID</param>
        /// <returns></returns>
        public static bool IsExistSameSmilieCode(string code, int currentid)
        {
            foreach (DataRow dr in Data.Smilies.GetSmiliesListDataTable().Rows)
            {
                if (dr["code"].ToString() == code && dr["id"].ToString() != currentid.ToString())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ������ɾ������
        /// </summary>
        /// <param name="type">����</param>
        public static void DeleteSmilyByType(int type)
        {
            if (type > 0)
                Data.Smilies.DeleteSmilyByType(type);
        }
    }//class
}
