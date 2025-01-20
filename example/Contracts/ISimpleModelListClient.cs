using Byndyusoft.ApiClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Byndyusoft.ApiClient.Contracts
{
    public interface ISimpleModelListClient
    {

        public Task AddListAsync(int id, CancellationToken cancellationToken);
        public Task DeleteListAsync(int id, CancellationToken cancellationToken);
        public Task<SimpleModel> GetSingleFromListModelAsync(int id, CancellationToken cancellationToken);
        public Task<List<SimpleModel>> GetAllFromListModelAsync(int id, CancellationToken cancellationToken);
        public Task<SimpleModel> PostModelToListAsync(int id, SimpleModel content, CancellationToken cancellationToken);
        public Task<SimpleModel> PutModelToListAsync(int id, SimpleModel content, CancellationToken cancellationToken);
        public Task<SimpleModel> PatchModelAtListAsync(int id, SimpleModel content, CancellationToken cancellationToken);
        public Task DeleteFromListAsync(int id, CancellationToken cancellationToken);
    }
}
