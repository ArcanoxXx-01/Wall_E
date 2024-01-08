namespace Wall_E;
public class Colors : Expression
{
    public string color { get; private set; }
    public Colors(string color)
    {
        this.color = color;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        if (color == "red")
        {
            Evaluator.colors.Push(Color.Red);
        }
        else if (color == "blue")
        {
            Evaluator.colors.Push(Color.Blue);
        }
        else if (color == "black")
        {
            Evaluator.colors.Push(Color.Black);
        }
        else if (color == "white")
        {
            Evaluator.colors.Push(Color.White);
        }
        else if (color == "yellow")
        {
            Evaluator.colors.Push(Color.Yellow);
        }
        else if (color == "magenta")
        {
            Evaluator.colors.Push(Color.Magenta);
        }
        else if (color == "green")
        {
            Evaluator.colors.Push(Color.Green);
        }
        else if (color == "orange")
        {
            Evaluator.colors.Push(Color.Orange);
        }
        else if(color=="brown")
        {
            Evaluator.colors.Push(Color.Brown);
        }

        else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Invalid color has been insert"));
        return null!;
    }

}