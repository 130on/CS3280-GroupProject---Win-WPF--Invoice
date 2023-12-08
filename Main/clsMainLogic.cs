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
    public class clsMainLogic
    {
        private invoiceDetail activeInvoice;

        private List<string> sSQLCommands = new List<string>();

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
                getItems();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Creates a new invoice; and sets  
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

        public void getItems()
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





        public int newInvoice(DateTime? newDateTime, int newTotalCost)
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


        public int newItem(string newItemCode)
        {
            try
            {
                ObservableCollection<itemDetail> x = new(activeInvoice.InvoiceItems.OrderBy(i => i.LineItemNum));
                itemDetail? lastInvoice = x.LastOrDefault();
 
                int lineNumber = lastInvoice != null? (int)lastInvoice.LineItemNum + 1: 1;
                
                sSQLCommands.Add(clsMainSQL.newItemInInvoice(activeInvoice.InvoiceNum, lineNumber, newItemCode));

                string sSQL = clsMainSQL.getItem(newItemCode);

                int iItem = 0;
                DataSet dsItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItem);
                foreach (DataRow row in dsItems.Tables[0].Rows)
                {
                    activeInvoice.InvoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"], lineNumber));

                }
                //string item = clsDataAccess.ExecuteScalarSQL(sSQL);

                return updateTotalCost();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        public int deleteItemFromInvoice(itemDetail deletingItem, ref bool noItems)
        {

            try
            {
                sSQLCommands.Add(clsMainSQL.removeItem(activeInvoice.InvoiceNum, (int)deletingItem.LineItemNum));

                activeInvoice.InvoiceItems.Remove(deletingItem);
                if (activeInvoice.InvoiceItems.Count <= 0)
                {
                    noItems = true;
                }
                return updateTotalCost();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        public int UpdateDataBase(bool update)
        {


            try
            {
                if (update)
                {
                    int i = 0;
                    foreach (string sSQL in sSQLCommands)
                    {
                        i += clsDataAccess.ExecuteNonQuery(sSQL);
                    }
                }
                else
                {
                    getItems();
                }
                sSQLCommands.Clear();

                int totalCost = updateTotalCost();
                clsDataAccess.ExecuteNonQuery(clsMainSQL.updateTotalCost(activeInvoice.InvoiceNum, activeInvoice.TotalCost));
                return totalCost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }




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
                return (int)Math.Ceiling(totalCost);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }


    }
}
