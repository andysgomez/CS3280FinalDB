﻿using GroupProject.Items;
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
        ObservableCollection<Item> cboItemsList;

        

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

               
                dgCurrentInvoice.ItemsSource = clsMainLogic.getCurrentInvoiceItems();
                
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
                this.Hide();
                wndItems.ShowDialog();
                this.Show();
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
                cboItems.ItemsSource = cboItemsList;
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
                cboItemsList = clsMainLogic.getItemsSold();
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

                    //sCode = toAdd.ItemCode;
                   
                    clsMainLogic.addItemToCurrInvoice(toAdd);
                   
                    txtInvoiceTotal.Text = clsMainLogic.CurrentInvoiceCost.ToString();
                }
               
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
