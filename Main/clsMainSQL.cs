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

        /// <summary>
        /// Takes an invoiceNumber and the new cost and updates in the database
        /// </summary>
        /// <param name="invoiceNumber">The invoiceNumber as an int ex 5000</param>
        /// <param name="newTotalCost">The new cost that should be updated as int ex 700</param>
        /// <returns>True if a row got updated or false if nothing changed</returns>
        public static bool updateTotalCost(int invoiceNumber, int newTotalCost)
        {
            string sSQL = $"UPDATE Invoices SET TotalCost = {newTotalCost} WHERE InvoiceNum = {invoiceNumber}";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);

            return rowsUpdated > 0 ? true : false;
        }

        /// <summary>
        /// Takes an invoiceNumber, the row the item is at and a item code, and adds it to the database
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="newLineItemNum"></param>
        /// <param name="newItemCode"></param>
        /// <returns>True if a row got updated or false if nothing changed</returns>
        public static bool newItemInInvoice(int invoiceNumber, int newLineItemNum, string newItemCode)
        {
            string sSQL = $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({invoiceNumber}, {newLineItemNum}, '{newItemCode}')";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);

            return rowsUpdated > 0 ? true : false;
        }

        /// <summary>
        /// Create a new invoice with starting values of the given time
        /// </summary>
        /// <param name="newDateTime">The time the invoice is as DateTIme can be null</param>
        /// <param name="newTotalCost">The cost of the invoice default 0 </param>
        /// <returns>An integer whether it working or not -1 didn't work else 1</returns>
        public static int newInvoice(DateTime? newDateTime = null, int newTotalCost = 0)
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

        /// <summary>
        /// Get the selected invoice from a given invoice number
        /// </summary>
        /// <param name="invoiceNumber">The invoice number</param>
        /// <returns>A string of the invoice might want to change to object</returns>
        public static string getInvoice(int invoiceNumber)
        {
            string sSQL = $"SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {invoiceNumber}";

            return clsDataAccess.ExecuteScalarSQL(sSQL);
        }
        /// <summary>
        /// Get the dictionary of items from an invoiceNumber
        /// </summary>
        /// <param name="invoiceNumber">The invoice number</param>
        /// <returns>A dictionary with the LineItemNum and ItemCode</returns>
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

        /// <summary>
        /// Gets a list of items from the given invoiceNumber.
        /// </summary>
        /// <param name="invoiceNumber">The invoiceNumber as an integer</param>
        /// <returns>A list of itemDetails</returns>
        public static List<itemDetail> TestStatement(int invoiceNumber)
        {
            string sSQL = $"SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {invoiceNumber}";
            int iItemCounter = 0;   //Number of return values
            DataSet dsInvoiceItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);
            List<itemDetail> invoiceItems = new();
            foreach (DataRow row in dsInvoiceItems.Tables[0].Rows)
            {
                invoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"]));

            }
            return invoiceItems;
        }

        /// <summary>
        /// Given an itemCode it returns the information of the object.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns>A string with the itemCode, ItemDesc, and Cost</returns>
        public static string getItem(string itemCode)
        {
            string sSQL = $"select ItemCode, ItemDesc, Cost from ItemDesc WHERE = '{itemCode}'";
            return clsDataAccess.ExecuteScalarSQL(sSQL);
        }

        /// <summary>
        /// Gets the list of items from the database for the combo-box
        /// </summary>
        /// <returns>A list of items as itemDetail</returns>
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
        /// <summary>
        /// Given an invoice and lineNumber it will delete a specific row
        /// </summary>
        /// <param name="invoiceNum">The invoiceNum as an int</param>
        /// <param name="lineNumber">The line the object is on</param>
        /// <returns></returns>
        public static bool removeItem(int invoiceNum, int lineNumber)
        {
            string sSQL = $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNum} AND lineNumber = {lineNumber}";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);

            return rowsUpdated > 0 ? true : false;
        }

        /// <summary>
        /// Deletes all the lines from invoiceNumber, used to delete the invoice
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public static bool deleteAllItems(int invoiceNum)
        {
            string sSQL = $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNum}";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);

            return rowsUpdated > 0 ? true : false;
        }

        /// <summary>
        /// Delete the invoice from the database
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public static bool deleteInvoice(int invoiceNum)
        {
            string sSQL = $"DELETE FROM Invoice WHERE InvoiceNum = {invoiceNum}";
            if (deleteAllItems(invoiceNum))
            {
                int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);
                return rowsUpdated > 0 ? true : false;
            }
            return false;
        }
    }
}
