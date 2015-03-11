using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;

using Virulent.Menu;
using Virulent.World;
using Virulent.Input;
using Virulent.Graphics;
using Virulent.Cinematic;
using Virulent.Storage;

namespace Virulent
{
    public class VirulentGame : Game
    {
        StorageManager storage;
        GraphicsManager graphics;
        MenuManager menu;
        WorldManager world;
        CinematicManager cinema;
        InputManager input;

        public VirulentGame()
        {
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));

            storage = new StorageManager();
            graphics = new GraphicsManager(new GraphicsDeviceManager(this));
            input = new InputManager();
            menu = new MenuManager();
            world = new WorldManager();
            cinema = new CinematicManager();
        }

        private TimeSpan delay;
        private TimeSpan prevdelay;
        protected override void Initialize()
        {
            base.Initialize();
            delay = new TimeSpan(0, 0, 0, 0, 0);
        }

        protected override void LoadContent()
        {
            graphics.LoadContent(Content);
            cinema.LoadContent(Content);
            world.LoadContent(Content);
            menu.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            if (!(gameTime.TotalGameTime - prevdelay >= delay))
            {
                return;
            }
            else
            {
                prevdelay = gameTime.TotalGameTime;
            }

            input.Update(gameTime);

            if (cinema.IsActive())
            {
                cinema.Update(gameTime, input);
                if (input.AnyPressed()) cinema.Deactivate();
            }
            else
            {
                menu.SetInTitleScreen(world.IsInTitleScreen());
                if (menu.IsActive())
                {
                    menu.Update(gameTime, input, world);
                    world.Pause();
                }
                else
                {
                    world.Unpause();
                    if (input.StartPressed())
                    {
                        menu.ResetRoot();
                        menu.Update(gameTime, input, world);
                        menu.Activate();
                    }
                }
                world.Update(gameTime, input);
            }

            if (menu.SaveGame() || world.SaveGame())
            {
                storage.DoSaveRequest(Guide.IsVisible, PlayerIndex.One);
            }

            storage.DoPendingSave();

            if (menu.QuitGame())
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (cinema.IsActive()) cinema.Draw(gameTime, graphics);
            else
            {
                world.Draw(gameTime, graphics);
                if (menu.IsActive())
                    menu.Draw(gameTime, graphics);
            }
            graphics.DrawAll(gameTime);

            base.Draw(gameTime);
        }
    }
}

//cinematic active  / menu inactive / world inactive | startup!
//cinematic inactive/ menu active   / world demoing  | main menu
//cinematic inactive/ menu inactive / world demoing  | watching demo
//cinematic inactive/ menu inactive / world active   | playing
//cinematic inactive/ menu active   / world paused   | paused menu