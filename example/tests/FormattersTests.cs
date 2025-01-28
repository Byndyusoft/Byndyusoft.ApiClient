namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Client;
using Models;
using Xunit;
using Xunit.Abstractions;

public abstract class FormattersTests(ITestOutputHelper testOutputHelper) : MvcTestFixture
{
    protected PersonModelListClient TestSubject;
    private static int _idProvider = 0;

    [Fact]
    protected async Task TryDeleteNotExistingPerson_ExpectingThrows_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {listId}");

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await TestSubject.DeletePersonAsync(
                new PersonId(listId, 0),
                CancellationToken.None
            )
        );
    }

    [Fact]
    protected async Task TryGetNotExistingPerson_ExpectingThrows_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        testOutputHelper.WriteLine($"id: {listId}");

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await TestSubject.GetPersonAsync(
                new PersonId(listId, 0),
                CancellationToken.None
            )
        );
    }

    [Fact]
    protected async Task TryAddingPerson_ExpectingGetSamePerson_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {listId}");
        var newPerson = PersonModel.Create();

        // Act
        await TestSubject.AddListAsync(listId, cancel);
        var response = await TestSubject.AddPersonAsync(listId, newPerson, cancel);
        var personId = new PersonId(listId, response.Id);
        response = await TestSubject.GetPersonAsync(personId, cancel);
        personId = new PersonId(listId, response.Id);
        await TestSubject.DeletePersonAsync(personId, cancel);
        await TestSubject.DeleteListAsync(listId, cancel);

        // Assert
        Assert.NotNull(response);
        var actualPerson = Assert.IsType<PersonModel>(response);
        Assert.Equal(newPerson, actualPerson);
    }

    [Fact]
    protected async Task TryReplaceExistingPerson_ExpectingGetNewPerson_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {listId}");
        var newPerson = PersonModel.Create();

        // Act
        await TestSubject.AddListAsync(listId, cancel);
        var response = await TestSubject.ReplacePersonAsync(listId, newPerson, cancel);
        var personId = new PersonId(listId, response.Id);
        response = await TestSubject.GetPersonAsync(personId, cancel);
        personId = new PersonId(listId, response.Id);
        await TestSubject.DeletePersonAsync(personId, cancel);
        await TestSubject.DeleteListAsync(listId, cancel);

        // Assert
        Assert.NotNull(response);
        var actualPerson = Assert.IsType<PersonModel>(response);
        Assert.Equal(newPerson, actualPerson);
    }

    [Fact]
    protected async Task TryReplaceNotExistingPerson_ExpectingGetSamePerson_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {listId}");
        var oldPerson = PersonModel.Create();
        var newPerson = PersonModel.Create();
        newPerson.FirstName = "Jane";
        newPerson.LastName = "Smith";
        newPerson.FavoriteDessert = FavoriteDessertEnum.Orange;
        newPerson.ChildrenNames = ["John", "James", "Jennifer"];

        // Act
        await TestSubject.AddListAsync(listId, cancel);
        await TestSubject.AddPersonAsync(listId, oldPerson, cancel);
        var response = await TestSubject.ReplacePersonAsync(listId, newPerson, cancel);
        var personId = new PersonId(listId, response.Id);
        response = await TestSubject.GetPersonAsync(personId, cancel);
        personId = new PersonId(listId, response.Id);
        await TestSubject.DeletePersonAsync(personId, cancel);
        await TestSubject.DeleteListAsync(listId, cancel);

        // Assert
        Assert.NotNull(response);
        var actualPerson = Assert.IsType<PersonModel>(response);
        Assert.Equal(newPerson, actualPerson);
    }

    [Fact]
    protected async Task TryUpdateExistingPerson_ExpectingGetPersonCombination_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {listId}");
        var oldPerson = PersonModel.Create();
        var newPerson = new PersonModel(
            id: oldPerson.Id,
            firstName: "Jane",
            childrenNames: ["John", "James", "Jennifer"]
        );
        var expectedPerson = PersonModel.Create();
        expectedPerson.FirstName = newPerson.FirstName;
        expectedPerson.ChildrenNames = newPerson.ChildrenNames;

        // Act
        await TestSubject.AddListAsync(listId, cancel);
        await TestSubject.AddPersonAsync(listId, oldPerson, cancel);
        var response = await TestSubject.UpdatePersonAsync(listId, newPerson, cancel);
        var personId = new PersonId(listId, response.Id);
        response = await TestSubject.GetPersonAsync(personId, cancel);
        personId = new PersonId(listId, response.Id);
        await TestSubject.DeletePersonAsync(personId, cancel);
        await TestSubject.DeleteListAsync(listId, cancel);

        // Assert
        Assert.NotNull(response);
        var actualPerson = Assert.IsType<PersonModel>(response);
        Assert.Equal(expectedPerson, actualPerson);
    }

    [Fact]
    protected async Task TryUpdateNotExistingPerson_ExpectingThrows_Test()
    {
        // Arrange
        var listId = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        testOutputHelper.WriteLine($"id: {listId}");
        var newPerson = PersonModel.Create();

        // Act
        await TestSubject.AddListAsync(listId, cancel);

        //Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await TestSubject.UpdatePersonAsync(listId, newPerson, cancel)
        );
        await TestSubject.DeleteListAsync(listId, cancel);
    }
}