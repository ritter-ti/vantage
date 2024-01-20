using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Goal.Infra.Crosscutting.Extensions;
using Xunit;

namespace Goal.Infra.Crosscutting.Tests.Extensions;

public class Attribute_GetAttributeFromEnumType
{
    [Fact]
    public void GivenEnumWithAttributeWhenReturnCorrectAttribute()
    {
        AttrEnumTest enumValue = AttrEnumTest.Value;
        DisplayAttribute? attribute = enumValue.GetAttributeFromEnumType<DisplayAttribute>();

        attribute.Should().NotBeNull().And.BeOfType<DisplayAttribute>();
        attribute?.Name.Should().Be("Value");
    }

    [Fact]
    public void GivenEnumWithoutAttributeThenReturnNull()
    {
        AttrEnumTest enumValue = AttrEnumTest.Text;
        DisplayAttribute? attribute = enumValue.GetAttributeFromEnumType<DisplayAttribute>();

        attribute.Should().BeNull();
    }

    private enum AttrEnumTest
    {
        [Display(Name = "Value")]
        Value = 0,

        Text = 1
    }
}
