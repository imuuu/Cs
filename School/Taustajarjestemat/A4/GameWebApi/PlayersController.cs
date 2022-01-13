using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using System;

namespace GameWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        //RepositoryManager _repoManager=new RepositoryManager(new FileRepository());
        //Player player=new Player {Name ="Tim",Id =Guid.NewGuid(),Score = 99,CreationTime =DateTime.Today,IsBanned=false,Level=10};
        //FileRepository _repoManager = new FileRepository();
        IRepository _repoManager=new FileRepository();
        // public PlayersController(RepositoryManager repoManager)
        // {
        //     this._repoManager = repoManager;


        // }
        [HttpGet]
        public Task<Player[]> Get()
        {
            return _repoManager.GetAll();
        }
        [HttpGet("{id}")]
        public Task<Player> Get(Guid id)
        {
            return _repoManager.Get(id);
        }

        

        [HttpPost]
        public Task<Player> Create(NewPlayer player)
        {
            
            Player p=new Player();
            p.Name=player.Name;
            p.Id=Guid.NewGuid();
            p.Score=0;
            p.Level=0;
            p.IsBanned=false;
            p.CreationTime=DateTime.Now;
            p.Items=new List<Item>();
            return _repoManager.Create(p);
        }

        [HttpPut("{id}")]
        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return _repoManager.Modify(id,player);
        }
        
        [HttpDelete("{id}")]
        public Task<Player> Delete(Guid id)
        {
            return _repoManager.Delete(id);
        }
        //public Task<Player> Post()


    }
}
