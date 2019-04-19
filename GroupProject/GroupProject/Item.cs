using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    /// <summary>
    /// this class will hold item information 
    /// extracted from the database
    /// </summary>
    public class Item : INotifyPropertyChanged
    {
        /// <summary>
        /// Single letter representing the 
        /// Items code
        /// </summary>
        private string itemCode;

        /// <summary>
        /// Short description of the item
        /// </summary>
        private string itemDescription;

        /// <summary>
        /// How much the item costs
        /// </summary>
        private double itemCost;

        /// <summary>
        /// Contract
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //auto generated public properties
        public string ItemCode
        {
            get => itemCode;
            set
            {
                itemCode = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemCode"));
            }
        }
        public string ItemDescription
        {
            get => itemDescription;
            set
            {
                itemDescription = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemDescription"));
            }
        }
        public double ItemCost
        {
            get => itemCost;
            set
            {
                itemCost = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemCost"));
            }
        }

        /// <summary>
        /// COnstructor for a jewelry item
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemDescription"></param>
        /// <param name="itemCost"></param>
        public Item(string itemCode, string itemDescription, double itemCost)
        {
            try
            {
                //ugh, watch the caps here
                ItemCode = itemCode;
                ItemDescription = itemDescription;
                ItemCost = itemCost;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// override the tostring to return a useful string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                //return "Item: " + ItemDescription + "Cost: " + ItemCost + "Code: " + ItemCode;
                return ItemCode + ": " + ItemDescription + " $" + ItemCost;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                        MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
