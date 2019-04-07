using GroupProject.Main;
using System;
using System.Collections.Generic;
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
        /// clsItemSQL constructor. 
        /// initializes the dbaccess class
        /// </summary>
        public clsItemsSQL()
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
        /// Get a list of all the items in the database
        /// </summary>
        /// <param name="itemDesc"></param>
        /// <returns></returns>
        public BindingList<Item> getItems(string itemDesc)
        {
            //select ItemCode, ItemDesc, Cost from ItemDesc
            try
            {
                int iRet = 0;
                BindingList<Item> temp = new BindingList<Item>();
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

                    temp.Add(item);
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
        /// returns a bindinglist of invoice numbers that include a certain item
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public BindingList<int> getInvoicesThatIncludeItem(string itemCode)
        {
            //select distinct(InvoiceNum) from LineItems where ItemCode = 'A'
            //I have never used "distict" in sql before... Im not sure this one will work
            try
            {
                BindingList<int> temp = new BindingList<int>();
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
        /// Update a specific item in the database
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemDesc"></param>
        /// <param name="newCost"></param>
        /// <returns>returns the number of items updated</returns>
        public int updateItem(string itemCode, string itemDesc, double newCost)
        {
            //Update ItemDesc Set ItemDesc = 'abcdef', Cost = 123 where ItemCode = 'A'
            try
            {
                string sSQL = "Update ItemDesc Set ItemDesc = '" + itemDesc +
                    "', Cost = " + newCost + " where ItemCode = '" + itemCode + "'";

               return db.ExecuteNonQuery(sSQL);
               
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
        public int addNewItemToDataBase(string itemCode, string itemDesc, double itemCost)
        {
            //Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('ABC', 'blah', 321)
            try
            {
                string sSQL = "INSERT into ItemDesc (ItemCode, ItemDesc, Cost) Values ('" +
                    itemCode + "', '" + itemDesc + "', " + itemCost + ")";
                return db.ExecuteNonQuery(sSQL);
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
        public int deleteItemFromDataBase(string itemCode)
        {
            try
            {
                string sSQL = "Delete from ItemDesc Where ItemCode = '" + itemCode + "'";
                return db.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }           
    }
}
