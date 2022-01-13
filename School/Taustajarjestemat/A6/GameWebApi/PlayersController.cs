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

        private IRepository _repoManager;

        public PlayersController(IRepository repoManager)
        {
            _repoManager = repoManager;            
        }

        [HttpGet]
        public Task<Player[]> Get()
        {
            return _repoManager.GetAll();
        }
        
        [HttpGet("{id:guid}")]
        
        public Task<Player> Get(Guid id)
        {
            return _repoManager.Get(id);
        }


        
        [HttpGet]
        [Route("score")]
        public Task<Player[]> GetPlayersLowerScore(string minScore)
        {
            return _repoManager.GetScoreBelow(int.Parse(minScore));
        }
        
        [HttpGet("{playerName:alpha}")]
        public Task<Player> GetPlayerByName(string playerName)
        {
            return _repoManager.GetPlayerByName(playerName);
        }

        [HttpGet("{getDecScoreTop10}")]
        public Task<Player[]> GetDecScoreTop10()
        {
            return _repoManager.GetDecScoreTop10();
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
        [HttpPost("{id:guid}")]
        public Task<Player> UpdatePlayerName(Guid id, [FromBody] Player player)
        {
            return _repoManager.UpdatePlayerName(id,player.Name);
        }

        [HttpPut("{id:guid}")]
        public Task<Player> UpdatePlayerScore(Guid id, [FromBody] Player player)
        {
            return _repoManager.UpdatePlayerScore(id,player);
        }
        [HttpDelete("{id}")]
        public Task<Player> Delete(Guid id)
        {
            return _repoManager.Delete(id);
        }
        

    }
}
