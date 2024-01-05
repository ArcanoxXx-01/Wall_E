using System.Net;

namespace Wall_E;

public class Randoms : IGenerateRandom
{
    public Randoms()
    {

    }
    private static Random random = new Random();
    private int bound_x = 220;
    private int bound_y = 275;
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
}