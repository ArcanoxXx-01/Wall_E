namespace Wall_E;
public class IfStatement : Statement
{
    public Expression ifCondicion { get; private set; }
    public Expression thenBody { get; private set; }
    public Expression elseBody { get; private set; }

    public IfStatement(Expression ifCondicion, Expression thenBody, Expression elseBody)
    {
        this.ifCondicion = ifCondicion;
        this.thenBody = thenBody;
        this.elseBody = elseBody;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        object condicion = Evaluator.GetValue(ifCondicion, values);
        if (condicion is bool)
        {
            if ((bool)condicion)
            {
                object thenbody = Evaluator.GetValue(thenBody, values);
                return thenbody;
            }
            else
            {
                object elsebody = Evaluator.GetValue(elseBody, values);
                return elsebody;
            }
        }

        else if (condicion is 0)
        {
            object elsebody = Evaluator.GetValue(elseBody, values);
            return elsebody;
        }

        else if (condicion is 1)
        {
            object thenbody = Evaluator.GetValue(thenBody, values);
            return thenbody;
        }

        else if (condicion is null)
        {
            object elsebody = Evaluator.GetValue(elseBody, values);
            return elsebody;
        }

        else if (condicion is Seq s)
        {
            if (s.count == 0)
            {
                object elsebody = Evaluator.GetValue(elseBody, values);
                return elsebody;
            }
        }

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Expected bool or 0 or undefined or {} or 1 as a value of condicion if"));
        return null!;
    }
}