
namespace Wall_E
{

    public abstract class Figure : IDrawable
    {
        public virtual void Draw(IPaint paint)
        {

        }
    }
    
    
    public class Point : Figure, IEquatable<Point>, IIntersectable<Point>, IIntersectable<Line>, IIntersectable<Segment>, IIntersectable<Circle>, IIntersectable<Arc>
    {
        public float x { get; private set; }
        public float y { get; private set; }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Point? p1)
        {
            if (p1 is null || this is null) return false;
            return x == p1.x && y == p1.y;
        }

        public float Measure(Point p2)
        {
            return (float)Math.Sqrt(Math.Pow(x - p2.x, 2) + Math.Pow(y - p2.y, 2));
        }

        public List<Point> Intersect(Circle c1)
        {
            List<Point> points = new();

            if (Measure(c1.centro) >= c1.radio - 0.01 && Measure(c1.centro) <= c1.radio + 0.01)
            {
                points.Add(this);
            }

            return points;
        }

        public List<Point> Intersect(Segment s1)
        {
            List<Point> points = Intersect(new Line(s1.p1, s1.p2));
            if (points.Count == 0) return points;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].x <= Math.Max(s1.p1.x, s1.p2.x) && points[i].x >= Math.Min(s1.p1.x, s1.p2.x) && points[i].y <= Math.Max(s1.p1.y, s1.p2.y) && points[i].y >= Math.Min(s1.p1.y, s1.p2.y)) return points;
                else points.Remove(points[i]);
            }

            return points;
        }

        public List<Point> Intersect(Line l1)
        {
            List<Point> points = new();
            if (l1.p1.x == l1.p2.x && l1.p1.x == x)
            {
                points.Add(this);
                return points;
            }

            double n = l1.p1.y - l1.pendiente * l1.p1.x;
            if (y >= l1.pendiente * x + n - 0.01 && y <= l1.pendiente * x + n + 0.01)
                points.Add(this);
            return points;
        }

        public List<Point> Intersect(Point p1)
        {
            List<Point> points = new();

            if (Equals(p1))
            {
                points.Add(this);
            }

            return points;
        }

        public List<Point> Intersect(Arc a1)
        {
            return a1.Intersect(this);
        }

        public override void Draw(IPaint paint)
        {
            paint.DrawPoint(this);
        }
    }



    public class Circle : Figure, IIntersectable<Point>, IIntersectable<Line>, IIntersectable<Circle>, IIntersectable<Segment>, IIntersectable<Arc>
    {
        public Point centro { get; private set; }

        public float radio { get; private set; }

        public Circle(Point centro, float radio)
        {
            this.centro = centro;
            this.radio = radio;
        }

        public bool Equals(Circle c1)
        {
            if (radio == c1.radio && centro.Equals(c1.centro)) return true;
            return false;
        }

        public List<Point> Intersect(Point p1)
        {
            return p1.Intersect(this);
        }

        public List<Point> Intersect(Line l1)
        {
            List<Point> points = new();

            if (l1.Measure(centro) > radio) return points;

            if (l1.p1.x == l1.p2.x)
            {
                float absisa = l1.p1.x;
                float y1 = (float)Math.Sqrt(radio * radio - Math.Pow(absisa - centro.x, 2)) + centro.y;
                float y2 = (float)-Math.Sqrt(radio * radio - Math.Pow(absisa - centro.x, 2)) + centro.y;
                points.Add(new Point(absisa, y1));
                points.Add(new Point(absisa, y2));
                return points;
            }

            float m = l1.pendiente;
            float n = l1.p1.y - m * l1.p1.x;

            if (l1.Measure(centro) == radio)
            {
                float x = (centro.y * m + centro.x - m * n) / (m * m + 1);
                float y = m * x + n;
                points.Add(new Point(x, y));
                return points;
            }

            else
            {
                float A = m * m + 1;
                float B = 2 * (m * (n - centro.y) - centro.x);
                float C = centro.x * centro.x + (n - centro.y) * (n - centro.y) - radio * radio;
                float Determinante = B * B - 4 * A * C;
                float x1 = (float)(-B - Math.Sqrt(Determinante)) / (2 * A);
                float x2 = (float)(-B + Math.Sqrt(Determinante)) / (2 * A);
                float y1 = m * x1 + n;
                float y2 = m * x2 + n;

                points.Add(new Point(x1, y1));
                points.Add(new Point(x2, y2));
                return points;

            }

        }

        public List<Point> Intersect(Segment s1)
        {
            List<Point> points = Intersect(new Line(s1.p1, s1.p2));

            if (points.Count == 0) return points;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Intersect(s1).Count == 0) points.Remove(points[i]);
            }

            return points;
        }

        public List<Point> Intersect(Circle c1)
        {
            List<Point> points = new();

            if (centro.Equals(c1.centro)) return points;

            if (radio + c1.radio < centro.Measure(c1.centro)) return points;

            if (centro.Measure(c1.centro) == radio - c1.radio || centro.Measure(c1.centro) == -(radio - c1.radio))
            {
                Line l1 = new Line(centro, c1.centro);
                List<Point> listthis = Intersect(l1);
                List<Point> listc1 = c1.Intersect(l1);
                foreach (Point x in listthis)
                {
                    foreach (Point y in listc1)
                    {
                        if (x.Equals(y))
                        {
                            points.Add(y);
                            return points;
                        }
                    }
                }
                return points;
            }

            if (centro.Measure(c1.centro) < radio - c1.radio || centro.Measure(c1.centro) < -radio + c1.radio)
            {
                return points;
            }

            if (radio + c1.radio == centro.Measure(c1.centro))
            {
                float razon = radio / c1.radio;
                float x1 = (centro.x + razon * c1.centro.x) / (razon + 1);
                float y1 = (centro.y + razon * c1.centro.y) / (razon + 1);
                points.Add(new Point(x1, y1));
                return points;
            }

            else
            {
                float q = centro.x - c1.centro.x;
                //System.Console.WriteLine("this is q " + q);
                float m1 = c1.radio * c1.radio - radio * radio + centro.y * centro.y - q * q - c1.centro.y * c1.centro.y;
                //System.Console.WriteLine("this is m1 " + m1);
                float n1 = 2 * (centro.y - c1.centro.y);
                //System.Console.WriteLine("this is n1 " + n1);
                float A1 = -(4 * q * q + n1 * n1);
                //System.Console.WriteLine("this is A " + A1);
                float B1 = 2 * (4 * q * q * centro.y + m1 * n1);
                //System.Console.WriteLine("this is B1 " + B1);
                float C1 = (float)(Math.Pow(2 * q * radio, 2) - (m1 * m1 + Math.Pow(2 * centro.y * q, 2)));
                //System.Console.WriteLine("this is C1 " + C1);
                float Dis1 = B1 * B1 - 4 * A1 * C1;
                //System.Console.WriteLine("this is discriminante " + Dis1);
                if (Dis1 == 0)
                {
                    float y1 = -B1 / 2 * A1;
                    float x1 = (float)(Math.Sqrt(c1.radio * c1.radio - (y1 - c1.centro.y) * (y1 - c1.centro.y)) + c1.centro.x);
                    points.Add(new Point(x1, y1));
                }
                if (Dis1 > 0)
                {
                    float y1 = (float)(-B1 + Math.Sqrt(Dis1)) / (2 * A1);
                    //System.Console.WriteLine(y1);
                    float x1 = (float)Math.Sqrt(radio * radio - (y1 - centro.y) * (y1 - centro.y)) + centro.x;
                    float y2 = (float)(-B1 - Math.Sqrt(Dis1)) / (2 * A1);
                    // System.Console.WriteLine(y2);
                    float x2 = (float)Math.Sqrt(radio * radio - (y2 - centro.y) * (y2 - centro.y)) + centro.x;
                    float x3 = (float)-Math.Sqrt(radio * radio - (y1 - centro.y) * (y1 - centro.y)) + centro.x;
                    float x4 = (float)-Math.Sqrt(radio * radio - (y2 - centro.y) * (y2 - centro.y)) + centro.x;
                    Point p1 = new Point(x1, y1);
                    //System.Console.WriteLine("this is p1 " + p1.x + " ; " + p1.y);
                    Point p2 = new Point(x3, y1);
                    //System.Console.WriteLine("this is p2 " + p2.x + " ; " + p2.y);
                    Point p3 = new Point(x2, y2);
                    //System.Console.WriteLine("this is p3 " + p3.x + " ; " + p3.y);
                    Point p4 = new Point(x4, y2);
                    //System.Console.WriteLine("this is p4 " + p4.x + " ; " + p4.y);
                    if (p1.Intersect(this).Count != 0 && p1.Intersect(c1).Count != 0 && !points.Contains(p1))
                    {
                        points.Add(p1);
                    }
                    if (p2.Intersect(this).Count != 0 && p2.Intersect(c1).Count != 0 && !points.Contains(p2))
                    {
                        points.Add(p2);
                    }
                    if (p3.Intersect(this).Count != 0 && p3.Intersect(c1).Count != 0 && !points.Contains(p3))
                    {
                        points.Add(p3);
                    }
                    if (p4.Intersect(this).Count != 0 && p4.Intersect(c1).Count != 0 && !points.Contains(p4))
                    {
                        points.Add(p4);
                    }
                    return points;
                }
            }

            return points;
        }

        public List<Point> Intersect(Arc a1)
        {
            return a1.Intersect(this);
        }

        public override void Draw(IPaint paint)
        {
            paint.DrawCircle(this);
        }
    }

    
    
    
    public class Line : Figure, IIntersectable<Point>, IIntersectable<Line>, IIntersectable<Circle>, IIntersectable<Segment>, IIntersectable<Arc>
    {
        public Point p1 { get; private set; }
        public Point p2 { get; private set; }
        private float Pendiente;
        public float pendiente { get { return Pendiente; } }

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            if (p1.x != p2.x)
                Pendiente = (p1.y - p2.y) / (p1.x - p2.x);
        }

        public bool Equals(Line l1)
        {
            if (p1.Equals(l1.p1) && p2.Equals(l1.p2)) return true;
            return false;
        }

        public List<Point> Intersect(Point p1)
        {
            return p1.Intersect(this);
        }

        public List<Point> Intersect(Circle c1)
        {
            return c1.Intersect(this);
        }

        public List<Point> Intersect(Line l1)
        {
            List<Point> points = new();
            if (pendiente == l1.pendiente) return points;
            float n1 = l1.p1.y - l1.pendiente * l1.p1.x;
            float n2 = p1.y - pendiente * p1.x;
            float x = (n2 - n1) / (l1.pendiente - pendiente);
            float y = pendiente * x + n2;
            points.Add(new Point(x, y));
            return points;
        }

        public List<Point> Intersect(Segment s1)
        {
            List<Point> points = Intersect(new Line(s1.p1, s1.p2));

            if (points.Count != 0)
            {
                if (points[0].Intersect(s1).Count == 0)
                {
                    points.Remove(points[0]);
                }
            }

            return points;
        }

        public float Measure(Point p)
        {
            float m = (p1.y - p2.y) / (p1.x - p2.x);
            float n = p1.y - m * p1.x;
            float dist = (m * p.x + n - p.y) / (float)Math.Sqrt(m * m + 1);

            if (dist >= 0) return dist;
            else return -dist;
        }

        public List<Point> Intersect(Arc a1)
        {
            return a1.Intersect(this);
        }

        public override void Draw(IPaint paint)
        {
            paint.DrawLine(this);
        }
    }

    
    
    
    public class Segment : Figure, IIntersectable<Point>, IIntersectable<Line>, IIntersectable<Circle>, IIntersectable<Segment>, IIntersectable<Arc>
    {
        public float longitud
        {
            get
            {
                return p1.Measure(p2);
            }
        }
        public Point p1 { get; private set; }
        public Point p2 { get; private set; }
        private float Pendiente;
        public float pendiente { get { return Pendiente; } }
        public Segment(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            if (p1.x != p2.x)
                Pendiente = (p1.y - p2.y) / (p1.x - p2.x);
        }

        public bool Equals(Segment s1)
        {
            if (p1.Equals(s1.p1) && p2.Equals(s1.p2)) return true;
            return false;
        }

        public List<Point> Intersect(Point p1)
        {
            return p1.Intersect(this);
        }

        public List<Point> Intersect(Circle c1)
        {
            return c1.Intersect(this);
        }
        public List<Point> Intersect(Line l1)
        {
            return l1.Intersect(this);
        }

        public List<Point> Intersect(Segment s1)
        {
            Line l1 = new Line(p1, p2);

            List<Point> points = new();

            if (s1.Pendiente == pendiente) return points;

            points = l1.Intersect(s1);

            if (points.Count == 0) return points;

            if (points[0].Intersect(this).Count == 0)
            {
                points.Remove(points[0]);
            }

            return points;
        }
        public List<Point> Intersect(Arc a1)
        {
            return a1.Intersect(this);
        }

        public override void Draw(IPaint paint)
        {
            paint.DrawSegment(this);
        }

    }

    
    
    public class Ray : Figure, IIntersectable<Point>, IIntersectable<Line>, IIntersectable<Circle>, IIntersectable<Segment>, IIntersectable<Arc>
    {
        public Point inicio { get; private set; }
        public Point p2 { get; private set; }
        public float pendiente { get; private set; }
        public Ray(Point inicio, Point p2)
        {
            this.inicio = inicio;
            this.p2 = p2;
            if (inicio.x != p2.x)
            {
                pendiente = (inicio.y - p2.y) / (inicio.x - p2.x);
            }
        }

        public bool Equals(Ray r1)
        {
            if (inicio.Equals(r1.inicio) && p2.Equals(r1.p2)) return true;
            return false;
        }

        public List<Point> Intersect(Point p1)
        {
            List<Point> points = p1.Intersect(new Line(inicio, p2));

            if (points.Count != 0)
            {
                if (inicio.y <= p2.y)
                {
                    if (p1.y < inicio.y) points.Remove(points[0]);
                }
                else
                {
                    if (p1.y > inicio.y) points.Remove(points[0]);
                }
            }

            if (points.Count != 0)
            {
                if (inicio.x <= p2.x)
                {
                    if (p1.x < inicio.x) points.Remove(points[0]);
                }
                else
                {
                    if (p1.x > inicio.x) points.Remove(points[0]);
                }
            }

            return points;
        }

        public List<Point> Intersect(Line l1)
        {
            List<Point> points = l1.Intersect(new Line(inicio, p2));
            if (points.Count == 0) return points;
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public List<Point> Intersect(Segment s1)
        {
            List<Point> points = s1.Intersect(new Line(inicio, p2));
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public List<Point> Intersect(Circle c1)
        {
            List<Point> points = c1.Intersect(new Line(inicio, p2));
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public List<Point> Intersect(Arc a1)
        {
            List<Point> points = a1.Intersect(new Line(inicio, p2));
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public override void Draw(IPaint paint)
        {
            paint.DrawRay(this);
        }

    }

    
    
    public class Arc : Figure, IIntersectable<Point>, IIntersectable<Line>, IIntersectable<Circle>, IIntersectable<Segment>, IIntersectable<Arc>
    {
        public Point p1 { get; private set; }
        public Point p2 { get; private set; }
        public Point centro { get; private set; }
        public float radio { get; private set; }
        public double AngleInicio { get; private set; }
        public double AngleFinal { get; private set; }

        public Arc(Point centro, Point p1, Point p2, float radio)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.centro = centro;
            this.radio = radio;
            AngleInicio = Math.Atan2(p1.y - centro.y, p1.x - centro.x);
            AngleFinal = Math.Atan2(p2.y - centro.y, p2.x - centro.x);
        }

        public bool Equals(Arc a1)
        {
            if (p1.Equals(a1.p1) && p2.Equals(a1.p2) && centro.Equals(a1.centro) && radio == a1.radio) return true;
            return false;
        }

        public List<Point> Intersect(Point p1)
        {
            List<Point> points = p1.Intersect(new Circle(centro, radio));
            if (points.Count != 0)
            {
                double AnglePoint = Math.Atan2(points[0].y - centro.y, points[0].x - centro.x);
                if (AnglePoint >= AngleInicio && AnglePoint <= AngleFinal) return points;
                else points.Remove(points[0]);
            }
            return points;
        }

        public List<Point> Intersect(Line l1)
        {
            List<Point> points = l1.Intersect(new Circle(centro, radio));
            if (points.Count == 0) return points;
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public List<Point> Intersect(Segment s1)
        {
            List<Point> points = s1.Intersect(new Circle(centro, radio));
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public List<Point> Intersect(Circle c1)
        {
            List<Point> points = c1.Intersect(new Circle(centro, radio));
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public List<Point> Intersect(Arc a1)
        {
            List<Point> points = a1.Intersect(new Circle(centro, radio));
            for (int i = 0; i < points.Count; i++)
            {
                if (Intersect(points[i]).Count == 0) points.Remove(points[i]);
            }
            return points;
        }

        public override void Draw(IPaint paint)
        {
            paint.DrawArc(this);
        }
    }
}