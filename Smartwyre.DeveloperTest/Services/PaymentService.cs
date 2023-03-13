using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

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

            var payment = PaymentFactory.Create(request.PaymentScheme, account);
            if (!payment.ValidateRequest(request))
            {
                result.Success = false;
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
