using System.Drawing;
namespace Wall_E
{
    public interface IDrawable
    {
        public void Draw(IPaint p1);
    }

    public interface IPaint
    {
        int Width { get; }
        int Height { get; }
        public void DrawPoint(Point p1);
        public void DrawCircle(Circle c1);
        public void DrawLine(Line l1);
        public void DrawSegment(Segment s);
        public void DrawRay(Ray r1);
        public void DrawArc(Arc a1);
    }

    public interface IIntersectable<T>
    {
        public List<Point> Intersect(T f1);
    }

    public interface IAplication
    {
        void Print(string message);
        void PrintError(ERROR error);
        void PrintErrors(List<ERROR> errors);
    }

    public interface IVisitor
    {
        public object Visit(Dictionary<string, (Type, object)> values);
    }

    public interface IGenerateRandom
    {
        Point GeneratePoint();
        Line GenerateLine();
        public Segment GenerateSegment();
        public Circle GenerateCircle();
        public Ray GenerateRay();
        public Arc GenerateArc();
    }

}