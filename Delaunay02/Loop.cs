using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino;
using Rhino.Geometry;

namespace _5.Classes
{
    class Loop : Rhino_Processing
    {
        List<Vertex> vertex = new List<Vertex>();
        //List<Triangle> triangles = new List<Triangle>();

        Random rnd = new Random();

        public override void Setup() // this runs once in the beginning.
        {
            for (int i = 0; i < 10; i++)
            {
                vertex.Add(new Vertex(0, 300,rnd));
            }
        }


        public override void Draw()
        {
            vertex[0].MarkVertex();
            //vertex[0].BaseIncremental(triangles);

            RhinoApp.WriteLine(String.Format("{0}", frame_no));

            for (int i = 1; i < vertex.Count; i++)
            {
                vertex[i].MarkVertex();
                //vertex[i].Incremental(vertex,triangles);
                vertex[i].Display(doc);
                RhinoApp.WriteLine(String.Format("{0}", i));
            }

            //for (int i = 0; i < vertex.Count; i++)
            {
                //triangles[i].Display(doc);
            }
        }
    }
}