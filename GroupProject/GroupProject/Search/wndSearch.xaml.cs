using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GroupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        clsSearchLogic clsSearchLogic;

        //private int invoiceToReturn;

        /// <summary>
        /// wndSearch initializer
        /// </summary>
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                clsSearchLogic = new clsSearchLogic();

                fillCBOS(clsSearchLogic.AllInvoices);
               

                dgInvoices.ItemsSource = clsSearchLogic.InvoicesToDisplay;

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Exception handling method, 
        /// call this from any top level methods and button click methods.
        /// rethrow the error if you are a helper method or not on the UI
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
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }

       

        /// <summary>
        /// block the close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Hide();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// select button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgInvoices.SelectedIndex != -1)
                {
                    Invoice temp = (Invoice)dgInvoices.SelectedItem;
                    clsSearchLogic.InvoiceToReturn = new Invoice(temp.InvoiceNumber, temp.InvoiceDate, temp.TotalCost, temp.Items);
                }
                else
                {
                    clsSearchLogic.InvoiceToReturn = null;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsSearchLogic.InvoiceToReturn = null;
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic for when the invoice numbers combo box is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboInvoiceNumbers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboInvoiceNumbers.SelectedIndex != -1)
                {
                    int temp = (int)cboInvoiceNumbers.SelectedItem;
                    clsSearchLogic.trimByInvoiceNumber(temp);
                    dgInvoices.ItemsSource = clsSearchLogic.InvoicesToDisplay;
                    fillCBOS(clsSearchLogic.InvoicesToDisplay);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic for when the costs combobox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboInvoiceCosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cboInvoiceCosts.SelectedIndex != -1)
                {
                    double temp = (double)cboInvoiceCosts.SelectedItem;
                    clsSearchLogic.trimByCost(temp);
                    dgInvoices.ItemsSource = clsSearchLogic.InvoicesToDisplay;
                    fillCBOS(clsSearchLogic.InvoicesToDisplay);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic for when the dates combobox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboInvoiceDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboInvoiceDates.SelectedIndex != -1)
                { 
                    DateTime temp = (DateTime)cboInvoiceDates.SelectedItem;
                    clsSearchLogic.trimListByDate(temp);
                    dgInvoices.ItemsSource = clsSearchLogic.InvoicesToDisplay;
                    fillCBOS(clsSearchLogic.InvoicesToDisplay);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic to reset the form to starting position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cboInvoiceNumbers.SelectedIndex = -1;
                cboInvoiceDates.SelectedIndex = -1;
                cboInvoiceCosts.SelectedIndex = -1;
                clsSearchLogic.resetWindow();
                dgInvoices.ItemsSource = clsSearchLogic.InvoicesToDisplay;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic for when the selection is changed on the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        /// returns the found invoice
        /// </summary>
        /// <returns></returns>
        public Invoice returnFoundInvoice()
        {
            try
            {
                Invoice temp = clsSearchLogic.returnInvoice();
                return temp;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            return null;
        }

        /// <summary>
        /// helper method to update the cbo's
        /// </summary>
        /// <param name="list"></param>
        private void fillCBOS(ObservableCollection<Invoice> list)
        {
            cboInvoiceNumbers.ItemsSource = clsSearchLogic.loadInvoiceNumberCBO(list);

            cboInvoiceDates.ItemsSource = clsSearchLogic.loadDTCBO(list);

            cboInvoiceCosts.ItemsSource = clsSearchLogic.loadCostCBO(list);
        }
    }
}
