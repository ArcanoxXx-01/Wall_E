namespace Wall_E;
public class Point : Figure, IEquatable<Point>
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

    public double Measure(Point p2)
    {
        return Math.Sqrt(Math.Pow(x - p2.x, 2) + Math.Pow(y - p2.y, 2));
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

    public List<Point> Intersect(Ray r1)
    {
        return r1.Intersect(this);
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

    public override void Draw(IPaint paint, Color color, string? message)
    {
        paint.DrawPoint(this, color, message);
    }

    public override List<Point> RandomPoints()
    {
        return new List<Point> { this };
    }
}
