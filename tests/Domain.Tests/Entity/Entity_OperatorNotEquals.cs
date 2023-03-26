using FluentAssertions;
using Xunit;

namespace Goal.Seedwork.Domain.Tests.Entity
{
    public class Entity_OperatorNotEquals
    {
        private class TestEntity : Domain.Aggregates.Entity<int>
        {
            public TestEntity(int id)
            {
                Id = id;
            }
        }

        [Fact]
        public void OperatorNotEquals_SameObjects_ReturnsFalse()
        {
            // Arrange
            var entity = new TestEntity(1);

            // Act
#pragma warning disable CS1718 // Comparison made to same variable
            var result = entity != entity;
#pragma warning restore CS1718 // Comparison made to same variable

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void OperatorNotEquals_DifferentObjects_ReturnsTrue()
        {
            // Arrange
            var entity1 = new TestEntity(1);
            var entity2 = new TestEntity(2);

            // Act
            var result = entity1 != entity2;

            // Assert
            result.Should().BeTrue();
        }
    }
}
