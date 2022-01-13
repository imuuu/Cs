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
                    lines.RemoveRange(counter, 6);
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

            for (int counter = 0; counter+6 < lines.Count; counter += 6)
            {
                Player p = new Player();
                p.Id = Guid.Parse(lines.ElementAt(counter));
                p.Name = lines.ElementAt(counter + 1);
                p.Score = int.Parse(lines.ElementAt(counter + 2));
                p.Level = int.Parse(lines.ElementAt(counter + 3));
                p.IsBanned = bool.Parse(lines.ElementAt(counter + 4));
                p.CreationTime = DateTime.Parse(lines.ElementAt(counter + 5));
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
                    break;
                }
                counter++;
            }

            return p;
        }
    }
}
