using GroupAssignmentAlonColetonWannes.Common;
using GroupAssignmentAlonColetonWannes.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Main
{
    public static class clsMainSQL
    {


        public static bool updateTotalCost(int invoiceNumber, int newTotalCost)
        {
            string sSQL = $"UPDATE Invoices SET TotalCost = {newTotalCost} WHERE InvoiceNum = {invoiceNumber}";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);

            return rowsUpdated > 0 ? true : false;
        }

        public static bool newItemInInvoice(int invoiceNumber, int newLineItemNum, string newItemCode)
        {
            string sSQL = $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({invoiceNumber}, {newLineItemNum}, '{newItemCode}')";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);

            return rowsUpdated > 0 ? true : false;
        }

        public static int newInvoice(DateTime newDateTime, int newTotalCost = 0)
        {
            string sSQL = $"INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#{newDateTime}#, {newTotalCost})";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);
            if(rowsUpdated > 0)
            {
                sSQL = "SELECT MAX(InvoiceNum) FROM Invoices";
                bool res = int.TryParse(clsDataAccess.ExecuteScalarSQL(sSQL), out int newestInvoiceNumber);
             
                return res ? newestInvoiceNumber : -1;
            }

            return -1;
        }

        public static string getInvoice(int invoiceNumber)
        {
            string sSQL = $"SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {invoiceNumber}";

            return clsDataAccess.ExecuteScalarSQL(sSQL);
        }
         
        public static Dictionary<int, string> getInvoiceItems(int invoiceNumber)
        {
            string sSQL = $"Select LineItemNum, ItemCode FROM LineItems WHERE InvoiceNum = {invoiceNumber}";
            int iItemCounter = 0;   //Number of return values
            DataSet dsInvoiceItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);
            Dictionary<int, string> invoiceItems = new();
            foreach (DataRow row in dsInvoiceItems.Tables[0].Rows)
            {
                int lineItemNum = (int)row["LineItemNum"];
                string ItemCode = (string)row["ItemCode"];
                invoiceItems.Add(lineItemNum, ItemCode);
            }
           
            return invoiceItems;
        }

        public static string getItem(string itemCode)
        {
            string sSQL = $"select ItemCode, ItemDesc, Cost from ItemDesc WHERE = '{itemCode}'";
            return clsDataAccess.ExecuteScalarSQL(sSQL);
        }

        public static List<itemDetail> getItemsList()
        {
            List <itemDetail>  items = new();
           
            int iItemCounter = 0;   //Number of return values
            string sSQL = "select ItemCode, ItemDesc, Cost from ItemDesc";
            DataSet dsItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);

            foreach(DataRow itemRow in dsItems.Tables[0].Rows)
            {
                string itemCode = (string)itemRow["ItemCode"];
                string itemDesc = (string)itemRow["ItemDesc"];
                decimal cost = (decimal)itemRow["Cost"];

                items.Add(new itemDetail(itemCode, itemDesc, cost));
            }
            
            return items;
        }

        //might still need the delete ones but the one I have in the doc is incorrect 
        public static bool removeItem(int invoiceNum, int lineNumber, string itemCode)
        {
            string sSQL = $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNum} AND lineNumber = {lineNumber} AND itemCode = {itemCode}";

            return false;
        }
    }
}
