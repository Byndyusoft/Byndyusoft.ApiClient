namespace Byndyusoft.ApiClient.Functional;

using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Client;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

public abstract class MvcFormattersTests(ITestOutputHelper testOutputHelper) : MvcTestFixture
{
    protected PersonModelListClient TestSubject;
    private static int _idProvider = 0;

    [Fact]
    protected async Task ThrowsTest()
    {
        var id = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {id}");

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await TestSubject.DeletePersonAsync(new PersonId(id, 0), CancellationToken.None));
    }

    [Fact]
    protected async Task AfterAddingPersonExpectingToGetSamePersonTest()
    {
        // Arrange
        var id = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {id}");
        var input = PersonModel.Create();

        // Act
        await TestSubject.AddListAsync(id, cancel);
        var response = await TestSubject.AddPersonAsync(id, input, cancel);
        var personId = new PersonId(id, response.Id!.Value);
        response = await TestSubject.GetPersonAsync(personId, cancel);
        personId = new PersonId(id, response.Id!.Value);
        await TestSubject.DeletePersonAsync(personId, cancel);
        await TestSubject.DeleteListAsync(id, cancel);

        // Assert
        Assert.NotNull(response);
        var model = Assert.IsType<PersonModel>(response);
        Assert.Equal(input, model);
    }
}