using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Vantage.Domain;
using Vantage.Infra.Crosscutting.Specifications;
using Vantage.Infra.Data.Seedwork.Tests.Extensions;
using Vantage.Infra.Data.Seedwork.Tests.Mocks;
using Xunit;

namespace Vantage.Infra.Data.Seedwork.Tests.Repositories
{
    public class Repository_Any
    {
        [Fact]
        public void ReturnsTrueGivenAnyEntity()
        {
            List<Test> mockedTests = MockTests();

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.Any();

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeTrue();
        }

        [Fact]
        public void ReturnsTrueGivenAnyEntityAsync()
        {
            List<Test> mockedTests = MockTests();

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.AnyAsync().GetAwaiter().GetResult();

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseGivenNoneEntity()
        {
            List<Test> mockedTests = MockTests(0);

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.Any();

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseGivenNoneEntityAsync()
        {
            List<Test> mockedTests = MockTests(0);

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.AnyAsync().GetAwaiter().GetResult();

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTrueGivenAnyActiveEntity()
        {
            List<Test> mockedTests = MockTests();
            mockedTests.First().Deactivate();

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISpecification<Test> spec = new DirectSpecification<Test>(t => t.Active);
            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.Any(spec);

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeTrue();
        }

        [Fact]
        public void ReturnsTrueGivenAnyActiveEntityAsync()
        {
            List<Test> mockedTests = MockTests();
            mockedTests.First().Deactivate();

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISpecification<Test> spec = new DirectSpecification<Test>(t => t.Active);
            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.AnyAsync(spec).GetAwaiter().GetResult();

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseGivenNoneActiveEntity()
        {
            List<Test> mockedTests = MockTests(1);
            mockedTests.First().Deactivate();

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISpecification<Test> spec = new DirectSpecification<Test>(t => t.Active);
            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.Any(spec);

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseGivenNoneActiveEntityAsync()
        {
            List<Test> mockedTests = MockTests(1);
            mockedTests.First().Deactivate();

            Mock<DbSet<Test>> mockDbSet = mockedTests.AsQueryable().BuildMockDbSet();

            var mockUnitOfWork = new Mock<IEFUnitOfWork>();
            mockUnitOfWork.Setup(p => p.Set<Test>()).Returns(mockDbSet.Object);

            ISpecification<Test> spec = new DirectSpecification<Test>(t => t.Active);
            ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
            bool any = testRepository.AnyAsync(spec).GetAwaiter().GetResult();

            mockUnitOfWork.Verify(x => x.Set<Test>(), Times.Once);
            any.Should().BeFalse();
        }

        [Fact]
        public void ThrowsArgumentNullExceptionGivenNullSpecification()
        {
            var mockUnitOfWork = new Mock<IEFUnitOfWork>();

            Action act = () =>
            {
                ISpecification<Test> spec = null;
                ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
                testRepository.Any(spec);
            };

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("specification");
        }

        [Fact]
        public void ThrowsArgumentNullExceptionGivenNullSpecificationAsync()
        {
            var mockUnitOfWork = new Mock<IEFUnitOfWork>();

            Action act = () =>
            {
                ISpecification<Test> spec = null;
                ISqlRepository<Test> testRepository = new GenericTestRepository(mockUnitOfWork.Object);
                testRepository.AnyAsync(spec).GetAwaiter().GetResult();
            };

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("specification");
        }

        private static List<Test> MockTests(int count)
        {
            var tests = new List<Test>();

            for (int i = 1; i <= count; i++)
            {
                tests.Add(new Test(i));
            }

            return tests;
        }

        private static List<Test> MockTests() => MockTests(5);
    }
}
