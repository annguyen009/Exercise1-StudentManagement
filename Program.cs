using System;
using System.Globalization;

namespace UserAccountApp
{
    public class UserAccount
    {
        // 1. Private backing fields
        private string _password;
        private decimal _balance;

        // TODO 1: AccountId (Chỉ gán qua Constructor, không cho sửa từ ngoài)
        public string AccountId { get; }

        // TODO 2: Username (Auto-Implemented)
        public string Username { get; set; }

        // TODO 3: Password (Write-Only)
        public string Password
        {
            set
            {
                _password = "[ENCRYPTED]_" + value;
            }
        }

        // TODO 4: Balance (Full Property with Validation)
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Error: Balance cannot be negative!");
                }
                else
                {
                    _balance = value;
                }
            }
        }

        // TODO 5: IsVIP (Computed Read-Only)
        public bool IsVIP
        {
            get
            {
                return _balance >= 10000m;
            }
        }

        // TODO 6: CreatedDate (Get-Only)
        public DateTime CreatedDate { get; }

        public UserAccount(string accountId)
        {
            AccountId = accountId;
            CreatedDate = DateTime.Now;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo culture = new CultureInfo("en-US");

            // --- TEST HARNESS ---

            // 1. Khởi tạo đối tượng
            UserAccount user = new UserAccount("ACC-99201")
            {
                Username = "Alice_Code",
                Password = "SuperSecretPassword123"
            };

            Console.WriteLine("Account ID: " + user.AccountId);
            Console.WriteLine("Username: " + user.Username);
            Console.WriteLine("Account Created: " + user.CreatedDate);

            // 2. Test Full Property Validation
            Console.WriteLine("\n--- Testing Balance Updates ---");
            user.Balance = 5000m;
            Console.WriteLine("Current Balance: " + user.Balance.ToString("C", culture));

            user.Balance = -200m;
            Console.WriteLine("Current Balance after invalid attempt: " + user.Balance.ToString("C", culture));

            // 3. Test Computed Read-Only Property (IsVIP)
            Console.WriteLine("\nIs VIP? " + user.IsVIP);

            user.Balance = 15000m;
            Console.WriteLine("Updated Balance: " + user.Balance.ToString("C", culture));
            Console.WriteLine("Is VIP now? " + user.IsVIP);
        }
    }
}