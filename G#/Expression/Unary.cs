namespace Wall_E;
public class Unary : Expression
{
    public Token operador { get; private set; }
    public Expression rigth { get; private set; }
    public Unary(Token operador, Expression rigth)
    {
        this.operador = operador;
        this.rigth = rigth;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        object rigth = Evaluator.GetValue(this.rigth, values);

        if (rigth is double v) return -v;

        else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '-' cannot be before expression " + rigth));
        return null!;
    }
}
