namespace Byndyusoft.ApiClient.Client;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Byndyusoft.ApiClient;
using Microsoft.Extensions.Options;
using Models;

public class TestFormatterSimpleModelClient : BaseClient
{
    public TestFormatterSimpleModelClient(
            HttpClient client,
            MediaTypeFormatter formatter,
            IOptions<ApiClientSettings> apiSettings
        )
        : base(client, apiSettings, formatter) { }


    public Task<SimpleModel> GetModelAsync(CancellationToken cancellationToken)
        => GetAsync<SimpleModel>("/SimpleModel/get", cancellationToken);

    public Task<SimpleModel> GetWithParamsAsync(ParamsTestModel model, CancellationToken cancellationToken)
        => GetAsync<ParamsTestModel, SimpleModel>("/SimpleModel/with_params", model, cancellationToken);

    public Task<SimpleModel> GetSingleFromListModelAsync(int id, CancellationToken cancellationToken)
        => GetAsync<SimpleModel>($"/SimpleModelList/get/{id}", cancellationToken);

    public Task<List<SimpleModel>> GetAllFromListModelAsync(int id, CancellationToken cancellationToken)
        => GetAsync<List<SimpleModel>>($"/SimpleModelList/getAll/{id}", cancellationToken);

    public Task PostAsync(string url, object content, CancellationToken cancellationToken) =>
        base.PostAsync(url, content, cancellationToken);

    public Task<TResult> PostAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
        base.PostAsync<TResult>(url, content, cancellationToken);

    public Task<TResult> PutAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
        base.PutAsync<TResult>(url, content, cancellationToken);

    public Task PutAsync(string url, object content, CancellationToken cancellationToken) =>
        base.PutAsync(url, content, cancellationToken);

    public Task<TResult> PatchAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
        base.PatchAsync<TResult>(url, content, cancellationToken);

    public Task PatchAsync(string url, object content, CancellationToken cancellationToken) =>
        base.PatchAsync(url, content, cancellationToken);

    public Task DeleteAsync(string url, CancellationToken cancellationToken) =>
        base.DeleteAsync(url, cancellationToken);

    public Task DeleteAsync<TParams>(string url, TParams parameters, CancellationToken cancellationToken) =>
        base.DeleteAsync(url, parameters, cancellationToken);
}