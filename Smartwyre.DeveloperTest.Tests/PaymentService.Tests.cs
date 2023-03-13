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
    }
}
