using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VoidEngine
{
    class Collision
    {
        public struct MapSegment
        {
            public Point point1;
            public Point point2;

            public MapSegment(Point a, Point b)
            {
                point1 = a;
                point2 = b;
            }

            public Vector2 getVector()
            {
                return new Vector2(point2.X - point1.X, point2.Y - point1.Y);
            }

            public Rectangle collisionRect()
            {
                return new Rectangle(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y), Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y));
            }
        }

        public static float magnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2 vectorNormal(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 unitVector(Vector2 vector)
        {
            return new Vector2(vector.X / (float)magnitude(vector), vector.Y / (float)magnitude(vector));
        }

        public static float dotProduct(Vector2 unitVector, Vector2 vector)
        {
            return unitVector.X * vector.X + unitVector.Y * vector.Y;
        }

        public static Vector2 reflectedVector(Vector2 vector, Vector2 reflectVector)
        {
            Vector2 normal = vectorNormal(reflectVector);
            float coeficient = -2 * (dotProduct(vector, normal) / (magnitude(normal) * magnitude(normal)));
            Vector2 r;
            r.X = vector.X + coeficient * normal.X;
            r.Y = vector.Y + coeficient * normal.Y;
            return r;
        }

        public struct Line2D
        {
            public Vector2 point;
            public Vector2 vector;

            public float yInt()
            {
                return (-vector.Y * point.X + vector.X * point.Y) / vector.X;
            }
            public float Slope()
            {
                return vector.Y / vector.X;
            }
        }

        public struct Circle
        {
            public Vector2 point;
            public double radius;

            public Circle(Vector2 point, double radius)
            {
                this.point = point;
                this.radius = radius;
            }
        }

        public static bool CheckCircleSegmentCollision(Circle C, MapSegment S)
        {
            Line2D L;
            L.point.X = S.point1.X;
            L.point.Y = S.point1.Y;
            L.vector.X = S.point2.X - S.point1.X;
            L.vector.Y = S.point2.Y - S.point1.Y;


            double OH = Math.Abs(((L.vector.X * (C.point.Y)) - (L.vector.Y * (C.point.X - L.point.X))) / (Math.Sqrt(L.vector.X * L.vector.X + L.vector.Y * L.vector.Y)));

            if (OH <= C.radius)
            {
                Vector2 CollisionPoint1;
                Vector2 CollisionPoint2;

                if (L.vector.X != 0)
                {
                    double Dv = L.vector.Y / L.vector.X;
                    double E = (L.vector.X * L.point.Y - L.vector.Y * L.point.X) / L.vector.X - C.point.Y;

                    double a = 1 + Dv * Dv;
                    double b = -2 * C.point.X + 2 * E * Dv;
                    double c = C.point.X * C.point.X + E * E - C.radius * C.radius;

                    CollisionPoint1.X = (float)((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                    CollisionPoint2.X = (float)((-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a));

                    CollisionPoint1.Y = L.Slope() * CollisionPoint1.X + L.yInt();
                    CollisionPoint2.Y = L.Slope() * CollisionPoint1.X + L.yInt();

                    bool cond1 = (Math.Min(S.point1.X, S.point2.X) <= CollisionPoint1.X && CollisionPoint1.X <= Math.Max(S.point1.X, S.point2.X));

                    bool cond2 = (Math.Min(S.point1.Y, S.point2.Y) <= CollisionPoint1.Y && CollisionPoint1.Y <= Math.Max(S.point1.Y, S.point2.Y));

                    bool cond3 = (Math.Min(S.point1.X, S.point2.X) <= CollisionPoint2.X && CollisionPoint2.X <= Math.Max(S.point1.X, S.point2.X));

                    bool cond4 = (Math.Min(S.point1.Y, S.point2.Y) <= CollisionPoint2.Y && CollisionPoint2.Y <= Math.Max(S.point1.Y, S.point2.Y));

                    return (cond1 && cond2) || (cond3 && cond4);
                }
            }

            return false;
        }
    }
}