namespace Wall_E;
public class Arc : Figure
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

    public override void Draw(IPaint paint, Color color)
    {
        paint.DrawArc(this, color);
    }

    public override List<Point> RandomPoints()
    {
        Circle c1 = new Circle(centro, radio);
        return c1.RandomPoints();
    }
}
