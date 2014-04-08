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
       /// ÅÅĞò·½Ê½
       /// </summary>
       public int Orderby
       {

           set { _orderby = value; }
           get { return _orderby; }
       
       }
       /// <summary>
       /// °ïÖúID
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
       /// °ïÖú±êÌâ
       /// </summary>
       public string Title
       {
           set { _title = value; }
           get { return _title; }
       
       }
       /// <summary>
       /// °ïÖúÄÚÈİ
       /// </summary>
       public string Message
       {
           set { _message = value; }
           get { return _message; }

        }

       /// <summary>
       /// Ìû×ÓID
       /// </summary>
       public int Pid
       {
           set { _pid = value; }
           get { return _pid; }
       }
   }
}
