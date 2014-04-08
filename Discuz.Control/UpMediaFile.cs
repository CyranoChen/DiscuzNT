using System;
using System.Web;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;


namespace Discuz.Control
{
    /// <summary>
    /// 上传图片控件
    /// </summary>
    [ToolboxItem(true), DefaultProperty("UpFilePath"), ToolboxData("<{0}:UpFile runat=server></{0}:UpFile>")]
    public class UpFile : Discuz.Control.WebControl
    {
        /// <summary>
        /// TextBox 控件变量
        /// </summary>
        protected System.Web.UI.WebControls.TextBox tb = new System.Web.UI.WebControls.TextBox();

        /// <summary>
        /// Label 控件变量
        /// </summary>
        protected System.Web.UI.WebControls.Label Msglabel = new Label();

        /// <summary>
        /// Button 控件变量
        /// </summary>
        protected System.Web.UI.WebControls.Button UploadButton = new System.Web.UI.WebControls.Button();

        /// <summary>
        /// FileUpload控件变量
        /// </summary>
        protected System.Web.UI.HtmlControls.HtmlInputFile FileUpload = new HtmlInputFile();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpFile()
        {
            FileUpload.Size = 32;
            this.Controls.Add(FileUpload);

            UploadButton.Text = "上传";
            this.Controls.Add(UploadButton);
            this.Controls.Add(Msglabel);

            FileUpload.Attributes.Add("onfocus", "this.className='colorfocus';");
            FileUpload.Attributes.Add("onblur", "this.className='colorblur';");
            FileUpload.Attributes.Add("Class", "colorblur");

            tb.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
            tb.Width = 285;
            tb.Attributes.Add("onfocus", "this.className='colorfocus';");
            tb.Attributes.Add("onblur", "this.className='colorblur';");
            tb.Attributes.Add("rows", "2");
            tb.Attributes.Add("cols", "53");
            tb.CssClass = "colorblur";
            this.Controls.Add(tb);

            this.Width = 350;
            this.Height = 30;
            this.BorderStyle = BorderStyle.Dotted;
            this.BorderWidth = 0;

            UploadButton.Click += new EventHandler(UpFile_Click);

        }

        /// <summary>
        /// 文件内容属性
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Text
        {
            get
            {
                return this.tb.Text;
            }

            set
            {
                this.tb.Text = value;
            }
        }

