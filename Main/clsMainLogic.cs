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
            GetItems();
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

        public void GetItems()
        {
            string sSQL = $"SELECT LineItems.LineItemNum, LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {activeInvoice.InvoiceNum}";
            int iItemCounter = 0;   //Number of return values
            DataSet dsInvoiceItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);
            activeInvoice.InvoiceItems.Clear();
            foreach (DataRow row in dsInvoiceItems.Tables[0].Rows)
            {
                // activeInvoice.InvoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"]));
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


        public static List<itemDetail> invoiceItemsList(int invoiceNumber)
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



        public static Dictionary<int, string> mightUseInstead(int invoiceNumber)
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
            //string sSQL = clsMainSQL.newItemInInvoice(activeInvoice.InvoiceNum, lineNumber, newItemCode);
            //clsDataAccess.ExecuteNonQuery(sSQL);

            string sSQL = clsMainSQL.getItem(newItemCode);

            int iItem = 0;
            string item = clsDataAccess.ExecuteScalarSQL(sSQL);
            DataSet dsItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItem);
            foreach (DataRow row in dsItems.Tables[0].Rows)
            {
                activeInvoice.InvoiceItems.Add(new itemDetail((string)row["ItemCode"], (string)row["ItemDesc"], (decimal)row["Cost"], lineNumber));

            }
            //string item = clsDataAccess.ExecuteScalarSQL(sSQL);

            return 0;
        }
    }
}
