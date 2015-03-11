using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virulent.World.Collision
{
    class Square
    {
        public int row = 0;
        public int col = 0;
        public List<Entity> ents;
        public List<Block> blocks;

        public Square()
        {
            ents = new List<Entity>();
            blocks = new List<Block>();
        }
    }
}
