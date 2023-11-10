using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Common
{
    public class invoiceDetail
    {
        #region Variables
        //identifier int id or number
        private int invoiceNum;
        //invoiceDate date object (optional?)
        private DateTime invoiceDate;
        //int totalCost
        private int totalCost;
        //list of items on the list
        private BindingList<itemDetail> invoiceItems = new();

        #endregion

        #region Get Statements
        public int InvoiceNum
        {
            get { return invoiceNum; }
        }
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
        }

        public int TotalCost
        {
            get { return totalCost; }
        }
        public BindingList<itemDetail> InvoiceItems
        {
            get { return invoiceItems; }
        }
        #endregion

        public invoiceDetail(int invoiceNum, DateTime invoiceDate, int totalCost)
        {
            this.invoiceNum = invoiceNum;
            this.invoiceDate = invoiceDate;
            this.totalCost = totalCost;
        }
    }
}
