using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Virulent.World.Collision
{
    //each square has blocklist and entlist
    //each ent has squarelist
    class CollisionManager
    {
        SquareManager sqrMan;
        RecycleArray<EntitySquares> entsqrs;
        EntitySquares addedEntSqrTemp;

        List<Entity> collideAgainstEnts;
        List<Block> collideAgainstBlocks;
        RecycleArray<EntityCollisionInfo> collideEntsInfo;
        RecycleArray<EntityCollisionInfo> collideBlocksInfo;
        EntityCollisionInfo addedECITemp;

        List<Collider> colliderList;

        public CollisionManager()
        {
            sqrMan = new SquareManager();
            entsqrs = new RecycleArray<EntitySquares>(EntitySquares.CopyMethod, EntitySquares.CreateCopyMethod);
            entsqrs.SetDataMode(false);
            addedEntSqrTemp = new EntitySquares();
            addedECITemp = new EntityCollisionInfo();

            collideAgainstEnts = new List<Entity>();
            collideAgainstBlocks = new List<Block>();

            collideEntsInfo = new RecycleArray<EntityCollisionInfo>(EntityCollisionInfo.CopyMethod, EntityCollisionInfo.CreateCopyMethod);
            collideEntsInfo.SetDataMode(false);
            collideBlocksInfo = new RecycleArray<EntityCollisionInfo>(EntityCollisionInfo.CopyMethod, EntityCollisionInfo.CreateCopyMethod);
            collideBlocksInfo.SetDataMode(false);

            colliderList = new List<Collider>();
        }

        //only called on worldmanager loadlevel()
        public void RemoveAllBlocks()
        {
            sqrMan.RemoveAllBlocks();
            for (int i = 0; i < entsqrs.Size(); ++i)
            {
                entsqrs.ElementAt(i).squares.Clear();
                entsqrs.ElementAt(i).squares.TrimExcess();
            }
        }

        public void AddBlock(Block addedBlock)
        {
            sqrMan.AddBlock(addedBlock);
        }

        public void AddEnt(Entity addedEntity)
        {
            addedEntSqrTemp.entity = addedEntity;
            //adds an entity to the appropriate Square(s)
            sqrMan.AddEntity(entsqrs.Add(addedEntSqrTemp));
        }

        public void Update(GameTime gameTime)
        {
            //entities have been added,
            //blocks and entities are already inside squares.
            //collide all entities with blocks within their squares.
            //  slide along surfaces
            //  second surface hit = no slide, sticky bounce
            //collide all entities with entities within their squares.
            //  naive collisions = gather all likely collisions
            //  eliminate collisions that won't happen (a->b, b->c)

            //  For each entity
            for(int i = 0, maxEnts = entsqrs.Size(); i < maxEnts; ++i)
            {
                EntitySquares e = entsqrs.ElementAt(i);

                //  Add other entities and block occupying the same square to "collideAgainst..." lists
                AddToCollideAgainst(e);

                //Calculate the EntityCollisionInfos for the things you added
                DoCollisions(e);
                //Apply the collision calculations
            	//if(collideBlocksInfo.Size() > 1) System.Console.WriteLine(collideBlocksInfo.Size());
                for (int j = collideBlocksInfo.Size() - 1; j >= 0; --j)
                {
                    EntityCollisionInfo b = collideBlocksInfo.ElementAt(j);
					e.entity.CollideBlock(b.collideBlock, b.collisionInfo);
                }
                for (int j = collideEntsInfo.Size() - 1; j >= 0; --j)
                {
                    EntityCollisionInfo e2 = collideEntsInfo.ElementAt(j);
					e.entity.CollideEntity(e2.collideEnt, e2.collisionInfo);
                }

                //ready for the next entity to use
                collideAgainstEnts.Clear();
                collideAgainstBlocks.Clear();
                collideEntsInfo.EmptyAll();
                collideBlocksInfo.EmptyAll();
            }

            //Finished collision detection for all entities.
            //reset the Entity-Squares temporary holder.
            entsqrs.EmptyAll();
        }


        private void DeleteAllEnts()
        {
            entsqrs.DeleteAll();
            collideAgainstBlocks.Clear();
            collideAgainstBlocks.TrimExcess();
            collideAgainstEnts.Clear();
            collideAgainstEnts.TrimExcess();
            collideEntsInfo.DeleteAll();
            collideBlocksInfo.DeleteAll();
        }

        //////////////////////////////////////////////////////
        //  Add other entities and block occupying the same square
        //////////////////////////////////////////////////////
        private void AddToCollideAgainst(EntitySquares e) 
        {
            //
            //  for each square touching to the entity...
            //
            for (int j = 0, maxSqrs = e.squares.Count; j < maxSqrs; ++j)
            {
                Square sqr = e.squares.ElementAt(j);

                //first square doesnt need to check for duplicates
                //any more squares than 1, it needs to check for duplicates

                //          First square
                if (j == 0)
                {
                    foreach (Block b in sqr.blocks)
                    {
                        collideAgainstBlocks.Add(b);
                    }
                    foreach (Entity collideEntity in sqr.ents)
                    {
                        collideAgainstEnts.Add(collideEntity);
                    }
                }
                else
                //      All squares after the first
                {
                    foreach (Block b in sqr.blocks)
                    {
                        //add if not duplicate
                        if (!collideAgainstBlocks.Contains(b))
                            collideAgainstBlocks.Add(b);
                    }
                    foreach (Entity collideEntity in sqr.ents)
                    {
                        //add if not duplicate
                        if (!collideAgainstEnts.Contains(collideEntity))
                            collideAgainstEnts.Add(collideEntity);
                    }
                }


                //remove references to itself from the squares it references.
                //this prevents collisions from being performed twice, once when a->b, another when b->a
                sqr.ents.Remove(e.entity);
            }
        }

		//collides e with all the entities and blocks in the "to collide" lists (collideAgainstEnts, collideAgainstBlocks), storing the results in collideEntsInfo and collideBlocksInfo
        private void DoCollisions(EntitySquares e)
        {
            Collider entityCollider = e.entity.GetCollider();
            foreach (Entity e2 in collideAgainstEnts)
            {
				Collider.CollisionInfo collideInfo = e2.GetCollider().DoCollide(entityCollider);
                if (EntityEntityCollision(e2.GetCollider(), entityCollider))
                {
					addedECITemp.collisionInfo = collideInfo;
                    addedECITemp.collideEnt = e2;
                    collideEntsInfo.Add(addedECITemp);
                    addedECITemp.collideBlock = null;
					addedECITemp.collideEnt = null;
                }
            }
            entityCollider = e.entity.GetCollider(); //unknown why this fixes weird bug
            foreach (Block b in collideAgainstBlocks)
            {
				Collider.CollisionInfo collideInfo = b.GetCollider().DoCollide(entityCollider);
				if (collideInfo.didCollide && collideInfo.collideTime < 1)
                {
					addedECITemp.collisionInfo = collideInfo;
                    addedECITemp.collideBlock = b;
                    collideBlocksInfo.Add(addedECITemp);
                    addedECITemp.collideBlock = null;
                    addedECITemp.collideEnt = null;
                }
            }
        }

        private bool EntityEntityCollision(Collider otherCollider, Collider entityCollider)
        {
            return false;// otherCollider.DoCollide(entityCollider);
        }
    }
}
