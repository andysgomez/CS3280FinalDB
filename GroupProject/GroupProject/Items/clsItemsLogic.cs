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

        /// <summary>
        /// public List used if item is on invoices
        /// </summary>
        private string SInvoiceList;

        /// <summary>
        /// Public constructor of the item logic
        /// </summary>
        public clsItemsLogic()
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
        /// Adds item to the item desc db, 
        /// checks double for cost
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="sCost"></param>
        /// <param name="sDescription"></param>
        /// <returns>true if cost is valid, false else</returns>
        public bool AddItem(string sItemCode, string sCost, string sDescription)
        {
            try
            {
                double dTryingCost = CostCheck(sCost);

                if (dTryingCost > 0)
                {
                    clsLogicSQL.addNewItemToDataBase(sItemCode, sDescription, dTryingCost);
                    items = clsLogicSQL.getItems();

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
        /// runs checkItemCode sql to check if item code already exists
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns>false if code doesn't exist, true if it does</returns>
        public bool CheckItemCode(string sItemCode)
        {
            //checks to make sure the item code is not already being used
            return clsLogicSQL.checkItemCode(sItemCode);
        }

        /// <summary>
        /// Updates an item according to item code
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <param name="dCost"></param>
        /// <param name="sDescription"></param>
        /// <returns></returns>
        public bool UpdateItem(string sItemCode, string sCost, string sDescription)
        {
            try
            {
                //check corrected cost
                double TryingCost = CostCheck(sCost);
                

                if (TryingCost >= 0)
                {
                    clsLogicSQL.updateItem(sItemCode, sDescription, TryingCost);
                    items.Clear();
                    items = clsLogicSQL.getItems();
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
        /// Deletes an Item from itemdesc if item is not in an invoice
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns>returns null collection if not on invoice
        /// , else string collection of invoices</returns>
        public string GetDelete(string sItemCode)
        {
            try
            {
                bool bIsNotInvoice;
                
                bIsNotInvoice = clsLogicSQL.CheckInvoices(sItemCode);

                if (bIsNotInvoice)
                {
                    clsLogicSQL.deleteItemFromDataBase(sItemCode);

                    items = clsLogicSQL.getItems();

                    return null;
                }
                else
                {
                    SInvoiceList = clsLogicSQL.getInvoicesThatIncludeItem(sItemCode);
                    return SInvoiceList;
                }

                

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Checks input for cost to be double
        /// </summary>
        /// <param name="sCost"></param>
        /// <returns>returns cost if double and not negative
        /// -1 if not a double or negative</returns>
        private double CostCheck(string sCost)
        {
            try
            {
                double TestCost;
                
                if(double.TryParse(sCost, out TestCost))
                {
                    if (TestCost < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return TestCost;
                    }
                }
                else
                {
                    return -1;
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
