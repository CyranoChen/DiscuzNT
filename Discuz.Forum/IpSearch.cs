using System;
using System.IO;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;

namespace Discuz.Forum
{
	/// <summary>
	/// 判断IP归属地类
	/// </summary>
	public class IpSearch
	{
        private static object lockHelper = new object();

		static PHCZIP pcz = new PHCZIP();

		static string filePath = "";
		
		static bool fileIsExsit = true;

		static IpSearch()
		{
			string ipdatadir = BaseConfigs.GetForumPath + "ipdata";
			if (!Directory.Exists(Utils.GetMapPath(ipdatadir)))
			{
				Utils.CreateDir(Utils.GetMapPath(ipdatadir));
			}

			filePath = Utils.GetMapPath(ipdatadir + "/ipdata.config");
			fileIsExsit = Utils.FileExists(filePath);
			
			if(fileIsExsit)
			{
				pcz.SetDbFilePath(filePath);
			}
		}

		/// <summary>
		/// 返回IP查找结果
		/// </summary>
		/// <param name="IPValue">要查找的IP地址</param>
		/// <returns></returns>
        public static string  GetAddressWithIP(string IPValue)
        {
            lock (lockHelper)
            {
                string result = pcz.GetAddressWithIP(IPValue.Trim());

                if (fileIsExsit)
                {
                    if (result.IndexOf("IANA") >= 0)
                        return "";
                    else
                        return result;
                }
                else
                    return null;
            }
        }

		/// <summary>
		/// 辅助类，用于保存IP索引信息
		/// </summary>
		///     
		public class CZ_INDEX_INFO
		{
			public UInt32 IpSet;
			public UInt32 IpEnd;
			public UInt32 Offset;

			public CZ_INDEX_INFO()
			{
				IpSet = 0;
				IpEnd = 0;
				Offset = 0;
			}
		}

		//读取纯真IP数据库类
		public class PHCZIP
		{
			protected bool bFilePathInitialized;
			protected string FilePath;
			protected FileStream FileStrm;
			protected UInt32 Index_Set;
			protected UInt32 Index_End;
			protected UInt32 Index_Count;
			protected UInt32 Search_Index_Set;
			protected UInt32 Search_Index_End;
			protected CZ_INDEX_INFO Search_Set;
			protected CZ_INDEX_INFO Search_Mid;
			protected CZ_INDEX_INFO Search_End;

			public PHCZIP()
			{
				bFilePathInitialized = false;
			}

			public PHCZIP(string dbFilePath)
			{
				bFilePathInitialized = false;
				SetDbFilePath(dbFilePath);
			}

			//使用二分法查找索引区，初始化查找区间
			public void Initialize()
			{
				Search_Index_Set = 0;
				Search_Index_End = Index_Count - 1;
			}

			//关闭文件
			public void Dispose()
			{
				if (bFilePathInitialized)
				{
					bFilePathInitialized = false;
					FileStrm.Close();
					//FileStrm.Dispose();
				}
			}


