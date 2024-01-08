namespace Wall_E;

public partial class Aplication : Form, IAplication
{
    public Pen pen;
    public Aplication()
    {
        InitializeComponent();
        pen = new Pen(new SolidBrush(Color.Black))
        {
            Width = 2
        };
    }

    private void Run_Click(object sender, EventArgs e)
    {
        Consola.Text = "";
        if (Entrada.Text == null)
        {
            Consola.Text = "An empty line has been entered";
            return;
        }
        if (Entrada.Text.Length == 0)
        {
            Consola.Text = " An empty line has been entered";
            return;
        }

        foreach (char x in Entrada.Text)
        {
            if (x != '\n' && x != ' ')
            {
                Bitmap map = new Bitmap(Plano.Width, Plano.Height);
                Paint paint = new Paint(pen, Graphics.FromImage(map), Plano.Width, Plano.Height);
                Evaluator evaluator = new Evaluator(paint, this, Entrada.Text);
                if (Evaluator.errors.Count != 0)
                {
                    this.PrintErrors(Evaluator.errors);
                }
                else
                {
                    foreach (var t in Evaluator.Dibuja)
                    {
                        t.Item1.Draw(paint, t.Item2,t.Item3);
                    }
                }
                Plano.Image = map;
                return;
            }
        }
        Consola.Text = " An empty line has been entered";

    }

    private void Clean_Click(object sender, EventArgs e)
    {
        Entrada.Text = "";
        Consola.Text = "";
        Plano.Image = null;
    }

    private void Save_Click(object sender, EventArgs e)
    {
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            StreamWriter file = new StreamWriter(saveFileDialog1.FileName, true);
            file.Write(Entrada.Text);
            file.Close();
        }
    }

    public void Print(string message)
    {
        Consola.Text += message;
        Consola.Text += '\n';
    }
    public void PrintError(ERROR error)
    {
        Consola.Text += error.ToString();
        Consola.Text += '\n';
    }
    public void PrintErrors(List<ERROR> errors)
    {
        foreach (ERROR error in errors)
        {
            PrintError(error);
        }
    }
}
