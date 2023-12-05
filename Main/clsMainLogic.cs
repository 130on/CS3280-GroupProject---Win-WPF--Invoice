using GroupAssignmentAlonColetonWannes.Common;
using GroupAssignmentAlonColetonWannes.Items;
using GroupAssignmentAlonColetonWannes.Search;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Main
{
    public class clsMainLogic
    {
        invoiceDetail activeInvoice;

        private List<string> sSQLCommands = new List<string>();


        public clsMainLogic(int invoiceNumber) {
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
        public string getInvoiceNum()
        {
            return activeInvoice.InvoiceNum.ToString();
        }

        public DateTime getInvoiceTime()
        {
            return activeInvoice.InvoiceDate;
        }

        public string getTotalCost()
        {
            return activeInvoice.TotalCost.ToString();
        }
     
        public ObservableCollection<itemDetail> getInvoiceItems()
        {
            return activeInvoice.InvoiceItems;
        }

        public void getItems()
        {
            string sSQL = $"SELECT LineItems.LineItemNum, LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {activeInvoice.InvoiceNum}";
            int iItemCounter = 0;   //Number of return values
            DataSet dsInvoiceItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);
            activeInvoice.InvoiceItems.Clear();
            foreach (DataRow row in dsInvoiceItems.Tables[0].Rows)
            {
                activeInvoice.InvoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"], (int)row["LineItemNum"]));
            }

        }
      

        public static BindingList<itemDetail> getItemList()
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


     


        public static int newInvoice(DateTime? newDateTime = null, int newTotalCost = 0)
        {
            string sSQL = $"INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#{newDateTime}#, {newTotalCost})";
            int rowsUpdated = clsDataAccess.ExecuteNonQuery(sSQL);
            if (rowsUpdated > 0)
            {
                sSQL = "SELECT MAX(InvoiceNum) FROM Invoices";
                bool res = int.TryParse(clsDataAccess.ExecuteScalarSQL(sSQL), out int newestInvoiceNumber);

                return res ? newestInvoiceNumber : -1;
            }

            return -1;
        }


        public int newItem(string newItemCode)
        {
            int lineNumber = 1;
            ObservableCollection<itemDetail> x = new(activeInvoice.InvoiceItems.OrderBy(i => i.LineItemNum));
            while (lineNumber <= activeInvoice.InvoiceItems.Count)
            {
                if (x[lineNumber - 1].LineItemNum != lineNumber)
                {
                    
                    break;
                }
                lineNumber++;
            }
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


        public int deleteItemFromInvoice(itemDetail deletingItem)
        {
            sSQLCommands.Add(clsMainSQL.removeItem(activeInvoice.InvoiceNum, (int)deletingItem.LineItemNum));

            activeInvoice.InvoiceItems.Remove(deletingItem);
            return updateTotalCost();
        }


        public int UpdateDataBase(bool update)
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

      


        private int updateTotalCost()
        {
            decimal totalCost = 0;
            foreach(itemDetail item in activeInvoice.InvoiceItems)
            {
                totalCost += item.Cost;
            }
            activeInvoice.TotalCost = (int)Math.Ceiling(totalCost);
            return (int)Math.Ceiling(totalCost);

        }
    }
}
