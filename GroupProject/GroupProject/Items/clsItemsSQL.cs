using GroupProject.Main;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Items
{
    /// <summary>
    /// this class will handle all db operations
    /// with the help of the clsDataAccess 
    /// class provided for us for the search window
    /// </summary>
    public class clsItemsSQL
    {

        /// <summary>
        /// Database access class
        /// </summary>
        private clsDataAccess db;

        /// <summary>
        /// collection to hold the items for the item class
        /// </summary>
        private ObservableCollection<Item> items;

        /// <summary>
        /// clsItemSQL constructor. 
        /// initializes the dbaccess class
        /// </summary>
        public clsItemsSQL()
        {
            try
            {
                db = new clsDataAccess();
                items = new ObservableCollection<Item>();
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all the items in the database
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Item> getItems()
        {
            //select ItemCode, ItemDesc, Cost from ItemDesc
            try
            {
                int iRet = 0;

                ObservableCollection<Item> temp = new ObservableCollection<Item>();

                string sSQL = "select ItemCode, ItemDesc, Cost from ItemDesc";

                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)
                {
                    string ic = ds.Tables[0].Rows[i][0].ToString();
                    string id = ds.Tables[0].Rows[i][1].ToString();
                    string iCost = ds.Tables[0].Rows[i][2].ToString();

                    double cost;
                    double.TryParse(iCost, out cost);
                    Item item = new Item(ic, id, cost);

                    items.Add(item);
                }

                return items;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }


        /// <summary>
        /// returns a bindinglist of invoice numbers that include a certain item
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public ObservableCollection<int> getInvoicesThatIncludeItem(string itemCode)
        {
            //select distinct(InvoiceNum) from LineItems where ItemCode = 'A'
            //I have never used "distict" in sql before... Im not sure this one will work
            try
            {
                ObservableCollection<int> temp = new ObservableCollection<int>();
                int iRet = 0;

                string sSQL = "select distinct(InvoiceNum) from LineItems where ItemCode = '" + itemCode + "'";

                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                for (int i = 0; i < iRet; i++)
                {
                    string sInvoiceNum = ds.Tables[0].Rows[i][0].ToString();

                    int invoiceNum;
                    Int32.TryParse(sInvoiceNum, out invoiceNum);

                    temp.Add(invoiceNum);
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
        /// Executes SQL that returns if item is in an invoice
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns>true if there are no invoices with that item, false if there is</returns>
        public bool CheckInvoices(string sItemCode)
        {
            try
            {
                int iRet = 0;

                string sSQL = "SELECT DISTINCT(InvoiceNum) FROM LineItems WHERE ItemCode = '" + sItemCode + "';";
                DataSet ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                //checks the returned number of selected rows
                if (iRet == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Update a specific item in the database
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemDesc"></param>
        /// <param name="newCost"></param>
        /// <returns>returns the number of items updated</returns>
        public bool updateItem(string itemCode, string itemDesc, double newCost)
        {
            //Update ItemDesc Set ItemDesc = 'abcdef', Cost = 123 where ItemCode = 'A'
            try
            {
                int iTest = 0;

                string sSQL = "Update ItemDesc Set ItemDesc = '" + itemDesc +
                    "', Cost = " + newCost + " where ItemCode = '" + itemCode + "'";

                iTest = db.ExecuteNonQuery(sSQL);

                if (iTest != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }



        /// <summary>
        /// adds a new item to the database
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemDesc"></param>
        /// <param name="itemCost"></param>
        /// <returns></returns>
        public bool addNewItemToDataBase(string itemCode, string itemDesc, double itemCost)
        {
            //Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('ABC', 'blah', 321)
            try
            {
                int iTester = 0;

                string sSQL = "INSERT into ItemDesc (ItemCode, ItemDesc, Cost) Values ('" +
                    itemCode + "', '" + itemDesc + "', " + itemCost + ")";
                iTester = db.ExecuteNonQuery(sSQL);

                if (iTester != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// uses SQL to determine if itemcode is already used in item desc table
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns>bool true if item code doesn't exist
        /// returns false if it does</returns>
        public bool checkItemCode(string sItemCode)
        {
            try
            {
                string sSQL = "SELECT DISTINCT(ItemCode) FROM ItemDesc WHERE ItemCode = '" + sItemCode + "'";

                string sHolderString = db.ExecuteScalarSQL(sSQL);


                if (sHolderString == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Delete an item from the database
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public bool deleteItemFromDataBase(string itemCode)
        {
            try
            {
                string sSQL = "Delete from ItemDesc Where ItemCode = '" + itemCode + "'";
                int iTester = db.ExecuteNonQuery(sSQL);

                if (iTester != 0)
                {
                    return true;
                }
                else
                {
                    return false;
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
