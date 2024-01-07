namespace Wall_E;
public class Variable : Expression
{
    public Token name { get; private set; }
    public Variable(Token name)
    {
        this.name = name;
    }
    public override object Visit(Dictionary<string, object> values)
    {
        if (values.ContainsKey((string)name.value))
        {
            return values[(string)name.value];
        }
        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name.value + " does not have a value assigned"));
        return null!;
    }
}