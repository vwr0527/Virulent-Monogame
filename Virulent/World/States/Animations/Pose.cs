using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virulent.Graphics;
using Microsoft.Xna.Framework;
using Virulent.Input;
using System.Diagnostics;

namespace Virulent.World.States.Animations
{
    class Pose
    {
        public struct SEInfo
        {
			public Vector2 pos;
			public Vector2 scale;
            public float rot;
			public Color col;
        }

        List<SEInfo> sprites;

        public Pose()
        {
            sprites = new List<SEInfo>();
        }

		public void Add(Vector2 pos, Vector2 scale, float rot, Color col)
		{
			SEInfo spriteinfo = new SEInfo();
			spriteinfo.pos = pos;
			spriteinfo.scale = scale;
			spriteinfo.rot = rot;
			spriteinfo.col = col;
			sprites.Add(spriteinfo);
		}

		public void Add(Vector2 pos, Vector2 scale, float rot, float r, float g, float b)
		{
			SEInfo spriteinfo = new SEInfo();
			spriteinfo.pos = pos;
			spriteinfo.scale = scale;
			spriteinfo.rot = rot;
			spriteinfo.col = new Color(r, g, b);
			sprites.Add(spriteinfo);
		}

		public void Add(float x, float y, Vector2 scale, float rot, float r, float g, float b)
		{
			SEInfo spriteinfo = new SEInfo();
			spriteinfo.pos.X = x;
			spriteinfo.pos.Y = y;
			spriteinfo.scale = scale;
			spriteinfo.rot = rot;
			spriteinfo.col = new Color(r, g, b);
			sprites.Add(spriteinfo);
		}

		public void Add(float x, float y, float scale, float rot, float r, float g, float b)
		{
			SEInfo spriteinfo = new SEInfo();
			spriteinfo.pos.X = x;
			spriteinfo.pos.Y = y;
			spriteinfo.scale.X = spriteinfo.scale.Y = scale;
			spriteinfo.rot = rot;
			spriteinfo.col = new Color(r, g, b);
			sprites.Add(spriteinfo);
		}

        public void Add(float x, float y, float scale, float rot)
        {
            SEInfo spriteinfo = new SEInfo();
            spriteinfo.pos.X = x;
			spriteinfo.pos.Y = y;
			spriteinfo.scale.X = spriteinfo.scale.Y = scale;
			spriteinfo.rot = rot;
			spriteinfo.col.R = 1;
			spriteinfo.col.G = 1;
			spriteinfo.col.B = 1;
            sprites.Add(spriteinfo);
        }

        public void Add(float x, float y)
        {
            SEInfo spriteinfo = new SEInfo();
            spriteinfo.pos.X = x;
			spriteinfo.pos.Y = y;
			spriteinfo.scale.X = spriteinfo.scale.Y = 1;
			spriteinfo.rot = 0;
			spriteinfo.col.R = 1;
			spriteinfo.col.G = 1;
			spriteinfo.col.B = 1;
            sprites.Add(spriteinfo);
        }

        public void PoseSpriteElement(SpriteElement s, int spriteNum, float x, float y, float rot)
        {
            if (spriteNum >= sprites.Count) return;
            SEInfo spriteInfo = sprites[spriteNum];

            s.pos.X = x + spriteInfo.pos.X;
			s.pos.Y = y + spriteInfo.pos.Y;
			s.scale.X = spriteInfo.scale.X;
			s.scale.Y = spriteInfo.scale.Y;
            s.rotation = rot + spriteInfo.rot;
			s.col = spriteInfo.col;
        }

