using System.Windows;
using System.Windows.Media;

namespace InverseKinematicMulti.Classes
{
    class Segment
    {
        public Vector2D a;
        public Vector2D b = new Vector2D();
        private double len;
        private double angle;
        public Segment parent;
        public Segment child;
        private Brush color;


        public Segment(Segment parent, double len)
        {
            this.parent = parent;
            a = parent.b;
            this.len = len;
            CalculateB();
        }

        public Segment(double x, double y, double len)
        {
            a = new Vector2D(x, y);
            this.len = len;
            CalculateB();
        }

        public void SetA(Vector2D pos) => a = pos.CopyToVector();

        public void AttachA() => SetA(parent.b);

        public void Follow()
        {
            Vector2D target = new Vector2D(child.a.X, child.a.Y);
            Follow(target);
        }

        public void Follow(Vector2D target)
        {
            Vector2D dir = Vector2D.Sub(target, a);
            angle = dir.HeadingRad();       // вычислили угол
            dir.SetMag(len);                // установили длину вектору
            dir.Mult(-1);                   // домножили на -1, чтобы он смотрел в другую сторону
            a = Vector2D.Add(target, dir);  // отняли от вектора цели
        }

        public void Update() => CalculateB();


        public void CalculateB()
        {
            double dx = len * Cos(angle);
            double dy = len * Sin(angle);
            b = new Vector2D(a.X + dx, a.Y + dy);
        }

        public void Show(DrawingContext dc)
        {
            var a = Vector2DToPoint(this.a);
            var b = Vector2DToPoint(this.b);
            dc.DrawLine(new Pen(color, 2), a, b);
        }

        public void SetColor(Brush color) => this.color = color;

        double Sin(double a) => Math.Sin(a);
        double Cos(double a) => Math.Cos(a);

        private Vector2D PointToVector2D(Point p) => new Vector2D(p.X, p.Y);
        private Point Vector2DToPoint(Vector2D v) => new Point(v.X, v.Y);

    }

}
