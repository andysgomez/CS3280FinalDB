using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//For sharing data between windows the logic classes will have getMethods that return and data structures
//the other classes might need, and to indicate that changes have been made or backed out of, we plan to use
//boolean flags that are set and can be checked from the other windows...
//so like if one of the windows changes the database, it will set a flag indicating that its been changed
//so the returning window can reload its data
namespace GroupProject.Search

    /* The user also needs to be able to search for invoices, which will be a choice from the menu.
     * On the search screen, all invoices should be displayed in a list (like a DataGrid) for the user to select.
     * The user may limit the invoices displayed by choosing an Invoice Number from a drop down,
     * selecting an invoice date, or selecting the total charge from a drop-down box. 
     * The total charges in the drop-down box should be the unique set of total charges
     * sorted from smallest to largest.  When a limiting item is selected, the list should only reflect
     * those invoices that match the criteria.  So, the user should be able to select a date and a 
     * total charge, and only invoices that match both will be displayed.  A clear selection button
     * should reset the form to its initial state.  Once an invoice is selected the user will click
     * a “Select Invoice” button, which will close the search form and open the selected invoice up
     * for viewing on the main screen.  From there the user may choose to Edit or Delete the invoice
     */
{
    class clsSearchLogic
    {
        /// <summary>
        /// the invoice number to be returned to the main window
        /// store the invoice number of whatever is found here
        /// </summary>
        private int foundInvoiceNumber;

        private clsSearchSQL clsSearchSQL;

        private ObservableCollection<Invoice> invoices;

        


        public int FoundInvoiceNumber { get => foundInvoiceNumber; set => foundInvoiceNumber = value; }

        /// <summary>
        /// Constructor for our search logic class
        /// </summary>
        public clsSearchLogic(ref int foundInvoice)
        {

            try
            {
                resetWindow(ref foundInvoice);
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }

       

        private void resetWindow(ref int foundInvoice)
        {
            try
            {

                //initialize all variables
                clsSearchSQL = new clsSearchSQL();
                invoices = new ObservableCollection<Invoice>();
                invoices = clsSearchSQL.loadInvoices();
               

                FoundInvoiceNumber = 5001;//this is to test while this window is under construction
                foundInvoice = FoundInvoiceNumber;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public ObservableCollection<int> loadInvoiceNumberCBO()
        {
            try
            {
                ObservableCollection<int> temp = new ObservableCollection<int>();

                foreach (Invoice i in invoices)
                {
                    temp.Add(i.InvoiceNumber);
                }

                return temp;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
