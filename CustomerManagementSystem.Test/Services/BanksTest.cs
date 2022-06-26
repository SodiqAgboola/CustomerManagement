using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.Test.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CustomerManagementSystem.Test.Services
{
    
   public class BanksTest
    {
        [Fact]
        public async Task FetchExistingBanks()
        {
            //arrange
            using ServicesDISetup _setup = new();
            var paymentService = _setup.ServiceProvider.GetService<IBankService>();
            var context = _setup.DbContext;

            //act
            var records = await paymentService.GetExistingBanks();

            var authorsCount = records.Data.result.Count();
           
            //assert
            Assert.False(records.Data.hasError);
            Assert.Equal(0, authorsCount);
        }
    }
}
