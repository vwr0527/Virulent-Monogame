using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Virulent.Graphics;
using Virulent.Input;

namespace Virulent.World.Levels
{
    class Level
    {
        public virtual void Init(GameTime gameTime)
        {
        }

        public virtual void LoadContent(ContentManager content)
        {
        }

        public void UnloadContent()
        {
        }

        public virtual void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
        }

        public virtual void Update(GameTime gameTime, InputManager inputMan)
        {
        }

        public virtual bool EntityPending()
        {
            return false;
        }

        public virtual Entity GetNextEntity()
        {
            return null;
        }

        public virtual void CatchPrevEntity(Entity actualSpawned)
        {
        }

        public virtual bool LevelEnded()
        {
            return false;
        }

        public virtual string GetNextLevel()
        {
            return null;
        }

        public virtual bool BlockPending()
        {
            return false;
        }

        public virtual Block GetNextBlock()
        {
            return null;
        }
    }
}
