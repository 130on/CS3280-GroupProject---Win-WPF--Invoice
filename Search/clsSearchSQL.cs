using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace GroupAssignmentAlonColetonWannes.Search
{
    public class clsSearchSQL
    {
        /// <summary>
        /// returns a sql that selects all invoices from DB
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string getAllInvoices()
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices ";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        
        /// <summary>
        /// returns a sql that selects one invoice
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string getOneInvoice(int invoiceNum)
        {
            try
            {
                string sSQL = "";

                    return sSQL;
            }
            catch (Exception ex) {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #region getOptions
        //public static List<int> getOptionsInvoiceNum()
        public static string getOptionsInvoiceNum()
        {
            try
            {
                string sSQL = "SELECT DISTINCT(InvoiceNum) From Invoices order by InvoiceNum";

                return sSQL;

                //int iInvoices = 0;
                //DataSet dsInvoiceNumbers = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);
                //List<int> invoiceNumbers = new();
                //foreach (DataRow row in dsInvoiceNumbers.Tables[0].Rows)
                //{
                //    invoiceNumbers.Add((int)row[0]);
                //}

                //return invoiceNumbers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        //public static List<DateTime> getOptionsInvoiceDate()
        public static string getOptionsInvoiceDate()
        {
            try
            {
                string sSQL = "SELECT DISTINCT(InvoiceDate) From Invoices order by InvoiceDate";

                return sSQL;
                //int iInvoices = 0;

                //DataSet dsInvoiceDates = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);
                //List<DateTime> invoiceDates = new();
                //foreach (DataRow row in dsInvoiceDates.Tables[0].Rows)
                //{
                //    invoiceDates.Add((DateTime)row[0]);
                //}

                //return invoiceDates;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        //public static List<int> getOptionsTotalCost() 
        public static string getOptionsTotalCost()
        {
            try
            {
                string sSQL = "SELECT DISTINCT(TotalCost) From Invoices order by TotalCost";

                return sSQL;
                //int iInvoices = 0;

                //DataSet dsTotalCost = clsDataAccess.ExecuteSQLStatement(sSQL, ref iInvoices);
                //List<int> totalCosts = new();
                //foreach (DataRow row in dsTotalCost.Tables[0].Rows)
                //{
                //    totalCosts.Add((int)row[0]);
                //}

                //return totalCosts;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion


    }
}
