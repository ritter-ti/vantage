using FluentAssertions;
using Goal.Seedwork.Infra.Crosscutting.Trying;
using Xunit;

namespace Goal.Seedwork.Infra.Crosscutting.Tests.Trying
{
    public class Option_Map
    {
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(4, 5, 9)]
        public void WithSome_ReturnsSome(int a, int b, int expected)
        {
            var option = Option.Of(a);
            var result = option.Map(x => x + b);
            result.IsSome.Should().BeTrue();
            result.Value.Should().Be(expected);
        }

        [Fact]
        public void WithNone_ReturnsNone()
        {
            var option = Option.Of<string>(null);
            var result = option.Map(x => x.ToUpper());
            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public void WithSomeValue_ShouldReturnOptionWithMappedValue()
        {
            // arrange
            int initial = 42;
            Option<int> option = Helpers.Some(initial);

            // act
            Option<string> result = option.Map(i => i.ToString());

            // assert
            result.IsSome.Should().BeTrue();
            result.Value.Should().Be(initial.ToString());
        }

        [Fact]
        public void WithNoneValue_ShouldReturnEmptyOption()
        {
            // arrange
            Option<int> option = Helpers.None;

            // act
            Option<string> result = option.Map(i => i.ToString());

            // assert
            result.IsNone.Should().BeTrue();
        }
    }

    public class Option_ImplicitOperator
    {
        [Fact]
        public void FromValue_ReturnsSome()
        {
            // arrange
            int value = 10;

            // act
            Option<int> option = value;

            // assert
            option.IsSome.Should().BeTrue();
            option.Value.Should().Be(value);
        }

        [Fact]
        public void FromNoneType_ReturnsNone()
        {
            // act
            Option<int> option = Helpers.None;

            // assert
            option.IsNone.Should().BeTrue();
        }
    }
}