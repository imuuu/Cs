using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

using System.Collections.Generic;

namespace GameWebApi
{
    public class FileRepository : IRepository
    {

        string path = Path.Combine(Directory.GetCurrentDirectory(), "game-dev.txt");

        public FileRepository()
        {
            if(!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
        }

        
        public Task<Player> Create(Player player)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            /*
            lines.Add("Guid:" + player.Id);
            lines.Add("Name:" + player.Name);
            lines.Add("Score:" + player.Score);
            lines.Add("Level:" + player.Level);
            lines.Add("isBanned:" + player.IsBanned.ToString());
            lines.Add("CreationTime:" + player.CreationTime.ToString());
             */


            lines.Add(player.Id.ToString());
            lines.Add(player.Name);
            lines.Add(player.Score.ToString());
            lines.Add(player.Level.ToString());
            lines.Add(player.IsBanned.ToString());
            lines.Add(player.CreationTime.ToString());
            string wholeItemLine = "";
            foreach (var item in player.Items)
            {
                string i = String.Format("{0}/{1}/{2}/{3}", item.Id, item.Level, item.itemType, item.CreationDate);
                if (String.IsNullOrEmpty(wholeItemLine))
                {
                    wholeItemLine = i;
                }
                else
                {
                    wholeItemLine += "," + i;
                }

            }
            lines.Add(wholeItemLine);

            File.WriteAllLines(path, lines);

            return Task.FromResult(player);
        }

        public Task<Player> Delete(Guid id)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            int counter = 0;
            foreach (var line in lines)
            {
                if (line.Contains(id.ToString()))
                {
                    lines.RemoveRange(counter, 7);
                    break;
                }
                counter++;
            }

            File.WriteAllLines(path, lines);

            return Task.FromResult(GetPlayer(id));
        }

        public Task<Player> Get(Guid id)
        {
            return Task.FromResult(GetPlayer(id));
        }

        public Task<Player[]> GetAll()
        {

            List<string> lines = File.ReadAllLines(path).ToList();
            var players = new List<Player>();

            for (int counter = 0; counter + 7 < lines.Count; counter += 7)
            {
                Player p = new Player();
                p.Id = Guid.Parse(lines.ElementAt(counter));
                p.Name = lines.ElementAt(counter + 1);
                p.Score = int.Parse(lines.ElementAt(counter + 2));
                p.Level = int.Parse(lines.ElementAt(counter + 3));
                p.IsBanned = bool.Parse(lines.ElementAt(counter + 4));
                p.CreationTime = DateTime.Parse(lines.ElementAt(counter + 5));
                p.Items = ItemSplitter2000(lines.ElementAt(counter + 6));
                players.Add(p);
            }

            return Task.FromResult(players.ToArray());
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            Player newPlayer = GetPlayer(id);
            newPlayer.Score = player.Score;
            await Delete(id);
            return await Create(newPlayer);
        }


        public Task<Player> refreshPlayer(Player player)
        {            
            Player player1=player;
            Delete(player.Id);
            return Create(player1);
                     
        }
        public Task<Item> CreateItem(Guid playerId, Item item)
        {   
                     
            Player player=GetPlayer(playerId);
            if(player.Level < 3 && item.itemType== ItemType.SWORD)
            {
                throw new ExpectationFilt();
            }
            player.Items.Add(item);
            refreshPlayer(player);
            
            return Task.FromResult(item);
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player p=GetPlayer(playerId);
            foreach(var i in p.Items)
            {
                if(i.Id == itemId)
                {
                    return Task.FromResult(i);
                }
            }
            return null;
        }
        public Task<Item[]> GetAllItems(Guid playerId)
        {
            return Task.FromResult(GetPlayer(playerId).Items.ToArray());
        }
        public Task<Item> UpdateItem(Guid playerId, Item item)
        {
            Player p=GetPlayer(playerId);
            List<Item> itemList=new List<Item>();
            foreach(var i in p.Items)
            {
                Item newItem=new Item();
                newItem=i;
                if(i.Id == item.Id)
                {
                   newItem=item;
                }
                itemList.Add(newItem);
            }
            p.Items=itemList;
            refreshPlayer(p);
            
            return Task.FromResult(item);
        }
        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            Player p=GetPlayer(playerId);
            Item newItem=new Item();
            foreach(var i in p.Items)
            {
                if(i.Id == itemId)
                {
                    p.Items.Remove(i);
                    newItem=i;
                    break;
                }
            }
            return Task.FromResult(newItem);
        }


        public Player GetPlayer(Guid id)
        {
            Player p = new Player();
            List<string> lines = File.ReadAllLines(path).ToList();
            int counter = 0;
            foreach (var line in lines)
            {
                if (line.Contains(id.ToString()))
                {
                    p.Id = Guid.Parse(line);
                    p.Name = lines.ElementAt(counter + 1);
                    p.Score = int.Parse(lines.ElementAt(counter + 2));
                    p.Level = int.Parse(lines.ElementAt(counter + 3));
                    p.IsBanned = bool.Parse(lines.ElementAt(counter + 4));
                    p.CreationTime = DateTime.Parse(lines.ElementAt(counter + 5));
                    p.Items = ItemSplitter2000(lines.ElementAt(counter + 6));

                    break;
                }
                counter++;
            }

            return p;
        }

        public List<Item> ItemSplitter2000(string wholeItemLine)
        {
            List<Item> items = new List<Item>();
            string[] strItems = wholeItemLine.Split(",", StringSplitOptions.None);

            foreach (var strItem in strItems)
            {
                Item item = new Item();
                string[] strItemList = strItem.Split("/", StringSplitOptions.None);

                item.Id=Guid.Parse(strItemList[0]);
                item.Level = int.Parse(strItemList[1]);
                item.itemType = (ItemType)Enum.Parse(typeof(ItemType), strItemList[2]);
                item.CreationDate = DateTime.Parse(strItemList[3]);
                items.Add(item);

            }
            return items;
        }
    }
}
