using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For sharing data between windows the logic classes will have getMethods that return and data structures
//the other classes might need, and to indicate that changes have been made or backed out of, we plan to use
//boolean flags that are set and can be checked from the other windows...
//so like if one of the windows changes the database, it will set a flag indicating that its been changed
//so the returning window can reload its data
namespace GroupProject.Search
{
    class clsSearchLogic
    {
        /// <summary>
        /// the invoice number to be returned to the main window
        /// store the invoice number of whatever is found here
        /// </summary>
        private int foundInvoiceNumber;


        public int FoundInvoiceNumber { get => foundInvoiceNumber; set => foundInvoiceNumber = value; }

        /// <summary>
        /// Constructor for our search logic class
        /// </summary>
        public clsSearchLogic(ref int foundInvoice)
        {
            FoundInvoiceNumber = 5001;//this is to test while this window is under construction
            foundInvoice = FoundInvoiceNumber;
        }

       
    }
}
