using System;
using System.Threading.Tasks;
namespace GameWebApi
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);

        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Item item);
        Task<Item> DeleteItem(Guid playerId, Guid itemId);

        Task<Player[]> GetBetweenLvl(int min,int max);
        Task<Player[]> GetScoreBelow(int score);

        Task<Player> GetPlayerByName(string name);

        Task<Player> refreshPlayer(Player player);

        Task<Player[]> GetDecScoreTop10();

        Task<Player> UpdatePlayerName(Guid Id, string name);

        Task<Player> UpdatePlayerScore(Guid Id, Player player);
    }
}
