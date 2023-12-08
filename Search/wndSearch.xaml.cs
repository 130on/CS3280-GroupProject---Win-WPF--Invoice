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

namespace GroupAssignmentAlonColetonWannes.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        // var that stores the selected invoice
        private invoiceDetail? selectedInvoice;

        // var that stores the selected invoice number 
        public static int? SelectedInvoiceNum { get; private set; }

        // declare the binding list that holds the invoice objects
        private BindingList<invoiceDetail> gridList = new BindingList<invoiceDetail>();

        public wndSearch()
        {
            InitializeComponent();

            loadWindow();   
        }

        /// <summary>
        /// populates the grid list and combo boxes from database  
        /// </summary>
        private void loadWindow()
        {
            // reset the datagrid and the class property that is used to pass info to main window
            invoiceGrid.ItemsSource = null;
            SelectedInvoiceNum = null;
            
            // loads the data from DB into datagrid and combo boxes
            gridList = clsSearchLogic.loadList();
            invoiceGrid.ItemsSource = gridList;

            //add invoice nums to combobox
            cbInvoiceNum.ItemsSource = gridList;
            cbInvoiceNum.DisplayMemberPath = "InvoiceNum";

            // add sorted and distinct invoices' total cost to combobox
            cbTotalCharge.ItemsSource = clsSearchLogic.sortList();
            cbTotalCharge.DisplayMemberPath = "TotalCost";
        }

        /// <summary>
        /// Updates UI based on selection of item in 'Invoice Number' combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // clear selection from the Total Charge combo box
                cbTotalCharge.SelectedItem = null;

                if (cbInvoiceNum.SelectedItem != null)
                {   
                    // Store the current combo box value in a variable
                    int selectedNum = ((invoiceDetail)cbInvoiceNum.SelectedItem).InvoiceNum;

                    // Filter the original list based on the selected InvoiceNum
                    clsSearchLogic.filterGridByInvoiceNum(selectedNum);
                    invoiceGrid.ItemsSource = clsSearchLogic.filterGridByInvoiceNum(selectedNum);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Updates UI based on selection of item in 'Total Cost' combo box and 'Invoice Date' date picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTotalCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // filter results if only a total cost is selected
                if (cbTotalCharge.SelectedItem != null && dpInvoiceDate.SelectedDate == null)
                {
                    // Store the current combo box value in a variable
                    int selectedCost = ((invoiceDetail)cbTotalCharge.SelectedItem).TotalCost;
                    // display filtered results in datagrid
                    invoiceGrid.ItemsSource = clsSearchLogic.filterGridBySelections(selectedCost, null);

                }
                
                // filter results in datagrid if both date and total cost are selected
                else if (cbTotalCharge.SelectedItem != null && dpInvoiceDate.SelectedDate != null)
                {
                    // Store the cost and date values in variables
                    int selectedCost = ((invoiceDetail)cbTotalCharge.SelectedItem).TotalCost;
                    DateTime selectedDate = (DateTime)dpInvoiceDate.SelectedDate;
                    // display filtered results in datagrid
                    invoiceGrid.ItemsSource = clsSearchLogic.filterGridBySelections(selectedCost, selectedDate);
                }

                // filter results in datagrid if only date is selected
                else if (cbTotalCharge.SelectedItem == null && dpInvoiceDate.SelectedDate != null)
                {
                    // Store the current combo box value in a variable
                    DateTime? selectedDate = (DateTime?)dpInvoiceDate.SelectedDate;
                    // display filtered results in datagrid
                    invoiceGrid.ItemsSource = clsSearchLogic.filterGridBySelections(null, selectedDate);
                }
                
            }

            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// handles the user's choice of invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoiceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // once a user selects a row in the datagrid, assign the invoice object to var  
                if (invoiceGrid.SelectedItem != null)
                {
                    selectedInvoice = (invoiceDetail)invoiceGrid.SelectedItem;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// handles click on select button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // check that the user selected an invoice in the datagrid
                if (selectedInvoice != null) {
                    // Extract the selected invoice number from the datagrid invoice object and assign
                    // it to the static var to be passed to mainWindow 
                    SelectedInvoiceNum = selectedInvoice.InvoiceNum;
                
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            } 
        }

        /// <summary>
        /// closes search window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// resets the search page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // clear all fields in select window
                dpInvoiceDate.SelectedDate = null;
                
                // reloads the search window and resets it
                loadWindow();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
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
