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
        public static void loadItemList()
        {
            DataSet dsItems = new DataSet();
            int iItemCounter = 0;   //Number of return values
            string sSQL = "select ItemCode, ItemDesc, Cost from ItemDesc";
            dsItems = clsDataAccess.ExecuteSQLStatement(sSQL, ref iItemCounter);

            for (int i = 0; i < iItemCounter; i++)
            {
                string itemCode = (string)dsItems.Tables[0].Rows[i]["ItemCode"];
                string itemDesc = (string)dsItems.Tables[0].Rows[i]["ItemDesc"];
                decimal cost = (decimal)dsItems.Tables[0].Rows[i]["Cost"];

                //listController.addItem(itemCode, itemDesc, cost);
                clsItemsLogic.ItemList.Add(new itemDetail(itemCode, itemDesc, cost));
            }
        }

        public static List<int> getInvoicesWithItem(string itemCod, ref int invoiceCount)
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

        public static bool updateItem(string itemCode, string newItemDesc, decimal newItemCost)
        {
            string sSQL = $"Update ItemDesc Set ItemDesc = '{newItemDesc}', Cost = {newItemCost} where ItemCode = '{itemCode}'";
            int effectedRows = clsDataAccess.ExecuteNonQuery(sSQL);

            if(effectedRows != 0 )
            {
                foreach (itemDetail item in clsItemsLogic.ItemList)
                {
                    if(item.ItemCode == itemCode)
                    {
                        item.ItemDesc = newItemDesc;
                        item.Cost = newItemCost;
                        break;
                    }
                }
                return true;
            }

            return false;
        }


        public static bool newItem(string newItemCode, string newItemDesc, decimal newItemCost) {

            string sSQL = $"Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{newItemCode}', '{newItemDesc}', {newItemCost})";
            int effectedRows = clsDataAccess.ExecuteNonQuery(sSQL);

            clsItemsLogic.ItemList.Add(new itemDetail(newItemCode, newItemDesc, newItemCost));

            return true;
        }


        public static bool deleteItem(string  itemCode)
        {
            string sSQL = $"Delete from ItemDesc Where ItemCode = '{itemCode}'";
            foreach (itemDetail item in clsItemsLogic.ItemList)
            {
                if (item.ItemCode == itemCode)
                {
                    clsItemsLogic.ItemList.Remove(item);
                }
            }
            return true;
        }
    }
}
