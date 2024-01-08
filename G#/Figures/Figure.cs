namespace Wall_E;
   public abstract class Figure : IDrawable, IIntersectable<Figure>
    {
        public virtual void Draw(IPaint paint, Color color,string? message)
        {

        }
        public virtual List<Point> RandomPoints()
        {
            if (this is Point p) return p.RandomPoints();
            if (this is Line l) return l.RandomPoints();
            if (this is Segment s) return s.RandomPoints();
            if (this is Circle c) return c.RandomPoints();
            if (this is Ray r) return r.RandomPoints();
            return new List<Point>();
        }
        public List<Point> Intersect(Figure f1)
        {
            if (this is Point p)
            {
                if (f1 is Point) return p.Intersect((Point)f1);
                if (f1 is Line) return p.Intersect((Line)f1);
                if (f1 is Segment) return p.Intersect((Segment)f1);
                if (f1 is Circle) return p.Intersect((Circle)f1);
                if (f1 is Arc) return p.Intersect((Arc)f1);
                else return p.Intersect((Ray)f1);
            }
            if (this is Line l)
            {
                if (f1 is Point) return l.Intersect((Point)f1);
                if (f1 is Line) return l.Intersect((Line)f1);
                if (f1 is Segment) return l.Intersect((Segment)f1);
                if (f1 is Circle) return l.Intersect((Circle)f1);
                if (f1 is Arc) return l.Intersect((Arc)f1);
                else return l.Intersect((Ray)f1);
            }
            if (this is Segment s)
            {
                if (f1 is Point) return s.Intersect((Point)f1);
                if (f1 is Line) return s.Intersect((Line)f1);
                if (f1 is Segment) return s.Intersect((Segment)f1);
                if (f1 is Circle) return s.Intersect((Circle)f1);
                if (f1 is Arc) return s.Intersect((Arc)f1);
                else return s.Intersect((Ray)f1);
            }
            if (this is Circle c)
            {
                if (f1 is Point) return c.Intersect((Point)f1);
                if (f1 is Line) return c.Intersect((Line)f1);
                if (f1 is Segment) return c.Intersect((Segment)f1);
                if (f1 is Circle) return c.Intersect((Circle)f1);
                if (f1 is Arc) return c.Intersect((Arc)f1);
                else return c.Intersect((Ray)f1);
            }
            if (this is Ray r)
            {
                if (f1 is Point) return r.Intersect((Point)f1);
                if (f1 is Line) return r.Intersect((Line)f1);
                if (f1 is Segment) return r.Intersect((Segment)f1);
                if (f1 is Circle) return r.Intersect((Circle)f1);
                if (f1 is Arc) return r.Intersect((Arc)f1);
                else return r.Intersect((Ray)f1);
            }
            else
            {
                if (f1 is Point) return ((Arc)this).Intersect((Point)f1);
                if (f1 is Line) return ((Arc)this).Intersect((Line)f1);
                if (f1 is Segment) return ((Arc)this).Intersect((Segment)f1);
                if (f1 is Circle) return ((Arc)this).Intersect((Circle)f1);
                if (f1 is Arc) return ((Arc)this).Intersect((Arc)f1);
                else return ((Arc)this).Intersect((Ray)f1);
            }
        }
    }
