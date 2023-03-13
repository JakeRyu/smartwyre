using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System.Configuration;
using System.Security;

namespace Smartwyre.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            if (account == null)
            {
                return new MakePaymentResult { Success = false };
            }
            
            var result = new MakePaymentResult{ Success = true };

            switch (request.PaymentScheme)
            {
                case PaymentScheme.BankToBankTransfer:
                    var bankTransferPayment = new BankToBankTransferPayment(account);
                    if (!bankTransferPayment.ValidateRequest(request))
                    {
                        result.Success = false;
                    }
                   
                    break;

                case PaymentScheme.ExpeditedPayments:
                    var expeditedPayment = new ExpeditedPayment(account);
                    if (!expeditedPayment.ValidateRequest(request))
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.AutomatedPaymentSystem:
                    var autoPayment = new AutomatedPayment(account);
                    if (!autoPayment.ValidateRequest(request))
                    {
                        result.Success = false;
                    }
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                var accountDataStoreUpdateData = new AccountDataStore();
                accountDataStoreUpdateData.UpdateAccount(account);
            }

            return result;
        }
    }
}
