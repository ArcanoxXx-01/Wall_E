using System.Drawing;
namespace Wall_E
{
    public interface IDrawable
    {
        public void Draw(IPaint p1,Color color);
    }

    public interface IPaint
    {
        int Width { get; }
        int Height { get; }
        public void DrawPoint(Point p1,Color color);
        public void DrawCircle(Circle c1,Color color);
        public void DrawLine(Line l1,Color color);
        public void DrawSegment(Segment s,Color cloro);
        public void DrawRay(Ray r1,Color color);
        public void DrawArc(Arc a1,Color color);
        public void DrawSeq(Seq seq,Color color);
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
        public object Visit(Dictionary<string, object> values);
    }

    public interface IGenerateRandom
    {
        public Point GeneratePoint();
        public Line GenerateLine();
        public Segment GenerateSegment();
        public Circle GenerateCircle();
        public Ray GenerateRay();
        public Arc GenerateArc();
        public Seq PointSeq();
        public Seq LineSeq();
        public Seq SegmentSeq();
        public Seq RaySeq();
        public Seq CircleSeq();
        public Seq ArcSeq();
        public Seq randoms();
        public Seq Points(Figure fig);

    }

}