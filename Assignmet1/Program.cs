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
        private string _FirstName, _LastName, _Address;
        private DateTime _DateofBirth;
        public string ContactNumber, Email;
        
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

        public Customer(string first_name, string last_name, string address, string date_of_birth,
            string contact_number, string email)
        {
            FirstName = first_name;
            LastName= last_name;
            Address = address;
            DateofBirth = DateTime.ParseExact(date_of_birth, "dd/MM/yyyy", null);
            ContactNumber = contact_number;
            Email = email;
        }

        public Customer(string first_name, string last_name, string address, string date_of_birth)
        {
            _FirstName = first_name;
            _LastName = last_name;
            _Address = address;
            _DateofBirth = DateTime.ParseExact(date_of_birth, "dd/MM/yyyy", null);
            ContactNumber = "";
            Email = "";
        }
        
        public Customer(Customer customer)
        {
            _FirstName = customer.FirstName;
            _LastName = customer.LastName;
            _Address = customer.Address;
            _DateofBirth = customer.DateofBirth;
            ContactNumber = customer.ContactNumber;
            Email = customer.Email;
        }

        public void AddAccount(Account account)
        {
            ListOfAccounts.Add(account);
        }

        public double SumBalance()
        {
            double sum = 0;
            foreach (Account account in ListOfAccounts)
            {
                sum += account.Balance;
            }
            return sum;
        }

        public override string ToString()
        {
            return "Name: " + FirstName + " " + LastName + "\t Address: " + Address + "\t DOB:" + DateofBirth.ToString("dd/MM/yyyy", null) + "\t Contact: " + ContactNumber + " "  + "\t Email: " + Email + " " + "\t Total balance: " + SumBalance().ToString("F1");
        }
    }

    abstract class Account
    {
        private static uint id = 1;
        private uint _ID = id++;
        public uint ID => _ID;  
        private DateTime _OpenedDate, _ClosedDate;
        private bool _Active;
        private double _Balance = 0;
        private Customer _Owner;
        private static string CurrentDateString = DateTime.Now.ToString("dd/MM/yyyy", null);
        protected DateTime CurrentDate = DateTime.ParseExact(CurrentDateString, "dd/MM/yyyy", null);
        protected DateTime OpenedDate => _OpenedDate;
        protected DateTime ClosedDate => _ClosedDate;
        public bool Active => _Active;
        public double Balance
        {
            get => _Balance;
            set => _Balance = value;
        }
        public Customer Owner => _Owner;

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
        public Account(Customer owner, double balance) : this(owner, DateTime.Now.ToString("dd/MM/yyyy", null), balance)
        {
        }

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
        public virtual void Transfer(Account account, double amount)
        {
            if (_Active && account.Active && _Balance > amount)
            {
                account._Balance += amount;
                _Balance -= amount;
            }
        }

        public virtual double CalculateInterest()
        {
            double interest = 0;
            return interest;
        }

        public virtual void UpdateBalance()
        {
            double interest = CalculateInterest();
            _Balance += interest;
        }

       public override string ToString()
        {
           if (Active)
               return "ID: " + ID + "\t Opened Date: " + OpenedDate.ToString("dd/MM/yyyy", null) + "\t Balance: " + Balance.ToString("F1") + "\t Owner: " + Owner.FirstName + " " + Owner.LastName;            
            return "ID: " + ID + "\t Opened Date: " + OpenedDate.ToString("dd/MM/yyyy", null) + "\t Balance: " + Balance.ToString("F1") + "\t Owner: " + Owner.FirstName + " " + Owner.LastName + " " + "\t - Closed on " + ClosedDate.ToString("dd/MM/yyyy", null);                
        }
    }

    class Type1Account : Account
    {
        private static double _AnnualRateOfInterest = 2.0;
        public static double AnnualRateOfInterest => _AnnualRateOfInterest;

        public Type1Account(Customer owner, string opened_date, double balance) : base(owner, opened_date, balance)
        {
        }
        public Type1Account(Customer owner, double balance): base(owner, balance)
        {
        }
        public void Deposit(double amount)
        {
            if (Active && amount > 0)
            {
                Balance += amount;
            }
        }
        public void Withdraw(double amount)
        {
            if (Active && amount <= Balance)
            {
                Balance -= amount;
            }   
        }

        public override void Transfer(Account account, double amount)
        {
            if (amount > 0 && amount <= Balance && Active && account.Active)
            {
                Balance -= amount;
                account.Balance += amount;
            }
        }

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
        private double _MonthlyDeposit;
        private double _AnnualRateOfInterest = 3.0;
        private static double _DepositRate = 4.0;
        public double MonthlyDeposit
        {
            get => _MonthlyDeposit;
            set => _MonthlyDeposit = value;
        }
        public double AnnualRateOfInterest => _AnnualRateOfInterest;
        public double DepositRate => _DepositRate;
        public Type2Account (Customer owner, string opened_date, double balance) : base (owner, opened_date, balance) {}
        public Type2Account (Customer owner, double balance) : base(owner, balance) {}
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
            //
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
            a1.UpdateBalance();
            a2.UpdateBalance();
            a3.UpdateBalance();
            a4.UpdateBalance();
            a5.UpdateBalance();
            a3.Close();
            a5.Close();
            Console.WriteLine("\nAccount's Information\n");
            Console.WriteLine(a1);
            Console.WriteLine(a2);
            Console.WriteLine(a3);
            Console.WriteLine(a4);
            Console.WriteLine(a5);
            Console.WriteLine("\nCustomers' information");   
            Console.WriteLine(c1);
            Console.WriteLine(c2);
            Console.WriteLine(c3);            
        }
    }
}
