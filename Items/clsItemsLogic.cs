using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Items
{
    /// <summary>
    /// Hold the main logic for the items
    /// </summary>
    public class clsItemsLogic
    {
       

        public clsItemsLogic()
        {

        }

        public BindingList<itemDetail> GetItems()
        {
            try
            {
                BindingList<itemDetail> Items = new BindingList<itemDetail>();

                DataSet ds;

                string sSQL = clsItemsSQL.GetItems();

                int iReturnValues = 0;

               ds = clsDataAccess.ExecuteSQLStatement(sSQL, ref iReturnValues);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Items.Add(new itemDetail(row["ItemCode"].ToString(), row["ItemDesc"].ToString(), (decimal)row["Cost"]));
                }

                return Items;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public void AddItem(itemDetail Item)
        {
            try
            {

                string sSQL = clsItemsSQL.AddItem(Item.ItemCode, Item.ItemDesc, Item.Cost.ToString());

                

               clsDataAccess.ExecuteNonQuery(sSQL);

                

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public void UpdateItem(itemDetail OldItem, itemDetail NewItem)
        {
            try
            {
                string sSQL = clsItemsSQL.UpdateItem(OldItem.ItemCode, NewItem.ItemDesc, NewItem.Cost.ToString());


                clsDataAccess.ExecuteNonQuery(sSQL);

                

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        public List<string> GetInvoicesWithItemCode(itemDetail Item)
        {
            try
            {
                List<string> Invoices = new List<string>();

                DataSet ds;

                string sSQL = clsItemsSQL.GetInvoicesWithItemCode(Item.ItemCode);

                int iReturnValues = 0;

                ds = clsDataAccess.ExecuteSQLStatement(sSQL, ref iReturnValues);

                if (iReturnValues > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Invoices.Add(row["InvoiceNum"].ToString());
                    }
                }

                

                return Invoices;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        public void DeleteItem(itemDetail Item)
        {
            try
            {
                string sSQL = clsItemsSQL.DeleteItem(Item.ItemCode);

                clsDataAccess.ExecuteNonQuery(sSQL);

                

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
            
    }
}

