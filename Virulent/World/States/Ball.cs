using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virulent.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using Virulent.Input;

namespace Virulent.World.States
{
    class Ball : State
    {
        Random rand;
        TimeSpan maxAge;

        public Ball()
        {
            rand = new Random();
            maxAge = new TimeSpan(0, 0, 30);
        }

        public override void InitEntity(Entity e)
        {
            e.sprite.col = new Color(0, 255, 0);
            e.sprite.Scale = 1.5f;
        }

        public override void UpdateEntity(Entity e, GameTime gameTime, InputManager inputMan)
        {
            e.vel.X += 0.002f * (float)(rand.NextDouble() - 0.5) * (float)(gameTime.ElapsedGameTime.Milliseconds);
            e.vel.Y += 0.005f * (float)(gameTime.ElapsedGameTime.Milliseconds);
            e.pos += e.vel * (float)(gameTime.ElapsedGameTime.Milliseconds) * 0.1f;
            if (e.pos.Y > 200.0f)
            {
                e.pos.Y = 200.0f;
                e.vel.Y *= -0.99f;
            }
            e.age += gameTime.ElapsedGameTime;
            e.sprite.col = new Color(0, 255, 0);
            e.sprite.Scale = 1.5f;
            if (e.age > maxAge) e.dead = true;
        }
    }
}
