using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Common
{
    public class itemDetail
    {
        #region Variables and Get statements
        //Item code string 
        private readonly string itemCode;

        public string ItemCode
        {
            get { return itemCode; }
        }
        //item desc string 
        private string itemDesc;
        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }
        //Item cost decimal
        private decimal cost;

        public decimal Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        #endregion

        public itemDetail(string itemCode, string itemDesc, decimal cost) { 
            this.itemCode = itemCode;
            this.itemDesc = itemDesc;
            this.cost = cost;
        }

        public override string ToString() {
            return $"{ItemDesc}";
        }

    }
}
