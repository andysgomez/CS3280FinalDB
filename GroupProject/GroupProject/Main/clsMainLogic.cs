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
        /// The current cost of the currently displayed invoice
        /// </summary>
        private double currentInvoiceCost;

        public double CurrentInvoiceCost { get => currentInvoiceCost; set => currentInvoiceCost = value; }

        /// <summary>
        /// Constructor for the MainLogic
        /// </summary>
        public clsMainLogic()
        {
            try
            {
                CurrentInvoiceCost = 0;
                clsMainSQL = new clsMainSQL();
                items = clsMainSQL.getItems();
                currentInvoiceItems = new ObservableCollection<Item>();

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
                    CurrentInvoiceCost += itemToAdd.ItemCost;
                }
                    
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
    }
}
