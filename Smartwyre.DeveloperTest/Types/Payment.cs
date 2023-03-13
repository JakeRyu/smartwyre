namespace Smartwyre.DeveloperTest.Types;

public abstract class Payment
{
    protected Account Account { get; }

    protected Payment(Account account)
    {
        Account = account;
    }

    public abstract bool ValidateRequest(MakePaymentRequest request);
}

public class BankToBankTransferPayment : Payment
{
    public BankToBankTransferPayment(Account account) : base(account)
    {
    }

    public override bool ValidateRequest(MakePaymentRequest request)
    {
        return Account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.BankToBankTransfer);
    }
}

public class ExpeditedPayment : Payment
{
    public ExpeditedPayment(Account account) : base(account)
    {
    }

    public override bool ValidateRequest(MakePaymentRequest request)
    {
        if (!Account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.ExpeditedPayments))
            return false;

        return Account.Balance >= request.Amount;
    }
}

public class AutomatedPayment : Payment
{
    public AutomatedPayment(Account account) : base(account)
    {
    }

    public override bool ValidateRequest(MakePaymentRequest request)
    {
        if (!Account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem))
            return false;

        return Account.Status == AccountStatus.Live;
    }
}