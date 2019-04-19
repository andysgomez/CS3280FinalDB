using GroupProject.Items;
using GroupProject.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GroupProject.Main
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        /// <summary>
        /// Window variable to open the items window
        /// </summary>
        wndItems wndItems;

        /// <summary>
        /// Window varibale to open the search window
        /// </summary>
        wndSearch wndSearch;

        /// <summary>
        /// business logic for main class
        /// </summary>
        clsMainLogic clsMainLogic;

        /// <summary>
        /// binding list used for the items combobox
        /// </summary>
        ObservableCollection<Item> lstItemsForSale;

        /// <summary>
        /// the number of the current invoice
        /// used to delete and edit invoices 
        /// from the search window
        /// </summary>
        int currentInvoiceNumber;

        Invoice foundInvoice;

        public int CurrentInvoiceNumber { get => currentInvoiceNumber; set => currentInvoiceNumber = value; }

        /// <summary>
        /// wndMain COnstructor
        /// initialize all windows in here
        /// </summary>
        public wndMain()
        {

            //  need to be able to:
            //      create new invoices
            //      edit existing invoices
            //      delete existing invoices
            try
            {
                InitializeComponent();

                //close all hidden windows when closing
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                //foundInvoice = null;
                //initialize the other windows
                //this will likely be where I pass
                //handles to the db and whatever
                wndItems = new wndItems();
                wndSearch = new wndSearch();

                //initialize business and sql handles
                clsMainLogic = new clsMainLogic();


                //get the items from the logic class
                updateItems();

                //populate combobox
                loadItemsCBO();

                //set the date to today
                dtDate.SelectedDate = DateTime.Today;
                dgCurrentInvoice.ItemsSource = clsMainLogic.getCurrentInvoiceItems();

                CurrentInvoiceNumber = -1;


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
        /// This is what happens when the search menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                this.Hide();
                wndSearch.ShowDialog();
                this.Show();
                //make sure that foundInvoice exists
                foundInvoice = wndSearch.returnFoundInvoice();
                if(foundInvoice == null || foundInvoice.InvoiceNumber == -1)//if cancel or x was clicked pretend nothing happened
                {
                    return;
                }
                
                clsMainLogic.loadInvoice(foundInvoice.InvoiceNumber);
                txtInvoiceNumber.Text = clsMainLogic.CurrentInvoiceNumber.ToString();
                txtInvoiceTotal.Text = "$" + clsMainLogic.CurrentInvoiceCost.ToString() + ".00";
                clsMainLogic.MakingNewInvoice = false;
                clsMainLogic.EditingInvoice = false;
                dgCurrentInvoice.ItemsSource = clsMainLogic.getCurrentInvoiceItems();
                //enable disable buttons
                btnEdit.IsEnabled = true;
                btnSaveInvoice.IsEnabled = false;
                btnDelete.IsEnabled = true;
                dgCurrentInvoice.IsEnabled = false;

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// this is what happens when the update items menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //maybe save some state info?
                //maybe it saves already?
                this.Hide();
                wndItems.ShowDialog();
                this.Show();

                //just update the combobox
                clsMainLogic.reloadCatalog();
                cboItems.ItemsSource = clsMainLogic.getItemsSold();

                //reload the current invoice in case its items changed
                if (foundInvoice != null && foundInvoice.InvoiceNumber != -1)
                {
                    clsMainLogic.loadInvoice(foundInvoice.InvoiceNumber);
                    txtInvoiceNumber.Text = clsMainLogic.CurrentInvoiceNumber.ToString();
                    txtInvoiceTotal.Text = "$" + clsMainLogic.CurrentInvoiceCost.ToString() + ".00";
                    dgCurrentInvoice.ItemsSource = clsMainLogic.getCurrentInvoiceItems();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// just closes the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, RoutedEventArgs e)
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
        /// This is what happens when the item selected from the drop down menu has changed
        /// it should change a label that displays the cost
        /// and enable the add item button
        /// to add it to the currently selected invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        /// call this to fill the items combo box
        /// </summary>
        private void loadItemsCBO()
        {
            try
            {
                cboItems.ItemsSource = lstItemsForSale;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the items bindinglist for use in the items combobox
        /// </summary>
        private void updateItems()
        {
            try
            {
                lstItemsForSale = clsMainLogic.getItemsSold();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Adds the currently selected Item to the current invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboItems.SelectedIndex != -1)
                {
                    Item toAdd = (Item)cboItems.SelectedItem;
                    btnSaveInvoice.IsEnabled = true;
                    dgCurrentInvoice.IsEnabled = true;

                    clsMainLogic.addItemToCurrInvoice(toAdd);

                    txtInvoiceTotal.Text = "$" + clsMainLogic.CurrentInvoiceCost.ToString() + ".00";
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Logic for handling a delete button click in the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Item toDelete = (Item)dgCurrentInvoice.SelectedItem;
                clsMainLogic.deleteFromCurrInvoice(toDelete);
                txtInvoiceTotal.Text = "$" + clsMainLogic.CurrentInvoiceCost.ToString() + ".00";


            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Logic for the save button.
        /// Should save the current invoice items 
        /// to the data base
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dt = (DateTime)dtDate.SelectedDate;
               
                clsMainLogic.saveInvoice(dt);
                int invoNum = clsMainLogic.CurrentInvoiceNumber;
                if (invoNum != -1)
                {
                    txtInvoiceNumber.Text = invoNum.ToString();
                    btnDelete.IsEnabled = true;
                    btnEdit.IsEnabled = true;
                    clsMainLogic.EditingInvoice = false;
                    clsMainLogic.MakingNewInvoice = false;
                    dgCurrentInvoice.IsEnabled = false;
                }
                else
                {
                    txtInvoiceNumber.Text = "Save Failed... you should never see this";
                }
                btnSaveInvoice.IsEnabled = false;
                mnuUpdateItems.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic for delete invoice button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int invoNum = clsMainLogic.CurrentInvoiceNumber;


                if (invoNum != -1)
                {
                    clsMainLogic.deleteCurrentInvoice();
                    clsMainLogic.EditingInvoice = false;
                    clsMainLogic.MakingNewInvoice = false;
                    txtInvoiceNumber.Text = "TBD";
                    txtInvoiceTotal.Text = "$0.00";
                    dgCurrentInvoice.IsEnabled = false;
                    btnAddItem.IsEnabled = false;
                    cboItems.SelectedIndex = -1;
                    cboItems.IsEnabled = false;
                    btnEdit.IsEnabled = false;
                    btnNew.IsEnabled = true;
                    btnDelete.IsEnabled = false;
                    btnSaveInvoice.IsEnabled = false;
                    mnuUpdateItems.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// logic for new button press
        /// should enable the combobox
        /// the save button
        /// and set some flags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAddItem.IsEnabled = true;
                mnuUpdateItems.IsEnabled = false;
                cboItems.IsEnabled = true;
                btnEdit.IsEnabled = false;
                clsMainLogic.MakingNewInvoice = true;
                clsMainLogic.EditingInvoice = false;                
                clsMainLogic.makeNewInvoice();
                dgCurrentInvoice.ItemsSource = clsMainLogic.getCurrentInvoiceItems();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// throw delete key press to the button logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Delete)
                {
                    btnDeleteItem_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
           
        }

        /// <summary>
        /// logic for the edit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsMainLogic.EditingInvoice = true;
                clsMainLogic.MakingNewInvoice = false;
                dgCurrentInvoice.IsEnabled = true;
                cboItems.IsEnabled = true;
                btnAddItem.IsEnabled = true;
                mnuUpdateItems.IsEnabled = false;
                
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
