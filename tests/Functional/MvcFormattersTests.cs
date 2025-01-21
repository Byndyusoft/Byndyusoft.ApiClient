namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Models;
using Client;
using Xunit;
using Xunit.Abstractions;

public abstract class MvcFormattersTests(ITestOutputHelper testOutputHelper) : MvcTestFixture
{
    protected TestFormatterSimpleModelClient TestSubject;
    private static int _idProvider = 0;

    [Fact]
    protected async Task PostAsyncTest()
    {
        // Arrange
        var input = SimpleModel.Create();

        // Act
        var response = await TestSubject.PostModelAsync(input, CancellationToken.None);
            
        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        Assert.Equal(input, model);
    }

    [Fact]
    protected async Task PutAsyncTest()
    {
        // Arrange
        var input = SimpleModel.Create();

        // Act
        var response = await TestSubject.PutModelAsync(input, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        Assert.Equal(input, model);
    }

    [Fact]
    protected async Task GetAsyncTest()
    {
        // Act
        var response = await TestSubject.GetModelAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        var expected = SimpleModel.Create();
        Assert.Equal(expected, model);
    }

    [Fact]
    protected async Task GetAsyncWithParamsTest()
    {
        // Arrange
        var input = new ParamsTestModel(10101, "tuple", null);

        // Act
        var response = await TestSubject.GetWithParamsAsync(input, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        var expected = SimpleModel.Create(input.property, field:input.field, nullable:input.nullable);
        Assert.Equal(expected, model);
    }

    [Fact]
    protected async Task ThrowsTest()
    {
        var id = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {id}");

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await TestSubject.DeleteFromListAsync(id, CancellationToken.None));
    }

    [Fact]
    protected async Task ListBaseTest()
    {
        // Arrange
        var id = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {id}");
        var input = SimpleModel.Create();

        // Act
        await TestSubject.AddListAsync(id, cancel);
        var response = await TestSubject.PostModelToListAsync(id, input, cancel);
        response = await TestSubject.PutModelToListAsync(id, response, cancel);
        response = await TestSubject.PatchModelAtListAsync(id, response, cancel);
        response = await TestSubject.GetSingleFromListModelAsync(id, cancel);
        await TestSubject.DeleteFromListAsync(id, cancel);
        await TestSubject.DeleteListAsync(id, cancel);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);
        Assert.Equal(input, model);
    }
}