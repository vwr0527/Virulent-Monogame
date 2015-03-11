using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Virulent.Graphics
{
    /**
     * Graphics Manager:
     * manages split screen, sprite rendering
     */
    class GraphicsManager
    {
        GraphicsDeviceManager graphicsDeviceManager;
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        RecycleArray<SpriteElement> guiSprites;
        RecycleArray<SpriteElement> worldSprites;

        Camera cam1;
        Viewport test;
        Viewport test2;

        PolyManager poly;
        SpriteElement temp;

        public GraphicsManager(GraphicsDeviceManager gdm)
        {
            graphicsDeviceManager = gdm;
            guiSprites = new RecycleArray<SpriteElement>(SpriteElement.CopyMembers, SpriteElement.CreateCopy);
            guiSprites.SetDataMode(false);
            worldSprites = new RecycleArray<SpriteElement>(SpriteElement.CopyMembers, SpriteElement.CreateCopy);
            worldSprites.SetDataMode(false); //changed these both to false. There really seems to be no point in having a set data mode anymore now...

            cam1 = new Camera();
            poly = new PolyManager();
            temp = new SpriteElement(new StringBuilder(), SpriteElement.defaultFont);
        }
        public void LoadContent(ContentManager content)
        {
            test = new Viewport(0, 0, 800, 240);
            test2 = new Viewport(0, 0, 400, 480);
            graphicsDevice = graphicsDeviceManager.GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            SpriteElement.LoadDefaultFont(content);
            poly.Initialize(graphicsDevice);
        }

        public void DrawAll(GameTime gameTime)
        {
            /*
            cam1.scale = 1.0f - (float)Math.Sin((double)(gameTime.TotalGameTime.TotalMilliseconds * 0.01f)) * (0.1f);
            cam1.pos.X = (float)Math.Sin((double)(gameTime.TotalGameTime.TotalMilliseconds * 0.0073f)) * (9f);
            cam1.pos.Y = (float)Math.Sin((double)(gameTime.TotalGameTime.TotalMilliseconds * 0.0023f)) * (6f);
            cam1.pos.X += (float)Math.Cos((double)(gameTime.TotalGameTime.TotalMilliseconds * 0.000312f)) * (38f);
            cam1.pos.Y += (float)Math.Cos((double)(gameTime.TotalGameTime.TotalMilliseconds * 0.0004f)) * (43f);
            cam1.rot += 0.01f;
            cam1.pos.X += 100;
            cam1.pos.Y += 100;
            Debug.WriteLine(cam1.rot);
             */

            graphicsDevice.Clear(Color.Black);
            //TODO: Multiple cameras
            int numCameras = 1;
            //graphicsDevice.Viewport = test2;
            cam1.CalcMatrix(graphicsDevice.Viewport);
            for (int j = 0; j < numCameras; ++j)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cam1.matrix);
                for (int i = 0; i < worldSprites.Size(); ++i)
                {
                    worldSprites.ElementAt(i).DrawWorld(graphicsDevice, spriteBatch);
                }
                spriteBatch.End();
            }

            spriteBatch.Begin();
            for (int i = 0; i < guiSprites.Size(); ++i)
            {
                guiSprites.ElementAt(i).DrawGUIStretched(graphicsDevice, spriteBatch);
            }
            spriteBatch.End();

            poly.Draw(gameTime, graphicsDevice, numCameras, cam1);

            poly.Update(gameTime);
            guiSprites.EmptyAll();
            worldSprites.EmptyAll();
        }

        public void DrawUISprite(SpriteElement addedElement)
        {
            guiSprites.Add(addedElement);
        }

        public void DrawWorldSprite(SpriteElement addedElement)
        {
            worldSprites.Add(addedElement);
            recursiveDrawSprite(addedElement.linkedSprite);
        }

        private void recursiveDrawSprite(SpriteElement addedElement)
        {
            if (addedElement != null)
            {
                worldSprites.Add(addedElement);
                recursiveDrawSprite(addedElement.linkedSprite);
            }
        }

        public void DrawString(float x, float y, Color c, float scale, float rotation, string str)
        {
            temp.text.Length = 0;
            temp.text.Append(str);
            temp.font = SpriteElement.defaultFont;
            temp.pos.X = x;
            temp.pos.Y = y;
            temp.col = c;
            temp.scale = scale;
            temp.rotation = rotation;
            SpriteElement result = worldSprites.Add(temp);
        }

        public void AddLine(float x1, float y1, Color c1, float x2, float y2, Color c2)
        {
            poly.AddLine(x1 + graphicsDevice.Viewport.Width / 2, y1 + graphicsDevice.Viewport.Height / 2, c1,
                x2 + graphicsDevice.Viewport.Width / 2, y2 + graphicsDevice.Viewport.Height / 2, c2);
        }

        public Camera GetCamera(int whichCamera)
        {
            return cam1;
        }
    }
}
