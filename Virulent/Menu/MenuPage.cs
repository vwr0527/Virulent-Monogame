using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Virulent.Input;
using Virulent.Graphics;
using Virulent.World;

namespace Virulent.Menu
{
    class MenuPage
    {
        protected bool exit = false;
        protected bool save = false;
        protected bool exitmenu = false;
        protected bool switching = false;

        public virtual void LoadContent(ContentManager content)
        {
        }

        public virtual void Update(GameTime gameTime, InputManager inputMan, WorldManager worldMan)
        {
        }

        public virtual void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
        }

        public virtual bool SwitchingPages()
        {
            if (switching == true)
            {
                switching = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual MenuPage GetNextPage()
        {
            return null;
        }

        public virtual bool ExitMenu()
        {
            if (exitmenu == true)
            {
                exitmenu = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool SaveGame()
        {
            if (save == true)
            {
                save = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool ExitGame()
        {
            if (exit == true)
            {
                exit = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
