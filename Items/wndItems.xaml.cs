using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupAssignmentAlonColetonWannes.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {

        private bool bItemModified;

        public bool ItemModified
        {
            get { return bItemModified; }
            set { bItemModified = value; }
        }

        clsItemsLogic ItemsLogic;

        BindingList<itemDetail> Items;

        private enum eCurrentMode
        {
            ViewItems,
            AddItem,
            EditItem
        }

        private eCurrentMode CurrentMode;

        public wndItems()
        {
            try
            {
                InitializeComponent();

                ItemModified = false;

                ItemsLogic = new clsItemsLogic();


                CurrentMode = eCurrentMode.ViewItems;

                LoadItems();
            } 
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
        }

        private void LoadItems()
        {
            try
            {
                Items = ItemsLogic.GetItems();

                dgItems.ItemsSource = Items;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        


    }
}
