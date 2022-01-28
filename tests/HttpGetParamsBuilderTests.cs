namespace Byndyusoft.ApiClient.Tests
{
    using FluentAssertions;
    using Xunit;

    public class HttpGetParamsBuilderTests
    {
        [Fact]
        public void BuildArrayTest()
        {
            var result = HttpGetParamsBuilder.Build(new {ids = new[] {1, 2, 3}});

            result.Should().Be("ids=1&ids=2&ids=3");
        }
    }
}