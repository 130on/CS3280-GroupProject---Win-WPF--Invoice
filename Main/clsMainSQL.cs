using System;
using System.Reflection;

namespace GroupAssignmentAlonColetonWannes.Main
{
    public static class clsMainSQL
    {

        /// <summary>
        /// A string that allows the updating of the total cost. 
        /// </summary>
        /// <param name="invoiceNumber">The invoice number being updated</param>
        /// <param name="newTotalCost">The new total-cost of the invoice</param>
        /// <returns>non-query sql string</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string updateTotalCost(int invoiceNumber, int newTotalCost)
        {
            try
            {
                return $"UPDATE Invoices SET TotalCost = {newTotalCost} WHERE InvoiceNum = {invoiceNumber}";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// A string that allows adding a new item to an invoice.
        /// </summary>
        /// <param name="invoiceNumber">A already made invoice</param>
        /// <param name="newLineItemNum">A unique number that shows when it was added</param>
        /// <param name="newItemCode">The item being added to the database</param>
        /// <returns>non-query sql string</returns>
        /// <exception cref="Exception">Standard Error</exception>

        public static string newItemInInvoice(int invoiceNumber, int newLineItemNum, string newItemCode)
        {
            try
            {
                return $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({invoiceNumber}, {newLineItemNum}, '{newItemCode}')";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a new string given a time and a total-cost
        /// </summary>
        /// <param name="newDateTime">The time the new invoice should have</param>
        /// <param name="newTotalCost">The total cost of the invoice upon creation</param>
        /// <returns>non-query sql string</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string newInvoice(DateTime? newDateTime = null, int newTotalCost = 0)
        {
            try
            {
                return $"INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#{newDateTime}#, {newTotalCost})";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the invoice number for a new invoice
        /// </summary>
        /// <returns>A query string that's get one number</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string getNewestInvoice()
        {
            try
            {
                return "SELECT MAX(InvoiceNum) FROM Invoices";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Get the selected invoice from a given invoice number
        /// </summary>
        /// <param name="invoiceNumber">The invoice number</param>
        /// <returns>A string of the invoice might want to change to object</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string getInvoice(int invoiceNumber)
        {
            try
            {
                return $"SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {invoiceNumber}";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Given a invoice number it creates a sql statement
        /// </summary>
        /// <param name="invoiceNumber">The invoiceNumber as an integer</param>
        /// <returns>A string to be executed returns many lines</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string invoiceItemSQLList(int invoiceNumber)
        {
            try
            {
                return $"SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost, LineItemNum FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {invoiceNumber}";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Given an itemCode it gives the sql string details of it
        /// </summary>
        /// <param name="itemCode">The item code you want details of</param>
        /// <returns>A string to be executed a dg string</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string getItem(string itemCode)
        {
            try
            {
                return $"select ItemCode, ItemDesc, Cost from ItemDesc WHERE ItemCode = '{itemCode}'";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Used to get the list of the items. 
        /// </summary>
        /// <returns>A string that gets the list of items</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string getItems()
        {
            try
            {
                return "select ItemCode, ItemDesc, Cost from ItemDesc";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// A string to delete item from an invoice
        /// </summary>
        /// <param name="invoiceNum">The invoiceNum as an int</param>
        /// <param name="lineNumber">The line the object is on</param>
        /// <returns>non-query sql string</returns>
        /// <exception cref="Exception">Standard Error</exception>
        public static string removeItem(int invoiceNum, int lineNumber)
        {
            try
            {
                return $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNum} AND LineItemNum = {lineNumber}";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