        public void PoseSpriteElementTween(SpriteElement s, int spriteNum, float x, float y, float rot, Pose next, float ratio)
        {
            if (spriteNum >= sprites.Count) return;
            if (spriteNum >= next.sprites.Count) return;
            SEInfo spriteInfo = sprites[spriteNum];
            SEInfo nextSpriteInfo = next.sprites[spriteNum];

            if (ratio > 1.0f) ratio = 1.0f;
            if (ratio < 0.0f) ratio = 0.0f;

			s.pos.X = x + ((spriteInfo.pos.X * (1f - ratio)) + (nextSpriteInfo.pos.X * ratio));
			s.pos.Y = y + ((spriteInfo.pos.Y * (1f - ratio)) + (nextSpriteInfo.pos.Y * ratio));
			s.scale.X = ((spriteInfo.scale.X * (1f - ratio)) + (nextSpriteInfo.scale.X * ratio));
			s.scale.Y = ((spriteInfo.scale.Y * (1f - ratio)) + (nextSpriteInfo.scale.Y * ratio));
            s.rotation = rot + ((spriteInfo.rot * (1f - ratio)) + (nextSpriteInfo.rot * ratio));
			s.col = new Color	(((spriteInfo.col.R * (1f - ratio)) + (nextSpriteInfo.col.R * ratio)),
								((spriteInfo.col.G * (1f - ratio)) + (nextSpriteInfo.col.G * ratio)),
								((spriteInfo.col.B * (1f - ratio)) + (nextSpriteInfo.col.B * ratio)));
        }

        public void Imitate(Pose other)
        {
            sprites.Clear();
            foreach(SEInfo s in other.sprites)
            {
                Add(s.pos,s.scale,s.rot,s.col);
            }
        }

        public void ImitateEditorPose()
        {
            Imitate(selectedPose);
        }

		#region POSE_EDITOR
		/// //////////////////////////////////////////////////////
		///                EDITOR STUFF Begin
		/// //////////////////////////////////////////////////////
		private static bool editor_active = false;
		private static bool editor_pose_loaded = false;
		private static Pose selectedPose;
		private static bool editor_part_selected = false;
		private static SEInfo selectedPart;
		private static bool editor_option_selected = false;
		private static int selectedOption = 0;
		private static int editor_current_part_index = 0;
		private static float editor_option_adjust_speed = 0.1f;
		private static Vector2 pose_pos;
		private static Vector2 pose_box;
		private static int num_part_options = 5;

		public static void ActivateEditor()
		{
			editor_active = true;
			selectedPose = new Pose();
		}

		public static void StopEditor()
		{
			editor_active = false;
		}

		public static void SelectPoseToEdit(Pose target)
		{
			if (!editor_active) return;

			selectedPose.Imitate(target);
			editor_pose_loaded = true;
		}

		public static void SetEditorPosePosSize(Vector2 p, Vector2 b)
		{
			pose_pos = p;
			pose_box = b;
		}

		public static void DrawEditor(GraphicsManager graphMan)
		{
			if (!editor_active) return;

			if (editor_pose_loaded && !editor_part_selected)
			{
				drawPoseBox(graphMan, Color.Green);
				drawPartBox(graphMan, Color.Purple, new Vector2(selectedPart.pos.X, selectedPart.pos.Y));
			}
			if (editor_pose_loaded && editor_part_selected && !editor_option_selected)
			{
				drawPoseBox(graphMan, new Color(0,0.2f,0));
				Color col = Color.Blue;
				drawPartBox(graphMan, new Vector2(selectedPart.pos.X, selectedPart.pos.Y));
			}
			if (editor_pose_loaded && editor_part_selected && editor_option_selected)
			{
				drawPoseBox(graphMan, Color.DarkBlue);
				drawPartBox(graphMan, new Vector2(selectedPart.pos.X, selectedPart.pos.Y));
			}
		}

		private static void drawPoseBox(GraphicsManager graphMan, Color col)
		{
			graphMan.AddLine(pose_pos.X - (pose_box.X / 2),
				pose_pos.Y - (pose_box.Y / 2), col,
				pose_pos.X + (pose_box.X / 2),
				pose_pos.Y - (pose_box.Y / 2), col);
			graphMan.AddLine(pose_pos.X + (pose_box.X / 2),
				pose_pos.Y - (pose_box.Y / 2), col,
				pose_pos.X + (pose_box.X / 2),
				pose_pos.Y + (pose_box.Y / 2), col);
			graphMan.AddLine(pose_pos.X + (pose_box.X / 2),
				pose_pos.Y + (pose_box.Y / 2), col,
				pose_pos.X - (pose_box.X / 2),
				pose_pos.Y + (pose_box.Y / 2), col);
			graphMan.AddLine(pose_pos.X - (pose_box.X / 2),
				pose_pos.Y + (pose_box.Y / 2), col,
				pose_pos.X - (pose_box.X / 2),
				pose_pos.Y - (pose_box.Y / 2), col);
		}

