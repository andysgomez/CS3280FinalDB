using GroupProject.Main;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject 
{
    /// <summary>
    /// A class to hold data for Invoice objects
    /// </summary>
    public class Invoice : INotifyPropertyChanged
    {
        /// <summary>
        /// The invoice number
        /// </summary>
        private int invoiceNumber;

        /// <summary>
        /// the date of the invoice
        /// </summary>
        private DateTime invoiceDate;

        /// <summary>
        /// the total cost of the invoice
        /// </summary>
        private double totalCost;

        /// <summary>
        /// the list of items on the invoice
        /// </summary>
        private ObservableCollection<Item> items;

        /// <summary>
        /// Contract
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        public int InvoiceNumber
        {
            get => invoiceNumber;
            set
            {
                invoiceNumber = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InvoiceNumber"));
            }
        }
        public DateTime InvoiceDate
        {
            get => invoiceDate;
            set
            {
                invoiceDate = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InvoiceDate"));
            }
        }
        public double TotalCost
        {
            get => totalCost;
            set
            {
                totalCost = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalCost"));
            }
        }
        public ObservableCollection<Item> Items
        {
            get => items;
            set
            {
                items = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
            }
        }


        /// <summary>
        /// blank constructor for an invoice
        /// </summary>
        public Invoice()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// constructor that takes in all data.
        /// use this with the sql classes
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="date"></param>
        /// <param name="totalCost"></param>
        /// <param name="items"></param>
        public Invoice(int invoiceNumber, DateTime date, double totalCost, ObservableCollection<Item> items)
        {
            try
            {
                InvoiceNumber = invoiceNumber;
                this.InvoiceDate = date;
                TotalCost = totalCost;
                Items = items;
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// public method to add an item to the invoice
        /// recalculates the total
        /// </summary>
        /// <param name="item"></param>
        public void addItemToInvoice(Item item)
        {
            try
            {
                Items.Add(item);
                calculateTotal();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Helper method to calculate the total cost of on invoice
        /// also updates this invoices cost
        /// </summary>
        /// <returns></returns>
        private double calculateTotal()
        {
            try
            {
                double temp = 0.0;

                foreach (Item item in Items)
                {
                    temp += item.ItemCost;
                }
                TotalCost = temp;
                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// gets the total of the invoice
        /// </summary>
        /// <returns></returns>
        public double getTotal()
        {
            try
            {
                return calculateTotal();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        
    }
}
