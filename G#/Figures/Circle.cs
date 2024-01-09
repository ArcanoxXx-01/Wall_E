namespace Wall_E;
public class Circle : Figure
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

        if ((centro.Measure(c1.centro) >= radio - c1.radio - 0.01 && centro.Measure(c1.centro) <= radio - c1.radio + 0.01) || (centro.Measure(c1.centro) >= -(radio - c1.radio) - 0.01 && centro.Measure(c1.centro) <= -(radio - c1.radio) + 0.01))
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

        if (radio + c1.radio <= centro.Measure(c1.centro) + 0.01 && radio + c1.radio >= centro.Measure(c1.centro) - 0.01)
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

    public List<Point> Intersect(Ray r1)
    {
        return r1.Intersect(this);
    }

    public override void Draw(IPaint paint, Color color, string? message)
    {
        paint.DrawCircle(this, color, message);
    }

    public override List<Point> RandomPoints()
    {
        List<Point> result = new List<Point>();
        Random random = new Random();
        int count = random.Next(20, 50);
        for (int i = 0; i < count; i++)
        {
            int x = random.Next((int)(centro.x - radio), (int)(centro.x + radio));
            int y = (int)(Math.Sqrt(radio * radio - (x - centro.x) * (x - centro.x)) + centro.y);
            result.Add(new Point(x, y));
        }
        return result;
    }
}