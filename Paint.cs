using System.Drawing.Design;
using System;

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
        public void DrawPoint(Point p1, Color color, string? message)
        {
            float x = width / 2 + p1.x;
            float y = height / 2 - p1.y;
            pen.Color = color;
            graphics.DrawEllipse(pen, x - 1f, y - 1f, 2f, 2f);
            if (message is not null)
            {
                DrawString(message, x + 5, y - 5);
            }
            return;
        }

        public void DrawCircle(Circle c1, Color color, string? message)
        {
            float xcentro = width / 2 + c1.centro.x;
            float ycentro = height / 2 - c1.centro.y;
            float r = c1.radio;
            pen.Color = color;
            graphics.DrawEllipse(pen, xcentro - r, ycentro - r, 2 * r, 2 * r);
            if (message is not null)
            {
                DrawString(message, xcentro + 5, ycentro - 5);
            }
            return;
        }

        public void DrawLine(Line l1, Color color, string? message)
        {
            float x1 = width / 2 + l1.p1.x;
            float y1 = height / 2 - l1.p1.y;
            float x2 = width / 2 + l1.p2.x;
            float y2 = height / 2 - l1.p2.y;

            if (x1 == x2)
            {
                y1 = 0;
                y2 = 1000;
                pen.Color = color;
                graphics.DrawLine(pen, new PointF(x1, y1), new PointF(x1, y2));
            }
            else
            {
                float pendiente = -l1.pendiente;
                float n = y1 - pendiente * x1;
                float xInicio = 0;
                float yInicio = n;
                float xFinal = 1000;
                float yFinal = 1000 * pendiente + n;
                pen.Color = color;
                graphics.DrawLine(pen, new PointF(xInicio, yInicio), new PointF(xFinal, yFinal));
            }
            if (message is not null)
            {
                DrawString(message, x1 + 5, y1 - 5);
            }
            return;
        }

        public void DrawSegment(Segment s1, Color color, string? message)
        {
            float x1 = s1.p1.x + width / 2;
            float y1 = height / 2 - s1.p1.y;
            float x2 = width / 2 + s1.p2.x;
            float y2 = height / 2 - s1.p2.y;
            pen.Color = color;
            graphics.DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
            if (message is not null)
            {
                DrawString(message, x1 + 5, y1 - 5);
            }
            return;
        }

        public void DrawRay(Ray r1, Color color, string? message)
        {
            float xinicio = r1.inicio.x + width / 2;
            float yinicio = height / 2 - r1.inicio.y;
            float x = r1.p2.x + width / 2;
            float y = height / 2 - r1.p2.y;

            if (xinicio == x)
            {
                if (yinicio > y) y = 0;
                else y = 1000;
                pen.Color = color;
                graphics.DrawLine(pen, new PointF(xinicio, yinicio), new PointF(x, y));
            }
            else
            {
                float pendiente = -r1.pendiente;
                float n = y - x * pendiente;
                if (xinicio > x) x = 0;
                else x = 1000;
                y = x * pendiente + n;
                pen.Color = color;
                graphics.DrawLine(pen, new PointF(xinicio, yinicio), new PointF(x, y));
            }
            if (message is not null)
            {
                DrawString(message, xinicio + 5, yinicio - 5);
            }
        }

        public void DrawArc(Arc a1, Color color, string? message)
        {
            float xcentro = a1.centro.x + width / 2;
            float ycentro = -a1.centro.y + height / 2;
            Point p1 = a1.p1;
            Point p2 = a1.p2;
            float r = a1.radio;
            double AngleInicio = a1.AngleInicio;
            double AngleFinal = a1.AngleFinal;
            int AInicGrados = (int)(AngleInicio / Math.PI * 180);
            int AFinGrados = (int)(AngleFinal / Math.PI * 180);
            int grados = 0;
            if (AFinGrados - AInicGrados >= 0)
            {
                grados = AFinGrados - AInicGrados;
            }
            else
            {
                grados = 360 - AInicGrados + AFinGrados;
            }
            pen.Color = color;
            graphics.DrawArc(pen, xcentro - r, ycentro - r, 2 * r, 2 * r, -AInicGrados, -grados);
            if (message is not null)
            {
                DrawString(message, xcentro + 5, ycentro - 5);
            }
        }

        public void DrawSeq(Seq sequence, Color color, string? message)
        {
            foreach (object? obj in sequence)
            {
                if (obj is IDrawable d)
                    d.Draw(this, color, message);
            }
        }

        public void DrawString(string text, float x, float y)
        {
            string DrawString = text;
            Font drawFont = new Font("Arial", 9);
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            graphics.DrawString(DrawString, drawFont, brush, x, y, drawFormat);
            drawFont.Dispose();
            brush.Dispose();
        }
    }
}