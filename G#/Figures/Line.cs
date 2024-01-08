namespace Wall_E;
public class Line : Figure
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
        if (p1.x == p2.x)
        {
            if (l1.p1.x == l1.p2.x) return points;

            else
            {
                float x1 = p1.x;
                float n = l1.p1.y - l1.pendiente * l1.p1.x;
                float y1 = l1.pendiente * x1 + n;
                points.Add(new Point(x1, y1));
                return points;
            }
        }

        else if (l1.p1.x == l1.p2.x)
        {
            float x1 = l1.p1.x;
            float n = p1.y - pendiente * p1.x;
            float y1 = pendiente * x1 + n;
            points.Add(new Point(x1,y1));
            return points;
        }
        
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

    public List<Point> Intersect(Ray r1)
    {
        return r1.Intersect(this);
    }

    public override void Draw(IPaint paint, Color color, string? message)
    {
        paint.DrawLine(this, color, message);
    }

    public override List<Point> RandomPoints()
    {
        List<Point> result = new List<Point>();
        Random random = new Random();
        int count = random.Next(20, 50);

        if (p1.x == p2.x)
        {
            for (int i = 0; i < count; i++)
            {
                result.Add(new Point(p1.x, random.Next(-275, 275)));
            }
        }
        else
        {
            double n = p1.y - pendiente * p1.x;
            for (int i = 0; i < count; i++)
            {
                int x = random.Next(-220, 220);
                int y = (int)(pendiente * x + n);
                result.Add(new Point(x, y));
            }

        }
        return result;
    }
}
