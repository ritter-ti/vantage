using System;
using FluentAssertions;
using Moq;
using Vantage.Domain;
using Vantage.Infra.Data.Seedwork.Tests.Mocks;
using Xunit;

namespace Vantage.Infra.Data.Seedwork.Tests.Repositories
{
    public class Repository_Constructor
    {
        [Fact]
        public void NotThrowsAnyExceptionGivenSimpleRepository()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            IRepository testRepository = new TestRepository(mockUnitOfWork.Object);

            testRepository.UnitOfWork.Should().NotBeNull().And.Be(mockUnitOfWork.Object);
        }

        [Fact]
        public void NotThrowsAnyExceptionGivenGenericRepository()
        {
            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);

            testRepository.UnitOfWork.Should().NotBeNull().And.Be(mockUnitOfWork.Object);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionGivenSimpleRepository()
        {
            Action act = () => { _ = new TestRepository(null); };
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("unitOfWork");
        }

        [Fact]
        public void ThrowsArgumentNullExceptionGivenGenericRepository()
        {
            Action act = () => { _ = new GenericTestRepository(null); };
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("unitOfWork");
        }
    }
}