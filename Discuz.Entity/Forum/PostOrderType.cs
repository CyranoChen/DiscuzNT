﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class PostOrderType
    {
        public enum OrderFiled
        { 
            Diggs,
            PostDateTime
        }

        private OrderFiled orderfield;

        public OrderFiled Orderfield
        {
            get { return orderfield; }
            set { orderfield = value; }
        }

        private OrderDirection orderdirection;

        public OrderDirection Orderdirection
        {
            get { return orderdirection; }
            set { orderdirection = value; }
        }
    }
}
