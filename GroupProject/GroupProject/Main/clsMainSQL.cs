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
