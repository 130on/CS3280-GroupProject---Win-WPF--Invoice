using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace GroupAssignmentAlonColetonWannes.Search
{
    static public class clsSearchSQL
    {
        public static List<invoiceDetail> loadInvoices(int? searchInvoiceNum = null, DateTime? searchDate = null, int? searchTotalCost = null)
        {

            int iInvoices = 0;
            string sSQL = "SELECT * FROM Invoices ";

            if(searchInvoiceNum != null || searchDate != null || searchTotalCost != null) {
                sSQL += "WHERE ";
            }
            string previous = "";
            if(searchInvoiceNum != null)
            {
                sSQL += $"{previous} InvoiceNum = {searchInvoiceNum} ";
                previous = "AND";
            }
            if(searchDate != null)
            {
                sSQL += $"{previous} InvoiceDate = #{searchDate}#";
                previous = "AND";
            }
            if(searchTotalCost != null)
            {
                sSQL += $"{previous} TotalCost = {searchTotalCost} ";
                previous = "AND";
            }

            DataSet dsInvoices = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);

            List<invoiceDetail> selectedItem = new();
            foreach(DataRow dataRow in dsInvoices.Tables[0].Rows)
            {
                int invoiceNum = (int)dataRow["InvoiceNum"];
                DateTime invoiceDate = (DateTime)dataRow["InvoiceDate"];
                int totalCost = (int)dataRow["TotalCost"];
                selectedItem.Add(new invoiceDetail(invoiceNum, invoiceDate, totalCost));
            }


            return selectedItem;
        }

        #region getOptions
        public static List<int> getOptionsInvoiceNum()
        {
           
            string sSQL = "SELECT DISTINCT(InvoiceNum) From Invoices order by InvoiceNum";

            int iInvoices = 0;
            DataSet dsInvoiceNumbers = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);
            List<int> invoiceNumbers = new();
            foreach (DataRow row in dsInvoiceNumbers.Tables[0].Rows)
            {
                invoiceNumbers.Add((int)row[0]);
            }

            return invoiceNumbers;
        }

        public static List<DateTime> getOptionsInvoiceDate()
        {
            string sSQL = "SELECT DISTINCT(InvoiceDate) From Invoices order by InvoiceDate";
            int iInvoices = 0;

            DataSet dsInvoiceDates = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);
            List<DateTime> invoiceDates = new();
            foreach (DataRow row in dsInvoiceDates.Tables[0].Rows)
            {
                invoiceDates.Add((DateTime)row[0]);
            }

            return invoiceDates;
        }

        public static List<int> getOptionsTotalCost()
        {
            string sSQL = "SELECT DISTINCT(TotalCost) From Invoices order by TotalCost";
            int iInvoices = 0;

            DataSet dsTotalCost = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);
            List<int> totalCosts = new();
            foreach (DataRow row in dsTotalCost.Tables[0].Rows)
            {
                totalCosts.Add((int)row[0]);
            }

            return totalCosts;
        }
        #endregion


    }
}
