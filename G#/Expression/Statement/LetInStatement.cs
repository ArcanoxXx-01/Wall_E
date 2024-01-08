namespace Wall_E;
public class LetIn : Statement
{
    public List<Expression> letBody { get; private set; }
    public Expression inBody { get; private set; }
    public Dictionary<string, object> variables;
    public LetIn(List<Expression> letBody, Expression inBody)
    {
        this.letBody = letBody;
        this.inBody = inBody;
        variables = new();
    }

    public override object Visit(Dictionary<string, object> values)
    {
        foreach (var x in values)
        {
            variables.Add(x.Key, x.Value);
        }
        foreach (Expression a in letBody)
        {
            a.Visit(variables);
        }

        object inbody = Evaluator.GetValue(inBody, variables);
        variables=new();        
        return inbody;
    }
}
