namespace Wall_E;
public class Sequence : Expression
{
    public List<Expression> arguments { get; private set; }
    public Sequence(List<Expression> arguments)
    {
        this.arguments = arguments;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        List<object> list= new();
        foreach (var x in arguments)
        {
            object val = Evaluator.GetValue(x, values);
            list.Add(val);
        }
        return new Seq(list, false);
    }
}
