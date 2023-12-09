using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GroupAssignmentAlonColetonWannes.Main
{
    /// <summary>
    /// The main logic of the mainWindow
    /// </summary>
    public class clsMainLogic
    {
        /// <summary>
        /// The selected invoice of the given user
        /// </summary>
        private invoiceDetail activeInvoice;

        /// <summary>
        /// A list of sql commands to be pushed when the user presses save
        /// </summary>
        private List<string> sSQLCommands = new List<string>();




        #region GetStaments
        /// <summary>
        /// Gets the active invoiceNumber simple get statement
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int getInvoiceNum()
        {
            try
            {
                return activeInvoice.InvoiceNum;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets the active datetime of the selected invoice
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DateTime? getInvoiceTime()
        {
            try
            {
                return activeInvoice.InvoiceDate;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets the totalCost of the selected invoice
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int getTotalCost()
        {
            try
            {
                return activeInvoice.TotalCost;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the list of items in the selected invoice
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ObservableCollection<itemDetail> getInvoiceItems()
        {
            try
            {
                return activeInvoice.InvoiceItems;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets the item collection. For the combo box.
        /// </summary>
        /// <returns>The list of items</returns>
        /// <exception cref="Exception"></exception>
        public static BindingList<itemDetail> getItemList()
        {
            try
            {
                BindingList<itemDetail> items = new();

                int iItemCounter = 0;   //Number of return values
                DataSet dsItems = clsDataAccess.ExecuteSQLStatement(clsMainSQL.getItems(), ref iItemCounter);

                foreach (DataRow itemRow in dsItems.Tables[0].Rows)
                {
                    string itemCode = (string)itemRow["ItemCode"];
                    string itemDesc = (string)itemRow["ItemDesc"];
                    decimal cost = (decimal)itemRow["Cost"];

                    items.Add(new itemDetail(itemCode, itemDesc, cost));
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion GetStaments


        /// <summary>
        /// The constructor class that pulls the given invoice from the database or if -1 creates new invoice
        /// </summary>
        /// <param name="invoiceNumber">The invoice number you want to pull if -1 it will create new</param>
        /// <exception cref="Exception"></exception>
        public clsMainLogic(int invoiceNumber)
        {
            try
            {
                if (invoiceNumber == -1)
                {
                    newInvoice();
                    return;
                }
                int iItemCounter = 0;
                DataSet dsInvoice = clsDataAccess.ExecuteSQLStatement(clsMainSQL.getInvoice(invoiceNumber), ref iItemCounter);

                foreach (DataRow dataRow in dsInvoice.Tables[0].Rows)
                {
                    int invoiceNum = (int)dataRow["InvoiceNum"];
                    DateTime invoiceDate = (DateTime)dataRow["InvoiceDate"];
                    int totalCost = (int)dataRow["TotalCost"];
                    activeInvoice = new invoiceDetail(invoiceNum, invoiceDate, totalCost);
                }
                updateInvoiceItemCollection();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a new invoice; and sets the internal invoiceNumber to the correct one 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void newInvoice()
        {
            try
            {
                activeInvoice = new invoiceDetail(-1, null, 0);
                string sSQL = clsMainSQL.getNewestInvoice();
                bool res = int.TryParse(clsDataAccess.ExecuteScalarSQL(sSQL), out int newestInvoiceNumber);

                activeInvoice.InvoiceNum = newestInvoiceNumber + 1;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Ran when actually saving the new invoice to the database
        /// </summary>
        /// <param name="newDateTime">The user selected time</param>
        /// <param name="newTotalCost">The total cost of the items</param>
        /// <returns>The invoice number of the new invoice</returns>
        /// <exception cref="Exception"></exception>
        public int saveNewInvoice(DateTime? newDateTime, int newTotalCost)
        {
            try
            {
                string sSQL = clsMainSQL.newInvoice(newDateTime, newTotalCost);
                int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);
                if (rowsUpdated > 0)
                {
                    sSQL = clsMainSQL.getNewestInvoice();
                    bool res = int.TryParse(clsDataAccess.ExecuteScalarSQL(sSQL), out int newestInvoiceNumber);

                    activeInvoice.InvoiceNum = newestInvoiceNumber;
                    return newestInvoiceNumber;
                }

                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a new item in the invoice not saved to database. Also finds the lastLine number and adds 1
        /// </summary>
        /// <param name="newItemCode">The item code that should be added</param>
        /// <returns>The new cost of all the items</returns>
        /// <exception cref="Exception"></exception>
        public int newItem(string newItemCode)
        {
            try
            {
                itemDetail? lastInvoice = activeInvoice.InvoiceItems.LastOrDefault();
                if (lastInvoice == null)
                {
                    return -1;
                }
                int lineNumber = (lastInvoice.LineItemNum != null ? (int)lastInvoice.LineItemNum : 0) + 1;

                sSQLCommands.Add(clsMainSQL.newItemInInvoice(activeInvoice.InvoiceNum, lineNumber, newItemCode));

                string sSQL = clsMainSQL.getItem(newItemCode);

                int iItem = 0;
                DataSet dsItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItem);
                foreach (DataRow row in dsItems.Tables[0].Rows)
                {
                    activeInvoice.InvoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"], lineNumber));
                }

                return updateTotalCost();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes an item from the invoice, not saved to database. Also determines the count.
        /// </summary>
        /// <param name="deletingItem">The item that should be deleted as an itemDetails</param>
        /// <param name="noItems">Whether there is no items left in the database</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int deleteItemFromInvoice(itemDetail deletingItem, ref bool noItems)
        {
            try
            {
                if (deletingItem.LineItemNum != null)
                {
                    sSQLCommands.Add(clsMainSQL.removeItem(activeInvoice.InvoiceNum, (int)deletingItem.LineItemNum));

                    activeInvoice.InvoiceItems.Remove(deletingItem);
                    if (activeInvoice.InvoiceItems.Count <= 0)
                    {
                        noItems = true;
                    }
                }
                return updateTotalCost();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the database and executes all sql commands; updates total cost.
        /// </summary>
        /// <param name="update">Whether the sqlCommands should be ran and saved to database</param>
        /// <returns>The cost of all the items in the invoice</returns>
        /// <exception cref="Exception"></exception>
        public int UpdateDataBase(bool update)
        {
            try
            {
                if (update)
                {
                    foreach (string sSQL in sSQLCommands)
                    {
                        clsDataAccess.ExecuteNonQuery(sSQL);
                    }
                }
                else
                {
                    updateInvoiceItemCollection();
                }
                sSQLCommands.Clear();

                int totalCost = updateTotalCost();
                clsDataAccess.ExecuteNonQuery(clsMainSQL.updateTotalCost(activeInvoice.InvoiceNum, totalCost));
                return totalCost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        #region Internal Methods

        /// <summary>
        /// Checks the database for all the items and updates the invoice with the items
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void updateInvoiceItemCollection()
        {
            try
            {
                string sSQL = clsMainSQL.invoiceItemSQLList(activeInvoice.InvoiceNum);
                int iItemCounter = 0;   //Number of return values
                DataSet dsInvoiceItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);
                activeInvoice.InvoiceItems.Clear();
                foreach (DataRow row in dsInvoiceItems.Tables[0].Rows)
                {
                    activeInvoice.InvoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"], (int)row["LineItemNum"]));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Updates the cost of the items in the invoice. Not saved to database; passes the totalcost back.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private int updateTotalCost()
        {
            try
            {
                decimal totalCost = 0;
                foreach (itemDetail item in activeInvoice.InvoiceItems)
                {
                    totalCost += item.Cost;
                }
                activeInvoice.TotalCost = (int)Math.Ceiling(totalCost);
                return activeInvoice.TotalCost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }


        #endregion Internal Methods
    }
}
