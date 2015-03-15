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
    class TutorialLevel : Level
    {
        SpriteElement bg;
        Block []brick;
        Entity e;
        TimeSpan respawnTime;
        TimeSpan prevSpawnTime;
        Random rand = new Random();

        int numPendingEntities = 1;
        int numPendingBlocks = 1;

        public override void Init(GameTime gameTime)
        {
            respawnTime = new TimeSpan(0, 0, 0, 100, 0);
            prevSpawnTime = gameTime.TotalGameTime - respawnTime;
            numPendingBlocks = 3;
        }

        public override void LoadContent(ContentManager content)
        {
            brick = new Block[3];
            brick[0] = new Block("platforms/platform1");
            brick[0].LoadContent(content);
            brick[0].SetPosition(0, 150);
            brick[0].SetScale(0.4f);
            brick[0].SetColor(new Color(1.0f, 0.1f, 0.0f));

            brick[1] = new Block("platforms/platform1");
            brick[1].LoadContent(content);
            brick[1].SetPosition(-100, 0);
            brick[1].SetScale(0.4f);
            brick[1].SetColor(new Color(0.0f, 0.5f, 1.0f));

            brick[2] = new Block("platforms/platform1");
            brick[2].LoadContent(content);
            brick[2].SetPosition(100, 0);
            brick[2].SetScale(0.4f);
            brick[2].SetColor(new Color(1.0f, 0.1f, 1.0f));

            bg = new SpriteElement(content.Load<Texture2D>("gradient"));
            bg.Scale = 5;
            bg.pos.X = 0.5f;
            bg.pos.Y = 0.5f;

            prevSpawnTime = new TimeSpan();

            e = new Entity();
            e.state = new Player();
            e.LoadContent(content);
        }

        public override void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
            //graphMan.GetCamera(1).rot = 0;
            graphMan.DrawWorldSprite(bg);
        }

        public override void Update(GameTime gameTime, InputManager inputMan)
        {
            //bg.rotation += gameTime.ElapsedGameTime.Milliseconds / 3000f;
            /*
            if (gameTime.TotalGameTime - prevSpawnTime >= respawnTime)
            {
                prevSpawnTime = gameTime.TotalGameTime;
                numPendingEntities += 1;
            }*/
        }

        public override bool EntityPending()
        {
            return numPendingEntities > 0;
        }

        public override Entity GetNextEntity()
        {
            --numPendingEntities;
            return e;
        }

        public override bool BlockPending()
        {
            return numPendingBlocks > 0;
        }

        public override Block GetNextBlock()
        {
            --numPendingBlocks;
            return brick[numPendingBlocks];
        }
    }
}
