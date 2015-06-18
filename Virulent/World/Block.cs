using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Virulent.Graphics;
using Virulent.World.Collision;
using System.Diagnostics;

namespace Virulent.World
{
    class Block
    {
        private SpriteElement sprite;
        private string textureName;
		private Collider collider;
		public Block()
		{
			collider = new Collider();
			textureName = "";
		}

		public Block(string nameOfTexture)
		{
			textureName = nameOfTexture;
			collider = new Collider();
			collider.AddVert(-160, -50);
			collider.AddVert(160, -50);
			collider.AddVert(160, 50);
			collider.AddVert(-160, 50);
		}

		public void SetVert(int index, float X, float Y)
		{
			collider.SetVert(index, X, Y);
		}

		public void AddVert(float X, float Y)
		{
			collider.AddVert(new Vector2(X, Y));
		}

		public void MakeTriangle()
		{
			collider.AddVert(160, -100);
			collider.AddVert(160, 100);
			collider.AddVert(-320, 100);
		}

        public void LoadContent(ContentManager content)
        {
			if (textureName != "") sprite = new SpriteElement(content.Load<Texture2D>(textureName));
        }

        public void Draw(GameTime gameTime, GraphicsManager graphMan)
        {
			if (sprite != null) sprite.pos.X = collider.pos.X;
			if (sprite != null) sprite.pos.Y = collider.pos.Y;

			if (sprite != null) graphMan.DrawWorldSprite(sprite);
            collider.Draw(graphMan);
        }

        public void SetPosition(float x, float y)
        {
            collider.SetLoc(x, y);//rect.Location = new Point((int)pos.X - 160, (int)pos.Y - 50);
        }
        public void SetScale(float scale)
        {
			if (sprite != null) sprite.Scale = scale;
            /*Point oldPos = collider.rect.Center;
            collider.rect.Width = (int)((float)collider.rect.Width * scale);
            collider.rect.Height = (int)((float)collider.rect.Height * scale);
            collider.rect.Location = new Point(oldPos.X - (collider.rect.Width / 2), oldPos.Y - (collider.rect.Height / 2));*/
			for (int i = 0; i < collider.Count(); ++i)
            {
                collider.pts[i] *= scale;
            }
        }
        public void SetColor(Color col)
        {
			if (sprite != null) sprite.col = col;
        }

        public void OnCollide(Entity e)
        {
        }

        public Collider GetCollider()
        {
            return collider;
        }
    }
}
