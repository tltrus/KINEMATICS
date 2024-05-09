using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using InverseKinematicMulti.Classes;

namespace InverseKinematicMulti
{
    // Based on Coding Challenge #64.4 "Inverse Kinematics - Multiple" https://codingtrain.github.io/website-archive/CodingChallenges/064.4-inverse-kinematics-multiple
    public partial class MainWindow : Window
    {
        public static Random rnd = new Random();

        DrawingVisual visual;
        DrawingContext dc;
        double width, height;

        List<Tentacle> tentacles = new List<Tentacle>();
        Vector2D mouse = new Vector2D();


        public MainWindow()
        {
            InitializeComponent();

            width = g.Width;
            height = g.Height;

            visual = new DrawingVisual();

            tentacles.Add(new Tentacle(0, height / 2));
            tentacles.Add(new Tentacle(width, height / 2));
        }

        void Draw()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach (var tentacle in tentacles)
                {
                    tentacle.Update(mouse);
                    tentacle.Show(dc);
                }

                dc.Close();
                g.AddVisual(visual);
            }

        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse.X = e.GetPosition(g).X;
            mouse.Y = e.GetPosition(g).Y;

            Draw();
        }
    }
}