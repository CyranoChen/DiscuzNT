namespace Discuz.Control
{
    using System;
    using System.Collections;
    using System.Web.UI;

    /// <summary>
    /// 属性页集合类
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public sealed class TabPageCollection : CollectionBase,IList
    {

        private TabControl _tabControl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="i_tabControl"></param>
        internal TabPageCollection(TabControl i_tabControl)
        {
            this._tabControl = i_tabControl;
        }

        /// <summary>
        /// 添加属性页
        /// </summary>
        /// <param name="pTab"></param>
        public void Add(TabPage pTab)
        {
            base.List.Add(pTab);
        }

        /// <summary>
        /// 是否包含属性页
        /// </summary>
        /// <param name="pTab"></param>
        /// <returns></returns>
        public bool Contains(TabPage pTab)
        {
            return base.List.Contains(pTab);
        }

        /// <summary>
        /// 拷页属性页
        /// </summary>
        /// <param name="pArray"></param>
        /// <param name="pIndex"></param>
        public void CopyTo(TabPage[] pArray, int pIndex)
        {
            base.List.CopyTo(pArray, pIndex);
        }

        public int IndexOf(TabPage pItem)
        {
            return base.List.IndexOf(pItem);
        }

        /// <summary>
        /// 插入指定位置的属性页
        /// </summary>
        /// <param name="pIndex"></param>
        /// <param name="pItem"></param>
        public void Insert(int pIndex, TabPage pItem)
        {
            base.List.Insert(pIndex, pItem);
        }

		/// <summary>
		/// 重写插入操作
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
        protected override void OnInsert(int index, object value)
        {
            if (value.GetType() != Type.GetType("Discuz.Control.TabPage"))
            {
                throw new ArgumentException("插入的子项类型必须为 [TabPage]");
            }
        }

        /// <summary>
        /// 重写插入完成操作
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnInsertComplete(int index, object value)
        {
            if (value.GetType() == Type.GetType("Discuz.Control.TabPage"))
            {
                ((TabPage)value).SetTabControl(this._tabControl);
            }
        }

        /// <summary>
        /// 重写移除操作
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnRemove(int index, object value)
        {
            if (value.GetType() != Type.GetType("Discuz.Control.TabPage"))
            {
                throw new ArgumentException("移除的子项类型必须为 [TabPage]");
            }
        }

        /// <summary>
        /// 重写设置操作
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue.GetType() != Type.GetType("Discuz.Control.TabPage"))
            {
                throw new ArgumentException("类型必须为 [TabPage]");
            }
        }

        /// <summary>
        /// 校验属性页类型
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValidate(object value)
        {
            if (value.GetType() != Type.GetType("Discuz.Control.TabPage"))
            {
                throw new ArgumentException("类型必须为 [TabPage]");
            }
        }
		
        /// <summary>
        /// 移除指定属性页
        /// </summary>
        /// <param name="pTab"></param>
        public void Remove(TabPage pTab)
        {
            base.List.Remove(pTab);
        }

        /// <summary>
        /// 属性页实例索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TabPage this[int index]
        {
            get
            {
                return (TabPage) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

    }
}

