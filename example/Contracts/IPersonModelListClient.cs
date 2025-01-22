using Byndyusoft.ApiClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Byndyusoft.ApiClient.Contracts
{
    public interface IPersonModelListClient
    {
        public Task AddListAsync(int id, CancellationToken cancellationToken);
        public Task DeleteListAsync(int id, CancellationToken cancellationToken);
        public Task<PersonModel> GetPersonAsync(PersonId id, CancellationToken cancellationToken);
        public Task<List<PersonModel>> GetPersonListAsync(int id, CancellationToken cancellationToken);
        public Task<PersonModel> AddPersonAsync(int id, PersonModel content, CancellationToken cancellationToken);
        public Task<PersonModel> ReplacePersonAsync(int id, PersonModel content, CancellationToken cancellationToken);
        public Task<PersonModel> UpdatePersonAsync(int id, PersonModel content, CancellationToken cancellationToken);
        public Task DeletePersonAsync(PersonId id, CancellationToken cancellationToken);
    }
}
