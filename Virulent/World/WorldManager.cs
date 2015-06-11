using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Virulent.Graphics;
using Virulent.Input;
using Virulent.World.Levels;
using Virulent.World.Collision;

namespace Virulent.World
{
    class WorldManager
    {
        private bool paused = false;
        private bool save = false;
        private bool demo = true;
        private bool init = true;
        private Level currentLevel;
        private Dictionary<String, Level> levels;
        private EntityManager entMan;
        private BlockManager blockMan;
        private CollisionManager collideMan;

        public WorldManager()
        {
            levels = new Dictionary<string, Level>();
            levels.Add("title", new TitleLevel());
            levels.Add("tutorial", new TutorialLevel());
            currentLevel = levels["title"];

            entMan = new EntityManager();
            blockMan = new BlockManager();
            collideMan = new CollisionManager();
        }

        //not quite done yet... need to load only the level we're currently on.
        public void LoadContent(ContentManager content)
        {
            foreach (Level level in levels.Values)
            {
                level.LoadContent(content);
            }
        }

        //
        public void Update(GameTime gameTime, InputManager inputMan)
        {
            if (init)
            {
                currentLevel.Init(gameTime);
                init = false;
            }

            while (currentLevel.BlockPending())
            {
                Block addedBlock = currentLevel.GetNextBlock();
                blockMan.AddBlock(addedBlock);

                if (addedBlock.GetCollider() != null)
                    collideMan.AddBlock(addedBlock);
            }

            while (currentLevel.EntityPending())
            {
                Entity addedEnt = currentLevel.GetNextEntity();
                entMan.AddEnt(addedEnt);
            }

            currentLevel.Update(gameTime, inputMan);
            entMan.Update(gameTime, inputMan, collideMan);
            //blockMan.Update(); //currently does nothing. here for consistancy. should collideman remember level blocks for more than a frame?
            collideMan.Update(gameTime);

            if (currentLevel.LevelEnded())
            {
                LoadLevel(currentLevel.GetNextLevel());
            }

			mousePos = inputMan.GetWorldMousePos ();
        }

        public void LoadLevel(String levelName)
        {
            if (levels.ContainsKey(levelName))
            {
                entMan.RemoveAllEnts();
                blockMan.RemoveAllBlocks();
                collideMan.RemoveAllBlocks();
                currentLevel = levels[levelName];
                init = true;
            }
        }

		Vector2 mousePos = new Vector2();

        public void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
			//Collider.DoTest(graphMan);
			graphMan.AddLine((-10 + mousePos.X),-10 + mousePos.Y,Color.White,10 + mousePos.X,10 + mousePos.Y,Color.Blue);
			graphMan.AddLine((10 + mousePos.X),-10 + mousePos.Y,Color.Red,-10 + mousePos.X,10 + mousePos.Y,Color.Green);
            currentLevel.Draw(gameTime, graphMan);
            entMan.Draw(gameTime, graphMan);
            blockMan.Draw(gameTime, graphMan);
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public bool IsPaused()
        {
            return paused;
        }

        public bool IsInTitleScreen()
        {
            return currentLevel == levels["title"];
        }

        public bool IsPlayingDemo()
        {
            return demo;
        }

        public bool SaveGame()
        {
            if (save)
            {
                save = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
