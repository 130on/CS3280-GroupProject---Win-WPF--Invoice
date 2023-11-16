using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Items
{
    public static class clsItemsSQL
    {

        /// <summary>
        /// Returns a list of all items
        /// </summary>
        /// <returns>The list of items</returns>
        public static List<itemDetail> getItemsList()
        {
            List<itemDetail> items = new();

            int iItemCounter = 0;   //Number of return values
            string sSQL = "select ItemCode, ItemDesc, Cost from ItemDesc";
            DataSet dsItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);

            foreach (DataRow itemRow in dsItems.Tables[0].Rows)
            {
                string itemCode = (string)itemRow["ItemCode"];
                string itemDesc = (string)itemRow["ItemDesc"];
                decimal cost = (decimal)itemRow["Cost"];

                items.Add(new itemDetail(itemCode, itemDesc, cost));
            }

            return items;
        }

        /// <summary>
        /// Returns a list of all invoices that contain a specific item code
        /// </summary>
        /// <param name="itemCod">The item code</param>
        /// <param name="invoiceCount">The number of invoices</param>
        /// <returns>The list of invoices</returns>
        public static List<int> getInvoicesWithItemCode(string itemCod, ref int invoiceCount)
        {
            DataSet dsInvoiceList = new DataSet();
            string sSQL = $"select distinct(InvoiceNum) from LineItems where ItemCode = '{itemCod}'";
            dsInvoiceList = clsDataAccess.ExecuteSQLStatement(sSQL, ref invoiceCount);

            List<int> invoicesNumbers = new List<int>();

            foreach(DataRow invoice in dsInvoiceList.Tables[0].Rows)
            {
                invoicesNumbers.Add((int)invoice["InvoiceNum"]);
            }

            return invoicesNumbers;
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="itemCode">New item code</param>
        /// <param name="newItemDesc">New item description</param>
        /// <param name="newItemCost">New item cost</param>
        /// <returns></returns>
        public static bool updateItem(string itemCode, string newItemDesc, decimal newItemCost)
        {
            string sSQL = $"Update ItemDesc Set ItemDesc = '{newItemDesc}', Cost = {newItemCost} where ItemCode = '{itemCode}'";
            int effectedRows = clsDataAccess.ExecuteNonQuery(sSQL);


            return effectedRows != 0 ? true : false;
        }

        /// <summary>
        /// Add a new item
        /// </summary>
        /// <param name="newItemCode">Item code</param>
        /// <param name="newItemDesc">Item description</param>
        /// <param name="newItemCost">Item cost</param>
        /// <returns></returns>
        public static bool newItem(string newItemCode, string newItemDesc, decimal newItemCost) {

            string sSQL = $"Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{newItemCode}', '{newItemDesc}', {newItemCost})";
            int effectedRows = clsDataAccess.ExecuteNonQuery(sSQL);


            return effectedRows != 0 ? true : false;
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="itemCode">The code for the item</param>
        /// <returns></returns>
        public static bool deleteItem(string  itemCode)
        {
            string sSQL = $"Delete from ItemDesc Where ItemCode = '{itemCode}'";

            int effectedRows = clsDataAccess.ExecuteNonQuery(sSQL);

            return effectedRows != 0 ? true : false;
        }
    }
}
