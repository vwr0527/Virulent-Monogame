using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Virulent.World.Collision
{

    class SquareManager
    {
        public List<Square> squares;
        public Vector2 topLeft;
        public Vector2 size;
        public int columns;
        public int rows;

        public SquareManager()
        {
            columns = 1;
            rows = 1;
            squares = new List<Square>();
            squares.Add(new Square());
            topLeft = new Vector2(-1, -1);
            size = new Vector2(2, 2);
        }

        public void RemoveAllBlocks()
        {
            foreach (Square s in squares)
            {
                s.blocks.Clear();
                s.blocks.TrimExcess();
            }
        }

        // TODO:
        public void AddBlock(Block addedBlock)
        {
            Square square = squares.ElementAt(0);
            if (square != null)
            {
                square.blocks.Add(addedBlock);
            }
        }

        // TODO:
        public void AddEntity(EntitySquares addedEntitySquare)
        {
            Square square = squares.ElementAt(0);
            if (square != null)
            {
                square.ents.Add(addedEntitySquare.entity);
                addedEntitySquare.squares.Add(square);
            }
        }
    }
}
