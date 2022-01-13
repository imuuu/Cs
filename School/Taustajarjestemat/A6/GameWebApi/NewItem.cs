
using System;

namespace GameWebApi
{
    public class NewItem
    {
        private int level;
        public int Level
        {
            get
            {
                return level;
            } 
            set
            {
                if((value > 0) && (value < 99))
                {
                    level = value;
                }
            }   
        }
        public ItemType itemType{get;set;}
        public DateTime CreationDate{get;set;}
    }
}