namespace Byndyusoft.ApiClient.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface ISimpleModelClient
    {
        public Task<SimpleModel> GetModelAsync(CancellationToken cancellationToken);
        public Task<SimpleModel> PostModelAsync(SimpleModel content, CancellationToken cancellationToken);
        public Task<SimpleModel> PutModelAsync(SimpleModel content, CancellationToken cancellationToken);
        public Task<SimpleModel> GetWithParamsAsync(ParamsTestModel model, CancellationToken cancellationToken);
    }
}
