using System;
using GroupProject.Main;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

//For sharing data between windows the logic classes will have getMethods that return and data structures
//the other classes might need, and to indicate that changes have been made or backed out of, we plan to use
//boolean flags that are set and can be checked from the other windows...
//so like if one of the windows changes the database, it will set a flag indicating that its been changed
//so the returning window can reload its data
namespace GroupProject.Items
{
    class clsItemsLogic
    {
        /// <summary>
        /// object for the Items SQL class
        /// </summary>
        private clsItemsSQL clsLogicSQL;

        /// <summary>
        /// binding list that will be used in the DataGrid
        /// </summary>
        private ObservableCollection<Item> items;

        //ObservableCollection<Item>

        /// <summary>
        /// global boolean flag that will be set when something in the database is changed
        /// </summary>
        public bool bDatabaseChange = false;

        /// <summary>
        /// Public constructor of the item logic
        /// </summary>
        public clsItemsLogicSQL()
        {
            try
            {
                clsLogicSQL = new clsItemsSQL();
                items = clsLogicSQL.getItems();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// returns the items in the list
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Item> RetrieveItems()
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
        /// 
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="dCost"></param>
        /// <param name="sDescription"></param>
        public void AddItem(string sItemCode, string sCost, string sDescription)
        {
            try
            {
                bool bTestItemCode;

                //checks to make sure the item code is not already being used
                bTestItemCode = clsLogicSQL.checkItemCode(sItemCode);

                if (bTestItemCode)
                {
                    bool getCost;

                    double TryingCost;
                    //check corrected cost
                    getCost = double.TryParse(sCost, out TryingCost);

                    if (getCost)
                    {
                        clsLogicSQL.addNewItemToDataBase(sItemCode, sDescription, TryingCost);
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
        /// Updates an item according to item code
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="dCost"></param>
        /// <param name="sDescription"></param>
        /// <returns></returns>
        public void UpdateItem(string sItemCode, string sCost, string sDescription)
        {
            try
            {
                bool getCost;

                double TryingCost;
                //check corrected cost
                getCost = double.TryParse(sCost, out TryingCost);

                if (getCost)
                {
                    clsLogicSQL.updateItem(sItemCode, sDescription, TryingCost);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes an Item from itemdesc if item is not in an invoice
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns>0 if item is in an invoice, else 1 if item deleted</returns>
        public void GetDelete(string sItemCode)
        {
            try
            {
                bool bIsNotInvoice;

                bIsNotInvoice = clsLogicSQL.CheckInvoices(sItemCode);

                if (bIsNotInvoice)
                {
                    clsLogicSQL.deleteItemFromDataBase(sItemCode);
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
