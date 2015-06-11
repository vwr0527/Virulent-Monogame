using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Virulent.Graphics
{
    class Camera
    {
        public Matrix matrix = new Matrix(1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1);
        public float rot = 0;
        public Vector2 pos = new Vector2();
        public float scale = 1;

        public void CalcMatrix(Viewport view)
        {
            int viewwidth = view.Width / 2;
            int viewheight = view.Height / 2;

            /*
            matrix = Matrix.Identity;
            matrix = Matrix.CreateTranslation(pos.X + viewwidth, pos.Y + viewheight, 0) * matrix;
            matrix = Matrix.CreateRotationZ(rot) * matrix;
            matrix = Matrix.CreateScale(scale) * matrix;
             *
            Matrix.CreateTranslation(-pos.X, -pos.Y, 0) *
            Matrix.CreateTranslation(viewwidth / scale, viewheight / scale, 0);
             * */
            matrix =
            Matrix.CreateTranslation(-pos.X, -pos.Y, 0) *
            Matrix.CreateScale(scale) *
            Matrix.CreateRotationZ(rot) *
            Matrix.CreateTranslation(viewwidth, viewheight, 0);
        }
    }
}
