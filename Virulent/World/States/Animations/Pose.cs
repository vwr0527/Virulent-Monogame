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
                drawPartBox(graphMan, Color.Purple, new Vector2(selectedPart.x, selectedPart.y));
            }
            if (editor_pose_loaded && editor_part_selected && !editor_option_selected)
            {
                drawPoseBox(graphMan, new Color(0,0.2f,0));
                Color col = Color.Blue;
                drawPartBox(graphMan, new Vector2(selectedPart.x, selectedPart.y));
            }
            if (editor_pose_loaded && editor_part_selected && editor_option_selected)
            {
                drawPoseBox(graphMan, Color.DarkBlue);
                drawPartBox(graphMan, new Vector2(selectedPart.x, selectedPart.y));
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

        public static void RunEditor(InputManager input)
        {
            //Debug.WriteLine("RunEditor running: " + editor_pose_loaded + " " + editor_part_selected + " " + editor_option_selected);
            if (!editor_active) return;

            //if a pose is selected, arrow key means change current selected part
            if (editor_pose_loaded && !editor_part_selected)
            {
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
            else
            if (editor_pose_loaded && editor_part_selected && !editor_option_selected)
            {
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
                if (input.BackspacePressed()) editor_part_selected = false;
            }
            else
            if (editor_pose_loaded && editor_part_selected && editor_option_selected)
            {
                if ((!input.IsUpPressed()) && (!input.IsDownPressed()) && (!input.IsLeftPressed()) && (!input.IsRightPressed())) editor_option_adjust_speed = 0.0f;
                else
                switch (selectedOption)
                {
                    default:
                    case 0:
                    if (input.IsLeftPressed())
                    {
                        selectedPart.x -= editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.1f;
                    }
                    if (input.IsRightPressed())
                    {
                        selectedPart.x += editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.1f;
                    }
                    if (input.IsDownPressed())
                    {
                        selectedPart.y += editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.1f;
                    }
                    if (input.IsUpPressed())
                    {
                        selectedPart.y -= editor_option_adjust_speed;
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
                    if (input.IsDownPressed() || input.IsRightPressed())
                    {
                        selectedPart.scale += editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    if (input.IsUpPressed() || input.IsLeftPressed())
                    {
                        selectedPart.scale -= editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    break;
                    case 3:
                    if (input.IsDownPressed() || input.IsRightPressed())
                    {
                        selectedPart.r += editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    if (input.IsUpPressed() || input.IsLeftPressed())
                    {
                        selectedPart.r -= editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    break;
                    case 4:
                    if (input.IsDownPressed() || input.IsRightPressed())
                    {
                        selectedPart.g += editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    if (input.IsUpPressed() || input.IsLeftPressed())
                    {
                        selectedPart.g -= editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    break;
                    case 5:
                    if (input.IsDownPressed() || input.IsRightPressed())
                    {
                        selectedPart.b += editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    if (input.IsUpPressed() || input.IsLeftPressed())
                    {
                        selectedPart.b -= editor_option_adjust_speed;
                        editor_option_adjust_speed += 0.01f;
                    }
                    break;
                }
                selectedPose.sprites[editor_current_part_index] = selectedPart;
                if (input.BackspacePressed()) editor_option_selected = false;
                if (input.EnterPressed())
                {
                    for (int i = 0; i < selectedPose.sprites.Count; ++i)
                    {
                        System.Console.WriteLine("anim.AddSpriteInfo(" + selectedPose.sprites[i].x
                                                         + "f," + selectedPose.sprites[i].y
                                                         + "f," + selectedPose.sprites[i].scale
                                                         + "f," + selectedPose.sprites[i].rot
                                                         + "f," + selectedPose.sprites[i].r
                                                         + "f," + selectedPose.sprites[i].g
                                                         + "f," + selectedPose.sprites[i].b
                                                         + "f);");
                    }
            //anim.AddSpriteInfo(-2f, -15f, 0.5f, 0, 0, 1f, 1f);//head
                }
            }
        }

        /// //////////////////////////////////////////////////////
        ///                EDITOR STUFF End
        /// //////////////////////////////////////////////////////
        #endregion
        public struct SEInfo
        {
            public float x;
            public float y;
            public float scale;
            public float rot;
            public float r;
            public float g;
            public float b;
        }

        List<SEInfo> sprites;

        public Pose()
        {
            sprites = new List<SEInfo>();
        }

        public void Add(float x, float y, float scale, float rot, float r, float g, float b)
        {
            SEInfo spriteinfo = new SEInfo();
            spriteinfo.x = x;
            spriteinfo.y = y;
            spriteinfo.scale = scale;
            spriteinfo.rot = rot;
            spriteinfo.r = r;
            spriteinfo.g = g;
            spriteinfo.b = b;
            sprites.Add(spriteinfo);
        }

        public void Add(float x, float y, float scale, float rot)
        {
            SEInfo spriteinfo = new SEInfo();
            spriteinfo.x = x;
            spriteinfo.y = y;
            spriteinfo.scale = scale;
            spriteinfo.rot = rot;
            spriteinfo.r = 1;
            spriteinfo.g = 1;
            spriteinfo.b = 1;
            sprites.Add(spriteinfo);
        }

        public void Add(float x, float y)
        {
            SEInfo spriteinfo = new SEInfo();
            spriteinfo.x = x;
            spriteinfo.y = y;
            spriteinfo.scale = 1;
            spriteinfo.rot = 0;
            spriteinfo.r = 1;
            spriteinfo.g = 1;
            spriteinfo.b = 1;
            sprites.Add(spriteinfo);
        }

        public void PoseSpriteElement(SpriteElement s, int spriteNum, float x, float y, float rot)
        {
            if (spriteNum >= sprites.Count) return;
            SEInfo spriteInfo = sprites[spriteNum];

            s.pos.X = x + spriteInfo.x;
            s.pos.Y = y + spriteInfo.y;
            s.scale = spriteInfo.scale;
            s.rotation = rot + spriteInfo.rot;
            s.col = new Color(spriteInfo.r, spriteInfo.g, spriteInfo.b);
        }

        public void PoseSpriteElementTween(SpriteElement s, int spriteNum, float x, float y, float rot, Pose next, float ratio)
        {
            if (spriteNum >= sprites.Count) return;
            if (spriteNum >= next.sprites.Count) return;
            SEInfo spriteInfo = sprites[spriteNum];
            SEInfo nextSpriteInfo = next.sprites[spriteNum];

            if (ratio > 1.0f) ratio = 1.0f;
            if (ratio < 0.0f) ratio = 0.0f;

            s.pos.X = x + ((spriteInfo.x * (1f - ratio)) + (nextSpriteInfo.x * ratio));
            s.pos.Y = y + ((spriteInfo.y * (1f - ratio)) + (nextSpriteInfo.y * ratio));
            s.scale = ((spriteInfo.scale * (1f - ratio)) + (nextSpriteInfo.scale * ratio));
            s.rotation = rot + ((spriteInfo.rot * (1f - ratio)) + (nextSpriteInfo.rot * ratio));
            s.col = new Color(((spriteInfo.r * (1f - ratio)) + (nextSpriteInfo.r * ratio)),
                                ((spriteInfo.g * (1f - ratio)) + (nextSpriteInfo.g * ratio)),
                                ((spriteInfo.b * (1f - ratio)) + (nextSpriteInfo.b * ratio)));
        }

        public void Imitate(Pose other)
        {
            sprites.Clear();
            foreach(SEInfo s in other.sprites)
            {
                Add(s.x,s.y,s.scale,s.rot,s.r,s.g,s.b);
            }
        }

        public void ImitateEditorPose()
        {
            Imitate(selectedPose);
        }
    }
}
