using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Virulent.Input;
using Virulent.Graphics;
using Virulent.World.Collision;
using Virulent.World.States.Animations;

namespace Virulent.World
{
    class EntityManager
    {
        //this should not manage spriteelements like this
        RecycleArray<Entity> entList;
        RecycleArray<SpriteElement> spriteList;

        public EntityManager()
        {
            entList = new RecycleArray<Entity>(Entity.CopyMembers, Entity.CreateCopy);
            spriteList = new RecycleArray<SpriteElement>(SpriteElement.CopyMembers, SpriteElement.CreateCopy);
            entList.SetDataMode(false);
            spriteList.SetDataMode(false);
        }

        //Also communicates latest entity info to collisionMan
        public void Update(GameTime gameTime, InputManager inputMan, CollisionManager collisionMan)
        {
            for (int i = 0; i < entList.Capacity(); ++i)
            {
                Entity cur = entList.ElementAt(i);
                if (cur.dead)
                {
                    ///////////why is sprite stuff here? ///////////////
                    recursiveDeleteSprite(cur.sprite);
                    cur.sprite = null;

                    entList.EmptyElementAt(i);
                }
                else
                {
                    cur.Update(gameTime, inputMan);
                    if (cur.GetCollider() != null)
                        collisionMan.AddEnt(cur);
                }
            }
            Pose.RunEditor(inputMan);
        }

        private void recursiveDeleteSprite(SpriteElement spriteElement)
        {
            if (spriteElement != null)
            {
                recursiveDeleteSprite(spriteElement.linkedSprite);
                spriteList.EmptyElement(spriteElement);
            }
        }

        public void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
            for (int i = 0; i < entList.Capacity(); ++i)
            {
                Entity cur = entList.ElementAt(i);
                if (!cur.dead)
                {
                    cur.Draw(gameTime, graphMan);
                }
            }
            Pose.DrawEditor(graphMan);
        }

        public Entity AddEnt(Entity entityToAdd)
        {
            Entity added = entList.Add(entityToAdd);
            SpriteElement b = entityToAdd.sprite;
            SpriteElement a;
            if (b != null)
            {
                added.sprite = spriteList.Add(b);
                a = added.sprite;
                while (b.linkedSprite != null)
                {
                    a.linkedSprite = spriteList.Add(b.linkedSprite);
                    b = b.linkedSprite;
                    a = a.linkedSprite;
                }
                a.linkedSprite = null;
            }
            added.Init();
            return added;
        }

        public void RemoveAllEnts()
        {
            for (int i = 0; i < entList.Capacity(); ++i)
            {
                entList.ElementAt(i).dead = true;
            }
            spriteList.EmptyAll();
        }

        public void DeleteAll()
        {
            entList.DeleteAll();
            spriteList.DeleteAll();
        }
    }
}
