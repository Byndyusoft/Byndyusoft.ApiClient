namespace Byndyusoft.ApiClient.Client;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiClient;
using Contracts;
using Interfaces;
using Microsoft.Extensions.Options;
using Models;

public class TestFormatterSimpleModelClient : BaseClient, ISimpleModelClient, ISimpleModelListClient
{
    private const string SimpleModelPrefix = "/SimpleModel";
    private const string SimpleModelListPrefix = "/SimpleModelList";

    public TestFormatterSimpleModelClient(
            HttpClient client,
            IOptions<ApiClientSettings> apiSettings,
            IFormatterProvider formatterProvider = null
        )
        : base(client, apiSettings, formatterProvider) { }


    public Task<SimpleModel> GetModelAsync(CancellationToken cancellationToken)
        => GetAsync<SimpleModel>($"{SimpleModelPrefix}/get", cancellationToken);

    public Task<SimpleModel> PostModelAsync(SimpleModel content, CancellationToken cancellationToken)
        => PostAsync<SimpleModel>($"{SimpleModelPrefix}/post", content, cancellationToken);

    public Task<SimpleModel> PutModelAsync(SimpleModel content, CancellationToken cancellationToken)
        => PutAsync<SimpleModel>($"{SimpleModelPrefix}/put", content, cancellationToken);

    public Task<SimpleModel> GetWithParamsAsync(ParamsTestModel model, CancellationToken cancellationToken)
        => GetAsync<ParamsTestModel, SimpleModel>($"{SimpleModelPrefix}/with_params", model, cancellationToken);

    public Task AddListAsync(int id, CancellationToken cancellationToken)
        => PostAsync($"{SimpleModelListPrefix}/addList/{id}", null, cancellationToken);

    public Task DeleteListAsync(int id, CancellationToken cancellationToken) =>
        DeleteAsync($"{SimpleModelListPrefix}/deleteList/{id}", cancellationToken);

    public Task<SimpleModel> GetSingleFromListModelAsync(int id, CancellationToken cancellationToken)
        => GetAsync<SimpleModel>($"{SimpleModelListPrefix}/get/{id}", cancellationToken);

    public Task<List<SimpleModel>> GetAllFromListModelAsync(int id, CancellationToken cancellationToken)
        => GetAsync<List<SimpleModel>>($"{SimpleModelListPrefix}/getAll/{id}", cancellationToken);

    public Task<SimpleModel> PostModelToListAsync(int id, SimpleModel content, CancellationToken cancellationToken)
        => PostAsync<SimpleModel>($"{SimpleModelListPrefix}/post/{id}", content, cancellationToken);

    public Task<SimpleModel> PutModelToListAsync(int id, SimpleModel content, CancellationToken cancellationToken)
        => PutAsync<SimpleModel>($"{SimpleModelListPrefix}/put/{id}", content, cancellationToken);

    public Task<SimpleModel> PatchModelAtListAsync(int id, SimpleModel content, CancellationToken cancellationToken)
        => PatchAsync<SimpleModel>($"{SimpleModelListPrefix}/patch/{id}", content, cancellationToken);

    public Task DeleteFromListAsync(int id, CancellationToken cancellationToken)
        => DeleteAsync($"{SimpleModelListPrefix}/delete/{id}", cancellationToken);
}