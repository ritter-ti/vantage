using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Infra.Data.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Goal.Seedwork.Infra.Data.Tests.Extensions
{
    public static class MoqExtensions
    {
        public static Mock<IQueryable<TEntity>> BuildMock<TEntity>(this IQueryable<TEntity> data) where TEntity : class
        {
            var mock = new Mock<IQueryable<TEntity>>();
            var enumerable = new TestAsyncEnumerable<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.ConfigureQueryableCalls(enumerable, data);
            return mock;
        }

        public static Mock<DbSet<TEntity>> BuildMockDbSet<TEntity>(this IQueryable<TEntity> data) where TEntity : class, IEntity
        {
            var mock = new Mock<DbSet<TEntity>>();
            var enumerable = new TestAsyncEnumerable<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.As<IQueryable<TEntity>>().ConfigureQueryableCalls(enumerable, data);
            mock.ConfigureDbSetCalls();

            mock.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(key => data.FirstOrDefault(d => d.Id == Guid.Parse(key[0].ToString())));

            mock.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(async key =>
                {
                    return await Task.FromResult(data.FirstOrDefault(d => d.Id == Guid.Parse(key[0].ToString())));
                });

            return mock;
        }

        public static Mock<DbSet<TEntity>> BuildMockDbSet<TEntity, TKey>(this IQueryable<TEntity> data)
            where TEntity : class, IEntity<TKey>
        {
            var mock = new Mock<DbSet<TEntity>>();
            var enumerable = new TestAsyncEnumerable<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.As<IQueryable<TEntity>>().ConfigureQueryableCalls(enumerable, data);
            mock.ConfigureDbSetCalls();

            mock.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(key => data.FirstOrDefault(d => d.Id.Equals(ParseKey<TKey>(key[0]))));

            mock.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(async key =>
                {
                    return await Task.FromResult(data.FirstOrDefault(d => d.Id.Equals(ParseKey<TKey>(key[0]))));
                });

            return mock;
        }

        private static TKey ParseKey<TKey>(object key) => (TKey)Convert.ChangeType(key, typeof(TKey));

        private static void ConfigureDbSetCalls<TEntity>(this Mock<DbSet<TEntity>> mock) where TEntity : class
            => mock.Setup(m => m.AsAsyncEnumerable()).Returns(mock.Object.AsAsyncEnumerable());

        private static void ConfigureQueryableCalls<TEntity>(
            this Mock<IQueryable<TEntity>> mock,
            IQueryProvider queryProvider,
            IQueryable<TEntity> data) where TEntity : class
        {
            mock.Setup(m => m.Provider).Returns(queryProvider);
            mock.Setup(m => m.Expression).Returns(data?.Expression);
            mock.Setup(m => m.ElementType).Returns(data?.ElementType);
            mock.Setup(m => m.GetEnumerator()).Returns(() => data?.GetEnumerator());
        }

        private static void ConfigureAsyncEnumerableCalls<TEntity>(
            this Mock<IAsyncEnumerable<TEntity>> mock,
            IAsyncEnumerable<TEntity> enumerable)
        {
            mock.Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(() => enumerable.GetAsyncEnumerator());
        }
    }
}
