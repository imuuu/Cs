using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace GameWebApi
{
    public class MongoDbRepository : IRepository
    {

        private IMongoCollection<Player> _col;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("game");
            _col = database.GetCollection<Player>("players");
            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
            //Console.WriteLine("DB READYYY!!");
        }

        public async Task<Player> Create(Player player)
        {
            // Item item=new Item();
            // item.Id=Guid.NewGuid();
            // item.Level=99;
            // item.itemType=ItemType.POTION;
            // item.CreationDate=DateTime.Now;
            // player.Items.Add(item);
            await _col.InsertOneAsync(player);
            return player;

        }

        public async Task<Player[]> GetBetweenLvl(int min,int max)
        {
            var fil=Builders<Player>.Filter.Gte(p => p.Level, min) & Builders<Player>.Filter.Lte(p=> p.Level, max);
            var ps= await _col.Find(fil).ToListAsync();
            return ps.ToArray();
        }

        public async Task<Player[]> GetScoreBelow(int score)
        {
            
            var fil=Builders<Player>.Filter.Lte(p=> p.Score, score);
            var ps = await _col.Find(fil).ToListAsync();
            return ps.ToArray();
        }

        public Task<Player> GetPlayerByName(string name)
        {
            var fil=Builders<Player>.Filter.Eq(player => player.Name, name);
            return _col.Find(fil).FirstAsync();
        }
        public async Task<Player> Delete(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(player => player.Id, id);
            return await _col.FindOneAndDeleteAsync(filter);

        }

        public Task<Player> Get(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
            return _col.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetAll()
        {
            var players = await _col.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }
        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            //var fil= Builders<Player>.Filter.Eq(player => player.Id, id);
            Player newPlayer = await Get(id);
            newPlayer.Score = player.Score;
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(player1 => player1.Id, id);
            await _col.ReplaceOneAsync(filter, newPlayer);
            return newPlayer;
        }

        public async Task<Player> refreshPlayer(Player player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(player1 => player1.Id, player.Id);
            await _col.ReplaceOneAsync(filter, player);
            return player;
        }
        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            Player player = await Get(playerId);
            player.Items.Add(item);
            await refreshPlayer(player);
            return await Task.FromResult(item);

        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player player = await Get(playerId);
            return player.Items.ToArray();
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Item[] items = await GetAllItems(playerId);
            //var item=await items.Where(i => i.Id.Equals(itemId));
            foreach (var i in items)
            {
                if (i.Id == itemId)
                {
                    return i;
                }

            }

            throw new NotFoundException();
        }

        public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            Player p = await Get(playerId);
            Item item = await GetItem(playerId, itemId);
            p.Items.Remove(item);
            await refreshPlayer(p);
            return item;

        }

        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            Player p = await Get(playerId);
            await DeleteItem(playerId, item.Id);
            p.Items.Add(item);
            await refreshPlayer(p);
            return item;

        }

        public async Task<Player[]> GetDecScoreTop10()
        {
            var fil=Builders<Player>.Sort.Descending(p=> p.Score);
            var ps= await _col.Find(new BsonDocument()).Sort(fil).ToListAsync();
            Player[] players=ps.Take(10).ToArray();           
            return players;

        }

        public async Task<Player> UpdatePlayerName(Guid Id, string name)
        {
            var fil = Builders<Player>.Filter.Eq(p => p.Id, Id);
            var update=Builders<Player>.Update.Set(p => p.Name, name);
            await _col.UpdateOneAsync(fil,update);
            return await Get(Id);
            

            
        }
        public async Task<Player> UpdatePlayerScore(Guid Id,Player player)
        {
            var fil = Builders<Player>.Filter.Eq(p => p.Id, Id);
            var update=Builders<Player>.Update.Set(p => p.Score, player.Score);
            await _col.UpdateOneAsync(fil,update);
            return await Get(Id);
        }
    }
}
