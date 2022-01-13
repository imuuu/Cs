namespace GameServer
{
    public interface IMonsterCard
    {
        int _attack { get; set; }
        int _defence { get; set; }
        
   
        public int TakeOrAddHealth(int amount);
        public int TakeOrAddAttack(int amount);
    }
}