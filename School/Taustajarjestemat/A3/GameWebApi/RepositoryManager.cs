using System;
using System.Threading.Tasks;

namespace GameWebApi
{
    public class RepositoryManager
    {
        private IRepository _repo;

        public RepositoryManager(IRepository repo)
        {
            _repo=repo;
        }

        public Task<Player> Get(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Player[]> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<Player> Create(Player player)
        {
            return _repo.Create(player);
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return _repo.Modify(id,player);
        }

        public Task<Player> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}
