using System.Windows.Media;

namespace InverseKinematicMulti.Classes
{
    class Tentacle
    {
        double segmentsnum = 15;
        Segment end, start;
        Vector2D basepoint;

        public Tentacle(double width, double height)
        {
            double len = 10;

            // ВАЖНО
            // Начало тела - это отросток, start.
            // Тело растет вверх и заканчивается сегментом end.

            start = new Segment(0, 0, len);
            start.SetColor(Brushes.Red);
            Segment current = start;

            for (int i = 0; i < segmentsnum; i++)
            {
                Segment next = new Segment(current, len);
                next.SetColor(Brushes.White);
                current.child = next;
                current = next;
            }
            end = current;
            basepoint = new Vector2D(width, height);
        }

        public void Update(Vector2D target)
        {
            end.Follow(target);
            end.Update();
        }

        public void Show(DrawingContext dc)
        {
            Segment next = end.parent;
            while (next != null)
            {
                next.Follow();
                next.Update();
                next = next.parent;
            }

            // привязывание базы к низу окна
            start.SetA(basepoint);
            start.CalculateB();
            next = start.child;
            while (next != null)
            {
                next.AttachA();
                next.CalculateB();
                next = next.child;
            }

            end.Show(dc);

            next = end.parent;
            while (next != null)
            {
                next.Show(dc);
                next = next.parent;
            }
        }
    }
}
