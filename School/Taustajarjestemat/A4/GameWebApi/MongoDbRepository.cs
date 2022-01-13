using System;
using System.Threading.Tasks;

namespace GameWebApi
{
    public class MongoDbRepository : IRepository
    {
        Task<Player> IRepository.Create(Player player)
        {
            throw new NotImplementedException();
        }

        Task<Item> IRepository.CreateItem(Guid playerId, Item item)
        {
            throw new NotImplementedException();
        }

        Task<Player> IRepository.Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<Item> IRepository.DeleteItem(Guid playerId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        Task<Player> IRepository.Get(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<Player[]> IRepository.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<Item[]> IRepository.GetAllItems(Guid playerId)
        {
            throw new NotImplementedException();
        }

        Task<Item> IRepository.GetItem(Guid playerId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        Task<Player> IRepository.Modify(Guid id, ModifiedPlayer player)
        {
            throw new NotImplementedException();
        }

        Task<Item> IRepository.UpdateItem(Guid playerId, Item item)
        {
            throw new NotImplementedException();
        }
    }
}
