using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GroupAssignmentAlonColetonWannes.Common
{
    public class invoiceDetail
    {
        #region Variables
        //identifier int id or number
        private int invoiceNum;
        //invoiceDate date object (optional?)
        private DateTime invoiceDate;
        //int getTotalCost
        private int totalCost;
        //list of items on the list
        private ObservableCollection<itemDetail> invoiceItems = new ();

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
        public ObservableCollection<itemDetail> InvoiceItems
        {
            get { return invoiceItems; }
        }

        public ObservableCollection<itemDetail> InvoiceItemsSorted
        {
            get { return (ObservableCollection<itemDetail>)invoiceItems.OrderBy(i => i.LineItemNum); }
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
