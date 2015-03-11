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
using Virulent.World;

namespace Virulent.Menu
{
    class TitleMenu : MenuPage
    {
        private SpriteElement title;
        private SpriteElement el_newgame;
        private SpriteElement el_options;
        private SpriteElement el_quit;
        private SpriteElement cursor;
        private int cursorpos;

        private StartGamePage startGamePage;
        private MenuPage nextPage;

        public override void LoadContent(ContentManager content)
        {
            SpriteFont font = content.Load<SpriteFont>("large9");
            SpriteFont titlefont = content.Load<SpriteFont>("Abstract");
            title = new SpriteElement(new StringBuilder("Virulent"), titlefont);
            el_newgame = new SpriteElement(new StringBuilder("Start Game"), font);
            el_options = new SpriteElement(new StringBuilder("Options"), font);
            el_quit = new SpriteElement(new StringBuilder("Quit Game"), font);
            cursor = new SpriteElement(content.Load<Texture2D>("cursor"));
            title.pos.X = 0.5f;
            title.scale = 0.5f;
            title.pos.Y = 0.42f;
            el_newgame.pos.X = 0.5f;
            el_newgame.scale = 0.5f;
            el_newgame.pos.Y = 0.55f;
            el_options.pos.X = 0.5f;
            el_options.scale = 0.5f;
            el_options.pos.Y = 0.6f;
            el_quit.pos.X = 0.5f;
            el_quit.scale = 0.5f;
            el_quit.pos.Y = 0.65f;
            cursor.pos.X = 0.35f;
            cursor.scale = 0.5f;
            cursor.pos.Y = 0.4f;
            cursor.scale = 0.5f;
            cursorpos = 0;

            startGamePage = new StartGamePage(this);
            startGamePage.LoadContent(content);
        }

        public override void Update(GameTime gameTime, InputManager inputMan, WorldManager worldMan)
        {
            //title.pos.Y = 0.4f + ((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 300.0) * 0.01f);

            //if (inputMan.StartPressed()) exitmenu = true;
            if (inputMan.DownPressed()) cursorpos += 1;
            if (inputMan.UpPressed()) cursorpos -= 1;
            if (cursorpos > 2) cursorpos = 0;
            if (cursorpos < 0) cursorpos = 2;

            cursor.pos.Y = 0.55f + ((float)cursorpos) * 0.05f;
            cursor.pos.X = 0.35f + ((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 100.0) * 0.005f);

            el_newgame.scale = 0.5f;
            el_options.scale = 0.5f;
            el_quit.scale = 0.5f;

            if (cursorpos == 0)
            {
                el_newgame.scale = 0.6f;
                if (inputMan.EnterPressed())
                {
                    switching = true;
                    nextPage = startGamePage;
                }
            }
            else if (cursorpos == 1)
            {
                el_options.scale = 0.6f;
                if (inputMan.EnterPressed())
                {
                    //switching = true;
                }
            }
            else if (cursorpos == 2)
            {
                el_quit.scale = 0.6f;
                if (inputMan.EnterPressed())
                {
                    exit = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
            graphMan.DrawUISprite(title);
            graphMan.DrawUISprite(el_newgame);
            graphMan.DrawUISprite(el_options);
            graphMan.DrawUISprite(el_quit);
            graphMan.DrawUISprite(cursor);
        }

        public override MenuPage GetNextPage()
        {
            return nextPage;
        }
    }
}
