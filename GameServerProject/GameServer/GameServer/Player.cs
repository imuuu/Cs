using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{   
    [Serializable]
    public class Player
    {
        public int _id;
        
        public string _username;

        private int _money = 0;
        private int _moneyIncome = 0;
        private int _mana = 0;
        private int _manaCap = 1;

        private int _actionPoints = 5;
        private int _round_actionsPoints = 0;
        private int _victoryPoints = 0;
        
        
        private Hand _hand;
        private Battlefield _battlefield;
        private Heroo _hero;

        Table _table;
        public Player(int id, string username)
        {
            _id = id;
            _username = username;
            
            _hand = new Hand(this);
            _battlefield = new Battlefield(this,4, 4);
            _table = new Table(this, Server._cardColManager);
            //InitHero(HeroEnum.CAPTAIN);
            
            _money = 0;
            _moneyIncome = 100;
            _mana = 1;
            _manaCap = _mana;
            _round_actionsPoints = _actionPoints;

            Events.onRoundStart.AddListener(OnRoundStart);
        }
        //private void InitHero(HeroEnum heroenum)
        //{
        //    _hero = Server._heroManager.GetHero(heroenum);
        //}

        public void SetHero(Heroo hero)
        {
            Console.WriteLine($"({_id}) has chosen hero {hero._name}");
            _hero = hero;
        }
        public void OnRoundStart(int clientID)
        {
            if (clientID != _id)
                return;

            Console.WriteLine("player: OnRoundStart()");
            
            _mana = _manaCap;
            _round_actionsPoints = _actionPoints;
            AddMoney(_moneyIncome);
            
            ServerSend.SendPlayerData(_id);
            GetTable().ResetDecksCost();

        }
        public void PrintData()
        {
            Console.WriteLine($"id: {_id}," +
                $" username: {_username}," +
                $" money: {_money}," +
                $" moneyIncome: {_moneyIncome}," +
                $" mana: {_mana}," +
                $" manaCap: {_manaCap}, " +
                $" actionPoints: {_actionPoints}, " +
                $" roundActionPoints: {_round_actionsPoints}, " +
                $" heroHealth: {_hero._hp}");
        }
        public Heroo GetHero()
        {
            return _hero;
        }
        public Hand GetHand()
        {
            return _hand;
        }
        public Battlefield GetBattlefield()
        {
            return _battlefield;
        }

        public Table GetTable()
        {
            return _table;
        }

        public void AddMana(int amount)
        {
            _mana += amount;
        }

        public int GetMana()
        {
            return _mana;
        }

        public void AddManaCap(int amount)
        {
            _manaCap += amount;
        }

        public int GetManaCap()
        {
            return _manaCap;
        }

        public void AddMoney(int amount)
        {
            _money += amount;
        }

        public int GetMoney()
        {
            return _money;
        }

        public void AddMoneyIncome(int amount)
        {
            _moneyIncome += amount;
        }

        public int GetMoneyIncome()
        {
            return _moneyIncome;
        }

        public int GetActionPoints()
        {
            return _actionPoints;
        }

        public int GetRoundActionPoints()
        {
            return _round_actionsPoints;
        }

        public void AddRoundActionPoints(int amount)
        {
            _round_actionsPoints += amount;
        }

        public int GetVictoryPoints()
        {
            return _victoryPoints;
        }

        public void SetVictoryPoints(int amount)
        {
            _victoryPoints = amount;
        }

        public void AddVictoryPoints(int amount)
        {
            _victoryPoints += amount;
        }

        /// <summary>
        /// Returns true if is able to reduce action points!
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ReduceRoundActionPoints(int amount)
        {
            if (GetRoundActionPoints() < amount)
            {
                Console.WriteLine($"Player (ID: {_username}) doesn't have enough actionPoints to buy from deck");
                return false;
            }
            AddRoundActionPoints(amount * -1);
            return true;
        }

        public bool isEnoughActionPoints(int amount)
        {
            return ( amount <= GetRoundActionPoints());
        }
    }
}
