namespace Wall_E;
public class Rango : Expression
{
    public Expression inicio { get; private set; }
    public Expression final { get; private set; }

    public Rango(Expression inicio, Expression final = null!)
    {
        this.inicio = inicio;
        this.final = final;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        object inicio = Evaluator.GetValue(this.inicio, values);
        object final = 400;
        bool isnull = true;
        if (this.final is not null)
        {
            final = Evaluator.GetValue(this.final, values);
            isnull = false;
        }

        if (inicio is double a && final is double b)
        {
            if (a - (int)a == 0 && b - (int)b == 0)
            {
                List<object> ints = new();
                for (double i = a; i < b; i++)
                {
                    ints.Add(i);
                }
                return new Seq(ints, isnull);
            }

        }

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Sequences of type '...' only receives ints as arguments"));
        return null!;
    }

}
