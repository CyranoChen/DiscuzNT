using System;
using System.Text;

namespace Discuz.Entity
{
   

   public class HelpInfo
   {
       private int _id;
       private string _title;
       private string _message;
       private int _pid;
       private int _orderby;

       /// <summary>
       /// ����ʽ
       /// </summary>
       public int Orderby
       {

           set { _orderby = value; }
           get { return _orderby; }
       
       }
       /// <summary>
       /// ����ID
       /// </summary>
       public int Id
       {
           set {
               _id = value;
           }
           get {
               return _id;   
           }
      }
       /// <summary>
       /// ��������
       /// </summary>
       public string Title
       {
           set { _title = value; }
           get { return _title; }
       
       }
       /// <summary>
       /// ��������
       /// </summary>
       public string Message
       {
           set { _message = value; }
           get { return _message; }

        }

       /// <summary>
       /// ����ID
       /// </summary>
       public int Pid
       {
           set { _pid = value; }
           get { return _pid; }
       }
   }
}
