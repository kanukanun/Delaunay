using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino;
using Rhino.Geometry;

namespace _5.Classes
{
    class Vertex : Rhino_Processing 
    {
        //properties
       private int min_size;
       private int max_size;
       private double x_coordinate;
       private double y_coordinate;

       private Point3d position;

        // constructors
        public Vertex
            (
            int _min_size,
            int _max_size,
            Random rnd
            )
       {
            min_size = _min_size;
            max_size = _max_size;
            x_coordinate = rnd.Next(_min_size, _max_size);
            y_coordinate = rnd.Next(_min_size, _max_size);
        }
        //methods

        public void MarkVertex()
        {
            position = new Point3d(x_coordinate, y_coordinate, 0);
            
        }

        public void BaseIncremental(List<Triangle> triangles)
        {
            double cx = max_size / 2;
            double cy = max_size / 2;
            double r = Math.Sqrt(2) * (max_size ^ 2 + min_size ^ 2) / 2;//from center to corner
            Point3d a = new Point3d(cx - Math.Sqrt(3) * r, cy + r, 0);
            Point3d b = new Point3d(cx, cy - 2 * r, 0);
            Point3d c = new Point3d(cx + Math.Sqrt(3) * r, cy + r, 0);

            Triangle tri = new Triangle(a, b, c);

            Point3d center = tri.Center();
            double radius = tri.Radius();

            Point3d position = new Point3d(x_coordinate, y_coordinate, 0);

            if (center.DistanceTo(position) < radius)
            {
            triangles.Add(new Triangle(a, b, position));
            triangles.Add(new Triangle(b, c, position));
            triangles.Add(new Triangle(c, a, position));
            triangles.Remove(triangles[triangles.Count - 4]);
            } 
        }

        public void Incremental(List<Vertex> vertex, List<Triangle> triangles)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                Point3d center = triangles[i].Center();
                double radius = triangles[i].Radius();

                Point3d a = triangles[i].A();
                Point3d b = triangles[i].B();
                Point3d c = triangles[i].C();

                if (center.DistanceTo(position) < radius)
                {
                    triangles.Add(new Triangle(a, b, position));
                    triangles.Add(new Triangle(b, c, position));
                    triangles.Add(new Triangle(c, a, position));
                    triangles.Remove(triangles[i]);//元の三角形を削除するようにする。
                }
            }
        }

        public void Display(RhinoDoc _doc)
        {
            _doc.Objects.AddPoint(position);
        }


       
    }
    class Triangle
    {
        private Point3d point_a;
        private Point3d point_b;
        private Point3d point_c;

        private Line side1;
        private Line side2;
        private Line side3;
 
        public Triangle
            (
            Point3d _point_a,
            Point3d _point_b,
            Point3d _point_c
            )
        {
            point_a = _point_a;
            point_b = _point_b;
            point_c = _point_c;

            Line line1 = new Line(point_a, point_b);
            Line line2 = new Line(point_b, point_c);
            Line line3 = new Line(point_c, point_a);
            side1 = line1;
            side2 = line2;
            side3 = line3;
        }

        public Point3d Center()
        {
            double x1 = point_a.X;
            double y1 = point_a.Y;
            double x2 = point_b.X;
            double y2 = point_b.Y;
            double x3 = point_c.X;
            double y3 = point_c.Y;

            double cn = 2 * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));
            double x = ((y3 - y1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (y1 - y2) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / cn;
            double y = ((x1 - x3) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / cn;
           
            Point3d center = new Point3d(x, y, 0);
            return center;
        }

        public Point3d A()
        {
            return point_a;
        }

        public Point3d B()
        {
            return point_b;
        }

        public Point3d C()
        {
            return point_c;
        }

        public double Radius()
        {
            
            double x1 = point_a.X;
            double y1 = point_a.Y;
            double x2 = point_b.X;
            double y2 = point_b.Y;
            double x3 = point_c.X;
            double y3 = point_c.Y;

            double cn = 2 * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));
            double x = ((y3 - y1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (y1 - y2) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / cn;
            double y = ((x1 - x3) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / cn;
           
            Point3d center = new Point3d(x, y, 0);
            
            double radius = center.DistanceTo(point_a);
            return radius;
        }

        public void Display(RhinoDoc _doc)
        {
            _doc.Objects.AddLine(side1);
            _doc.Objects.AddLine(side2);
            _doc.Objects.AddLine(side3);
        }
    }
}

//Drawが動いてない。