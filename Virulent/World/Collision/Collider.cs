using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Virulent.World.Collision
{
    class Collider
    {
        public Vector2 pos;
        public Vector2 ppos;
        public float rot; //rotation
        public float prot; //previous rotation
        public List<Vector2> pts; //collider points relative to the center point of the entity, unrotated

        //bounding box
        public float maxx;
        public float maxy;
        public float minx;
        public float miny;

        //unit tangent vector
		public CollisionInfo collisionInfo;

        public Collider()
        {
            pts = new List<Vector2>();
			collisionInfo = new CollisionInfo();
			collisionInfo.collideTime = 1;
        }

        //if returns 1, means no collision happened
        //anything between 0 and 1 indicates a collision
        //0 means the collision happened right after they started moving
        //0.5 means the collision happened halfway between ppos and pos.
        //1 means the collision happened after the maximum amount of time, when the objects were at their respective pos', they have just barely touched.
		//edit: return collisioninfo instead
		public CollisionInfo DoCollide(Collider other)
        {
            //for each line created by each point moving from ppos to pos and rotating from prot to rot,
            //we test against the wall created by connecting point 1 to point 2, then point 2 to point 3, and so on for the entirety of points in the other Collider.
            //  this would normally be lineStart=ppos+pts[i] and lineEnd=pos+pts[i], but we have to take into account the movement of the other Collider
            //  to take into account the other collider, we view the collision from the perspective of the collider forming the walls, matching his movement and rotation change.
            //  therefore, to us the pos and ppos of collider is always 0, likewise the rot and prot.
            //  instead, our position and rotation has changed to what it would look like from the perspective of the collider forming the walls.
            //the 'wall', which consists of other.pts[1] and other.pts[2], or [2] and [3] or so on, will bisect the line, which consists of an arrow going from ppos+pts[i] to pos+pts[i]. 
            //  (which was then transformed for rotation, and also transformed for the fact that it's viewed from the perspective of other Collider(which has it's own rotation and movement))
            //the line which was bisected is separated into 'outside' line segment, and 'inside' line segment.
            //think of time as going from 0.0 to 1.0 from the start of the frame being 0 and the end of the frame being 1.
            //The length of the 'outside' line segment divided by the length of the full line is the fraction of time spent before collision.
            //We keep track of the lowest amount of time spent outside.
            //then repeat the process going from other Collider to this one.
            //We find the lowest amount of time spent outside for collisions both ways.
            //That amount of time is your soonest collision time.
			collisionInfo.collideTime = 1;
			static_hitDirection = 1;
			collideOneWay(this, other);
			static_hitDirection = -1;
			collideOneWay(other, this);

			return collisionInfo;
        }
        private void collideOneWay(Collider a, Collider b)
        {
            for (int i = 0; i < a.pts.Count; ++i)
            {
                Vector2 lineStart = a.ppos;
                lineStart += a.pts[i];
                lineStart -= b.ppos;

                Vector2 lineEnd = a.pos;
                lineEnd += a.pts[i];
                lineEnd -= b.pos;

                static_ptHit = i;
                for (int j = 0; j < b.pts.Count; ++j)
                {
                    Vector2 wallStart = b.pts[j];
                    Vector2 wallEnd;
                    if (j == b.pts.Count - 1)
                        wallEnd = b.pts[0];
                    else
                        wallEnd = b.pts[j + 1];

					static_wallHit = j;
                    CollisionInfo thisCollisionInfo = get_line_intersection(lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, wallStart.X, wallStart.Y, wallEnd.X, wallEnd.Y);
					if (thisCollisionInfo.didCollide && thisCollisionInfo.collideTime < collisionInfo.collideTime)
                    {
						collisionInfo = thisCollisionInfo;
                    }
                }
            }
        }

        public void Draw(Graphics.GraphicsManager graphMan)
        {
            for (int i = 0; i < pts.Count; ++i)
            {
                Vector2 p1 = pts[i];
                Vector2 p2;
                if (i + 1 == pts.Count)
                    p2 = pts[0];
                else
                    p2 = pts[i + 1];

                if (rot != 0)
                {
                    Vector2 p3 = rotatePoint(p1, Vector2.Zero, rot);
                    Vector2 p4 = rotatePoint(p2, Vector2.Zero, rot);
                    graphMan.AddLine(p3.X + pos.X, p3.Y + pos.Y, Color.Red, p4.X + pos.X, p4.Y + pos.Y, Color.Blue);
                }
                else
                {
                    graphMan.AddLine(p1.X + pos.X, p1.Y + pos.Y, Color.Red, p2.X + pos.X, p2.Y + pos.Y, Color.Blue);
                }
            }
        }

        public static void DoTest(Graphics.GraphicsManager graphMan)
        {
            graphMan.AddLine(33f, 44f, Color.Blue, 100f, 100f, Color.White);
            graphMan.AddLine(50f, -50f, Color.White, 70, 80f, Color.Red);
			float t = get_line_intersection(33, 44, 100, 100, 50, -50, 70, 80).collideTime;
            graphMan.AddLine(33f, 44f, Color.Blue, ((100 - 33) * t) + 33f, ((100 - 44) * t) + 44f, Color.Blue);
        }

        public void AddVert(float x, float y)
        {
            pts.Add(new Vector2(x, y));
        }
        public void AddVert(Vector2 p)
        {
            pts.Add(p);
        }
        public void SetLoc(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
            ppos.X = x;
            ppos.Y = y;
        }
        public void SetRot(float newrot)
        {
            prot = newrot;
            rot = newrot;
        }
        public void Move(float x, float y)
        {
            ppos = pos;
            pos.X += x;
            pos.Y += y;
        }
        public void Rotate(float angle)
        {
            prot = rot;
            rot += angle;
        }

        public Vector2 GetPushOut()
        {
			return collisionInfo.pushOut;
        }

        private bool IsLeft(Vector2 a, Vector2 b, Vector2 c)
        {
            return (b.X - a.X) * (c.Y - a.Y) > (b.Y - a.Y) * (c.X - a.X);
        }

        private Vector2 rotatePoint(Vector2 point, Vector2 center, float angle)
        {
            angle = (float)(angle * (Math.PI/180)); // Convert to radians
            float rotatedX = (float)(Math.Cos(angle) * (point.X - center.X) - Math.Sin(angle) * (point.Y-center.Y) + center.X);
            float rotatedY = (float)(Math.Sin(angle) * (point.X - center.X) + Math.Cos(angle) * (point.Y-center.Y) + center.Y);
 
            return new Vector2(rotatedX,rotatedY);
        }

        public static int static_wallHit = 0;
        public static int static_ptHit = 0;
		public static float static_hitDirection = 0;

        public struct CollisionInfo
        {
            public Vector2 point;
			public bool didCollide;
			public Vector2 slide;
			public Vector2 wall;
			public float collideTime;
			public Vector2 pushOut;
			public int wallHit;
            public int ptHit;
			public float direction;
			public CollisionInfo(Vector2 ip, Vector2 endp, Vector2 w, float t)
			{
                direction = static_hitDirection;
                wallHit = static_wallHit;
                ptHit = static_ptHit;
				point = ip;
				didCollide = true;
				wall = Vector2.Normalize(w);
				collideTime = t;
				pushOut = wall;
				float temp = pushOut.X;
				pushOut.X = -pushOut.Y;
				pushOut.Y = temp;
				pushOut *= 0.1f;
				pushOut *= direction;
				Vector2 leftOver = endp - ip;
				slide = wall * Vector2.Dot(leftOver, wall) * -direction;
			}
        }

		public static Vector2 GetVelBounce(Vector2 vel, Vector2 wall, float friction, float elasticity)
		{
			Vector2 outbound = (2 * (Vector2.Dot(vel, wall) * wall)) - vel;
			Vector2 alongWall = 0.5f * (vel + outbound);
			Vector2 bounceOut = outbound - alongWall;
			return (alongWall * friction) + (bounceOut * elasticity);
		}

		// t = intersection time(0.0~1.0)
		public static CollisionInfo get_line_intersection(float p0_x, float p0_y, float p1_x, float p1_y,
            float p2_x, float p2_y, float p3_x, float p3_y)
        {
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
				Vector2 point = new Vector2(p0_x + (t * s1_x), p0_y + (t * s1_y));
				Vector2 wall = new Vector2(s2_x, s2_y);
				Vector2 endpoint = new Vector2(p1_x, p1_y);
				return new CollisionInfo(point, endpoint, wall, t);
            }
			CollisionInfo nothing = new CollisionInfo();
			nothing.collideTime = 1;
			return nothing;
        }
    }
}
