using Moq;
using Shouldly;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        [Fact]
        public void MakePayment_WhenSucceed_ShouldReturnSuccessResult()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var mockStore = new Mock<IAccountDataStore>();
            var sut = new PaymentService(mockStore.Object);

            mockStore.Setup(x => x.GetAccount(It.IsAny<string>()))
                .Returns(new Account
                {
                    AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments
                });

            // Act
            var result = sut.MakePayment(request);

            // Assert
            result.Success.ShouldBeTrue();
        }
        
        [Fact]
        public void MakePayment_WhenPassingValidation_ShouldDeductPaymentAmountAndUpdateAccount()
        {
            // Arrange
            var request = new MakePaymentRequest{ Amount = 200 };
            var mockStore = new Mock<IAccountDataStore>();
            var sut = new PaymentService(mockStore.Object);

            var account = new Account
            {
                Balance = 1_000,
                AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments
            };
            mockStore.Setup(x => x.GetAccount(It.IsAny<string>()))
                .Returns(account);

            // Act
            var result = sut.MakePayment(request);

            // Assert
            result.Success.ShouldBeTrue();
            account.Balance.ShouldBe(1_000 - 200);
            mockStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }
    }
}
