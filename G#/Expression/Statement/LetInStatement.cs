namespace Wall_E;
public class LetIn : Statement
{
    public List<Assign> letBody { get; private set; }
    public Expression inBody { get; private set; }
    public Dictionary<string, object> variables;
    public LetIn(List<Assign> letBody, Expression inBody)
    {
        this.letBody = letBody;
        this.inBody = inBody;
        variables = new();
    }

    public override object Visit(Dictionary<string, object> values)
    {
        foreach (Assign a in letBody)
        {
            if (variables.ContainsKey((string)a.name.value) || values.ContainsKey((string)a.name.value))
            {
                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + a.name.value + " already has been declared "));
                return null!;
            }

            object value = Evaluator.GetValue(a.value, values);
            variables.Add((string)a.name.value, value);
            values.Add((string)a.name.value, value);
        }

        object letbody = Evaluator.GetValue(inBody, values);
        foreach (var x in variables)
        {
            values.Remove(x.Key);
        }

        return letbody;
    }
}
