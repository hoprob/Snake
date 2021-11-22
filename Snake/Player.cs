using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Player
    {
        public string name { get; }
        public int score { get; }
        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
        public string Info()
        {
            return $"{name}  -  {score}";
        }
    }
}
