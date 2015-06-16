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
        private bool inputEnable;
        private const bool editingPose = true;

        public Player()
        {
            rand = new Random();
            inputEnable = true;
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

			anim.CreatePose("prejump");
			anim.AddSpriteInfo(1f,-7.399999f,0.5f,0.5f,0.1700001f,0f,255f,255f);
			anim.AddSpriteInfo(-1.099999f,6.099999f,0.4f,0.4f,0.21f,51f,255f,127f);
			anim.AddSpriteInfo(-2.3f,18.9f,0.4f,0.4f,0f,0f,255f,255f);
			anim.AddSpriteInfo(-2.8f,26.6f,0.39f,0.39f,-0.7499999f,0f,255f,255f);
			anim.AddSpriteInfo(-5f,38f,0.5f,0.5f,0.5499999f,0f,255f,255f);
			anim.AddSpriteInfo(-8.499999f,46f,0.4f,0.4f,3.14f,0f,255f,255f);
			anim.AddSpriteInfo(7.8f,25.1f,0.45f,0.45f,-0.8699999f,0f,255f,255f);
			anim.AddSpriteInfo(10.9f,39.3f,0.5f,0.5f,0.1900001f,0f,255f,255f);
			anim.AddSpriteInfo(11.1f,46f,0.4f,0.4f,3.14f,0f,255f,255f);
			anim.AddSpriteInfo(-15.3f,4.8f,0.36f,0.36f,0.6499999f,0f,255f,255f);
			anim.AddSpriteInfo(-20.3f,13f,0.31f,0.31f,0f,0f,255f,255f);
			anim.AddSpriteInfo(11f,5.6f,0.36f,0.36f,-0.3f,0f,255f,255f);
			anim.AddSpriteInfo(16.3f,15.8f,0.31f,0.31f,-0.8399999f,0f,255f,251f);
			anim.AddSpriteInfo(-10.1f,-0.9f,0.33f,0.33f,-0.3f,51f,255f,127f);
			anim.AddSpriteInfo(9f,2.5f,0.33f,0.33f,0.2f,51f,255f,127f);
			anim.AddSpriteInfo(20f,20f,0.22f,0.22f,2.189999f,0f,255f,255f);
			anim.AddSpriteInfo(-22.2f,19f,0.22f,0.22f,1.6f,0f,255f,255f);

            anim.CreatePose("jumpup1");
            anim.AddSpriteInfo(-4.7f,-21f,0.5f,0.5f,-0.31f,0f,255f,255f);
            anim.AddSpriteInfo(-3.2f,-7.6f,0.4f,0.4f,0.009999998f,255f,255f,255f);
            anim.AddSpriteInfo(-1.1f,5.899999f,0.31f,0.4f,-0.3f,255f,255f,255f);
            anim.AddSpriteInfo(-4.300001f,17.9f,0.45f,0.45f,0f,0f,255f,255f);
            anim.AddSpriteInfo(-6f,36.3f,0.5f,0.5f,0f,0f,255f,255f);
            anim.AddSpriteInfo(-5f,51.1f,0.4f,0.4f,3.919999f,0f,255f,255f);
            anim.AddSpriteInfo(9.3f,8.599998f,0.45f,0.37f,-1.41f,0f,255f,255f);
            anim.AddSpriteInfo(12.5f,21.3f,0.5f,0.5f,0.2f,0f,255f,255f);
            anim.AddSpriteInfo(11f,30.4f,0.4f,0.4f,4.189999f,0f,255f,255f);
            anim.AddSpriteInfo(-11.1f,-3.500001f,0.36f,0.36f,0.1f,0f,255f,255f);
            anim.AddSpriteInfo(-14.79999f,4.5f,0.41f,0.19f,6.509996f,0f,255f,255f);
            anim.AddSpriteInfo(10.8f,-2.400001f,0.36f,0.36f,-0.3f,0f,255f,255f);
            anim.AddSpriteInfo(13f,4.9f,0.41f,0.19f,-0.9400004f,0f,255f,255f);
            anim.AddSpriteInfo(-10.6f,-11.8f,0.33f,0.33f,-0.3f,255f,255f,255f);
            anim.AddSpriteInfo(6.9f,-10.3f,0.33f,0.33f,0.2f,255f,255f,255f);
            anim.AddSpriteInfo(16.3f,8.999997f,0.22f,0.22f,1.95f,0f,255f,255f);
            anim.AddSpriteInfo(-18.5f,8.599999f,0.22f,0.2500001f,3.989999f,0f,255f,255f);


            anim.CreatePose("jumpup2");
            anim.AddSpriteInfo(-4.7f,-22.5f,0.5f,0.5f,-0.1200001f,0f,255f,255f);
            anim.AddSpriteInfo(-4.3f,-8.599999f,0.4f,0.4f,0.06999999f,255f,255f,255f);
            anim.AddSpriteInfo(-2.2f,4.5f,0.31f,0.4f,-0.2f,255f,255f,255f);
            anim.AddSpriteInfo(-3.200001f,15.8f,0.45f,0.45f,-0.16f,0f,255f,255f);
            anim.AddSpriteInfo(-4f,33.9f,0.5f,0.5f,0f,0f,255f,255f);
            anim.AddSpriteInfo(-4.1f,45.4f,0.4f,0.4f,3.679999f,0f,255f,255f);
            anim.AddSpriteInfo(6.6f,8.499997f,0.45f,0.37f,-0.9400001f,0f,255f,255f);
            anim.AddSpriteInfo(8.2f,21.7f,0.5f,0.5f,0.23f,0f,255f,255f);
            anim.AddSpriteInfo(5.5f,32.2f,0.4f,0.4f,3.829999f,0f,255f,255f);
            anim.AddSpriteInfo(-16.5f,-8.000002f,0.36f,0.36f,0.6499999f,0f,255f,255f);
            anim.AddSpriteInfo(-21.09999f,-5.2f,0.3100001f,0.12f,4.599999f,0f,255f,255f);
            anim.AddSpriteInfo(12.6f,-8.099999f,0.36f,0.36f,-1.14f,0f,255f,255f);
            anim.AddSpriteInfo(17.8f,-5.899999f,0.41f,0.12f,-2.11f,0f,255f,255f);
            anim.AddSpriteInfo(-13.3f,-12.8f,0.33f,0.33f,-0.09000003f,255f,255f,255f);
            anim.AddSpriteInfo(5.1f,-11.6f,0.33f,0.33f,0.2f,255f,255f,255f);
            anim.AddSpriteInfo(21.2f,-6.600004f,0.22f,0.22f,0.9699998f,0f,255f,255f);
            anim.AddSpriteInfo(-17.4f,-3.700002f,0.22f,0.2500001f,1.650001f,0f,255f,255f);

            anim.CreatePose( "jumpup3" );
            anim.AddSpriteInfo(-4.7f,-21.5f,0.5f,0.5f,0.07999989f,0f,255f,255f);
            anim.AddSpriteInfo(-4.3f,-8.199999f,0.4f,0.4f,0.06999999f,255f,255f,255f);
            anim.AddSpriteInfo(-3.5f,3.6f,0.31f,0.4f,-0.04000001f,255f,255f,255f);
            anim.AddSpriteInfo(-0.700001f,10.3f,0.45f,0.45f,-0.8299999f,0f,255f,255f);
            anim.AddSpriteInfo(-2.6f,23.5f,0.5f,0.5f,0.7899999f,0f,255f,255f);
            anim.AddSpriteInfo(-6.5f,29.20001f,0.4f,0.4f,3.889999f,0f,255f,255f);
            anim.AddSpriteInfo(4.4f,11f,0.45f,0.37f,-0.5200002f,0f,255f,255f);
            anim.AddSpriteInfo(5.799999f,25.7f,0.5f,0.5f,0.1100001f,0f,255f,255f);
            anim.AddSpriteInfo(5.299999f,37f,0.4f,0.4f,3.849999f,0f,255f,255f);
            anim.AddSpriteInfo(-14.5f,-9.800002f,0.36f,0.14f,0.03999997f,0f,255f,255f);
            anim.AddSpriteInfo(-12.59999f,-11f,0.3700001f,0.25f,4.079998f,0f,255f,255f);
            anim.AddSpriteInfo(9.700001f,-12.2f,0.43f,0.21f,-1.75f,0f,255f,255f);
            anim.AddSpriteInfo(14.5f,-14.1f,0.41f,0.2f,-2.89f,0f,255f,255f);
            anim.AddSpriteInfo(-13.3f,-13.8f,0.33f,0.33f,-0.09000003f,255f,255f,255f);
            anim.AddSpriteInfo(4.400001f,-12.5f,0.33f,0.33f,0.1f,255f,255f,255f);
            anim.AddSpriteInfo(16.9f,-18.2f,0.22f,0.22f,0.35f,0f,255f,255f);
            anim.AddSpriteInfo(-7.200013f,-13.7f,0.22f,0.2500001f,0.6500012f,0f,255f,255f);

            anim.CreatePose( "jumpup4" );
            anim.AddSpriteInfo(-3.8f,-22.1f,0.5f,0.5f,0.3399999f,0f,255f,255f);
            anim.AddSpriteInfo(-5.3f,-9.599999f,0.4f,0.4f,0.17f,255f,255f,255f);
            anim.AddSpriteInfo(-5.1f,3.2f,0.31f,0.4f,-0.11f,255f,255f,255f);
            anim.AddSpriteInfo(2.099999f,6.4f,0.36f,0.4f,-1.34f,0f,255f,255f);
            anim.AddSpriteInfo(0.1000004f,15.3f,0.5f,0.43f,0.9499998f,0f,255f,255f);
            anim.AddSpriteInfo(-6.5f,21.00001f,0.4f,0.4f,4.11f,0f,255f,255f);
            anim.AddSpriteInfo(1.3f,12f,0.45f,0.37f,-0.5200002f,0f,255f,255f);
            anim.AddSpriteInfo(1.8f,29.2f,0.5f,0.5f,1.41561E-07f,0f,255f,255f);
            anim.AddSpriteInfo(1.299999f,41.1f,0.4f,0.4f,3.849999f,0f,255f,255f);
            anim.AddSpriteInfo(-16.3f,-22.40001f,0.35f,0.27f,-3.38f,0f,255f,255f);
            anim.AddSpriteInfo(-16.19999f,-31.00001f,0.3700001f,0.27f,3.149997f,0f,255f,255f);
            anim.AddSpriteInfo(9.700001f,-18.8f,0.39f,0.29f,-2.44f,0f,255f,255f);
            anim.AddSpriteInfo(11.4f,-24.8f,0.34f,0.29f,-3.659999f,0f,255f,255f);
            anim.AddSpriteInfo(-13.9f,-15.2f,0.33f,0.33f,0.01999996f,255f,255f,255f);
            anim.AddSpriteInfo(4.400001f,-14f,0.33f,0.33f,0.1f,255f,255f,255f);
            anim.AddSpriteInfo(9.6f,-30.2f,0.22f,0.22f,-0.4799998f,0f,255f,255f);
            anim.AddSpriteInfo(-14.20001f,-35.20001f,0.22f,0.2400001f,0.1900014f,0f,255f,255f);


            anim.currentPose = anim.poseList["standing"];
            anim.nextPose = anim.poseList["prejump"];

            if (editingPose)
			{
				Pose.ActivateEditor();
				Pose.SelectPoseToEdit(anim.poseList["standing"]);
            }
        }

        public override void InitEntity(Entity e)
        {
            e.sprite.col = new Color(0, 255, 0);
            e.sprite.Scale = 1f;
            e.sprite.linkedSprite.Scale = 0.1f;
        }

        int jumpHeld = 0;
        bool alreadyJumped = false;
        int jumpCoolDown = 6;
		int jumpAngle = 0;
		int landedTime = 0;
		int midairTime = 0;

        public override void UpdateEntity(Entity e, GameTime gameTime, InputManager inputMan)
        {
			Pose.RunEditor(inputMan, e.pos);

            e.vel.X += 0.0001f * (float)(rand.NextDouble() - 0.5) * (float)(gameTime.ElapsedGameTime.Milliseconds);
            e.vel.Y += 0.01f * (float)(gameTime.ElapsedGameTime.Milliseconds);
            inputEnable = true;
            if (inputEnable)
            {
                if (inputMan.MoveLeftPressed())
                {
                    if (bottom_touching > 0 && jumpHeld <= 1)
                    {
                        e.vel.X -= 0.012f * (float)(gameTime.ElapsedGameTime.Milliseconds);
                    }
                    if (bottom_left_touching > 0)
                    {
                        e.vel.X -= 0.1f;
                        e.vel.Y -= 0.2f;
                    }
                    e.vel.X -= 0.005f * (float)(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (inputMan.MoveRightPressed())
                {
                    if (bottom_touching > 0 && jumpHeld <= 1)
                    {
                        e.vel.X += 0.012f * (float)(gameTime.ElapsedGameTime.Milliseconds);
                    }
                    if (bottom_right_touching > 0)
                    {
                        e.vel.X += 0.1f;
                        e.vel.Y -= 0.2f;
                    }
                    e.vel.X += 0.005f * (float)(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (inputMan.MoveUpPressed())
                {
                    e.vel.Y -= 0.002f * (float)(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (inputMan.MoveDownPressed())
                {
                    e.vel.Y += 0.002f * (float)(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (!inputMan.IsJumpPressed())
                {
                    if (jumpCoolDown > 0)
                    {
                        e.vel.Y += 0.15f;
                    }
                }
                alreadyJumped = false;
                if (jumpCoolDown > 0)
                    --jumpCoolDown;
                if (inputMan.IsJumpPressed() || inputMan.JumpReleased())
                {
                    if (jumpHeld > 4)
                        jumpHeld = 4;
                    if (jumpCoolDown > 0)
                    {
                        jumpHeld = 0;
                        jumpAngle = 0;
                    }

                    float jumpStrength = ((float)jumpHeld + 2.0f) / 5.0f;

                    if (bottom_left_touching == 1 || bottom_right_touching == 1)
                    {
                        jumpHeld = 4;
                        if (bottom_left_touching == 1 && inputMan.MoveRightPressed())
                            jumpStrength = 0.7f;
                        if (bottom_right_touching == 1 && inputMan.MoveLeftPressed())
                            jumpStrength = 0.7f;
                    }

                    if (jumpHeld >= 4 || (jumpHeld > 0 && inputMan.JumpReleased()))
                    {
                        if (jumpCoolDown == 0)
                        {
                            alreadyJumped = true;
                        }
                        jumpCoolDown = 16;
                        jumpHeld = 0;
                    }
					if (bottom_touching > 0) {
						++jumpHeld;
						if (inputMan.MoveRightPressed ()) {
							jumpAngle++;
						}
						if (inputMan.MoveLeftPressed ()) {
							jumpAngle--;
						}
						if (inputMan.MoveUpPressed ()) {
							int diag = 0;
							if (inputMan.MoveLeftPressed () || inputMan.MoveRightPressed ()) {
								diag = 2;
							}
							if (jumpAngle > diag)
								jumpAngle--;
							if (jumpAngle < -diag)
								jumpAngle++;
						}
						if (alreadyJumped) {
							e.vel.Y -= (6 - (Math.Abs (jumpAngle) / 3f)) * jumpStrength;
							e.vel.X += (float)(jumpAngle) / 2.5f;
							landingAnimation = false;
							jumpAngle = 0;
						}

					}
                    if (bottom_left_touching > 0)
                    {
                        jumpHeld += 1;
                        e.vel.X -= 0.2f;
                        e.vel.Y -= 0.1f;
                        if (alreadyJumped)
                        {
                            e.vel.X += 1.1f * jumpStrength;
                            e.vel.Y -= 5 * jumpStrength;
                            if (inputMan.MoveRightPressed())
                            {
                                e.vel.X += 2.0f * jumpStrength;
                                e.vel.Y += 1 * jumpStrength;
                            }
                            else if (inputMan.MoveLeftPressed())
                            {
                                e.vel.X -= 1.5f * jumpStrength;
                                e.vel.Y += 1 * jumpStrength;
                            }
                            if (inputMan.MoveDownPressed())
                            {
                                e.vel.Y += 4 * jumpStrength;
                                e.vel.X += 2f * jumpStrength;
                            }
                        }
                    }
                    if (bottom_right_touching > 0)
                    {
                        jumpHeld += 1;
                        e.vel.X += 0.2f;
                        e.vel.Y -= 0.1f;
                        if (alreadyJumped)
                        {
                            e.vel.X -= 1.1f * jumpStrength;
                            e.vel.Y -= 5 * jumpStrength;
                            if (inputMan.MoveRightPressed())
                            {
                                e.vel.X += 1.5f * jumpStrength;
                                e.vel.Y += 1 * jumpStrength;
                            }
                            else if (inputMan.MoveLeftPressed())
                            {
                                e.vel.X -= 2.0f * jumpStrength;
                                e.vel.Y += 1 * jumpStrength;
                            }
                            if (inputMan.MoveDownPressed())
                            {
                                e.vel.Y += 4 * jumpStrength;
                                e.vel.X -= 2f * jumpStrength;
                            }
                        }
                    }
                }
                else
                {
                    jumpHeld = 0;
                    jumpAngle = 0;
                }
            }


			e.vel.X *= 0.995f;
			e.vel.Y *= 0.995f;

            e.pos += e.vel * (float)(gameTime.ElapsedGameTime.Milliseconds) * 0.1f;
            if (e.pos.Y > 200.0f)
            {
                e.pos.Y = 200.0f;
                e.vel.Y *= 0f;
				bottom_touching = 4;
            }
            e.age += gameTime.ElapsedGameTime;
            e.sprite.col = new Color(0, 255, 0);
            e.sprite.Scale = 1.5f;
            collider.ppos = e.ppos;
            collider.pos = e.pos;
            //if (e.age > maxAge) e.dead = true;

			if (bottom_touching > 0)
			{
				--bottom_touching;
				midairTime = 0;
				landedTime += 1;
			}
			else
			{
				landedTime = 0;
				midairTime += 1;
			}
            if (bottom_left_touching > 0)
            {
                e.vel.X -= 0.1f;
                e.vel.Y -= 0.05f;
                --bottom_left_touching;
            }
            if (bottom_right_touching > 0) 
            {
                e.vel.X += 0.1f;
                e.vel.Y -= 0.05f;
                --bottom_right_touching;
            }
			//System.Console.WriteLine(bottom_touching + " " + bottom_left_touching + " " + bottom_right_touching);
        }

        public override Collider GetCollider(Entity e)
        {
            collider.ppos = e.ppos;
            collider.pos = e.pos;
            return collider;
        }

		int bottom_touching = 0;
		int bottom_left_touching = 0;
		int bottom_right_touching = 0;
		public override void CollideBlock(Entity e, Block b, Collider.CollisionInfo info)
        {
            e.pos = e.ppos + ((e.pos - e.ppos) * info.collideTime);
			e.pos += info.pushOut;
			e.pos += info.slide;
			e.vel = Collider.GetVelBounce(e.vel, info.wall, 0.92f, 0f);
			if (info.direction == 1)
			{
				if (info.wallHit == 2)
				{
					bottom_left_touching = 3;
				}
				else if (info.wallHit == 1)
				{
					bottom_right_touching = 3;
				}
			}
            else if (info.direction == -1 && info.ptHit == 2)
			{
				bottom_touching = 2;
			}

            collider.pos = e.pos;
            collider.ppos = e.ppos;
            b.OnCollide(e);
        }
        public bool facingLeft = false;
		public float landingHardness = 0.0f;
		public bool landingAnimation = false;
        public override void PositionSprites(Entity e, GameTime gameTime)
        {
            //float ratio = (float)((Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 100.0) + 1.0) / 2.0);
            //System.Console.WriteLine(ratio);
            if (bottom_touching > 0)
            {
                float ratio = jumpHeld / 4.0f;
                if (landedTime <= 4)
                {
                    ratio = 1.0f - (landedTime / 4.0f);
					if (!alreadyJumped && !landingAnimation)
					{
						landingHardness = (Math.Min (1.0f, Math.Max (0, e.pvel.Y / 5.0f)));
						landingAnimation = true;
					}
					if (landingAnimation)
					{
						ratio *= landingHardness;
					}
                }
				anim.currentPose = anim.poseList ["standing"];
				anim.nextPose = anim.poseList ["prejump"];
				anim.DoTweenPose (e, ratio);
            }
            else
            {
                float ratio = 0;
                if (e.vel.Y <= -4)
                {
                    anim.currentPose = anim.poseList["jumpup1"];
					anim.nextPose = anim.poseList["jumpup2"];
					//8=1
					//4=0
					ratio = 1.0f - ((-e.vel.Y - 4.0f) / 4.0f);
                }
                else if (e.vel.Y <= 0)
                {
                    anim.currentPose = anim.poseList["jumpup2"];
                    anim.nextPose = anim.poseList["jumpup3"];
					//4=1
					//0=0
					ratio = 1.0f-(-e.vel.Y / 4.0f);
                }
                else if (e.vel.Y > 0)
                {
                    anim.currentPose = anim.poseList["jumpup3"];
                    anim.nextPose = anim.poseList["jumpup4"];
                    ratio = e.vel.Y / 8.0f;
                }
                if (ratio < 0)
                    ratio = 0;
                if (ratio > 1)
                    ratio = 1;
                anim.DoTweenPose(e, ratio);
            }

			//float ratio2 = 0;
            if (editingPose)
            {
                ///*
                anim.currentPose.ImitateEditorPose();
                Pose.SetEditorPosePosSize(e.pos, new Vector2(100, 100));
                anim.DoPose(e);
                //*/
                /*
                if (ratio2 > 2.0f)
                {
                    anim.currentPose = anim.poseList["jumpup3"];
                    anim.nextPose = anim.poseList["jumpup4"];
                    anim.DoTweenPose(e, ratio2 - 2.0f);
                }
                else if (ratio2 > 1.0f)
                {
                    anim.currentPose = anim.poseList["jumpup2"];
                    anim.nextPose = anim.poseList["jumpup3"];
                    anim.DoTweenPose(e, ratio2 - 1.0f);
                }
                else if (ratio2 > 0.0f)
                {
                    anim.currentPose = anim.poseList["jumpup1"];
                    anim.nextPose = anim.poseList["jumpup2"];
                    anim.DoTweenPose(e, ratio2);
                }
                ratio2 += 0.1f;
                if (ratio2 > 3)
                    ratio2 = 0;
                */
            }
            if (e.vel.X > 0.1f)
                facingLeft = false;
            else if (e.vel.X < -0.1f)
                facingLeft = true;
            anim.Facing(e, facingLeft);
        }

        public override void DrawPoly(Entity e, GraphicsManager graphMan, GameTime gameTime)
        {
            //collider.Draw(graphMan);
			//need to find a proper place for camera positioning
            Camera c = graphMan.GetCamera(0);
            c.scale = 1.2f - ((e.pos - c.pos).Length() * 0.001f);
			c.pos += (e.pos - c.pos) * 0.2f;
        }
    }
}
