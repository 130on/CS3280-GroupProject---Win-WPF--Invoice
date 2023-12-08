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
using System.Xml.Serialization;

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

        itemDetail SelectedItem;

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

        private void dgItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedItem = (itemDetail) dgItems.SelectedItem;

                if (SelectedItem != null)
                {
                    txtCode.Text = SelectedItem.ItemCode;
                    txtDescription.Text = SelectedItem.ItemDesc;
                    txtCost.Text = SelectedItem.Cost.ToString();
                }
                else
                {
                    txtCode.Text = "";
                    txtDescription.Text = "";
                    txtCost.Text = "";
                }

                btnUpdateItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;
            } 
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentMode = eCurrentMode.AddItem;

                EnableControls();



            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void EnableControls()
        {
            try
            {
                switch(CurrentMode)
                {
                    case eCurrentMode.AddItem:
                        txtCode.IsEnabled = true;
                        txtDescription.IsEnabled = true;
                        txtCost.IsEnabled = true;

                        txtCode.Text = "";
                        txtDescription.Text = "";
                        txtCost.Text = "";

                        btnAddItem.IsEnabled = false;
                        btnUpdateItem.IsEnabled = false;
                        btnDeleteItem.IsEnabled = false;
                        btnSaveItem.IsEnabled = true;

                        dgItems.IsEnabled = false;
                        
                        break;

                    case eCurrentMode.ViewItems:
                        txtCode.IsEnabled = false;
                        txtDescription.IsEnabled = false;
                        txtCost.IsEnabled = false;

                        btnAddItem.IsEnabled = true;
                        btnUpdateItem.IsEnabled = false;
                        btnDeleteItem.IsEnabled = false;
                        btnSaveItem.IsEnabled = false;

                        dgItems.IsEnabled = true;

                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }



        

        private void btnSaveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch(CurrentMode)
                {
                    case eCurrentMode.AddItem:

                        if (ValidateInput() && IsCodeUnique())
                        {
                            itemDetail NewItem = new itemDetail(txtCode.Text, txtDescription.Text, decimal.Parse(txtCost.Text));
                            ItemsLogic.AddItem(NewItem);

                            //Set local variable to true for MainWindow
                            ItemModified = true;

                            CurrentMode = eCurrentMode.ViewItems;

                            EnableControls();
                            LoadItems();
                            
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private bool ValidateInput()
        {
            try
            {
                if (txtCode.Text == "")
                {
                    txtError.Text = "Please enter an item code";                 
                    return false;
                }
                if (txtDescription.Text == "")
                {
                    txtError.Text = "Please enter an item description";
                    return false;
                }
                if (txtCost.Text == "")
                {
                    txtError.Text = "Please enter an item cost";
                    return false;
                }

                if (txtCode.Text.Length > 4)
                {
                    txtError.Text = "Code must be 4 characters or less";
                    return false;
                }

                if (txtDescription.Text.Length > 20)
                {
                    txtError.Text = "Description must be 20 characters or less";
                    return false;
                }

                if (!decimal.TryParse(txtCost.Text, out _))
                {
                    txtError.Text = "Cost must be a valid number";
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        private bool IsCodeUnique()
        {
            try
            {
                foreach (itemDetail item in Items)
                {
                    if (item.ItemCode == txtCode.Text)
                    {
                        txtError.Text = "Item code must be unique";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        

        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// displays a message with info about the error
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
