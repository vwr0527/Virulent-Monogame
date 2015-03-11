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
    class GearDisk : State
    {
        public GearDisk()
        {
        }

        public override void LoadEntityContent(Entity e, ContentManager content)
        {
            e.sprite = new SpriteElement(content.Load<Texture2D>("geardisk"));
        }

        public override void InitEntity(Entity e)
        {
        }

        public override void UpdateEntity(Entity e, GameTime gameTime, InputManager inputMan)
        {
            e.rot += e.rotvel * gameTime.ElapsedGameTime.Milliseconds;
            e.age += gameTime.ElapsedGameTime;

            e.sprite.rotation = e.rot;

            float thing = (e.vel.Y*10000f) + (float)e.age.TotalMilliseconds * e.vel.Y;
            while (thing >= 1.0f) thing -= 1.0f;
            float r = 1.0f - (3f*Math.Abs(0.3333f - thing));
            float g = 1.0f - (3f*Math.Abs(0.6667f - thing));
            float b = 1.0f - (3f*Math.Abs(1f - thing));

            if (thing < 0.3333)
            {
                g = 0;
                b = 1.0f - (thing * 3f);
            }
            else if (thing < 0.6667)
            {
                b = 0;
            }
            else if (thing < 1)
            {
                r = 0;
            }

            e.sprite.col = new Color(r, g, b) * e.vel.X;
        }
    }
}
