using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Items
{
    public class clsItemsLogic
    {
        private static BindingList<itemDetail> itemList = new BindingList<itemDetail>();
        public static BindingList<itemDetail> ItemList
        {
            get
            {
                return itemList;
            }
        }
    }
}
