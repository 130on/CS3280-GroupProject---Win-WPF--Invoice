using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Search
{
    public class clsSearchLogic
    {
        // declare and instantiate a binding list to hold the data for the datagrid
        private static BindingList<invoiceDetail>? gridInvoiceList = new BindingList<invoiceDetail>();

        /// <summary>
        /// query the DB and stores the results in a bindinglist
        /// </summary>
        /// <param name="searchInvoiceNum"></param>
        /// <param name="searchDate"></param>
        /// <param name="searchTotalCost"></param>
        /// <returns>returns a bindingList that contains all rows from Invoices table </returns>
        public static BindingList<invoiceDetail> loadInvoices(int? searchInvoiceNum = null, DateTime? searchDate = null, int? searchTotalCost = null)
        {
            // number of dataset rows
            int iInvoices = 0;

            string sqlCommand = clsSearchSQL.getAllInvoices();

            DataSet dsInvoices = clsDataAccess.ExecuteSQLStatement(sqlCommand, ref iInvoices);

            BindingList<invoiceDetail> selectedItem = new();
            foreach (DataRow dataRow in dsInvoices.Tables[0].Rows)
            {
                int invoiceNum = (int)dataRow["InvoiceNum"];
                DateTime invoiceDate = (DateTime)dataRow["InvoiceDate"];
                int totalCost = (int)dataRow["TotalCost"];
                selectedItem.Add(new invoiceDetail(invoiceNum, invoiceDate, totalCost));
            }

            return selectedItem;
        }

        /// <summary>
        /// loads the Invoices table into bindinglist
        /// </summary>
        /// <returns>returns a bindingList that contains the rows from Invoices tables</returns>
        /// <exception cref="Exception"></exception>
        public static BindingList<invoiceDetail> loadList()
        {
            try
            {
                // load Invoices to Datagrid using a bindingList generated from DB 
                gridInvoiceList = clsSearchLogic.loadInvoices();
                
                return gridInvoiceList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sort the list based on total cost from small to large using LINQ method
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BindingList<invoiceDetail> sortList()
        {
            try
            {
                BindingList<invoiceDetail> sortedList;
                sortedList = new BindingList<invoiceDetail>(gridInvoiceList.OrderBy(item => item.TotalCost).ToList());

                return displayDistinctTot(sortedList);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// filters a list based on distinct total cost values
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BindingList <invoiceDetail> displayDistinctTot(BindingList<invoiceDetail> list)
        {
            try
            {
                var distinctList = new BindingList<invoiceDetail>(
                        list
                            .GroupBy(invoice => invoice.TotalCost)  // Group by TotalCost
                            .Select(group => group.First())        // Select the first item from each group
                            .ToList()
                    );
                return distinctList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// filters the list of invoices in the datagrid based on the invoice number selected in the combo box
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BindingList <invoiceDetail> filterGridByInvoiceNum(int invoiceNum)
        {
            try
            {
                var filteredList = new BindingList<invoiceDetail>(gridInvoiceList.Where
                        (invoice => invoice.InvoiceNum == invoiceNum).Distinct().ToList());

                return filteredList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// filters the list of invoices in the datagrid based on the total cost and/or date selected in the datagrid
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BindingList <invoiceDetail> filterGridBySelections(int? totalCost, DateTime? date)
        {
            try
            {
                if (totalCost != null && date == null)
                {
                    var filteredList = new BindingList<invoiceDetail>(gridInvoiceList.Where
                            (invoice => invoice.TotalCost == totalCost).Distinct().ToList());

                    return filteredList;
                }
                else if (totalCost == null && date != null)
                {
                    var filteredList = new BindingList<invoiceDetail>(gridInvoiceList.Where
                            (invoice => invoice.InvoiceDate == date).Distinct().ToList());

                    return filteredList;
                }
                else
                {
                    var filteredList = new BindingList<invoiceDetail>(gridInvoiceList.
                                        Where(invoice => invoice.TotalCost == totalCost &&
                                        invoice.InvoiceDate == date).Distinct().ToList());
                    return filteredList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
