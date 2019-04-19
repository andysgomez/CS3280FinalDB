using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GroupProject.Main;
using GroupProject.Search;

namespace GroupProject.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        /// <summary>
        /// business logic for the Item window
        /// </summary>
        clsItemsLogic clsItemLogic;

        /// <summary>
        /// bool to let main know if any items in the item desc database have changed
        /// </summary>
        private bool bItemChange = false;

        /// <summary>
        /// holds list of invoices if item to delete
        /// is on any invoices
        /// </summary>
        private ObservableCollection<string> OcInvoices;

        /// <summary>
        /// public bool to alert when itemdesc db changes
        /// </summary>
        public bool bItemChanged
        {
            get
            {
                return bItemChange;
            }
        }

        /// <summary>
        /// Item window/wndItems initializer
        /// </summary>
        public wndItems()
        {
            try
            {
                InitializeComponent();

                clsItemLogic = new clsItemsLogic();
                OcInvoices = new ObservableCollection<string>();
                dgItemDesc.ItemsSource = clsItemLogic.RetrieveItems();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
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
        /// button click to add item to the itemdesc database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                string sBoxItemCode = txtBoxAddItemCode.Text;
                string sBoxCostChange = txtBoxAddCostChange.Text.ToString();
                string sBoxDescriptionChange = txtBoxAddDescriptionChange.Text;

                //checks empty strings
                if (sBoxItemCode == "" || sBoxCostChange == "" || sBoxDescriptionChange == "")
                {
                    lblNewError.Content = "Item Code, Description, and Cost can not be blank";
                }
                else
                {
                    bool bItemCodeTest = clsItemLogic.CheckItemCode(sBoxItemCode);

                    //Checks if itemCode is in use
                    if (bItemCodeTest)
                    {
                        lblNewError.Content = "That Item Code already exists";
                    }
                    else
                    {
                        //determine if cost is valid, executes addItems
                        if (!(clsItemLogic.AddItem(sBoxItemCode, sBoxCostChange, sBoxDescriptionChange)))
                        {
                            lblNewError.Content = "Enter only positive numbers for cost";
                        }
                        else
                        {
                            bItemChange = true;
                            ResetText();
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Edit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //check that item is selected
                if (dgItemDesc.SelectedIndex == -1)
                {
                    lblEditDeleteError.Content = "Select an Item from list";
                }
                else
                {
                    string sListItemCode = ((Item)dgItemDesc.SelectedItem).ItemCode;
                    string sListItemCost = txtBoxUpdateCost.Text.ToString();
                    string sListItemDesc = txtBoxUpdateDesc.Text;

                    if (sListItemCost == "" || sListItemDesc == "")
                    {
                        lblEditDeleteError.Content = "Item cost and description can't be blank";
                    }
                    else
                    {
                        if (!(clsItemLogic.UpdateItem(sListItemCode, sListItemCost, sListItemDesc)))
                        {
                            lblEditDeleteError.Content = "Item cost needs to be a positive number";
                        }
                        else
                        {
                            bItemChange = true;
                            dgItemDesc.ItemsSource = clsItemLogic.RetrieveItems();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// button click to delete items that aren't in an invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItemDesc.SelectedIndex == -1)
                {
                    lblEditDeleteError.Content = "Select an Item from list";
                }
                else
                {
                    string sInvoices;
                    string sListItemCode = ((Item)dgItemDesc.SelectedItem).ItemCode;

                    sInvoices = clsItemLogic.GetDelete(sListItemCode);

                    
                    if (sInvoices != null)
                    {
                        
                        string sDeleteError = "Item cannot be deleted as it is in the following Invoices: \n"
                                + sInvoices;
                        MessageBox.Show(sDeleteError);
                    }
                    else
                    {
                        bItemChange = true;
                        dgItemDesc.ItemsSource = clsItemLogic.RetrieveItems();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }   
        

        /// <summary>
        /// updates the txtboxes for edit or delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgItemDesc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ResetText();

                if(dgItemDesc.SelectedItem != null)
                {
                    lblItemCode.Content += ((Item)dgItemDesc.SelectedItem).ItemCode;
                    txtBoxUpdateCost.Text = ((Item)dgItemDesc.SelectedItem).ItemCost.ToString();
                    txtBoxUpdateDesc.Text = ((Item)dgItemDesc.SelectedItem).ItemDescription;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// button to clear current used txtboxes and updates datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetText();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Resets the text in added labels and textboxes
        /// </summary>
        private void ResetText()
        {
            try
            {
                if (txtBoxAddCostChange.Text != "" || txtBoxAddDescriptionChange.Text != "" || txtBoxAddItemCode.Text != "")
                {
                    txtBoxAddItemCode.Text = "";
                    txtBoxAddDescriptionChange.Text = "";
                    txtBoxAddCostChange.Text = "";
                }
                else if(lblItemCode.Content.ToString() != "" || txtBoxUpdateCost.Text != "" || txtBoxUpdateDesc.Text != "")
                {
                    lblItemCode.Content = "Item Code: ";
                    txtBoxUpdateCost.Text = "";
                    txtBoxUpdateDesc.Text = "";
                }

                dgItemDesc.ItemsSource = clsItemLogic.RetrieveItems();

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
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
        /// this is just a nothing method to help with formatting and try catch blocks
        /// </summary>
        private void exampleHelper()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Returns user to main Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// prevent non number input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBoxAddCostChange_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
       
    }
}
