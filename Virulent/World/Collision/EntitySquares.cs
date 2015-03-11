using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Virulent.World.Collision
{
    //keeps track of which squares each entity is in
    //and holds on to a collider that is associated
    //with the same entity.
    class EntitySquares
    {
        public List<Square> squares;
        public Entity entity;

        public EntitySquares()
        {
            squares = new List<Square>();
        }
        public static void CopyMethod(EntitySquares dst, EntitySquares src)
        {
            dst.squares.Clear();
            //Won't ever use this, because CopyMethod will only be used for AddEnt, and it will never start with any squares.
            //for (int i = 0, max = src.squares.Count; i < max; ++i)
            //{
            //    dst.squares.Add(src.squares[i]);
            //}
            dst.entity = src.entity;
        }
        public static EntitySquares CreateCopyMethod(EntitySquares src)
        {
            EntitySquares created = new EntitySquares();
            CopyMethod(created, src);
            return created;
        }
    }
}