		private static void drawPartBox(GraphicsManager graphMan, Vector2 offset)
		{
			Color col = Color.Blue;
			float x = offset.X + pose_pos.X;
			float y = offset.Y + pose_pos.Y;
			float s = 0.5f;
			float r = 0.0f;
			switch (selectedOption)
			{
				case 0: col = Color.White; graphMan.DrawString(x, y, col, s, r, "xy"); break;
				case 1: col = Color.Blue; graphMan.DrawString(x, y, col, s, r, "R"); break;
				case 2: col = Color.Yellow; graphMan.DrawString(x, y, col, s, r, "S"); break;
				case 3: col = Color.Red; graphMan.DrawString(x, y, col, s, r, "r"); break;
				case 4: col = Color.Green; graphMan.DrawString(x, y, col, s, r, "g"); break;
				case 5: col = Color.Blue; graphMan.DrawString(x, y, col, s, r, "b"); break;
				default: break;
			}
			drawPartBox(graphMan, col, offset);
		}
		private static void drawPartBox(GraphicsManager graphMan, Color col, Vector2 offset)
		{
			graphMan.AddLine(offset.X + pose_pos.X - (pose_box.X / 16),
				offset.Y + pose_pos.Y - (pose_box.Y / 16), col,
				offset.X + pose_pos.X + (pose_box.X / 16),
				offset.Y + pose_pos.Y - (pose_box.Y / 16), col);
			graphMan.AddLine(offset.X + pose_pos.X + (pose_box.X / 16),
				offset.Y + pose_pos.Y - (pose_box.Y / 16), col,
				offset.X + pose_pos.X + (pose_box.X / 16),
				offset.Y + pose_pos.Y + (pose_box.Y / 16), col);
			graphMan.AddLine(offset.X + pose_pos.X + (pose_box.X / 16),
				offset.Y + pose_pos.Y + (pose_box.Y / 16), col,
				offset.X + pose_pos.X - (pose_box.X / 16),
				offset.Y + pose_pos.Y + (pose_box.Y / 16), col);
			graphMan.AddLine(offset.X + pose_pos.X - (pose_box.X / 16),
				offset.Y + pose_pos.Y + (pose_box.Y / 16), col,
				offset.X + pose_pos.X - (pose_box.X / 16),
				offset.Y + pose_pos.Y - (pose_box.Y / 16), col);
		}

