using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Virulent.Graphics
{
    class PolyManager
    {
        int numLines = 0;
        int numVerts = 0;
        const int MAX_LINES = 65536;
        BasicEffect basicEffect;
        VertexPositionColor[] vertices;

        public PolyManager()
        {
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,     // left, right
                graphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1000);

            vertices = new VertexPositionColor[MAX_LINES];
        }
        
        public void Draw(GameTime gameTime, GraphicsDevice gd, int numCameras, Camera cam1)
        {
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (-(gd.Viewport.Width / 2), (gd.Viewport.Width / 2),     // left, right
                (gd.Viewport.Height / 2), -(gd.Viewport.Height / 2),    // bottom, top
                0, 1000);
            basicEffect.World = Matrix.CreateScale(cam1.scale);
            basicEffect.World = Matrix.CreateTranslation(-cam1.pos.X - gd.Viewport.Width / 2, -cam1.pos.Y - gd.Viewport.Height / 2, 0) * basicEffect.World;
            basicEffect.World = basicEffect.World * Matrix.CreateRotationZ(cam1.rot);

            if (numLines > 0)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, numVerts);
            }

            //Debug.WriteLine("num lines = " + numLines);
        }

        public void Update(GameTime gameTime)
        {
            numVerts = 0;
            numLines = 0;
        }

        public void StartShape()
        {
        }

        public void AddLine(float x1, float y1, Color col1, float x2, float y2, Color col2)
        {
            vertices[numVerts].Position.X = x1;
            vertices[numVerts].Position.Y = y1;
            vertices[numVerts].Color = col1;
            ++numVerts;
            vertices[numVerts].Position.X = x2;
            vertices[numVerts].Position.Y = y2;
            vertices[numVerts].Color = col2;
            ++numVerts;
            ++numLines;
        }

        public void EndShape()
        {
        }

        public void RemoveAllShapes()
        {
        }
    }
}