			public bool SetDbFilePath(string dbFilePath)
			{
				if (dbFilePath == "")
					return false;

				try
				{
					FileStrm = new FileStream(dbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch 
				{
					return false;
				}
				//检查文件长度
				if (FileStrm.Length < 8)
				{
					FileStrm.Close();
					//FileStrm.Dispose();
					return false;
				}
				//得到第一条索引的绝对偏移和最后一条索引的绝对偏移
				FileStrm.Seek(0, SeekOrigin.Begin);
				Index_Set = GetUInt32();
				Index_End = GetUInt32();

				//得到总索引条数
				Index_Count = (Index_End - Index_Set) / 7 + 1;
				bFilePathInitialized = true;

				return true; 
			}

			public string GetAddressWithIP(string IPValue)
			{
				if (!bFilePathInitialized)
					return "";

				Initialize();
				UInt32 ip = IPToUInt32(IPValue);

				while (true)
				{

					//首先初始化本轮查找的区间

					//区间头
					Search_Set = IndexInfoAtPos(Search_Index_Set);
					//区间尾
					Search_End = IndexInfoAtPos(Search_Index_End);

					//判断IP是否在区间头内
					if (ip >= Search_Set.IpSet && ip <= Search_Set.IpEnd)
						return ReadAddressInfoAtOffset(Search_Set.Offset);


					//判断IP是否在区间尾内
					if (ip >= Search_End.IpSet && ip <= Search_End.IpEnd)
						return ReadAddressInfoAtOffset(Search_End.Offset);

					//计算出区间中点
					Search_Mid = IndexInfoAtPos((Search_Index_End + Search_Index_Set) / 2);

					//判断IP是否在中点
					if (ip >= Search_Mid.IpSet && ip <= Search_Mid.IpEnd)
						return ReadAddressInfoAtOffset(Search_Mid.Offset);

					//本轮没有找到，准备下一轮
					if (ip < Search_Mid.IpSet)
						//IP比区间中点要小，将区间尾设为现在的中点，将区间缩小1倍。
						Search_Index_End = (Search_Index_End + Search_Index_Set) / 2;
					else
						//IP比区间中点要大，将区间头设为现在的中点，将区间缩小1倍。
						Search_Index_Set = (Search_Index_End + Search_Index_Set) / 2;
				}
			}

			private string ReadAddressInfoAtOffset(UInt32 Offset)
			{
				string country = "";
				string area = "";
				UInt32 country_Offset = 0;
				byte Tag = 0;
				//跳过4字节，因这4个字节是该索引的IP区间上限。
				FileStrm.Seek(Offset + 4, SeekOrigin.Begin);

				//读取一个字节，得到描述国家信息的“寻址方式”
				Tag = GetTag();

				if (Tag == 0x01)
				{
					//模式0x01，表示接下来的3个字节是表示偏移位置
					FileStrm.Seek(GetOffset(), SeekOrigin.Begin);

					//继续检查“寻址方式”
					Tag = GetTag();
					if (Tag == 0x02)
					{
						//模式0x02，表示接下来的3个字节代表国家信息的偏移位置
						//先将这个偏移位置保存起来，因为我们要读取它后面的地区信息。
						country_Offset = GetOffset();
						//读取地区信息（注：按照Luma的说明，好像没有这么多种可能性，但在测试过程中好像有些情况没有考虑到，
						//所以写了个ReadArea()来读取。
						area = ReadArea();
						//读取国家信息
						FileStrm.Seek(country_Offset, SeekOrigin.Begin);
						country = ReadString();
					}
					else
					{
						//这种模式说明接下来就是保存的国家和地区信息了，以'\0'代表结束。
						FileStrm.Seek(-1, SeekOrigin.Current);
						country = ReadString();
						area = ReadArea();

					}
				}
				else if (Tag == 0x02)
				{
					//模式0x02，说明国家信息是一个偏移位置
					country_Offset = GetOffset();
					//先读取地区信息
					area = ReadArea();
					//读取国家信息
					FileStrm.Seek(country_Offset, SeekOrigin.Begin);
					country = ReadString();
				}
				else
				{
					//这种模式最简单了，直接读取国家和地区就OK了
					FileStrm.Seek(-1, SeekOrigin.Current);
					country = ReadString();
					area = ReadArea();
				}
                return country + " " + area;
			}

			private UInt32 GetOffset()
			{
				byte[] TempByte4 = new byte[4];
				TempByte4[0] = (byte)FileStrm.ReadByte();
				TempByte4[1] = (byte)FileStrm.ReadByte();
				TempByte4[2] = (byte)FileStrm.ReadByte();
				TempByte4[3] = 0;
				return BitConverter.ToUInt32(TempByte4, 0);
			}

			protected string ReadArea()
			{
				byte Tag = GetTag();

				if (Tag == 0x01 || Tag == 0x02)
					FileStrm.Seek(GetOffset(), SeekOrigin.Begin);
				else
					FileStrm.Seek(-1, SeekOrigin.Current);

                return ReadString();
			}

			protected string ReadString()
			{
				UInt32 Offset = 0;
				byte[] TempByteArray = new byte[256];
				TempByteArray[Offset] = (byte)FileStrm.ReadByte();
				while (TempByteArray[Offset] != 0x00)
				{
					Offset += 1;
					TempByteArray[Offset] = (byte)FileStrm.ReadByte();
				}
				return System.Text.Encoding.GetEncoding("GB2312").GetString(TempByteArray).TrimEnd('\0');
			}

			protected byte GetTag()
			{
				return (byte)FileStrm.ReadByte();
			}

			protected CZ_INDEX_INFO IndexInfoAtPos(UInt32 Index_Pos)
			{
				CZ_INDEX_INFO Index_Info = new CZ_INDEX_INFO();
				//根据索引编号计算出在文件中在偏移位置
				FileStrm.Seek(Index_Set + 7 * Index_Pos, SeekOrigin.Begin);
				Index_Info.IpSet = GetUInt32();
				Index_Info.Offset = GetOffset();
				FileStrm.Seek(Index_Info.Offset, SeekOrigin.Begin);
				Index_Info.IpEnd = GetUInt32();

				return Index_Info;
			}

			/// <summary>
			/// 从IP转换为Int32
			/// </summary>
			/// <param name="IpValue"></param>
			/// <returns></returns>
			public UInt32 IPToUInt32(string IpValue)
			{
				string[] IpByte = IpValue.Split('.');
				Int32 nUpperBound = IpByte.GetUpperBound(0);
				if (nUpperBound != 3)
				{
					IpByte = new string[4];
					for (Int32 i = 1; i <= 3 - nUpperBound; i++)
						IpByte[nUpperBound + i] = "0";
				}

				byte[] TempByte4 = new byte[4];
				for (Int32 i = 0; i <= 3; i++)
				{
				    if (IsNumeric(IpByte[i]))
						TempByte4[3 - i] = (byte)(Convert.ToInt32(IpByte[i]) & 0xff);
				}

				return BitConverter.ToUInt32(TempByte4, 0);
			}

			/// <summary>
			/// 判断是否为数字
			/// </summary>
			/// <param name="str">待判断字符串</param>
			/// <returns></returns>
			protected bool IsNumeric(string str)
			{
				if (str != null && System.Text.RegularExpressions.Regex.IsMatch(str, @"^-?\d+$"))
					return true;
				else
					return false;
			}

			protected UInt32 GetUInt32()
			{
				byte[] TempByte4 = new byte[4];
				FileStrm.Read(TempByte4, 0, 4);
				return BitConverter.ToUInt32(TempByte4, 0);
			}
		}
	}
}
