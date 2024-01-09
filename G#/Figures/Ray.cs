namespace Wall_E;
public class Ray : Figure
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

    public List<Point> Intersect(Ray r1)
    {
        List<Point> points = new();
        Line line = new Line(r1.inicio, r1.p2);
        points = Intersect(line);

        if (points.Count != 0 && Intersect(points[0]).Count == 0)
        {
            points.Remove(points[0]);
        }
        return points;
    }

    public override void Draw(IPaint paint, Color color, string? message)
    {
        paint.DrawRay(this, color, message);
    }

    public override List<Point> RandomPoints()
    {
        List<Point> result = new List<Point>();
        Random random = new Random();
        int count = random.Next(20, 50);

        if (p2.x > inicio.x)
        {
            double n = inicio.y - pendiente * inicio.x;
            for (int i = 0; i < count; i++)
            {
                int x = random.Next((int)inicio.x, 220);
                int y = (int)(pendiente * x + n);
                result.Add(new Point(x, y));
            }
        }
        if (p2.x < inicio.x)
        {
            double n = inicio.y - pendiente * inicio.x;
            for (int i = 0; i < count; i++)
            {
                int x = random.Next(-220, (int)inicio.x);
                int y = (int)(pendiente * x + n);
                result.Add(new Point(x, y));
            }
        }
        if (inicio.x == p2.x)
        {
            if (p2.y > inicio.y)
                for (int i = 0; i < count; i++)
                {
                    result.Add(new Point(inicio.x, random.Next((int)inicio.y, 275)));
                }
            if (p2.y < inicio.y)
                for (int i = 0; i < count; i++)
                {
                    result.Add(new Point(inicio.x, random.Next(-275, (int)inicio.y)));
                }
        }
        return result;
    }

}