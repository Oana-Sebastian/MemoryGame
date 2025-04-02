using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class User
    {
        public string Username { get; set; }
        public string ImagePath { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
    }
}
