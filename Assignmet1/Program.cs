using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assigment1
{
    class Customer
    {
        // Variables declared
        private string _FirstName, _LastName, _Address;
        private DateTime _DateofBirth;
        public string ContactNumber, Email;
        // Encapsulation applied
        public string FirstName
        {
            get => _FirstName;
            set => _FirstName = value;
        }
        
        public string LastName
        {
            get => _LastName;
            set => _LastName = value;
        }
        
        public string Address
        {
            get => _Address;
            set => _Address = value;
        }

        public DateTime DateofBirth
        {
            get => _DateofBirth;
            set => _DateofBirth = value;
        }
        List<Account> _ListOfAccounts= new List<Account>();

        public List<Account> ListOfAccounts => _ListOfAccounts;
        // Constructor 1
        public Customer(string first_name, string last_name, string address, string date_of_birth,
            string contact_number, string email)
        {
            DateofBirth = DateTime.ParseExact(date_of_birth, "dd/MM/yyyy", null);
            if (DateTime.Now.Year - DateofBirth.Year > 16)
            {
                FirstName = first_name;
                LastName = last_name;
                Address = address;
                // Validation Checks for contact number length
                if (contact_number.Length == 10)
                    ContactNumber = contact_number;
                else
                    ContactNumber = "";
                Email = email;
            }
            else
                Console.WriteLine("Invalid Age.Please re run the program");
        }
        // Constructor 2
        public Customer(string first_name, string last_name, string address, string date_of_birth)
        {
            _FirstName = first_name;
            _LastName = last_name;
            _Address = address;
            _DateofBirth = DateTime.ParseExact(date_of_birth, "dd/MM/yyyy", null);
            ContactNumber = "";
            Email = "";
        }
        // Constructor 3 => Copy Constructor
        public Customer(Customer customer)
        {
            _FirstName = customer.FirstName;
            _LastName = customer.LastName;
            _Address = customer.Address;
            _DateofBirth = customer.DateofBirth;
            ContactNumber = customer.ContactNumber;
            Email = customer.Email;
        }
        // Function for addition of account to a list of accounts
        public void AddAccount(Account account)
        {
            ListOfAccounts.Add(account);
        }
        // This function calculates the sum of balance of all the accounts    
        public double SumBalance()
        {
            double sum = 0;
            foreach (Account account in ListOfAccounts)
            {
                sum += account.Balance;
            }
            return sum;
        }
        // This method is used to override the ToString function to generate appropriate formatted output
        public override string ToString()
        {
            return "Name: " + FirstName + " " + LastName + "\t Address: " + Address + "\t DOB:" + DateofBirth.ToString("dd/MM/yyyy", null) + "\t Contact: " + ContactNumber + " "  + "\t Email: " + Email + " " + "\t Total balance: " + SumBalance().ToString("F1");
        }
    }

    abstract class Account
    {
        // Variables declared
        private static uint id = 1;
        private uint _ID = id++;
        public uint ID => _ID;  
        private DateTime _OpenedDate, _ClosedDate;
        private bool _Active;
        private double _Balance = 0;
        private Customer _Owner;
        private static string CurrentDateString = DateTime.Now.ToString("dd/MM/yyyy", null);
        protected DateTime CurrentDate = DateTime.ParseExact(CurrentDateString, "dd/MM/yyyy", null);
        // Encapsulation applied
        protected DateTime OpenedDate => _OpenedDate;
        protected DateTime ClosedDate => _ClosedDate;
        public bool Active => _Active;
        public double Balance
        {
            get => _Balance;
            set => _Balance = value;
        }
        public Customer Owner => _Owner;
        // Constructor 1
        public Account(Customer owner, string opened_date, double balance)
        {
            _OpenedDate = DateTime.ParseExact(opened_date, "dd/MM/yyyy", null);
            if (balance <= 0) 
                balance = 0;
            else 
                _Balance = balance;
            int result = DateTime.Compare(CurrentDate, _OpenedDate);
            if (result >= 0)
                _OpenedDate = DateTime.ParseExact(opened_date, "dd/MM/yyyy", null);
            else 
                _OpenedDate = CurrentDate;
            _Active = true;
            _Owner = owner;
            _Owner.AddAccount(this);
        }
        // Constructor 2 : this constructor again calls constructor 1
        public Account(Customer owner, double balance) : this(owner, DateTime.Now.ToString("dd/MM/yyyy", null), balance)
        {
        }    
        // Function to close a given account
        public void Close()
        {
            string CurrentDateString = DateTime.Now.ToString("dd/MM/yyyy", null);
            DateTime CurrentDate = DateTime.ParseExact(CurrentDateString, "dd/MM/yyyy", null);
            if (_Active == true)
            {
                _ClosedDate = CurrentDate;
                _Active = false;
            }
        }
        // Function to transfer funds in between accounts
        // This function is overriden in derived class
        public virtual void Transfer(Account account, double amount)
        {
            if (_Active && account.Active && _Balance > amount)
            {
                account._Balance += amount;
                _Balance -= amount;
            }
        }
        // Function to calculate interest 
        // This function is overriden in derived class
        public virtual double CalculateInterest()
        {
            double interest = 0;
            return interest;
        }
        // Function to update balance of an account
        // This function is overriden in derived class
        public virtual void UpdateBalance()
        {
            double interest = CalculateInterest();
            _Balance += interest;
        }
       // This method is used to override the ToString function to generate appropriate formatted output
       public override string ToString()
        {
           if (Active)
               return "ID: " + ID + "\t Opened Date: " + OpenedDate.ToString("dd/MM/yyyy", null) + "\t Balance: " + Balance.ToString("F1") + "\t Owner: " + Owner.FirstName + " " + Owner.LastName;            
            return "ID: " + ID + "\t Opened Date: " + OpenedDate.ToString("dd/MM/yyyy", null) + "\t Balance: " + Balance.ToString("F1") + "\t Owner: " + Owner.FirstName + " " + Owner.LastName + " " + "\t - Closed on " + ClosedDate.ToString("dd/MM/yyyy", null);                
        }
    }

    class Type1Account : Account
    {
        // Variables declared
        private static double _AnnualRateOfInterest = 2.0;
        // Variables encapsulated
        public static double AnnualRateOfInterest => _AnnualRateOfInterest;
        // Constructor 1 for this class => this calls the constructor 1 form base class 
        public Type1Account(Customer owner, string opened_date, double balance) : base(owner, opened_date, balance)
        {
        }
        // Constructor 2 for this class => this calls the constructor 2 form base class
        public Type1Account(Customer owner, double balance): base(owner, balance)
        {
        }
        // Function to deposit a particular amount of money
        public void Deposit(double amount)
        {
            if (Active && amount > 0)
            {
                Balance += amount;
            }
        }
        // Function to withdraw from account 
        public void Withdraw(double amount)
        {
            if (Active && amount <= Balance)
            {
                Balance -= amount;
            }   
        }
        // Function to transfer funds in between two accounts
        // This function overrides the function defined in base class account
        public override void Transfer(Account account, double amount)
        {
            if (amount > 0 && amount <= Balance && Active && account.Active)
            {
                Balance -= amount;
                account.Balance += amount;
            }
        }
        // Function to calculate interest
        // This function overrides the function defined in base class account
        public override double CalculateInterest()
        {
            int nDays;
            if (OpenedDate.Month != CurrentDate.Month)
                nDays = CurrentDate.Day - 1;
            else
                nDays = CurrentDate.Day - OpenedDate.Day;
            double interest = (AnnualRateOfInterest/365/100) * nDays * Balance;
            return interest;
        }
    }

    class Type2Account : Account
    {
        // Variables defined
        private double _MonthlyDeposit;
        private double _AnnualRateOfInterest = 3.0;
        private static double _DepositRate = 4.0;
        // Encapsulation applied
        public double MonthlyDeposit
        {
            get => _MonthlyDeposit;
            set => _MonthlyDeposit = value;
        }
        public double AnnualRateOfInterest => _AnnualRateOfInterest;
        public double DepositRate => _DepositRate;
        // Constructor 1 for this class => this calls the constructor 1 form base class 
        public Type2Account(Customer owner, string opened_date, double balance) : base(owner, opened_date, balance)
        {
        }
        // Constructor 2 for this class => this calls the constructor 2 form base class 
        public Type2Account(Customer owner, double balance) : base(owner, balance)
        {
        }
        // Function to transfer funds in between two accounts
        // This function overrides the function defined in base class account
        public override void Transfer (Account account, double ammount) 
        {
            if (Active && account.Active && Balance >= ammount && account.GetType() == typeof(Type1Account) && ammount > 0 && Owner == account.Owner) 
            {
                account.Balance += ammount;
                Balance -= ammount;
            }
            else 
                Console.WriteLine();
        }
        // Function to calculate interest
        // This function overrides the function defined in base class account
        public override double CalculateInterest() 
        {
            int nDays; 
            if (OpenedDate.Month != CurrentDate.Month) 
                nDays = CurrentDate.Day - 1;
            else
                nDays = CurrentDate.Day - OpenedDate.Day;          
            double interest = (AnnualRateOfInterest/365/100) * Balance * nDays + (_DepositRate/365/100 * nDays * MonthlyDeposit);
            return interest;
        }        
        // Function to update balance
        // This function overrides the function defined in base class account
        public override void UpdateBalance() 
        {
            base.UpdateBalance();
            MonthlyDeposit = 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Customer Object Declaration
            Customer c1 = new Customer("Arley", "Praise", "12 Hay Rd", "02/10/1990", "0412232116", "arlyp@gmail.com");
            Customer c2 = new Customer("Joseph", "Abot", "4/1 Mandy Pl", "11/05/1970", "0413221624", "");
            Customer c3  = new Customer("Rose", "Magaret", "30 Buxton St", "06/07/1980", "", "rmt@yahoo.com");
            // Account Object Declaration
            Type1Account a1 = new Type1Account(c1, "01/02/2018", 100);
            Type2Account a2 = new Type2Account(c1, "15/02/2018", 5000);
            Type1Account a3 = new Type1Account(c2, "20/03/2018", 0);
            Type2Account a4 = new Type2Account(c3, "04/03/2018", 2000);
            Type2Account a5 = new Type2Account(c3, 3000);
            // Deposit and Transfer operations performed
            a1.Deposit(-200);
            a2.Transfer(a1, 6000);
            a2.Transfer(a1, 2000);
            a1.Transfer(a2, 1000);
            a2.MonthlyDeposit = 200;
            a3.Deposit(5000);
            a3.Withdraw(6000);
            a3.Transfer(a1, 1000);
            a3.Transfer(a2, 500);
            a4.MonthlyDeposit = 1000;
            a4.Transfer(a1, 100);
            a5.MonthlyDeposit = 1500;
            a5.Transfer(a4, 500);
            // Calculated Interest Displayed
            Console.WriteLine("Calculate Interest\n");
            Console.WriteLine("Interest of a{0}: {1:F1}", a1.ID, a1.CalculateInterest());
            Console.WriteLine("Interest of a{0}: {1:F1}", a2.ID, a2.CalculateInterest());
            Console.WriteLine("Interest of a{0}: {1:F1}", a3.ID, a3.CalculateInterest());
            Console.WriteLine("Interest of a{0}: {1:F1}", a4.ID, a4.CalculateInterest());
            Console.WriteLine("Interest of a{0}: {1:F1}", a5.ID, a5.CalculateInterest());
            Console.WriteLine("\nUpdating Balance\n");
            // Updating balance of accounts
            a1.UpdateBalance();
            a2.UpdateBalance();
            a3.UpdateBalance();
            a4.UpdateBalance();
            a5.UpdateBalance();
            // Closing acconts
            a3.Close();
            a5.Close();
            // Displaying Account's information
            Console.WriteLine("\nAccount's Information\n");
            Console.WriteLine(a1);
            Console.WriteLine(a2);
            Console.WriteLine(a3);
            Console.WriteLine(a4);
            Console.WriteLine(a5);
            // Displaying Customer's information
            Console.WriteLine("\nCustomers' information");   
            Console.WriteLine(c1);
            Console.WriteLine(c2);
            Console.WriteLine(c3);            
        }
    }
}