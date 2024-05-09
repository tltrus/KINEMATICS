using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using InverseKinematicSingle.Classes;


namespace InverseKinematicSingle
{

    // Based on Coding Challenge #64.2 "Inverse Kinematics" https://codingtrain.github.io/website-archive/CodingChallenges/064.2-inverse-kinematics.html
    public partial class MainWindow : Window
    {
        public static Random rnd = new Random();
        public static int segmentsnum;
        DrawingVisual visual;
        DrawingContext dc;
        double width, height;

        Segment tentacle;
        Vector2D mouse = new Vector2D();


        public MainWindow()
        {
            InitializeComponent();

            width = g.Width;
            height = g.Height;

            visual = new DrawingVisual();

            Init();
        }

        private void Init()
        {
            segmentsnum = int.Parse(tbSegments.Text);
            double len = 10;

            /*
             * Create body.
             * Parent creating first (tail).
             * Then from tail to head - creating of segments.
             */
            Segment current = new Segment(width / 2, height / 2, len, 0);
            current.SetColor(Brushes.Red);

            for (int i = 0; i < segmentsnum; i++)
            {
                Segment next = new Segment(current, len, i);
                next.SetColor(Brushes.White);
                current.child = next;
                current = next;
            }
            tentacle = current; // Last created segment is head
            current.SetColor(Brushes.Yellow);
        }

        void Draw()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                tentacle.Follow(mouse.X, mouse.Y);
                tentacle.Update();
                tentacle.Show(dc);

                /*
                 * Главный родитель - конец хвоста. От туда идут потомки.
                 * У головы нет детей, поэтому у tentacle переменная child = null
                 * В цикле идет по родителям, доходя до конца хвоста
                 */
                Segment next = tentacle.parent;
                while (next != null)
                {
                    next.Follow();
                    next.Update();
                    next.Show(dc);
                    next = next.parent;
                }

                dc.Close();
                g.AddVisual(visual);
            }

        }

        private void tbSegments_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => Init();

        private void g_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) && tbSegments.Text.Length != 0))
            {
                e.Handled = true;
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