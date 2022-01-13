using System;
using System.Collections.Generic;
using System.Text;
using GameServer.Enums;
using GameServer.Cards;
using GameServer.Cards.Spells;
namespace GameServer.Managers
{
    public class CardCollectionManager
    {
        private Card[] _allCards;

        public CardCollectionManager()
        {
            InitCards();
        }

        private void InitCards()
        {

            List<Card> allCards = new List<Card>();
            //Card car = new Card(id, name, desct, attack, def, goldcost, manacost, type);

            //Card car = new Card(id, name, desct, attack, def, goldcost, manacost, type);

            allCards.Add(new MonsterCard(0, "NULL", "No desc",0, 0, 0).BelongsTo(HeroEnum.NONE));
            //allCards.Add(new MonsterCard(1, "Captain", "All other SOLDIER units gets +1 attack", 2, 3, 5));
            ////_allCards[1].SetBuff(new BuffFaction(BuffType.GIVE_ATTACK, Pattern.AROUND_CARDS, BuffState.TEMP, CardFaction.DARK,BuffTriggerType.ON_PLAY, false,50));
            //allCards.Add(new MonsterCard(2, "Veteran", "Rush, When Veteran dies, summon a Recruit", 5, 2, 3));
            //allCards.Add(new MonsterCard(3, "Private", "Rush", 3, 1, 2));
            //((IBuffCard)allCards[3]).SetBuff(new Buff(BuffType.GIVE_OR_TAKE_HEALTH, Pattern.AROUND_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 10));

            //// =======> new Buff()
            //// =======> new BuffChosenCard()           
            //// =======> new BuffRound() 
            //// =======> new BuffChosenRoundCard()

            //Basic cards for general, 2 x each in draw deck.
            allCards.Add(new MonsterCard(1, "Recruit", "-", 2, 1, 1)); //
            //((IBuffCard)allCards[1]).AddBuff(new Buff(BuffType.GIVE_OR_TAKE_ATTACK, Pattern.AROUND_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 5));
            allCards.Add(new MonsterCard(2, "Private", "-", 3, 2, 2));
            //((IBuffCard)allCards[2]).AddBuff(new Buff(BuffType.GIVE_OR_TAKE_ATTACK, Pattern.AROUND_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 5));
            allCards.Add(new MonsterCard(3, "Soldier", "-", 4, 2, 3));
            //((IBuffCard)allCards[3]).AddBuff(new Buff(BuffType.GIVE_OR_TAKE_ATTACK, Pattern.AROUND_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 5));
            allCards.Add(new MonsterCard(4, "Volunteer", "Give 2 chosen attack buff of 5", 2, 3, 2));
            ((IBuffCard)allCards[4]).AddBuff(new BuffChosenCard(2,BuffType.GIVE_OR_TAKE_ATTACK, Pattern.AROUND_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 5));
            allCards.Add(new MonsterCard(5, "Grunt", "-", 5, 3, 3));
            allCards.Add(new MonsterCard(6, "Jeep", "-", 2, 4, 3));
            allCards.Add(new MonsterCard(7, "Assault Vehicle", "-", 5, 2, 4));
            allCards.Add(new MonsterCard(8, "Defence Vehicle", "Gives 2 health sides cards", 2, 5, 5));
            ((IBuffCard)allCards[8]).AddBuff(new Buff(BuffType.GIVE_OR_TAKE_HEALTH, Pattern.SIDE_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 2));
            allCards.Add(new MonsterCard(9, "Support Heli", "Gives 2 attack around the card", 4, 1, 2));
            ((IBuffCard)allCards[9]).AddBuff(new Buff(BuffType.GIVE_OR_TAKE_ATTACK, Pattern.AROUND_CARDS, BuffState.PERM, BuffTriggerType.ON_PLAY, false, 2));
            
            allCards.Add(new MonsterCard(10, "Airship", "-", 6, 3, 5).SetCardType(CardType.VEHICLE));

            //Basic cards for admiral, 2 x each in draw deck.
            allCards.Add(new MonsterCard(11, "Scrap Mech", "-", 1, 2, 1).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(12, "Basic Mech", "-", 2, 3, 2).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(13, "Defender Mech", "-", 0, 4, 3).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(14, "Assault Mech", "-", 5, 1, 3).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(15, "Guard Mech", "-", 4, 4, 4).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(16, "Spaceship", "-", 3, 3, 3).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(17, "Cannon Ship", "-", 5, 3, 5).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(18, "Scout Ship", "-", 1, 4, 3).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(19, "Old Ship", "-", 3, 2, 3).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(20, "Basic Ship", "-", 5, 5, 5).SetCardType(CardType.SHIP));

            //General buy deck low, 4x each card in deck.
            allCards.Add(new MonsterCard(21, "Assault Squad", "Rush", 3, 2, 2).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(22, "Defensive Squad", "Gain +1 defence for each friendly unit", 1, 3, 2).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(23, "Fresh Recruits", "Rush", 3, 3, 3).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(24, "Car", "-", 2, 4, 3).SetCardType(CardType.VEHICLE));

            //General buy deck low legendaries, 1 x each in deck.
            allCards.Add(new MonsterCard(25, "Spec ops", "Can't take damage from abilities or spells", 3, 3, 3).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(26, "Sniper", "Can attack any target", 4, 1, 3).SetCardType(CardType.SOLDIER));

            //General buy deck med, 4x each card in deck.
            allCards.Add(new MonsterCard(27, "Veteran", "Rush, when Veteran dies summon a Recruit", 4, 3, 5).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(28, "Troop Transport", "All soldiers have RUSH", 2, 5, 4).SetCardType(CardType.VEHICLE));
            allCards.Add(new MonsterCard(29, "Medic", "Heal 2 damage to any SOLDIER", 2, 4, 4).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(30, "Artillery", "Deal 2 damage to adjacent targets when attacking", 5, 2, 5).SetCardType(CardType.VEHICLE));

            //General buy deck med legendaries, 1 x each in deck.
            allCards.Add(new MonsterCard(31, "Generals bodyguard", "General can't take dmg", 4, 4, 5).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(32, "Attack Helicopter", "Deal 5 damage to one target", 6, 1, 5).SetCardType(CardType.VEHICLE));

            //General buy deck high, 4x each card in deck.
            allCards.Add(new MonsterCard(33, "Machine Gunner", "Rush", 4, 4, 7).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(34, "Commander", "Adjacent units get +1", 3, 3, 7).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(35, "Heavy Tank", "-", 7, 7, 8).SetCardType(CardType.VEHICLE));
            allCards.Add(new MonsterCard(36, "Field Command", "All units gain +3 attack", 3, 4, 8).SetCardType(CardType.VEHICLE));

            //General buy deck high legendaries, 1 x each in deck.
            allCards.Add(new MonsterCard(37, "Liutenant", "All units attack every turn, all units get +2 attack when attacking", 3, 5, 8).SetCardType(CardType.SOLDIER));
            allCards.Add(new MonsterCard(38, "Battle Engineer", "All units are considered Front Line", 5, 5, 7).SetCardType(CardType.SOLDIER));

            //Admiral buy deck low, 3x each card in deck.
            allCards.Add(new MonsterCard(39, "Corvette", "Gets +1 to attack and defence each turn", 1, 1, 1).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(40, "Destroyer", "-", 3, 3, 2).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(41, "Private", "Rush", 3, 1, 1).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(42, "Private", "Rush", 3, 1, 1).SetCardType(CardType.MECHA));

            //Admiral buy deck low legendaries, 1 x each in deck.
            allCards.Add(new MonsterCard(43, "Titan Mech", "Titan can attack twice", 3, 4, 3).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(44, "Mothership", "Heals every SHIP unit 2 health", 3, 3, 3).SetCardType(CardType.SHIP));

            //Admiral buy deck med, 3x each card in deck.
            allCards.Add(new MonsterCard(45, "Cheap Ship", "-", 5, 4, 5).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(46, "Scrap Ship", "Rush", 6, 2, 4).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(47, "Jager Mech", "Jager is immune to spells and abilities", 4, 4, 5).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(48, "Assault Mech", "When Assault Mech enters the battlefield, deal 2 damage to one battleline.", 4, 2, 6).SetCardType(CardType.MECHA));

            //Admiral buy deck medw legendaries, 1 x each in deck.
            allCards.Add(new MonsterCard(49, "Space Fortress", "Can't attack, All other units can take one hit without taking damage.", 5, 5, 6).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(50, "Juggernaut Mech", "Juggernaut blocks all attacks to backline if Juggernaut is on Frontline", 2, 8, 6).SetCardType(CardType.MECHA));

            //Admiral buy deck high, 3x each card in deck.
            allCards.Add(new MonsterCard(51, "DD6000", "When DD6000 enters the battlefield, Destroy one enemy unit.", 4, 5, 8).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(52, "Captial Ship", "Capital Ship has double stats when alone on board", 6, 6, 9).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(53, "Heavy Mech", "Rush", 7, 7, 7).SetCardType(CardType.MECHA));
            allCards.Add(new MonsterCard(54, "Mech Ship", "also is a SHIP", 8, 9, 9).SetCardType(CardType.MECHA));

            //Admiral buy deck high legendaries, 1 x each in deck.
            allCards.Add(new MonsterCard(55, "Engineer Ship", "Other units cost 2 mana less", 5, 5, 7).SetCardType(CardType.SHIP));
            allCards.Add(new MonsterCard(56, "Colossus Mech", "Cannot attack enemy leader", 15, 15, 8).SetCardType(CardType.MECHA));

            allCards.Add(new VictoryCard(57, "Victory Card", "In used gives 1 extra action point and 300 gold", 1, 1, 300).BelongsTo(HeroEnum.NONE));
            allCards.Add(new FireSpell  (58, "Fire Spell", "Deal Damage to enemies 6",3, 6).BelongsTo(HeroEnum.NONE));
           
            _allCards = allCards.ToArray();


        }

        public Card GetCard(int id)
        {
            Card card = _allCards[id].Clone(_allCards[id].GetNewInstance());
            if (_allCards[id] is IBuffCard)
            {
                //Console.WriteLine("Getting buffs for card");
                ((IBuffCard)card).SetAllSettedBuffs(((IBuffCard)_allCards[id]).GetAllBuffs());
            }
            return card;
        }

        public Card[] GetHerosCards(Heroo hero)
        {
            List<Card> cards = new List<Card>();
            foreach(Card card in _allCards)
            {
                if(card.HasHerosCard(hero))
                {
                    cards.Add(GetCard(card._id));
                }
            }

            return cards.ToArray();
        }

        public Card[] GetAllCards()
        {
            return _allCards;
        }

        public int GetTotalCards()
        {
            return _allCards.Length;
        }
    }
}
