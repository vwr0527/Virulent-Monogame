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
using Virulent.World;

namespace Virulent.Menu
{
    class MenuManager
    {
        private MenuPage rootMenu;
        private TitleMenu titleMenu;
        private MainMenu inGameMenu;
        private MenuPage currentMenu;
        private bool active = true;
        private bool quit = false;
        private bool save = false;

        private SpriteElement darken;
        private bool inTitle = true;

        public MenuManager()
        {
            titleMenu = new TitleMenu();
            inGameMenu = new MainMenu(titleMenu);
            rootMenu = titleMenu;
            currentMenu = rootMenu;
        }

        public void LoadContent(ContentManager content)
        {
            inGameMenu.LoadContent(content);
            titleMenu.LoadContent(content);

            darken = new SpriteElement(content.Load<Texture2D>("white"));
            darken.pos.X = 0.5f;
            darken.pos.Y = 0.5f;
            darken.col = new Color(0, 0, 0, 0.5f);
            darken.Scale = 100f;
        }

        public void Update(GameTime gameTime, InputManager inputMan, WorldManager worldMan)
        {
            if (currentMenu.SwitchingPages())
            {
                currentMenu = currentMenu.GetNextPage();
            }

            currentMenu.Update(gameTime, inputMan, worldMan);

            if (currentMenu.SaveGame())
            {
                save = true;
            }

            if (currentMenu.ExitMenu())
            {
                active = false;
            }

            if (currentMenu.ExitGame())
            {
                quit = true;
            }
        }

        public void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
            if (!inTitle) graphMan.DrawUISprite(darken);
            currentMenu.Draw(gameTime, graphMan);
        }

        public void ResetRoot()
        {
            currentMenu = rootMenu;
        }

        public bool IsActive()
        {
            return active;
        }

        public void Activate()
        {
            active = true;
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

        public bool QuitGame()
        {
            return quit;
        }

        public void SetInTitleScreen(bool isInTitleScreen)
        {
            inTitle = isInTitleScreen;
            if (inTitle)
            {
                rootMenu = titleMenu;
            }
            else
            {
                rootMenu = inGameMenu;
            }
        }
    }
}