        private string m_waterMarkText = null;
        /// <summary>
        /// 进行水印所使用的文字
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null)]
        public string WaterMarkText 
        {
            get
            {
                return m_waterMarkText;
            }

            set
            {
                m_waterMarkText = value;
            }
        }

        /// <summary>
        /// 要上传的文件路径
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string UpFilePath
        {
            get
            {
                object o = ViewState["RequiredFieldType"];
                return (o == null) ? "" : o.ToString();
            }
            set
            {
                ViewState["RequiredFieldType"] = value;
            }
        }

        /// <summary>
        /// 上传后所记入数据库时所引用的路径
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HttpPath
        {
            get
            {
                object o = ViewState["HttpPath"];
                return (o == null) ? "" : o.ToString();
            }
            set
            {
                ViewState["HttpPath"] = value;
            }
        }

        /// <summary>
        /// 要上传的文件的类型，如：.gif , .jpg
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("jpg,gif,")]
        public string FileType
        {
            get
            {
                object o = ViewState["FileType"];
                return (o == null) ? "" : o.ToString();
            }
            set
            {
                ViewState["FileType"] = value;
            }
        }


        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("不生成缩略图"), TypeConverter(typeof(ThumbnailImageConverter)), Description("要滚动的对象。")]
        public string ThumbnailImage
        {
            get
            {
                object o = ViewState["ThumbnailImage"];
                return (o == null) ? "" : o.ToString();
            }
            set
            {
                ViewState["ThumbnailImage"] = value;
            }
        }

        /// <summary>
        /// 是否显示上传的文本框
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("true")]
        public bool IsShowTextArea
        {
            get
            {
                object o = ViewState["IsShowTextArea"];

                bool result;
                if (o == null)
                {
                    result = true;
                }
                else
                {
                    if (o.ToString() == "True")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                return result;
            }
            set
            {
                ViewState["IsShowTextArea"] = value;
                tb.Visible = value;
                UploadButton.Visible = value;
            }
        }

        /// <summary> 
        /// 输出html,在浏览器中显示控件
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {
            //output.Write(Text);
            //RenderChildren(output);
            CreateChildControls();

            if (this.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }

            base.Render(output);

            if (this.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
        }

        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            UploadButton.Click += new EventHandler(UpFile_Click);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public string UpdateFile()
        {
            string sSavePath = UpFilePath;
            if (FileUpload.PostedFile != null)
            {
                HttpPostedFile myFile = FileUpload.PostedFile;
                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                {
                    Msglabel.Text = "<br /><font color=red>没有选定被上传的文件</font></b>";
                    return "";
                }

                if (FileType.IndexOf(System.IO.Path.GetExtension(myFile.FileName).ToLower()) < 0)
                {
                    Msglabel.Text = "<br /><font color=red>文件必须是以" + FileType.Replace("|", " , ") + "为扩展名的文件</font></b>";
                    return "";
                }

                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);

                DateTime filenamedate = DateTime.Now;
                string sFilename = filenamedate.ToString("yyyy-MM-dd") + "_" + filenamedate.Hour.ToString() + "-" + filenamedate.Minute.ToString() + "-" + filenamedate.Second.ToString() + System.IO.Path.GetExtension(myFile.FileName).ToLower();
                int file_append = 0;
                while (System.IO.File.Exists(sSavePath + sFilename))
                {
                    file_append++;
                    sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + System.IO.Path.GetExtension(myFile.FileName).ToLower();
                }

            
                //上传图片文件jpg,gif
                if ((System.IO.Path.GetExtension(myFile.FileName).ToLower() == ".jpg") || (System.IO.Path.GetExtension(myFile.FileName).ToLower() == ".gif"))
                {   
                    Bitmap myBitmap;
                    try
                    {
                        System.IO.FileStream newFile = new System.IO.FileStream(sSavePath + sFilename, System.IO.FileMode.Create);
                        newFile.Write(myData, 0, myData.Length);
                        newFile.Close();

                        myBitmap = new Bitmap(sSavePath + sFilename);

                        file_append = 0;
                        Text = HttpPath + sFilename;

                        if (ThumbnailImage == "生成缩略图")
                        {
                            int width = myBitmap.Width / 3;
                            int height = myBitmap.Height / 3;
                            string sFileThumname = filenamedate.ToShortDateString() + "_" + filenamedate.Hour.ToString() + "-" + filenamedate.Minute.ToString() + "-" + filenamedate.Second.ToString() + "thum" + System.IO.Path.GetExtension(myFile.FileName).ToLower();

                            GetThumbnailImage(width, height, width / 2 - 60, height - 20, sSavePath + sFilename, sSavePath + sFileThumname);
                        }

                        myBitmap.Dispose();
                        Msglabel.Text = "上传成功！";
                        return Text;
                    }
                    catch (ArgumentException errArgument)
                    {

                        Msglabel.Text = errArgument.ToString();

                        //System.IO.File.Delete(sSavePath + sFilename);
                        //return "";
                    }
                }
                else  //上传除图片文件以外的全部文件
                {
                    myFile.SaveAs(sSavePath + sFilename);

                    try
                    {
                        Text = HttpPath + sFilename;
                        return Text;
                    }
                    catch (ArgumentException errArgument)
                    {
                        Msglabel.Text = errArgument.ToString();
                        System.IO.File.Delete(sSavePath + sFilename);
                        return "";
                    }

                }
            }
            return "";
        }

        /// <summary>
        /// 上传单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpFile_Click(object sender, System.EventArgs e)
        {
            UpdateFile();
        }


        /// <summary>
        /// 生成缩略图函数
        /// </summary>
        /// <param name="width">原始图片的宽度</param>
        /// <param name="height">原始图片的高度</param>
        /// <param name="left">水印字符的生成位置</param>
        /// <param name="right">水印字符的生成位置</param>
        /// <param name="picpath">原图的所在路径</param>
        /// <param name="picthumpath">生成缩略图的所在路径</param>
        public void GetThumbnailImage(int width, int height, int left, int right, string picpath, string picthumpath)
        {
            string newfile = picthumpath;
            System.Drawing.Image oldimage = System.Drawing.Image.FromFile(picpath);
            System.Drawing.Image thumbnailImage =
                oldimage.GetThumbnailImage(width, height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            //Response.Clear();
            Bitmap output = new Bitmap(thumbnailImage);
            Graphics g = Graphics.FromImage(output);

            if ((WaterMarkText == null) || (WaterMarkText == ""))
                g.DrawString(null, new Font("Courier New", 14), new SolidBrush(Color.White), left, right);
            else
                g.DrawString(WaterMarkText, new Font("Courier New", 14), new SolidBrush(Color.Blue), left, right);//写入文字到图片中

            output.Save(picthumpath, System.Drawing.Imaging.ImageFormat.Jpeg);
            output.Dispose();
            //Response.ContentType = "image/gif";
        }


        public bool ThumbnailCallback()
        {
            return true;
        }

    }

    /// <summary>
    /// 缩略图选项转换器
    /// </summary>
    public class ThumbnailImageConverter : StringConverter
    {
        public ThumbnailImageConverter() { }

        /// <summary>
        /// 下拉列表编辑属性 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 获取标准值列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ArrayList controlsArray = new ArrayList();
            controlsArray.Add("不生成缩略图");
            controlsArray.Add("生成缩略图");

            return new StandardValuesCollection(controlsArray);

        }
        
        /// <summary>
        /// return ture的话只能选，return flase可选可填 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
