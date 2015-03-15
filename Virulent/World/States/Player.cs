using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virulent.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using Virulent.Input;
using Virulent.World.Collision;
using Virulent.World.States.Animations;

namespace Virulent.World.States
{
    class Player : State
    {
        Random rand;
        TimeSpan maxAge;
        Collider collider;

        private Animator anim;

        public Player()
        {
            rand = new Random();
            maxAge = new TimeSpan(1, 1, 30);
            collider = new Collider();
            collider.AddVert(0, -20);
            collider.AddVert(10, 0);
            collider.AddVert(0, 50);
            collider.AddVert(-10, 0);

            anim = new Animator();
        }

        public override void LoadEntityContent(Entity e, ContentManager content)
        {
            e.sprite = new SpriteElement(content.Load<Texture2D>("char/head"));
            SpriteElement cur = e.sprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/body"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/pelvis"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/head")); //footl
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/head")); //footr
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/legrt"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/shoulder"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/shoulder"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/shoulder"));
            cur = cur.linkedSprite;
            cur.linkedSprite = new SpriteElement(content.Load<Texture2D>("char/shoulder"));

            anim.CreatePose("standing");
            anim.AddSpriteInfo(-2f, -15f, 0.5f, 0, 0, 1f, 1f);//head
            anim.AddSpriteInfo(0, 0, 0.4f, 0, 0.2f, 1f, 0.5f);//body
            anim.AddSpriteInfo(1f, 12f, 0.4f, 0, 0, 1f, 1f); //pelvis
            anim.AddSpriteInfo(-3f, 23f, 0.45f, 0, 0, 1f, 1f); //leg right thigh
            anim.AddSpriteInfo(-5f, 38f, 0.5f, 0, 0, 1f, 1f); //leg right calf
            anim.AddSpriteInfo(-5f, 46f, 0.4f, 3.14f, 0, 1f, 1f); //foot right
            anim.AddSpriteInfo(5f, 23f, 0.45f, -0.15f, 0, 1f, 1f); //leg left thigh
            anim.AddSpriteInfo(6f, 38f, 0.5f, -0.15f, 0, 1f, 1f); //leg left calf
            anim.AddSpriteInfo(8f, 46f, 0.4f, 3.14f, 0, 1f, 1f); //foot left
            anim.AddSpriteInfo(-9f, 2f, 0.36f, 0.1f, 0f, 1f, 1f); //arm upper right
            anim.AddSpriteInfo(-11f, 13f, 0.31f, 0f, 0f, 1f, 1f); //arm lower right
            anim.AddSpriteInfo(11f, 2f, 0.36f, -0.3f, 0f, 1f, 1f); //arm upper left
            anim.AddSpriteInfo(13f, 13f, 0.31f, -0.3f, 0f, 1f, 1f); //arm lower left
            anim.AddSpriteInfo(-8f, -5f, 0.33f, -0.3f, 0.2f, 1f, 0.5f); //shoulder right
            anim.AddSpriteInfo(8f, -5f, 0.33f, 0.2f, 0.2f, 1f, 0.5f); //shoulder left
            anim.AddSpriteInfo(13f, 19f, 0.22f, 1.3f, 0f, 1f, 1f); //hand right
            anim.AddSpriteInfo(-12f, 19f, 0.22f, 1.6f, 0f, 1f, 1f); //hand left

