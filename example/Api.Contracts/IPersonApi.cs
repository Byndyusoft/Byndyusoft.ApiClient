namespace Api.Contracts
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IPersonApi
    {
        public Task AddListAsync(int id, CancellationToken cancellationToken);
        public Task DeleteListAsync(int id, CancellationToken cancellationToken);
        public Task<PersonModel> GetPersonAsync(PersonId id, CancellationToken cancellationToken);
        public Task<Dictionary<ulong, PersonModel>> GetEveryPersonAsync(int id, CancellationToken cancellationToken);
        public Task<PersonModel> AddPersonAsync(int id, PersonModel content, CancellationToken cancellationToken);
        public Task<PersonModel> ReplacePersonAsync(int id, PersonModel content, CancellationToken cancellationToken);
        public Task<PersonModel> UpdatePersonAsync(int id, PersonModel content, CancellationToken cancellationToken);
        public Task DeletePersonAsync(PersonId id, CancellationToken cancellationToken);
    }
}
