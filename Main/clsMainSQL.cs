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
        public static string updateTotalCost(int invoiceNumber, int newTotalCost)
        {
            return $"UPDATE Invoices SET TotalCost = {newTotalCost} WHERE InvoiceNum = {invoiceNumber}";
        }

        /// <summary>
        /// Takes an invoiceNumber, the row the item is at and a item code, and adds it to the database
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="newLineItemNum"></param>
        /// <param name="newItemCode"></param>
        /// <returns>True if a row got updated or false if nothing changed</returns>
        public static string newItemInInvoice(int invoiceNumber, int newLineItemNum, string newItemCode)
        {
            return $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({invoiceNumber}, {newLineItemNum}, '{newItemCode}')";
        }

        /// <summary>
        /// Create a new invoice with starting values of the given time
        /// </summary>
        /// <param name="newDateTime">The time the invoice is as DateTIme can be null</param>
        /// <param name="newTotalCost">The cost of the invoice default 0 </param>
        /// <returns>An integer whether it working or not -1 didn't work else 1</returns>
        public static string newInvoice(DateTime? newDateTime = null, int newTotalCost = 0)
        {
            return $"INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#{newDateTime}#, {newTotalCost})";
        }

        public static string getNewestInvoice()
        {
            return "SELECT MAX(InvoiceNum) FROM Invoices";
        }

        /// <summary>
        /// Get the selected invoice from a given invoice number
        /// </summary>
        /// <param name="invoiceNumber">The invoice number</param>
        /// <returns>A string of the invoice might want to change to object</returns>
        public static string getInvoice(int invoiceNumber)
        {
            return $"SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {invoiceNumber}";
        }

        /// <summary>
        /// Given a invoice number it creates a sql statement
        /// </summary>
        /// <param name="invoiceNumber">The invoiceNumber as an integer</param>
        /// <returns>A string to be executed</returns>
        public static string invoiceItemSQLList(int invoiceNumber)
        {
            return $"SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {invoiceNumber}";
        }

        /// <summary>
        /// Given an itemCode it gives the sql string details of it
        /// </summary>
        /// <param name="itemCode">The item code you want details of</param>
        /// <returns>A string to be executed</returns>
        public static string getItem(string itemCode)
        {
            return $"select ItemCode, ItemDesc, Cost from ItemDesc WHERE ItemCode = '{itemCode}'";
        }

        /// <summary>
        /// Used to get the list of the items. 
        /// </summary>
        /// <returns>A string that gets the list of items</returns>
        public static string getItems()
        {
            return "select ItemCode, ItemDesc, Cost from ItemDesc"; ;
        }
        /// <summary>
        /// A string to delete item from an invoice
        /// </summary>
        /// <param name="invoiceNum">The invoiceNum as an int</param>
        /// <param name="lineNumber">The line the object is on</param>
        /// <returns>A string the needs to be run</returns>
        public static string removeItem(int invoiceNum, int lineNumber)
        {
            return $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNum} AND lineNumber = {lineNumber}";
        }

        /// <summary>
        /// The string to delete all the lines from invoiceNumber, used to delete the invoice
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public static string deleteAllItems(int invoiceNum)
        {
            return $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNum}";
        }

        /// <summary>
        /// The command to delete an invoice from the database make sure to run deleteAllItems first
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public static string deleteInvoice(int invoiceNum)
        {
            return $"DELETE FROM Invoice WHERE InvoiceNum = {invoiceNum}";
        }
    }
}