            anim.CreatePose("ready");
            anim.AddSpriteInfo(0.8f, -9.8f, 0.5f, 0, 0, 1, 1);
            anim.AddSpriteInfo(-0.3f, 3.7f, 0.4f, 0.18f, 0.2f, 1f, 0.5f);
            anim.AddSpriteInfo(-0.1f, 17.4f, 0.4f, 0, 0, 1, 1);
            anim.AddSpriteInfo(-8.6f, 27.7f, 0.45f, 0.35f, 0, 1, 1);
            anim.AddSpriteInfo(-16.2f, 39f, 0.5f, 0.37f, 0, 1, 1);
            anim.AddSpriteInfo(-18.9f, 46f, 0.4f, 3.14f, 0, 1, 1);
            anim.AddSpriteInfo(10.1f, 23.7f, 0.45f, -0.77f, 0, 1, 1);
            anim.AddSpriteInfo(17.8f, 36.6f, 0.49f, -0.47f, 0, 1, 1);
            anim.AddSpriteInfo(21.9f, 46f, 0.4f, 3.14f, 0, 1, 1);
            anim.AddSpriteInfo(-16.2f, 4.1f, 0.35f, 0.56f, 0, 1, 1);
            anim.AddSpriteInfo(-21.5f, 13.2f, 0.31f, 0.31f, 0, 1, 1);
            anim.AddSpriteInfo(14.7f, 7f, 0.36f, -0.63f, 0, 1, 1);
            anim.AddSpriteInfo(20f, 13.9f, 0.31f, -1.03f, 0, 1, 1);
            anim.AddSpriteInfo(-10.3f, -2.8f, 0.33f, 0.21f, 0.2f, 1, 0.5f);
            anim.AddSpriteInfo(9.5f, 2.3f, 0.33f, 0.2f, 0.2f, 1, 0.5f);
            anim.AddSpriteInfo(24.0f, 18.6f, 0.22f, 0.7f, 0, 1, 1);
            anim.AddSpriteInfo(-25.3f, 18f, 0.22f, 2.0f, 0, 1, 1);

            anim.currentPose = anim.poseList["standing"];
            anim.nextPose = anim.poseList["ready"];

            Pose.ActivateEditor();
            Pose.SelectPoseToEdit(anim.currentPose);
        }

        public override void InitEntity(Entity e)
        {
            e.sprite.col = new Color(0, 255, 0);
            e.sprite.Scale = 1f;
            e.sprite.linkedSprite.Scale = 0.1f;
        }

        public override void UpdateEntity(Entity e, GameTime gameTime, InputManager inputMan)
        {
            Pose.RunEditor(inputMan);

            e.vel.X += 0.005f * (float)(rand.NextDouble() - 0.5) * (float)(gameTime.ElapsedGameTime.Milliseconds);
            e.vel.Y += 0.005f * (float)(gameTime.ElapsedGameTime.Milliseconds);

            if (inputMan.MoveLeftPressed())
            {
                e.vel.X -= 0.01f * (float)(gameTime.ElapsedGameTime.Milliseconds);
            }
            if (inputMan.MoveRightPressed())
            {
                e.vel.X += 0.01f * (float)(gameTime.ElapsedGameTime.Milliseconds);
            }
            if (inputMan.MoveUpPressed())
            {
                e.vel.Y -= 0.01f * (float)(gameTime.ElapsedGameTime.Milliseconds);
            }
            if (inputMan.MoveDownPressed())
            {
                e.vel.Y += 0.01f * (float)(gameTime.ElapsedGameTime.Milliseconds);
            }

            e.pos += e.vel * (float)(gameTime.ElapsedGameTime.Milliseconds) * 0.1f;
            if (e.pos.Y > 200.0f)
            {
                e.pos.Y = 200.0f;
                e.vel.Y *= -0.5f;
            }
            e.age += gameTime.ElapsedGameTime;
            e.sprite.col = new Color(0, 255, 0);
            e.sprite.Scale = 1.5f;
            collider.ppos = e.ppos;
            collider.pos = e.pos;
            //if (e.age > maxAge) e.dead = true;
        }

        public override Collider GetCollider(Entity e)
        {
            collider.ppos = e.ppos;
            collider.pos = e.pos;
            return collider;
        }

        public override void CollideBlock(Entity e, Block b, float collideTime, Vector2 pushOut)
        {
            e.vel.X = 0;
            e.vel.Y = 0;
            e.pos = e.ppos + ((e.pos - e.ppos) * collideTime);
            e.pos += pushOut * 0.1f;

            collider.pos = e.pos;
            collider.ppos = e.ppos;
            b.OnCollide(e);
            //Debug.WriteLine(pushOut + " " + collideTime);
        }

        public override void PositionSprites(Entity e, GameTime gameTime)
        {
            float ratio = (float)((Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 100.0) + 1.0) / 2.0);
            System.Console.WriteLine(ratio);
            anim.DoTweenPose(e, ratio);
            anim.currentPose.ImitateEditorPose();
            Pose.SetEditorPosePosSize(e.pos, new Vector2(100, 100));

            anim.DoPose(e);
        }

        public override void DrawPoly(Entity e, GraphicsManager graphMan, GameTime gameTime)
        {
            //collider.Draw(graphMan);
            Camera c = graphMan.GetCamera(0);
            c.pos = e.pos;
        }
    }
}
