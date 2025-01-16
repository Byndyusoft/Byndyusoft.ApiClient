namespace Byndyusoft.ApiClient.Functional;

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Client;
using Models;
using Xunit;
using Xunit.Abstractions;

public abstract class MvcFormattersTests(ITestOutputHelper testOutputHelper) : MvcTestFixture
{
    protected TestFormatterClient TestSubject;
    private static int _idProvider = 0;

    [Fact]
    protected async Task PostAsyncTest()
    {
        // Arrange
        var input = SimpleModel.Create();

        // Act
        var response = await TestSubject.PostAsync<SimpleModel>("/formatter/post", input, CancellationToken.None);
            
        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);
        model.Verify();
    }

    [Fact]
    protected async Task PutAsyncTest()
    {
        // Arrange
        var input = SimpleModel.Create();

        // Act
        var response = await TestSubject.PutAsync<SimpleModel>("/formatter/put", input, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        model.Verify();
    }

    [Fact]
    protected async Task GetAsyncTest()
    {
        // Act
        var response = await TestSubject.GetAsync<SimpleModel>("/formatter/get", CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        model.Verify();
    }

    [Fact]
    protected async Task GetAsyncWithParamsTest()
    {
        // Arrange
        var input = new ParamsTestModel(
            10101,
            "tuple",
            null);

        // Act
        var response = await TestSubject.GetAsync<ParamsTestModel,SimpleModel>(
            "/formatter/with_params",
            CancellationToken.None,
            input);
        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);

        model.Verify(input);
    }

    [Fact]
    protected async Task ThrowsTest()
    {
        var id = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {id}");
        ListModel.Data.Add(id, new List<SimpleModel>());
        // Assert
        await Assert.ThrowsAsync<HttpRequestWithContentException>(
            async () => 
                await TestSubject.DeleteAsync(
            $"/formatter/list/delete/{id}",
            CancellationToken.None));
    }

    [Fact]
    protected async Task ListBaseTest()
    {
        // Arrange
        var id = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {id}");
        ListModel.Data.Add(id, new List<SimpleModel>());
        var input = SimpleModel.Create();

        // Act
        var response = await TestSubject.PostAsync<SimpleModel>
        (
            $"/formatter/list/post/{id}",
            input,
            CancellationToken.None
        );
        response = await TestSubject.PutAsync<SimpleModel>
        (
            $"/formatter/list/put/{id}",
            response,
            CancellationToken.None
        );
        response = await TestSubject.PatchAsync<SimpleModel>
        (
            $"/formatter/list/patch/{id}",
            response,
            CancellationToken.None
        );
        response = await TestSubject.GetAsync<SimpleModel>
        (
            $"/formatter/list/get/{id}",
            CancellationToken.None
        );
        await TestSubject.DeleteAsync
        (
            $"/formatter/list/delete/{id}",
            CancellationToken.None
        );

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<SimpleModel>(response);
        model.Verify();
    }

    [Fact]
    protected async Task ListStressTest()
    {
        // Arrange
        var id = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {id}");
        ListModel.Data.Add(id, new List<SimpleModel>());
        var input = SimpleModel.Create();
        var length = 10_000;
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        for (var i = 0; i < length; i++)
        {
            await TestSubject.PostAsync
            (
                $"/formatter/list/post/{id}",
                input,
                CancellationToken.None
            );
        }
        stopwatch.Stop();
        testOutputHelper.WriteLine(stopwatch.Elapsed.ToString());

        stopwatch.Reset();
        stopwatch.Start();
        var response = await TestSubject.GetAsync<List<SimpleModel>>
        (
            $"/formatter/list/getAll/{id}",
            CancellationToken.None
        );
        stopwatch.Stop();
        testOutputHelper.WriteLine(stopwatch.Elapsed.ToString());
        
        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<List<SimpleModel>>(response);
        Assert.Equal(ListModel.Data[id], model);

        stopwatch.Reset();
        stopwatch.Start();
        for (var i = 0; i < length; i++)
        {
            await TestSubject.DeleteAsync($"/formatter/list/delete/{id}", CancellationToken.None);
        }
        stopwatch.Stop();
        testOutputHelper.WriteLine(stopwatch.Elapsed.ToString());
    }
}