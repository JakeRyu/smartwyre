using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = new Account
            {
                AccountNumber = "1234567", 
                AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments,
                Balance = 1_000, 
                Status = AccountStatus.Live
            };
            Console.WriteLine($"Current balance of of account ({account.AccountNumber}): {account.Balance}");
            
            var service = new PaymentService(new AccountDataStore());

            var request = new MakePaymentRequest { Amount = 200 };
            var result = service.MakePayment(request);
            
            // AccountDataStore is not implemented hence GetAccount() returns null.
            // Null account is the first condition to check in MakePayment(), so this operation fails.
            if (result.Success)
            {
                Console.WriteLine("Payment made successfully");
                Console.WriteLine($"New balance: {account.Balance}");
            }
            else
            {
                Console.WriteLine("Payment was not processed");
            }

        }
    }
}
