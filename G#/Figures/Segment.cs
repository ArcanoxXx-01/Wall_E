namespace Wall_E;
public class Segment : Figure
{
    public float longitud
    {
        get
        {
            return (float)p1.Measure(p2);
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
    public List<Point> Intersect(Ray r1)
    {
        return r1.Intersect(this);
    }

    public override void Draw(IPaint paint, Color color, string? message)
    {
        paint.DrawSegment(this, color, message);
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
                result.Add(new Point(p1.x, random.Next((int)p1.y, (int)p2.y)));
            }
        }
        else
        {
            double n = p1.y - pendiente * p1.x;
            for (int i = 0; i < count; i++)
            {
                int x = random.Next((int)p1.x, (int)p2.x);
                int y = (int)(pendiente * x + n);
                result.Add(new Point(x, y));
            }

        }
        return result;
    }

}
