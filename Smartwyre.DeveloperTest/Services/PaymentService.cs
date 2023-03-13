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
                return CreateFailResult();
            
            var payment = PaymentFactory.Create(request.PaymentScheme, account);
            if (!payment.ValidateRequest(request))
                return CreateFailResult();

            account.Balance -= request.Amount;
            _accountDataStore.UpdateAccount(account);

            return new MakePaymentResult{ Success = true };
        }

        private static MakePaymentResult CreateFailResult()
        {
            return new MakePaymentResult { Success = false };
        }
    }
}