		public static void RunEditor(InputManager input, Vector2 offset)
		{
			//Debug.WriteLine("RunEditor running: " + editor_pose_loaded + " " + editor_part_selected + " " + editor_option_selected);
			if (!editor_active) return;

			//if a pose is selected, arrow key means change current selected part
			if (editor_pose_loaded && !editor_part_selected)
			{
				for (int i = 0; i < selectedPose.sprites.Count; ++i) {
					Vector2 partpos = selectedPose.sprites [i].pos + offset;
					float startx = partpos.X - 5;
					float starty = partpos.Y - 5;
					float endx = partpos.X + 5;
					float endy = partpos.Y + 5;
					Vector2 mpos = input.GetWorldMousePos ();
					if (mpos.X > startx && mpos.X < endx)
					{
						if (mpos.Y > starty && mpos.Y < endy)
						{
							editor_current_part_index = i;
							if(input.MouseLB() == 1)
							{
								editor_part_selected = true;
							}
						}
					}
				}
				if (input.DownPressed())
				{
					++editor_current_part_index;
					if (editor_current_part_index > selectedPose.sprites.Count - 1)
					{
						editor_current_part_index = 0;
					}
				}
				if (input.UpPressed())
				{
					--editor_current_part_index;
					if (editor_current_part_index < 0)
					{
						editor_current_part_index = selectedPose.sprites.Count - 1;
					}
				}
				selectedPart = selectedPose.sprites[editor_current_part_index];
				if (input.EnterPressed())
				{
					editor_part_selected = true;
				}
				//if (input.BackspacePressed()) editor_pose_loaded = false;
			}
			else if (editor_pose_loaded && editor_part_selected && !editor_option_selected)
			{
				Vector2 partpos = selectedPose.sprites[editor_current_part_index].pos + offset;
				Vector2 mpos = input.GetWorldMousePos ();
				Vector2 sqrsize = pose_box / 8;
				//Stretching nodes
				//left edge
				Vector2 start = partpos + new Vector2(-sqrsize.X, -sqrsize.Y / 2);
				Vector2 end = partpos + new Vector2(-sqrsize.X / 2, sqrsize.Y / 2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 2;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//right edge
				start = partpos + new Vector2(sqrsize.X / 2, -sqrsize.Y / 2);
				end = partpos + new Vector2(sqrsize.X, sqrsize.Y / 2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 2;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//top edge
				start = partpos + new Vector2(-sqrsize.X / 2, -sqrsize.Y);
				end = partpos + new Vector2(sqrsize.X / 2, -sqrsize.Y / 2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 2;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//bottom edge
				start = partpos + new Vector2(-sqrsize.X / 2, sqrsize.Y / 2);
				end = partpos + new Vector2(sqrsize.X / 2, sqrsize.Y);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 2;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//translation node
				start = partpos + (-sqrsize/2);
				end = partpos + (sqrsize / 2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 0;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//rotation nodes
				//top left corner
				start = partpos + (-sqrsize/2) - new Vector2(2,2);
				end = partpos + (-sqrsize/2) + new Vector2(2,2);
				Console.WriteLine("mpos" + mpos + "\tstart" + start + "\tend"+end);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 1;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//top right corner
				Vector2 corner =  new Vector2(sqrsize.X / 2, -sqrsize.Y / 2);
				start = partpos + corner - new Vector2(2,2);
				end = partpos + corner + new Vector2(2,2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 1;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//bottom right corner
				start = partpos + (sqrsize/2) - new Vector2(2,2);
				end = partpos + (sqrsize/2) + new Vector2(2,2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 1;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				//bottom left corner
				corner =  new Vector2(-sqrsize.X / 2, sqrsize.Y / 2);
				start = partpos + corner - new Vector2(2,2);
				end = partpos + corner + new Vector2(2,2);
				if (mpos.X > start.X  && mpos.X < end.X)
				{
					if (mpos.Y > start.Y && mpos.Y < end.Y)
					{
						selectedOption = 1;
						if(input.MouseLB() == 1)
						{
							editor_option_selected = true;
						}
					}
				}
				/*
				if (input.DownPressed())
				{
					++selectedOption;
					if (selectedOption > num_part_options)
					{
						selectedOption = 0;
					}
				}
				if (input.UpPressed())
				{
					--selectedOption;
					if (selectedOption < 0)
					{
						selectedOption = num_part_options;
					}
				}
				if (input.EnterPressed())
				{
					editor_option_selected = true;
				}
				*/
				if (input.BackspacePressed() || (input.MouseRB() == 1)) editor_part_selected = false;
			}
			else if (editor_pose_loaded && editor_part_selected && editor_option_selected)
			{
				/*if ((!input.IsUpPressed()) && (!input.IsDownPressed()) && (!input.IsLeftPressed()) && (!input.IsRightPressed())) editor_option_adjust_speed = 0.0f;
				else
					switch (selectedOption)
				{
					default:
					case 0:
						if (input.IsLeftPressed())
						{
							selectedPart.pos.X -= editor_option_adjust_speed;
							editor_option_adjust_speed += 0.1f;
						}
						if (input.IsRightPressed())
						{
							selectedPart.pos.X += editor_option_adjust_speed;
							editor_option_adjust_speed += 0.1f;
						}
						if (input.IsDownPressed())
						{
							selectedPart.pos.Y += editor_option_adjust_speed;
							editor_option_adjust_speed += 0.1f;
						}
						if (input.IsUpPressed())
						{
							selectedPart.pos.Y -= editor_option_adjust_speed;
							editor_option_adjust_speed += 0.1f;
						}
						break;
					case 1:
						if (input.IsDownPressed() || input.IsRightPressed())
						{
							selectedPart.rot += editor_option_adjust_speed;
							editor_option_adjust_speed += 0.01f;
						}
						if (input.IsUpPressed() || input.IsLeftPressed())
						{
							selectedPart.rot -= editor_option_adjust_speed;
							editor_option_adjust_speed += 0.01f;
						}
						break;
					case 2:
						if (input.IsRightPressed())
						{
							selectedPart.scale.X += editor_option_adjust_speed;
							editor_option_adjust_speed += 0.01f;
						}
						if (input.IsLeftPressed())
						{
							selectedPart.scale.X -= editor_option_adjust_speed;
							editor_option_adjust_speed += 0.01f;
						}
						if (input.IsDownPressed())
						{
							selectedPart.scale.Y += editor_option_adjust_speed;
							editor_option_adjust_speed += 0.01f;
						}
						if (input.IsUpPressed())
						{
							selectedPart.scale.Y -= editor_option_adjust_speed;
							editor_option_adjust_speed += 0.01f;
						}
						break;
					case 3:
						if (input.IsDownPressed() || input.IsRightPressed())
						{
							selectedPart.col.R += 1;
						}
						if (input.IsUpPressed() || input.IsLeftPressed())
						{
							selectedPart.col.R -= 1;
						}
						break;
					case 4:
						if (input.IsDownPressed() || input.IsRightPressed())
						{
							selectedPart.col.G += 1;
						}
						if (input.IsUpPressed() || input.IsLeftPressed())
						{
							selectedPart.col.G -= 1;
						}
						break;
					case 5:
						if (input.IsDownPressed() || input.IsRightPressed())
						{
							selectedPart.col.B += 1;
						}
						if (input.IsUpPressed() || input.IsLeftPressed())
						{
							selectedPart.col.B -= 1;
						}
						break;
				}
				*/
				Vector2 partpos = selectedPose.sprites[editor_current_part_index].pos + offset;
				Vector2 mpos = input.GetWorldMousePos ();
				switch(selectedOption)
				{
				default:
				case 0:
					selectedPart.pos.X += mpos.X - partpos.X;
					selectedPart.pos.Y += mpos.Y - partpos.Y;
					break;
				case 1:
					selectedPart.rot = (float)Math.Atan2(mpos.Y - partpos.Y, mpos.X - partpos.X);
					break;
				case 2:
					selectedPart.scale.X = (50 + mpos.X - partpos.X) / 100;
					selectedPart.scale.Y = (50 + mpos.Y - partpos.Y) / 100;
					break;
				}
				selectedPose.sprites[editor_current_part_index] = selectedPart;
				if (input.BackspacePressed() || (input.MouseRB() == 1)) editor_option_selected = false;
				if (input.EnterPressed() || (input.MouseLB() == 1))
                {
                    System.Console.WriteLine("anim.CreatePose(  );");

					for (int i = 0; i < selectedPose.sprites.Count; ++i)
					{
						System.Console.WriteLine("anim.AddSpriteInfo(" + selectedPose.sprites[i].pos.X
							+ "f," + selectedPose.sprites[i].pos.Y
							+ "f," + selectedPose.sprites[i].scale.X
							+ "f," + selectedPose.sprites[i].scale.Y
							+ "f," + selectedPose.sprites[i].rot
							+ "f," + selectedPose.sprites[i].col.R
							+ "f," + selectedPose.sprites[i].col.G
							+ "f," + selectedPose.sprites[i].col.B
							+ "f);");
					}
				}
			}
		}

		/// //////////////////////////////////////////////////////
		///                EDITOR STUFF End
		/// //////////////////////////////////////////////////////
		#endregion
    }
}
