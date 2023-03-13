using Shouldly;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentTests
{
    [Theory]
    [InlineData(AllowedPaymentSchemes.BankToBankTransfer, true)]
    [InlineData(AllowedPaymentSchemes.ExpeditedPayments, false)]
    [InlineData(AllowedPaymentSchemes.AutomatedPaymentSystem, false)]
    public void BankToBankTransferPayment_OnlyBankToBankScheme_ShouldBeAllowed(AllowedPaymentSchemes allowedScheme,
        bool expected)
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = allowedScheme
        };
        var request = new MakePaymentRequest();
        var sut = new BankToBankTransferPayment(account);

        // Act
        var result = sut.ValidateRequest(request);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.BankToBankTransfer, false)]
    [InlineData(AllowedPaymentSchemes.ExpeditedPayments, true)]
    [InlineData(AllowedPaymentSchemes.AutomatedPaymentSystem, false)]
    public void ExpeditedPayment_OnlyExpeditedPaymentScheme_ShouldBeAllowed(AllowedPaymentSchemes allowedScheme,
        bool expected)
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = allowedScheme
        };
        var request = new MakePaymentRequest();
        var sut = new ExpeditedPayment(account);

        // Act
        var result = sut.ValidateRequest(request);

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void ExpeditedPayment_WhenPaymentAmountExceedsAccountBalance_ShouldReturnFalse()
    {
        // Arrange
        var account = new Account
        {
            Balance = 1_000,
            AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments
        };
        var request = new MakePaymentRequest { Amount = 2_000 };
        var sut = new ExpeditedPayment(account);

        // Act
        var result = sut.ValidateRequest(request);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.BankToBankTransfer, false)]
    [InlineData(AllowedPaymentSchemes.ExpeditedPayments, false)]
    [InlineData(AllowedPaymentSchemes.AutomatedPaymentSystem, true)]
    public void AutomatedPayment_OnlyAutomatedPaymentSystemScheme_ShouldBeAllowed(AllowedPaymentSchemes allowedScheme,
        bool expected)
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = allowedScheme
        };
        var request = new MakePaymentRequest();
        var sut = new AutomatedPayment(account);

        // Act
        var result = sut.ValidateRequest(request);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(AccountStatus.Live, true)]
    [InlineData(AccountStatus.Disabled, false)]
    [InlineData(AccountStatus.InboundPaymentsOnly, false)]
    public void AutomatedPayment_WhenAccountStatusIsLive_ShouldReturnTrue(AccountStatus accountStatus, bool expected)
    {
        // Arrange
        var account = new Account
        {
            Status = accountStatus,
            AllowedPaymentSchemes = AllowedPaymentSchemes.AutomatedPaymentSystem
        };
        var request = new MakePaymentRequest();
        var sut = new AutomatedPayment(account);

        // Act
        var result = sut.ValidateRequest(request);

        // Assert
        result.ShouldBe(expected);
    }
}