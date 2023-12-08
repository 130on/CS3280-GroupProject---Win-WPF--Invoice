using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GroupAssignmentAlonColetonWannes.Items
{
    /// <summary>
    /// Holds all SQL logic for the items
    /// </summary>
    public static class clsItemsSQL
    {

       

        public static string GetItems()
        {
            try
            {
                string sSQL = "select ItemCode, ItemDesc, Cost from ItemDesc";

                return sSQL;
            } 
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        

        public static string GetInvoicesWithItemCode(string ItemCode)
        {
            try
            {
                string sSQL = $"select distinct(InvoiceNum) from LineItems where ItemCode = '{ItemCode}'";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

       

        public static string UpdateItem(string ItemCode, string NewDescription, string NewCost)
        {
            try
            {
                string sSQL = $"Update ItemDesc Set ItemDesc = '{NewDescription}', Cost = {NewCost} where ItemCode = '{ItemCode}'";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        

        public static string AddItem(string ItemCode, string ItemDescription, string ItemCost)
        {
            try
            {
                string sSQL = $"Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{ItemCode}', '{ItemDescription}', {ItemCost})";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        

        public static string DeleteItem(string ItemCode)
        {
            try
            {
                string sSQL = $"Delete from ItemDesc Where ItemCode = '{ItemCode}'";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
    }
}
