using GroupProject.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    /// <summary>
    /// this class will handle all db operations
    /// with the help of the clsDataAccess 
    /// class provided for us for the search window
    /// </summary>
    public class clsSearchSQL
    {

        /// <summary>
        /// Database access class
        /// </summary>
        private clsDataAccess db;

        

        public clsSearchSQL()
        {
            try
            {
                db = new clsDataAccess();                
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the invoices from the database and returns BindingList of them
        /// if a totalCost is passed in it only returns a list of the invoices
        /// that cost that much, ditto with the date. the date must be a string
        /// if it is a DateTime object use .ToShortDateString()
        /// </summary>
        /// <returns></returns>
        public BindingList<Invoice> loadInvoices(double totalCost = -999.9, string sDate = "1/1/1")//this makes it optional
        {
            try
            {
                BindingList<Invoice> invoices = new BindingList<Invoice>();

                int iRet = 0;

                string sSQL = "SELECT * FROM Invoices";
                if(totalCost!=-999.9)
                {
                    sSQL += " WHERE TotalCost = " + totalCost;
                    if(!(sDate.Equals("1/1/1")))
                    {
                        sSQL += " AND ";
                    }
                }
                if (!sDate.Equals("1/1/1"))
                {
                    DateTime dt;
                    DateTime.TryParse(sDate, out dt);
                    sSQL += " WHERE InvoiceDate = #" + dt.ToShortDateString() + "#";
                }

                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)
                {
                    string strInvoiceNum = ds.Tables[0].Rows[i][0].ToString();
                    string strDate = ds.Tables[0].Rows[i][1].ToString();
                    string strCost = ds.Tables[0].Rows[i][2].ToString();

                    int invoiceNumber;
                    DateTime date;
                    double cost;

                    Int32.TryParse(strInvoiceNum, out invoiceNumber);
                    DateTime.TryParse(strDate, out date);
                    double.TryParse(strCost, out cost);

                    BindingList<Item> items = loadInvoiceItems(invoiceNumber);
                    Invoice invoice = new Invoice(invoiceNumber, date, cost, null);

                    invoices.Add(invoice);
                }

                return invoices;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Returns a single invoice given the invoice number as an argument
        /// or invoice number and date
        /// or invoice number and date and cost
        /// pass in the date as a string, so if its a DateTime object use .ToShortDateString()
        /// to turn it into a string
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public Invoice loadInvoice(int invoiceNum, string sDate = "1/1/1", double cost = -999.9 )
        {
            try
            {
                int iRet = 0;
                string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum =" + invoiceNum;
                if(!(sDate.Equals("1/1/1")))//attach the date if it is specified
                {
                    DateTime dt;
                    DateTime.TryParse(sDate, out dt);
                    sSQL += " AND InvoiceDate = #" + dt.ToShortDateString() + "#";
                }
                if(cost != -999.9)//attach the cost if it is specified
                {
                    sSQL += " AND TotalCost=" + cost;
                }
                Invoice invoice = null;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)//iRet will be 1 if it worked and 0 if it didnt
                {

                    string strDate = ds.Tables[0].Rows[i][1].ToString();
                    string strCost = ds.Tables[0].Rows[i][2].ToString();

                    DateTime date;
                    //double cost;
                                       
                    DateTime.TryParse(strDate, out date);
                    double.TryParse(strCost, out cost);

                    BindingList<Item> items = loadInvoiceItems(invoiceNum);
                    invoice = new Invoice(invoiceNum, date, cost, items);
                }
                return invoice;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// returns a binding list of all the items on a selected invoice
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        private BindingList<Item> loadInvoiceItems(int invoiceNumber)
        {
            try
            {
                BindingList<Item> tempList = new BindingList<Item>();
                int iRet = 0;

                //these sql statements are taken from the help files
                string sSQL = "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems," +
                    "ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum =" + invoiceNumber;

                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)
                {
                    string ic = ds.Tables[0].Rows[i][0].ToString();
                    string id = ds.Tables[0].Rows[i][1].ToString();
                    string iCost = ds.Tables[0].Rows[i][2].ToString();

                    double cost;
                    double.TryParse(iCost, out cost);
                    Item item = new Item(ic, id, cost);
                    tempList.Add(item);
                }

                return tempList;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
