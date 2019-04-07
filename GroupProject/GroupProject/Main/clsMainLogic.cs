using System;
using System.Collections.Generic;
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

    /// <summary>
    /// All the logic behind the main window
    /// </summary>
    public class clsMainLogic
    {

        /// <summary>
        /// object to use to access the database
        /// </summary>
        clsMainSQL clsMainSQL;

        /// <summary>
        /// binding list used for the items combobox
        /// </summary>
        BindingList<Item> items;

        /// <summary>
        /// Constructor for the MainLogic
        /// </summary>
        public clsMainLogic()
        {
            try
            {
                clsMainSQL = new clsMainSQL();
                items = clsMainSQL.getItems();
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
