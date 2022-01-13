using System;
using System.Collections.Generic;
using System.Linq;

namespace A2
{
    class Program
    {
        //static List<Player> players=new List<Player>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            Player[] players=makePlayerList(1000000);
           
            if(players.Length != players.Distinct().Count())
            {
                Console.WriteLine("Dups");
            }
            
            Player bot=players[0];
            Random rand=new Random();
            
            for(int i = 0; i < 5; i++)
            {
                Item item=new Item();
                item.Id=Guid.NewGuid();
                item.Level=rand.Next(100);
                bot.Items.Add(item);
            }
            
            //delegates
            ProcessEachItem(bot,PrintItem);
            //lambda
            ProcessEachItem(bot,item => Console.WriteLine("Id: " + item.Id + " Level: " + item.Level));

            var game=new Game<Player>(makePlayerList(100).ToList());
            List<PlayerForAnotherGame> playerForAnothers=new List<PlayerForAnotherGame>();
            for(int i= 0; i < 100; i++)
            {
                PlayerForAnotherGame pl=new PlayerForAnotherGame();
                pl.Id=Guid.NewGuid();
                pl.Score=rand.Next(100);
                playerForAnothers.Add(pl);

            }

            var anotherGame=new Game<PlayerForAnotherGame>(playerForAnothers);
            Console.WriteLine(game.GetTop10Players());
            Console.WriteLine(anotherGame.GetTop10Players());
        }


        static Player[] makePlayerList(int amount)
        {
            Player[] players=new Player[amount];
            //var rand=new Random();
            for(int i=0; i < amount ; i++)
            {
                players[i]=new Player();
                players[i].Id=Guid.NewGuid();
                players[i].Score=0;
                players[i].Items=new List<Item>();
                

            }
            return players;
        }

        Item GetHighestLevelItem(Player p)
        {            
            Item i=p.Items[0];
            foreach(var item in p.Items)
            {
                if(i.Level < item.Level)
                    i=item;
            }
            return i;
        }

        Item[] GetItems(Player p)
        {
            Item[] items=new Item[p.Items.Count];

            for(int i = 0; i < items.Length; i++)
            {
                items[i]=p.Items[i];
            }
                return items;
        }

        Item[] GetItemsWithLinq(Player p)
        {
            Item[] arrayItems=p.Items.ToArray();
            return arrayItems;
        }

        Item FirstItem(Player p)
        {
            if(p.Items.Count < 1)
                return null;
        
            return p.Items[0];
        }   
        Item FirstItemWithLinq(Player p)
        {       
            return p.Items.FirstOrDefault();
        }

        static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach(var item in player.Items)
            {
                process(item);
            }
        }

        static void PrintItem(Item i)
        {
            Console.WriteLine("Id: " + i.Id + " Level: " + i.Level);
        }

        
    }

public class PlayerForAnotherGame: IPlayer
{
     public Guid Id { get; set; }
    public int Score { get; set; }
    public List<Item> Items { get; set; }
}
public class Player : IPlayer
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public List<Item> Items { get; set; }

    
}

public class Item
{
    public Guid Id { get; set; }
    public int Level { get; set; }
}
}
