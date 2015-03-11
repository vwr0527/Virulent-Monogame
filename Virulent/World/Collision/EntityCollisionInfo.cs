using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Virulent.World.Collision
{
    class EntityCollisionInfo
    {
        public Vector2 pushOut;
        public float collideTime;
        public Entity collideEnt;
        public Block collideBlock;

        public EntityCollisionInfo()
        {
            pushOut = new Vector2();
            collideTime = 1;
        }
        public static void CopyMethod(EntityCollisionInfo dst, EntityCollisionInfo src)
        {
            dst.pushOut = src.pushOut;
            dst.collideTime = src.collideTime;
            dst.collideBlock = src.collideBlock;
            dst.collideEnt = src.collideEnt;
        }
        public static EntityCollisionInfo CreateCopyMethod(EntityCollisionInfo src)
        {
            EntityCollisionInfo created = new EntityCollisionInfo();
            CopyMethod(created, src);
            return created;
        }
    }
}
