using GroupAssignmentAlonColetonWannes.Common;
using System;
using System.Collections.Generic;
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
        // var that stores the selected invoice number 
        static int? SelectedInvoiceNum;
        public wndSearch()
        {
            InitializeComponent();

            // load Invoices to Datarid 
            List<invoiceDetail> gridList = new List<invoiceDetail>();
            gridList = clsSearchLogic.loadInvoices();
            invoiceGrid.ItemsSource = gridList;


            // add invoice nums to combobox
            cbInvoiceNum.ItemsSource = gridList;
            cbInvoiceNum.DisplayMemberPath = "InvoiceNum";

            // add invoice total cost to combobox
            cbTotalCharge.ItemsSource = gridList;
            cbTotalCharge.DisplayMemberPath = "TotalCost";

        }


        /// <summary>
        /// handles selection of item in Invoice Number combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
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
                // Select invoice and store it in static var using storeId()

                // pass invoice id to the mainwindow before close

                this.Close();

            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

            

        }

        /// <summary>
        /// handles click on 
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

            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        // invoice number comboBox - loadInvoices(SearchInvoicNum) 

        // invoice total - loadInvoices(searchTotalcost)

        // invoice date - loadInvoices(searchDate)

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
