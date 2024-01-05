namespace Wall_E
{
    public class Paint : IPaint
    {
        public Pen pen;
        public Stack<Color> colors;
        public Graphics graphics { get; private set; }
        private int width;
        private int height;

        public Paint(Pen pen, Graphics graphics, int width, int height)
        {
            this.pen = pen;
            this.graphics = graphics;
            this.width = width;
            this.height = height;
            colors = new();
        }

        public int Width => width;
        public int Height => height;
        public void DrawPoint(Point p1)
        {
            float x = width / 2 + p1.x;
            float y = height / 2 - p1.y;
            graphics.DrawEllipse(pen, x - 1f, y - 1f, 2f, 2f);
            return;
        }

        public void DrawCircle(Circle c1)
        {
            float xcentro = width / 2 + c1.centro.x;
            float ycentro = height / 2 - c1.centro.y;
            float r = c1.radio;

            graphics.DrawEllipse(pen, xcentro - r, ycentro - r, 2 * r, 2 * r);
            return;
        }

        public void DrawLine(Line l1)
        {
            float x1 = width / 2 + l1.p1.x;
            float y1 = height / 2 - l1.p1.y;
            float x2 = width / 2 + l1.p2.x;
            float y2 = height / 2 - l1.p2.y;

            if (x1 == x2)
            {
                y1 = 0;
                y2 = 1000;
                graphics.DrawLine(pen, new PointF(x1, y1), new PointF(x1, y2));
                return;
            }

            float pendiente = -l1.pendiente;
            float n = y1 - pendiente * x1;
            float xInicio = 0;
            float yInicio = n;
            float xFinal = 1000;
            float yFinal = 1000 * pendiente + n;

            graphics.DrawLine(pen, new PointF(xInicio, yInicio), new PointF(xFinal, yFinal));
            return;
        }

        public void DrawSegment(Segment s1)
        {
            float x1 = s1.p1.x + width / 2;
            float y1 = height / 2 - s1.p1.y;
            float x2 = width / 2 + s1.p2.x;
            float y2 = height / 2 - s1.p2.y;

            graphics.DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
            return;
        }

        public void DrawRay(Ray r1)
        {
            float xinicio = r1.inicio.x + width / 2;
            float yinicio = height / 2 - r1.inicio.y;
            float x = r1.p2.x + width / 2;
            float y = height / 2 - r1.p2.y;

            if (xinicio == x)
            {
                if (yinicio > y) y = 0;
                else y = 1000;
                graphics.DrawLine(pen, new PointF(xinicio, yinicio), new PointF(x, y));
                return;
            }

            float pendiente = -r1.pendiente;
            float n = y - x * pendiente;
            if (xinicio > x) x = 0;
            else x = 1000;
            y = x * pendiente + n;
            graphics.DrawLine(pen, new PointF(xinicio, yinicio), new PointF(x, y));
        }

        public void DrawArc(Arc a1)
        {
            float xcentro = a1.centro.x + width / 2;
            float ycentro = -a1.centro.y + height / 2;
            float r = a1.radio;
            double AngleInicio = a1.AngleInicio;
            double AngleFinal = a1.AngleFinal;
            int AInicGrados = (int)(AngleInicio / Math.PI * 180);
            int AFinGrados = (int)(AngleFinal / Math.PI * 180);

            graphics.DrawArc(pen, xcentro - r, ycentro - r, 2 * r, 2 * r, -1 * AInicGrados, AInicGrados - AFinGrados);
        }
    }
}