namespace Byndyusoft.ApiClient.Example.Client;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using ApiClient;
using Contracts;
using Microsoft.Extensions.Options;
using Models;

public class PersonApi : BaseClient, IPersonApi
{
    public PersonApi(
            HttpClient client,
            IOptions<ApiClientSettings> apiSettings,
            MediaTypeFormatter? formatter = null
        )
        : base(client, apiSettings, formatter) { }

    public Task AddListAsync(int id, CancellationToken cancellationToken)
        => PostAsync($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.AddListCmd}/{id}", null, cancellationToken);

    public Task DeleteListAsync(int id, CancellationToken cancellationToken) =>
        DeleteAsync($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.DeleteListCmd}/{id}", cancellationToken);

    public Task<PersonModel> GetPersonAsync(PersonId id, CancellationToken cancellationToken)
        => GetAsync<PersonId, PersonModel>($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.GetPersonCmd}", id, cancellationToken);

    public Task<Dictionary<ulong, PersonModel>> GetEveryPersonAsync(int id, CancellationToken cancellationToken)
        => GetAsync<Dictionary<ulong, PersonModel>>($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.GetEveryPersonCmd}/{id}", cancellationToken);

    public Task<PersonModel> AddPersonAsync(int id, PersonModel content, CancellationToken cancellationToken)
        => PostAsync<PersonModel>($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.AddPersonCmd}/{id}", content, cancellationToken);

    public Task<PersonModel> ReplacePersonAsync(int id, PersonModel content, CancellationToken cancellationToken)
        => PutAsync<PersonModel>($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.ReplacePersonCmd}/{id}", content, cancellationToken);

    public Task<PersonModel> UpdatePersonAsync(int id, PersonModel content, CancellationToken cancellationToken)
        => PatchAsync<PersonModel>($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.UpdatePersonCmd}/{id}", content, cancellationToken);

    public Task DeletePersonAsync(PersonId id, CancellationToken cancellationToken)
        => DeleteAsync($"/{PersonModelListRoutes.PersonModelListPrefix}/{PersonModelListRoutes.DeletePersonCmd}", id, cancellationToken);
}