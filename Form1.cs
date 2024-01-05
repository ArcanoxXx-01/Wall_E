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
        Consola.Text="";
        Bitmap map = new Bitmap(Plano.Width, Plano.Height);
        Paint paint = new Paint(pen, Graphics.FromImage(map), Plano.Width, Plano.Height);

        Evaluator evaluator = new Evaluator(paint, this, Entrada.Text);


        Plano.Image = map;
    }

    private void Clear_Click(object sender, EventArgs e)
    {
        Entrada.Text = "";
        Consola.Text = "";
        Plano.Image = null;
    }

    private void Save_Click(object sender, EventArgs e)
    {

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
