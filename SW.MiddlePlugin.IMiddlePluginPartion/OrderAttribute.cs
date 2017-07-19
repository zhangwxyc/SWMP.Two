using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SW.MiddlePlugin.IMiddlePluginPartion
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute : Attribute
    {
        public int OrderIndex { get; set; }

        public OrderAttribute(int orderIndex)
        {
            OrderIndex = orderIndex;
        }
    }
}
