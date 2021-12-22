using System;
using System.Transactions;
using FluentAssertions;
using Vantage.Application.Services;
using Xunit;

namespace Vantage.Application.Seedwork.Tests.Services
{
    public class AppService_CreateTransactionScope
    {
        [Fact]
        public void CreateTransactionScopeSuccessfully()
        {
            Func<bool> func = () =>
            {
                var appService = new TestAppService();
                return appService.Test();
            };

            func.Should().NotThrow();
            func.Invoke().Should().BeTrue();
        }

        public class TestAppService : AppService
        {
            public TestAppService() : base()
            {
            }

            public virtual bool Test()
            {
                using TransactionScope transaction = CreateTransactionScope();

                transaction.Complete();

                return true;
            }
        }
    }
}
