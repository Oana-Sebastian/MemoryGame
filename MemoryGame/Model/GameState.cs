using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class GameState
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string SelectedCategory { get; set; }
        public double TimeLeftSeconds { get; set; }
        public List<CardState> Cards { get; set; }
    }
}
