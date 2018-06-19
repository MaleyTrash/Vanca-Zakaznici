using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZakazniciMVVM
{
    class MainWindowContext : INotifyPropertyChanged
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public ObservableCollection<Customer> CustomersCollection { get; set; } = new ObservableCollection<Customer>();
        public RelayCommand<String> SaveCustomerCommand { get; set; }
        public RelayCommand<String> DeleteCustomerCommand { get; set; }
        public RelayCommand<String> EditCustomerCommand { get; set; }

        private Customer selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return this.selectedCustomer;
            }
            set
            {
                if(value != null)
                {
                    FirstName = value.FirstName;
                    LastName = value.LastName;
                    Mail = value.Contact.Mail;
                    Adress = value.Contact.Adress;
                    
                }
                else
                {
                    Debug.WriteLine("neni");
                }
                this.selectedCustomer = value;
                PropertyChanged(this.selectedCustomer, new PropertyChangedEventArgs("SelectedCustomer"));
                PropertyChanged(this.FirstName, new PropertyChangedEventArgs("FirstName"));
                PropertyChanged(this.LastName, new PropertyChangedEventArgs("LastName"));
                PropertyChanged(this.Mail, new PropertyChangedEventArgs("Mail"));
                PropertyChanged(this.Adress, new PropertyChangedEventArgs("Adress"));
            }
        }

        private string fileName = "zakaznici.json";

        public string FirstNameHelper { get; set; } = "Křestní jméno";
        public string LastNameHelper { get; set; } = "Příjmení";
        public string MailHelper { get; set; } = "Mail";
        public string AdressHelper { get; set; } = "Adresa";

        private string firstName;
        public string FirstName { get { return this.firstName; } set { this.firstName = value; } }

        private string lastName;
        public string LastName { get { return this.lastName; } set { this.lastName = value; } }

        private string mail;
        public string Mail { get { return this.mail; } set { this.mail = value; } }

        private string adress;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Adress { get { return this.adress; } set { this.adress = value; } }

        private void saveCustomer(string obj)
        {
            Customer customer = new Customer() { FirstName = firstName, LastName = lastName, Contact = new Contact() { Mail = mail, Adress = adress } };
            Customers.Add(customer);
            CustomersCollection.Add(customer);
            Debug.WriteLine(Customers);
            PropertyChanged(Customers, new PropertyChangedEventArgs("Customers"));
            saveFile(Customers);
        }
        private void editCustomer(string obj)
        {
            Customer customer = selectedCustomer;

            Customers.Remove(selectedCustomer);
            CustomersCollection.Remove(selectedCustomer);

            customer.FirstName = firstName;
            customer.LastName= lastName;
            customer.Contact.Mail= mail;
            customer.Contact.Adress= adress;

            Customers.Add(customer);
            CustomersCollection.Add(customer);
            PropertyChanged(Customers, new PropertyChangedEventArgs("Customers"));
            saveFile(Customers);

        }
        private void deleteCustomer(string obj)
        {
            Customers.Remove(this.selectedCustomer);
            CustomersCollection.Remove(this.selectedCustomer);
            PropertyChanged(Customers, new PropertyChangedEventArgs("Customers"));
            saveFile(Customers);
        }
        public MainWindowContext()
        {
            SaveCustomerCommand = new RelayCommand<String>(saveCustomer);
            DeleteCustomerCommand = new RelayCommand<String>(deleteCustomer);
            EditCustomerCommand = new RelayCommand<String>(editCustomer);
            Customers = loadFile(fileName);

            CustomersCollection = new ObservableCollection<Customer>(Customers);
        }
        private void saveFile(List<Customer> customers)
        {
            string json = JsonConvert.SerializeObject(customers);
            System.IO.File.WriteAllText(fileName, json);
        }
        private List<Customer> loadFile(string jsonName)
        {
            string text = System.IO.File.ReadAllText(jsonName);
            return JsonConvert.DeserializeObject<List<Customer>>(text);
        }
    }
}
