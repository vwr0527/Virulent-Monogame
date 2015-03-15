using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

using Virulent.Graphics;
using Virulent.Input;

namespace Virulent.Cinematic
{
    class CinematicManager
    {
        SpriteElement bg;
        SpriteElement thing;
        bool active = true;

        public void LoadContent(ContentManager content)
        {
            bg = new SpriteElement(content.Load<Texture2D>("placeholder_cinematic_background"));
            bg.pos.X = 0.5f;
            bg.pos.Y = 0.5f;
			bg.Scale = 0.65f;
            bg.col = new Color(0, 0, 0);
            thing = new SpriteElement(content.Load<Texture2D>("placeholder_button_test"));
            thing.pos.X = -0.5f;
            thing.pos.Y = 0.5f;
			thing.Scale = 0.5f;
        }

        float speed = 1.0f;
        float val = 0.0f;

        public void Update(GameTime gameTime, InputManager input)
        {
            if (speed < 0.001f)
                speed = 0;
            else
                speed *= 0.95f;
            val = 1.0f - speed;
            thing.pos.X = val - 0.5f;
            bg.col = new Color(val, val, val);

            if (gameTime.TotalGameTime.Seconds > 3)
            {
                float fadeout = 1.0f - (((float)gameTime.TotalGameTime.TotalMilliseconds - 4000f) / 2000f);
                bg.col = new Color(fadeout, fadeout, fadeout);
                thing.col = new Color(fadeout, fadeout, fadeout);
            }
            if (gameTime.TotalGameTime.Seconds > 5) active = false;
        }

        public void Draw(GameTime gameTime, GraphicsManager graphics)
        {
            graphics.DrawUISprite(bg);
            graphics.DrawUISprite(thing);
        }

        public bool IsActive()
        {
            return active;
        }

        public void Deactivate()
        {
            active = false;
        }
    }
}
