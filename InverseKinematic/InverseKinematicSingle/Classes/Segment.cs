using System.Windows;
using System.Windows.Media;

namespace InverseKinematicSingle.Classes
{
    class Segment
    {
        public Vector2D a = new Vector2D();
        public Vector2D b = new Vector2D();
        private double len;
        private double angle;
        private double sw;
        public Segment parent;
        public Segment child;
        private Brush color;


        public Segment(Segment parent, double len, double i)
        {
            this.parent = parent;
            sw = Tools.Map(i, 0, MainWindow.segmentsnum, 1, 8);
            a = parent.b;
            this.len = len;
            CalculateB();
        }

        public Segment(double x, double y, double len, double i)
        {
            a = new Vector2D(x, y);
            sw = Tools.Map(i, 0, MainWindow.segmentsnum, 1, 8);
            this.len = len;
            CalculateB();
        }

        public void Follow()
        {
            double targetX = child.a.X;
            double targetY = child.a.Y;
            Follow(targetX, targetY);
        }

        public void Follow(double tx, double ty)
        {
            Vector2D target = new Vector2D(tx, ty);
            Vector2D dir = Vector2D.Sub(target, a);
            angle = dir.HeadingRad();       // вычислили угол
            dir.SetMag(len);                // установили длину вектора
            dir.Mult(-1);                   // домножили на -1, чтобы он смотрел в другую сторону
            a = Vector2D.Add(target, dir);  // отняли от вектора цели
        }

        public void Update() => CalculateB();


        void CalculateB()
        {
            double dx = len * Cos(angle);
            double dy = len * Sin(angle);
            b = new Vector2D(a.X + dx, a.Y + dy);
        }

        public void Show(DrawingContext dc)
        {
            var a = Vector2DToPoint(this.a);
            var b = Vector2DToPoint(this.b);
            dc.DrawLine(new Pen(color, sw), a, b);
        }

        public void SetColor(Brush color) => this.color = color;

        double Sin(double a) => Math.Sin(a);
        double Cos(double a) => Math.Cos(a);

        private Point Vector2DToPoint(Vector2D v) => new Point(v.X, v.Y);
    }

}
