using System.Net;

namespace Wall_E;

public class Randoms : IGenerateRandom
{
    public Randoms()
    {

    }
    private static Random random = new Random();
    private int bound_x = 404;
    private int bound_y = 411;
    public Point GeneratePoint()
    {
        return new Point(random.Next(-bound_x, bound_x), random.Next(-bound_y, bound_y));
    }
    public Line GenerateLine()
    {
        Point p1 = GeneratePoint();
        Point p2 = GeneratePoint();
        return new Line(p1, p2);
    }
    public Segment GenerateSegment()
    {
        Point p1 = GeneratePoint();
        Point p2 = GeneratePoint();
        return new Segment(p1, p2);
    }
    public Circle GenerateCircle()
    {
        Point centro = GeneratePoint();
        int radio = random.Next(Math.Min(bound_x, bound_y));
        return new Circle(centro, radio);
    }
    public Ray GenerateRay()
    {
        Point inicio = GeneratePoint();
        Point p2 = GeneratePoint();
        return new Ray(inicio, p2);
    }
    public Arc GenerateArc()
    {
        Point centro = GeneratePoint();
        Point p1 = GeneratePoint();
        Point p2 = GeneratePoint();
        int radio = random.Next(Math.Min(bound_x, bound_y));
        return new Arc(centro, p1, p2, radio);
    }

    public Seq Points(Figure f)
    {
        List<object> list = new();
        foreach (var x in f.RandomPoints())
        {
            list.Add(x);
        }
        return new Seq(list, true);
    }
    public Seq PointSeq()
    {
        List<object> result = new List<object>();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(new Point(random.Next(-bound_x, bound_x), random.Next(-bound_y, bound_y)));
        }

        return new Seq(result, true);
    }
    public Seq LineSeq()
    {
        List<object> result = new List<object>();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(new Line(GeneratePoint(), GeneratePoint()));
        }

        return new Seq(result, true);
    }
    public Seq SegmentSeq()
    {
        List<object> result = new List<object>();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(new Segment(GeneratePoint(), GeneratePoint()));
        }

        return new Seq(result, true);
    }
    public Seq RaySeq()
    {
        List<object> result = new List<object>();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(new Ray(GeneratePoint(), GeneratePoint()));
        }

        return new Seq(result, true);
    }

    public Seq CircleSeq()
    {
        List<object> result = new List<object>();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(new Circle(GeneratePoint(), random.Next(bound_y)));
        }

        return new Seq(result, true);
    }

    public Seq ArcSeq()
    {
        List<object> result = new List<object>();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(new Arc(GeneratePoint(), GeneratePoint(), GeneratePoint(), random.Next(bound_y)));
        }

        return new Seq(result, true);
    }
    public Seq randoms()
    {
        List<object> result = new();
        int count = random.Next(30, 50);
        for (int i = 0; i < count; i++)
        {
            result.Add(random.NextDouble());
        }

        return new Seq(result, true);
    }
}