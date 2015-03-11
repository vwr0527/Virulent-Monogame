using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Virulent.Graphics
{
    //graphicElement contains the starting and ending indecies of the solid or line
    //a flag to distinguish whether to draw it as a series of connected lines or a single solid
    //a link to a VectorBank entry
    //currentcolor and previouscolor
    //and previousmatrix and currentmatrix.
    //graphicsManager then uses these to generate the final verticies and vertex indicies, taking
    // into account the camera position, movement and other states.
    class GraphicElement
    {
        public Vector2 pos;
        public static void CopyMembers(GraphicElement subject, GraphicElement target)
        {
            subject.pos.X = target.pos.X;
            subject.pos.Y = target.pos.Y;
        }
        public void Update(GameTime gameTime)
        {
            
        }
    }
}
