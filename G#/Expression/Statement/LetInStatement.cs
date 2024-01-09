namespace Wall_E;
public class LetIn : Statement
{
    public List<Expression> letBody { get; private set; }
    public Expression inBody { get; private set; }

    public LetIn(List<Expression> letBody, Expression inBody)
    {
        this.letBody = letBody;
        this.inBody = inBody;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        Dictionary<string, object> variables = new();
        foreach (var x in values)
        {
            if (variables.ContainsKey(x.Key)) continue;
            variables.Add(x.Key, x.Value);
        }
        foreach (Expression a in letBody)
        {
            a.Visit(variables);
        }

        object inbody = Evaluator.GetValue(inBody, variables);
        return inbody;
    }
}
