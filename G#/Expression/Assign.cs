namespace Wall_E;
public class Assign : Expression
{
    public Token name { get; private set; }
    public Expression value { get; private set; }
    public Assign(Token name, Expression value)
    {
        this.name = name;
        this.value = value;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        object value = Evaluator.GetValue(this.value, values);
        if (values.ContainsKey((string)name.value))
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name.value + " already has been declared "));
            return null!;
        }
        values.Add((string)name.value, value);
        return null!;
    }
}