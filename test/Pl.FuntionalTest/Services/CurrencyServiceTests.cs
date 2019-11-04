using Moq;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Services;
using Xunit;
using Xunit.Abstractions;

namespace Pl.FuntionalTest.Services
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyData> mockCurrencyData;
        private readonly Mock<IAsyncCacheService> mockCacheService;
        private readonly ITestOutputHelper _testOutputHelper;

        public CurrencyServiceTests(ITestOutputHelper testOutputHelper)
        {
            mockCurrencyData = new Mock<ICurrencyData>();
            mockCacheService = new Mock<IAsyncCacheService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestGetCurrencyString()
        {
            Currency defaultCurrency = new Currency()
            {
                Name = "VN Đồng",
                CurrencyCode = "VND",
                CustomFormatting = "0,000 VND",
                Culture = "vi-VN",
                DisplayOrder = 1,
                IsPrimaryExchange = false,
                IsPrimarySystem = true,
                Published = true,
                Rate = 0
            };
            CurrencyService currencyService = new CurrencyService(mockCacheService.Object, mockCurrencyData.Object);
            string stringValue = currencyService.CurrencyToString(300000, defaultCurrency);
            _testOutputHelper.WriteLine(stringValue);
            Assert.True(stringValue == "300,000 VND");
        }
    }
}
