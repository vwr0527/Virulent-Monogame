using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Virulent.Graphics
{
    //sprite element contains text, font, texture, position, scale, rotation, and color information
    class SpriteElement
    {
        public Vector2 pos;
        private Vector2 transformedPos;
        private Vector2 orig;
        public Color col;
        public StringBuilder text;
        public Texture2D texture;
        public SpriteFont font;
        public float scale;
        public float rotation;
        public static SpriteFont defaultFont;
        public SpriteElement linkedSprite;
        public static void LoadDefaultFont(ContentManager content)
        {
            defaultFont = content.Load<SpriteFont>("Segoe");
        }
        
        public SpriteElement() { }
        public SpriteElement(SpriteElement spriteCopy)
        {
            col = spriteCopy.col;
            font = spriteCopy.font;
            text = spriteCopy.text;
            rotation = spriteCopy.rotation;
            scale = spriteCopy.scale;
            texture = spriteCopy.texture;
            transformedPos = spriteCopy.transformedPos;
            orig = spriteCopy.orig;
        }
        public SpriteElement(Texture2D textureSource) : this(textureSource, null, null) { }
        public SpriteElement(StringBuilder textSource) : this(null, textSource, defaultFont) { }
        public SpriteElement(StringBuilder textSource, SpriteFont fontSource) : this(null, textSource, fontSource) { }
        public SpriteElement(Texture2D textureSource, StringBuilder textSource, SpriteFont fontSource)
        {
            texture = textureSource;
            text = textSource;
            font = fontSource;

            if (texture != null)
                orig = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
            else if (font != null)
                orig = font.MeasureString(text) / 2;
            else
                orig = Vector2.Zero;

            scale = 1.0f;
            col = Color.White;
            pos = new Vector2(0, 0);
            transformedPos = new Vector2(0, 0);
            rotation = 0;
        }

        public static void CopyMembers(SpriteElement subject, SpriteElement target)
        {
            subject.pos.X = target.pos.X;
            subject.pos.Y = target.pos.Y;
            subject.scale = target.scale;
            subject.rotation = target.rotation;

            subject.col.R = target.col.R;
            subject.col.G = target.col.G;
            subject.col.B = target.col.B;
            subject.col.A = target.col.A;

            if (target.text != null)
            {
                if (subject.text == null) subject.text = new StringBuilder();
                subject.text.Length = 0;
                subject.text.Append(target.text.ToString());
            }
            subject.texture = target.texture;
            subject.font = target.font;
            if (target.texture != null)
                subject.orig = target.orig;
            else if (target.font != null)
                subject.orig = target.font.MeasureString(subject.text) / 2;
            else
                subject.orig = Vector2.Zero;
        }
        public static SpriteElement CreateCopy(SpriteElement target)
        {
            SpriteElement subject = new SpriteElement();
            CopyMembers(subject, target);
            return subject;
        }
        public void DrawGUIStretched(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Vector2 transformedPos = new Vector2(
            pos.X * graphicsDevice.Viewport.Width,
            pos.Y * graphicsDevice.Viewport.Height);
            Vector2 scaleVec = new Vector2(1, 1);
            scaleVec.X = scale * graphicsDevice.Viewport.Width / 800f;
            scaleVec.Y = scale * graphicsDevice.Viewport.Height / 480f;
            Draw(graphicsDevice, spriteBatch, transformedPos, scaleVec);
        }
        public void DrawGUIUnstretchedFixedHeight(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Vector2 transformedPos = new Vector2(
            pos.X * graphicsDevice.Viewport.Width,
            pos.Y * graphicsDevice.Viewport.Height);
            Vector2 scaleVec = new Vector2(1, 1);
            scaleVec.X = scaleVec.Y = scale * graphicsDevice.Viewport.Height / 480f;
            Draw(graphicsDevice, spriteBatch, transformedPos, scaleVec);
        }
        public void DrawGUIUnstretchedFixedWidth(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Vector2 transformedPos = new Vector2(
            pos.X * graphicsDevice.Viewport.Width,
            pos.Y * graphicsDevice.Viewport.Height);
            Vector2 scaleVec = new Vector2(1, 1);
            scaleVec.X = scaleVec.Y = scale * graphicsDevice.Viewport.Width / 800f;
            Draw(graphicsDevice, spriteBatch, transformedPos, scaleVec);
        }
        public void DrawWorld(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Vector2 transformedPos = new Vector2(pos.X, pos.Y);
            Vector2 scaleVec = new Vector2(scale, scale);
            Draw(graphicsDevice, spriteBatch, transformedPos, scaleVec);
        }
        private void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 transformedPos, Vector2 scaleVec)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, transformedPos, null, col, rotation, orig, scaleVec, SpriteEffects.None, 0);
            }
            if (text != null && font != null)
            {
                spriteBatch.DrawString(font, text, transformedPos, col, rotation, orig, scaleVec, SpriteEffects.None, 0);
            }
        }
    }
}
