using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    //For sharing data between windows the logic classes will have getMethods that return and data structures
    //the other classes might need, and to indicate that changes have been made or backed out of, we plan to use
    //boolean flags that are set and can be checked from the other windows...
    //so like if one of the windows changes the database, it will set a flag indicating that its been changed
    //so the returning window can reload its data


    /*
     * The main window should allow the user to create new invoices, edit existing invoices, or delete existing invoices.
     * There should be just one window for all functionality of the main window.  So, the main window will NOT open other
     * windows to add/edit/delete invoices.  It will also have a menu (at the top, use a menu control) that will have two
     * functionalities.  The first will allow the user to update a def table that contains the items.  The next will be to
     * open a search screen used to search for invoices.
    If a new invoice is created the user may enter data pertaining to that invoice.  Since an auto-generated number is used in
    the database for the invoice number, when a user creates a new invoice, just display “TBD” for the Invoice Number.  An invoice
    date will also be assigned by the user.  Next different items will be entered by the user.  The items will be selected from 
    a drop-down box and the cost for that item will be put into a read only textbox.  This will be the default cost of an item.
    Once the item is selected, the user can add the item.  As many items as needed should be able to be added.  All items entered
    should be displayed for viewing in a list (something like a DataGrid).  Items may be deleted from the list.  A running total 
    of the cost of all items should be displayed as items are entered or deleted.
    Once all the items are entered the user can save the invoice.  Once the Invoice is saved, query the max 
    invoice number from the database, to display for the invoice number (for our project, this will work, since
    the last inserted invoice, will be the max).  This will lock the data in the invoice for viewing only.  
    From here the user may choose to Edit the Invoice or Delete the Invoice.  

     * */

    /// <summary>
    /// All the logic behind the main window
    /// </summary>
    public class clsMainLogic
    {

        /// <summary>
        /// object to use to access the database
        /// </summary>
        private clsMainSQL clsMainSQL;

        /// <summary>
        /// binding list used for the items combobox
        /// </summary>
        private ObservableCollection<Item> items;

        /// <summary>
        /// A binding list for the current invoices Items
        /// </summary>
        ObservableCollection<Item> currentInvoiceItems;

        /// <summary>
        /// The invoice number of the invoice
        /// currently being edited
        /// </summary>
        private int currentInvoiceNumber;

        /// <summary>
        /// The current cost of the currently displayed invoice
        /// </summary>
        private double currentInvoiceCost;

        /// <summary>
        /// flag used for program context
        /// </summary>
        private bool makingNewInvoice;

        /// <summary>
        /// flag used for program context
        /// </summary>
        private bool editingInvoice;

        public double CurrentInvoiceCost { get => currentInvoiceCost; set => currentInvoiceCost = value; }
        public int CurrentInvoiceNumber { get => currentInvoiceNumber; set => currentInvoiceNumber = value; }
        public bool MakingNewInvoice { get => makingNewInvoice; set => makingNewInvoice = value; }
        public bool EditingInvoice { get => editingInvoice; set => editingInvoice = value; }

        /// <summary>
        /// Constructor for the MainLogic
        /// </summary>
        public clsMainLogic()
        {
            try
            {
                CurrentInvoiceNumber = -1;
                CurrentInvoiceCost = 0;
                clsMainSQL = new clsMainSQL();
                items = clsMainSQL.getItems();
                currentInvoiceItems = new ObservableCollection<Item>();
                MakingNewInvoice = false;
                EditingInvoice = false;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Adds another item to the currentInvoiceItems list
        /// </summary>
        /// <param name="itemToAdd"></param>
       public void addItemToCurrInvoice(Item itemToAdd)
        {
            try
            {
                if(itemToAdd != null)
                {
                    currentInvoiceItems.Add(itemToAdd);
                    currentInvoiceCost = getCalculateInvoiceCost();
                }
                    
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// calculates and returns the cost of the invoice currently 
        /// visible
        /// </summary>
        /// <returns></returns>
        private double getCalculateInvoiceCost()
        {

            try
            {
                double temp = 0;

                foreach (Item item in currentInvoiceItems)
                {
                    temp += item.ItemCost;
                }

                return temp;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }

        /// <summary>
        /// gets the items object
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Item> getItemsSold()
        {
            try
            {
                return items;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// gets the items object
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Item> getCurrentInvoiceItems()
        {
            try
            {
                return currentInvoiceItems;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// remove an item from the current invoice
        /// </summary>
        /// <param name="toDelete"></param>
        internal void deleteFromCurrInvoice(Item toDelete)
        {
            try
            {
                currentInvoiceItems.Remove(toDelete);
                currentInvoiceCost = getCalculateInvoiceCost();
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }

        /// <summary>
        /// save an invoice to the database
        /// </summary>
        internal void saveInvoice()
        {
            try
            {
                if(getCalculateInvoiceCost() > 0 && MakingNewInvoice == true)//make sure the invoice has something in it
                {
                    int lineItem = 1;
                    int invoiceNumber = clsMainSQL.addInvoiceToDataBase(DateTime.Now, getCalculateInvoiceCost());
                    CurrentInvoiceNumber = invoiceNumber;
                    foreach (Item item in currentInvoiceItems)
                    {
                        clsMainSQL.addItemToInvoice(invoiceNumber, lineItem, item.ItemCode);
                        lineItem++;
                    }
                    MakingNewInvoice = false;                   
                }
                else if(EditingInvoice)
                {
                    if(getCalculateInvoiceCost() == 0)//if all items deleted and invoice "saved" delete invoice
                    {
                        deleteCurrentInvoice();
                        EditingInvoice = false;
                    }
                    else
                    {
                        clsMainSQL.clearItemsFromInvoice(currentInvoiceNumber);
                        int lineItem = 1;
                        foreach (Item item in currentInvoiceItems)
                        {  
                            clsMainSQL.addItemToInvoice(CurrentInvoiceNumber, lineItem, item.ItemCode);
                            lineItem++;
                        }
                        EditingInvoice = false;
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// method called to delete an invoice from the database
        /// </summary>
        internal void deleteCurrentInvoice()
        {
            try
            {
                if(currentInvoiceNumber != -1)
                {
                    clsMainSQL.deleteInvoice(currentInvoiceNumber);
                    currentInvoiceNumber = -1;
                    currentInvoiceItems.Clear();
                    EditingInvoice = false;
                    MakingNewInvoice = false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
