using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Heros;
namespace GameServer.Managers
{
    class HeroManager
    {
        Dictionary<HeroEnum, Heroo> _allHeroes = new Dictionary<HeroEnum, Heroo>();

        public HeroManager()
        {
            InitHeroes();
        }

        private void InitHeroes()
        {
            Heroo hero;// = new Heroo("AI", 10);
            //_allHeroes.Add(hero._name, hero);
            hero = new HeroCaptain(HeroEnum.CAPTAIN);
            _allHeroes.Add(HeroEnum.CAPTAIN, hero);
        }

        public Heroo[] GetHeros()
        {
            Heroo[] heros = new Heroo[_allHeroes.Count];
            foreach (Heroo hero in _allHeroes.Values) 
            {
                heros[(int)hero._heroEnum] = hero;
            }
            return heros;
        }

        public Heroo GetHero(HeroEnum heroEnum)
        {
            return Program.Clone<Heroo>(_allHeroes[heroEnum]).SetNewGuid();
           // return _allHeroes[name].Clone();
        }

        public Player FindPlayerHero(Guid heroUUID)
        {
            //Console.WriteLine("try to find hero with uuid: " + heroUUID.ToString());
            for (int i = 1; i <= Server._maxPlayers; ++i)
            {
                Heroo hero = Server._clients[i]._player.GetHero();
                //Console.WriteLine($"==> Client {i} hero uuid: " + hero.ToString());
                if (hero != null && hero._uuid.Equals(heroUUID))
                {
                    //Console.WriteLine($"=====>> Client {i} was correct");
                    return Server._clients[i]._player;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates hero data for all clients
        /// </summary>
        public void SendHeroesToClients(int clientID)
        {
            GameInfo gameInfo = Server._gameScene.GetGameInfo(clientID);

            foreach(Client client in new Client[] {gameInfo._owner, gameInfo._opponent})
            {
                Heroo hero = client._player.GetHero();
                if (hero != null)
                {
                    int opponentID = Server.GetOpponentForId(client._id);
                    ServerSend.InitHero(client._id, hero, 1);
                    ServerSend.InitHero(opponentID, hero, 2);
                    //Console.WriteLine($"Sending client {client._id} own hero and sending {opponentID} hero as enemy.. hero uuid: {hero._uuid} and health: {hero._hp}");
                }else
                {
                    Console.WriteLine("SENDING HERO FAILED!");
                }
            }
            //for (int i = 1; i <= Server._maxPlayers; ++i)
            //{
            //    if (Server._clients[i]._tcp._socket == null)
            //        continue;

            //    Heroo hero = Server._clients[i]._player.GetHero();
            //    if (hero != null)
            //    {
            //        ServerSend.InitHero(i, hero, 1);
            //        ServerSend.InitHero(-i, hero, 2);
            //    }
            //}
        }
    }
}
