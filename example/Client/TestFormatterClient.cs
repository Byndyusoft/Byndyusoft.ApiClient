namespace Byndyusoft.ApiClient.Client;

using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Byndyusoft.ApiClient;
using Microsoft.Extensions.Options;

public class TestFormatterClient : BaseClient
{
    public TestFormatterClient
        (
            HttpClient client,
            MediaTypeFormatter formatter,
            IOptions<ApiClientSettings> apiSettings
        )
        : base
        (
            client,
            apiSettings,
            formatter
        )
    {
    }


    public Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        => base.GetAsync<TResult>(url, cancellationToken);

    public Task<TResult> GetAsync<TParams, TResult>(string url, CancellationToken cancellationToken, TParams? dto = null)
        where TParams : class
        => base.GetAsync<TParams, TResult>(url, cancellationToken, dto);

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