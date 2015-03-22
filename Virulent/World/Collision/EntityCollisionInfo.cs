using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Virulent.World.Collision
{
    class EntityCollisionInfo
    {
		public Collider.CollisionInfo collisionInfo;
        public Entity collideEnt;
        public Block collideBlock;

        public EntityCollisionInfo()
        {
			collisionInfo = new Collider.CollisionInfo();
			collisionInfo.collideTime = 1;
        }
        public static void CopyMethod(EntityCollisionInfo dst, EntityCollisionInfo src)
        {
			dst.collisionInfo = src.collisionInfo;
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
