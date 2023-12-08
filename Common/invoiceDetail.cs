using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace GroupAssignmentAlonColetonWannes.Common
{
    public class invoiceDetail
    {
        #region Variables
        //identifier int id or number
        private int invoiceNum;
        //invoiceDate date object (optional?)
        private DateTime? invoiceDate;
        //int getTotalCost
        private int totalCost;
        //list of items on the list
        private ObservableCollection<itemDetail> invoiceItems = new();

        #endregion

        #region Get Statements
        public int InvoiceNum
        {
            get
            {
                try
                {
                    return invoiceNum;
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
                    invoiceNum = value;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }
        public DateTime? InvoiceDate
        {
            get
            {
                try
                {
                    return invoiceDate;
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
                    invoiceDate = value;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }

        public int TotalCost
        {
            get
            {
                try
                {
                    return totalCost;
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
                    totalCost = value;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }
        public ObservableCollection<itemDetail> InvoiceItems
        {
            get
            {
                try
                {
                    return invoiceItems;
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
        }

        #endregion



        public invoiceDetail(int invoiceNum, DateTime? invoiceDate, int totalCost)
        {
            try
            {
                this.invoiceNum = invoiceNum;
                this.invoiceDate = invoiceDate;
                this.totalCost = totalCost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
    }
}
