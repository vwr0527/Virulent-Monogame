using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Virulent.Graphics;
using Virulent.Input;
using Virulent.World.States;

namespace Virulent.World.Levels
{
    class TitleLevel : Level
    {
        SpriteElement bg;
        SpriteElement grid;
        Random rand = new Random();
        int numPendingEntities = 0;

        Entity[] rings = new Entity[10];

        public override void Init(GameTime gameTime)
        {
            numPendingEntities = 10;
        }

        public override bool EntityPending()
        {
            return numPendingEntities != 0;
        }

        public override void LoadContent(ContentManager content)
        {
            bg = new SpriteElement(content.Load<Texture2D>("gradient"));
            bg.Scale = 5;
            bg.pos.X = 0f;
            bg.pos.Y = 0f;
            bg.col *= 0.4f;

            grid = new SpriteElement(content.Load<Texture2D>("grid"));
            grid.Scale = 1.2f;
            grid.col = new Color(0, 200, 100) * 0.5f;

            rings = new Entity[10];
            //spawnMan.AddLoadState(new GearDisk());
            //spawnMan.LoadContent(content);
            for (int i = 0; i < 10; ++i)
            {
                /*
                int index = spawnMan.CreateSpawn("geardisk", new Vector2(0, 0),
                    new Vector2((((float)(11 - i)) * 0.1f), (float)(rand.NextDouble() * 0.0005)),
                    0, 0.00025f - (float)(rand.NextDouble() * 0.0005));
                SpriteElement se = spawnMan.GetSpriteAt(index);
                se.col = new Color(255, 0, 0) * (((float)(11 - i)) * 0.1f);
                se.scale = ((float)(11 - i)) * 0.1f;
                 * */
                rings[i] = new Entity();
                rings[i].state = new GearDisk();
                rings[i].sprite = new SpriteElement(content.Load<Texture2D>("geardisk"));
                rings[i].sprite.Scale = ((float)(11 - i)) * 0.1f;
                rings[i].rotvel = 0.00025f - (float)(rand.NextDouble() * 0.0005);
                rings[i].sprite.col = new Color(255,0,0) * (((float)(11 - i)) * 0.1f);
                rings[i].vel.X = (((float)(11 - i)) * 0.1f);
                rings[i].vel.Y = (float)(rand.NextDouble() * 0.0005);
                ++numPendingEntities;
            }
            //entities in levels are merely references to be copied.
        }

        public override void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
            Camera cam1 = graphMan.GetCamera(1);
            cam1.scale = 1.3f + ((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 10000.0)) * 0.15f;
            //cam1.rot += 0.000016f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            graphMan.DrawWorldSprite(bg);
            graphMan.DrawWorldSprite(grid);
        }

        public override void Update(GameTime gameTime, InputManager inputMan)
        {
            bg.rotation += gameTime.ElapsedGameTime.Milliseconds / 10000f;
        }

        public override void CatchPrevEntity(Entity actualSpawned)
        {
        }

        public override Entity GetNextEntity()
        {
            --numPendingEntities;
            return rings[numPendingEntities];
        }
    }
}
