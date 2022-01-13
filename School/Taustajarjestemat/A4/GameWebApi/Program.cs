using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace GameWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // string path = Path.Combine(Directory.GetCurrentDirectory(),"game-dev.txt");
            // List<string> lines=File.ReadAllLines(path).ToList();
            // foreach(var line in lines)
            // {
            //     Console.WriteLine(line);
            // }
            IRepository fr=new FileRepository();
            Item i1=new Item{Level=1,CreationDate=DateTime.Now,itemType=ItemType.SWORD};
            Item i2=new Item{Level=2,CreationDate=DateTime.Now,itemType=ItemType.SHIELD};
            List<Item> itemii=new List<Item>();
            itemii.Add(i1);
            itemii.Add(i2);
            Player player=new Player {Name ="Tim",Id =Guid.NewGuid(),Score = 99,CreationTime =DateTime.Today,IsBanned=false,Level=10,Items=itemii};
            fr.Create(player);
            Console.WriteLine("LOOOOOOOOOOOOOOL");
            
            CreateWebHostBuilder(args).Build().Run();
           
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
