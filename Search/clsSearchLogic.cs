using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Search
{
    public class clsSearchLogic
    {

        public static List<invoiceDetail> loadInvoices(int? searchInvoiceNum = null, DateTime? searchDate = null, int? searchTotalCost = null)
        {
            // number of dataset rows
            int iInvoices = 0;

            string sqlCommand = clsSearchSQL.getAllInvoices();
            // FIXME - connct to the clsSearchSQL

            
            //if (searchInvoiceNum != null || searchDate != null || searchTotalCost != null)
            //{
            //    sSQL += "WHERE ";
            //}
            //string previous = "";
            //if (searchInvoiceNum != null)
            //{
            //    sSQL += $"{previous} InvoiceNum = {searchInvoiceNum} ";
            //    previous = "AND";
            //}
            //if (searchDate != null)
            //{
            //    sSQL += $"{previous} InvoiceDate = #{searchDate}#";
            //    previous = "AND";
            //}
            //if (searchTotalCost != null)
            //{
            //    sSQL += $"{previous} TotalCost = {searchTotalCost} ";
            //    previous = "AND";
            //}

            DataSet dsInvoices = clsDataAccess.ExecuteSQLStatement(sqlCommand, ref iInvoices);

            List<invoiceDetail> selectedItem = new();
            foreach (DataRow dataRow in dsInvoices.Tables[0].Rows)
            {
                int invoiceNum = (int)dataRow["InvoiceNum"];
                DateTime invoiceDate = (DateTime)dataRow["InvoiceDate"];
                int totalCost = (int)dataRow["TotalCost"];
                selectedItem.Add(new invoiceDetail(invoiceNum, invoiceDate, totalCost));
            }


            return selectedItem;
        }
        // static var that stores the invoice ID

        // storeId()

        // displayInvoices() - in datagrid 

        // resetForm() - reset the search window


    }
}
