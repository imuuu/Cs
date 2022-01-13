using System.Collections.Generic;
using System.Linq;

namespace A2
{
    public class Game<T> where T : IPlayer
{
    private List<T> _players;

    public Game(List<T> players) 
    {
        _players = players;
    }

    public T[] GetTop10Players() 
    {
        // ... write code that returns 10 players with highest scores
        List<T> top10=new List<T>();
        _players.OrderByDescending(x => x.Score);
        for(int i= 0 ; i < 10; i++)
        {
            top10[i]=_players[i];
        }
        return top10.ToArray();
    }
}
}
