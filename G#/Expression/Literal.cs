namespace Wall_E;
public class Literal : Expression
{
    public object value { get; private set; }
    public Literal(object value)
    {
        this.value = value;
    }
    public override object Visit(Dictionary<string, object> values)
    {
        return value;
    }
}