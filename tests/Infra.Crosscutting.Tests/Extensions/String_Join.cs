using System.Linq;
using FluentAssertions;
using Goal.Seedwork.Infra.Crosscutting.Extensions;
using Xunit;

namespace Goal.Seedwork.Infra.Crosscutting.Tests.Extensions;

public class String_Join
{
    [Fact]
    public void ReturnJoinedStringGivenValidStringArray()
    {
        string[] stringArray = ["test", "test1"];
        string joinedArray = stringArray.Join(", ");

        joinedArray.Should().NotBeNull().And.Be("test, test1");
    }

    [Fact]
    public void ReturnJoinedStringGivenValidObjectArray()
    {
        object[] stringArray = ["test", 1, true];
        string joinedArray = stringArray.Join(", ");

        joinedArray.Should().NotBeNull().And.Be("test, 1, True");
    }
}
