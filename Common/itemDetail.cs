using System;
using System.Reflection;

namespace GroupAssignmentAlonColetonWannes.Common
{
    public class itemDetail
    {
        #region Variables and Get statements
        //Item code string 
        private readonly string itemCode;

        public string ItemCode
        {
            get
            {
                try
                {
                    return itemCode;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }
        //item desc string 
        private string itemDesc;
        public string ItemDesc
        {
            get
            {
                try
                {
                    return itemDesc;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
            set
            {
                try
                {
                    itemDesc = value;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }
        //Item cost decimal
        private decimal cost;

        public decimal Cost
        {
            get
            {
                try
                {
                    return cost;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
            set
            {
                try
                {
                    cost = value;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }


        private int? lineItemNum;

        public int? LineItemNum
        {
            get
            {
                try
                {
                    return lineItemNum;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
            set
            {
                try
                {
                    lineItemNum = value;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }
        #endregion

        public itemDetail(string itemCode, string itemDesc, decimal cost, int? lineItemNum = null)
        {

            try
            {
                this.itemCode = itemCode;
                this.itemDesc = itemDesc;
                this.cost = cost;
                this.lineItemNum = lineItemNum;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public override string ToString()
        {

            try
            {
                return $"{ItemDesc}";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
