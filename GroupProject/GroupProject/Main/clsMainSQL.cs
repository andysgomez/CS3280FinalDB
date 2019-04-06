using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    /// <summary>
    /// this class will handle all db operations
    /// with the help of the clsDataAccess 
    /// class provided for us
    /// </summary>
    public class clsMainSQL
    {
        /// <summary>
        /// Database access class
        /// </summary>
        private clsDataAccess db;

        /// <summary>
        /// A binding list for the items
        /// to be used as a source for a dropdown menu
        /// </summary>
        private BindingList<Item> items;

        /// <summary>
        /// Constructor for sql worker
        /// </summary>
        public clsMainSQL()
        {
            try
            {
                db = new clsDataAccess();
                items = new BindingList<Item>();

                loadItems();
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        

        /// <summary>
        /// delete an invoice from the database
        /// </summary>
        /// <param name="invoiceNumber">The invoice number to delete</param>
        private void deleteInvoice(int invoiceNumber)
        {
            try
            {
                string sSQL = "DELETE from LineItems where InvoiceNum =" + invoiceNumber;
                db.ExecuteNonQuery(sSQL);
                sSQL = "DELETE from Invoices where InvoiceNum =" + invoiceNumber;
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// this method adds an invoice to the database and returns 
        /// that invoices number
        /// </summary>
        /// <param name="date"></param>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        private int addInvoiceToDataBase(DateTime date, double totalCost)
        {
            try
            {
                string dateString = date.ToShortDateString();
                string sSQL = "INSERT INTO Invoices (InvoiceDate, TotalCost) Values ('#" + dateString + "#'," + totalCost + ")";
                db.ExecuteNonQuery(sSQL);
                int invoiceNum = 0;
                sSQL = "SELECT MAX(InvoiceNum) FROM Invoices";
                string inv = db.ExecuteScalarSQL(sSQL);
                Int32.TryParse(inv, out invoiceNum);
                return invoiceNum;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// add another item to an existing invoice
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="lineItemNumber"></param>
        /// <param name="itemCode"></param>
        private void addItemToInvoice(int invoiceNumber, int lineItemNumber, string itemCode)
        {
            try
            {
                //may have an issue with itemCode
                string sSQL = "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ("
                    + invoiceNumber + "," + lineItemNumber + ",'" + itemCode + "')";
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// returns an invoice with the selected invoice number
        /// if the invoice doesnt exist it returns a null pointer
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        private Invoice getInvoice(int invoiceNum)
        {
            try
            {
                int iRet = 0;
                Invoice temp = new Invoice();
                string sSQL = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum =" + invoiceNum;
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)
                {
                    string invNum = ds.Tables[0].Rows[i][0].ToString();
                    string invDate = ds.Tables[0].Rows[i][1].ToString();
                    string invCost = ds.Tables[0].Rows[i][2].ToString();

                    int iNumber;
                    Int32.TryParse(invNum, out iNumber);
                    DateTime dt;
                    DateTime.TryParse(invDate, out dt);
                    double tc;
                    double.TryParse(invCost, out tc);
                   
                    BindingList<Item> items = loadInvoiceItems(iNumber);
                    temp = new Invoice(iNumber, dt, tc, items);
                }

                if(iRet == 0)
                {
                    temp = null;
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

        /// <summary>
        /// Update the cost of an invoice in the database
        /// </summary>
        /// <param name="cost">the new cost</param>
        /// <param name="invoiceNumber">The invoice number</param>
        private void updateCost(double cost, int invoiceNumber)
        {
            try
            {
                string sSQL = "UPDATE Invoices SET TotalCost = "+ cost + " WHERE InvoiceNum = " + invoiceNumber;
                db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// goes to the database and loads all the items into a datastructure
        /// </summary>
        private void loadItems()
        {
            try
            {
                int iRet = 0;

                string sSQL = "SELECT ItemDesc.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM ItemDesc";

                DataSet ds1 = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)
                {
                    string iC = ds1.Tables[0].Rows[i][0].ToString();

                    string iD = ds1.Tables[0].Rows[i][1].ToString();

                    string iCost = ds1.Tables[0].Rows[i][2].ToString();
                    double cost;
                    double.TryParse(iCost, out cost);

                    Item item = new Item(iC, iD, cost);
                    items.Add(item);
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
        public BindingList<Item> getItems()
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
    }
}
