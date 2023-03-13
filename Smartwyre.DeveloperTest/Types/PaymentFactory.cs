namespace Smartwyre.DeveloperTest.Types;

public static class PaymentFactory
{
    public static Payment Create(PaymentScheme scheme, Account account)
    {
        return scheme switch
        {
            PaymentScheme.BankToBankTransfer => new BankToBankTransferPayment(account),
            PaymentScheme.ExpeditedPayments => new ExpeditedPayment(account),
            PaymentScheme.AutomatedPaymentSystem => new AutomatedPayment(account),
            _ => new ExpeditedPayment(account) // TBD
        };
    }
}