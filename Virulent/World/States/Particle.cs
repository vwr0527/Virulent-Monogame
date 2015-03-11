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
    class Particle : State
    {
        Random rand;
        TimeSpan maxAge;

        public Particle()
        {
            rand = new Random();
            maxAge = new TimeSpan(0, 0, 10);
        }

        public override void InitEntity(Entity e)
        {
            e.sprite.col = new Color(255, 0, 0) * 0.0f;
        }

        public override void UpdateEntity(Entity e, GameTime gameTime, InputManager inputMan)
        {
            e.vel.X += 0.002f * (float)(rand.NextDouble() - 0.5) * (float)(gameTime.ElapsedGameTime.Milliseconds);
            e.vel.Y += 0.002f * (float)(rand.NextDouble() - 0.5) * (float)(gameTime.ElapsedGameTime.Milliseconds);
            e.vel *= 0.1f;
            e.pos += e.vel * (float)(gameTime.ElapsedGameTime.Milliseconds) * 0.001f;
            e.age += gameTime.ElapsedGameTime;
            float halfwayPeak = (float)(e.age.TotalMilliseconds / (maxAge.TotalMilliseconds/2));
            if (halfwayPeak > 1.0f) halfwayPeak = 1.0f - (halfwayPeak - 1.0f);
            float transparency = halfwayPeak;

            e.sprite.col = new Color(255, 0, 0) * transparency;
            if (e.age > maxAge) e.dead = true;
        }
    }
}
