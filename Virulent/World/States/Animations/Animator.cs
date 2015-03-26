using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virulent.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Virulent.World.States.Animations
{
    class Animator
    {
        public Pose currentPose;
        public Pose nextPose;
        public Dictionary<string, Pose> poseList;
        public Animator()
        {
            poseList = new Dictionary<string, Pose>();
        }

        public void SetPose(string name)
        {
            if (!poseList.ContainsKey(name)) return;

            currentPose = poseList[name];
        }

        public void SetNextPose(string name)
        {
            if (!poseList.ContainsKey(name)) return;

            nextPose = poseList[name];
        }

        public void DoPose(Entity e)
        {
            if (currentPose == null) return;
            SpriteElement cur = e.sprite;
            int i = 0;
            while (cur != null)
            {
                currentPose.PoseSpriteElement(cur, i, e.pos.X, e.pos.Y, e.rot);
                i++;
                cur = cur.linkedSprite;
            }
        }

        public void DoTweenPose(Entity e, float ratio)
        {
            if (currentPose == null || nextPose == null) return;
            SpriteElement cur = e.sprite;
            int i = 0;
            while (cur != null)
            {
                currentPose.PoseSpriteElementTween(cur, i, e.pos.X, e.pos.Y, e.rot,nextPose,ratio);
                i++;
                cur = cur.linkedSprite;
            }
        }

        public void CreatePose(string name)
        {
            Pose p = new Pose();
            poseList.Add(name, p);
            currentPose = p;
        }

		public void AddSpriteInfo(Vector2 pos, Vector2 scale, float rot, float r, float g, float b)
		{
			currentPose.Add(pos, scale, rot, r, g, b);
		}

        public void AddSpriteInfo(float x, float y, float scale, float rot, float r, float g, float b)
        {
            currentPose.Add(x, y, scale, rot, r, g, b);
        }

        public void AddSpriteInfo(float x, float y, float scalex, float scaley, float rot, float r, float g, float b)
        {
            currentPose.Add(new Vector2(x,y), new Vector2(scalex, scaley), rot, r, g, b);
        }

        public void ImitatePose(Pose other)
        {
            currentPose.Imitate(other);
        }
        public void ImitateEditorPose()
        {
            currentPose.ImitateEditorPose();
        }
    }
}
