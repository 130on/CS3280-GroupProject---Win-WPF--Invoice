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
    }
}
