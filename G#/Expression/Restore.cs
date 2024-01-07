namespace Wall_E;
public class Restore : Expression
{
    public Restore()
    {

    }

    public override object Visit(Dictionary<string, object> values)
    {
        if (Evaluator.colors.Count != 1)
        {
            Evaluator.colors.Pop();
        }
        return null!;
    }
}