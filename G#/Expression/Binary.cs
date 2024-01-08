namespace Wall_E;
public class Binary : Expression
{
    public Expression rigth { get; private set; }
    public Token operador { get; private set; }
    public Expression left { get; private set; }

    public Binary(Expression left, Token operador, Expression rigth)
    {
        this.rigth = rigth;
        this.operador = operador;
        this.left = left;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        Token.TokenType type = operador.type;
        object left = Evaluator.GetValue(this.left, values);
        object rigth = Evaluator.GetValue(this.rigth, values);
        if (type == Token.TokenType.Suma)
        {
            return Suma(left, rigth);
        }

        if (type == Token.TokenType.Menos)
        {
            return Resta(left, rigth);
        }

        if (type == Token.TokenType.Aster)
        {
            return Multiplicacion(left, rigth);
        }

        if (type == Token.TokenType.Div)
        {
            return Division(left, rigth);
        }

        if (type == Token.TokenType.Mod)
        {
            return Modulo(left, rigth);
        }

        if (type == Token.TokenType.IgualIgual)
        {
            return IgualIgual(left, rigth);
        }

        if (type == Token.TokenType.Diferente)
        {
            return NoIgual(left, rigth);
        }

        if (type == Token.TokenType.And)
        {
            return And(left, rigth);
        }

        if (type == Token.TokenType.Or)
        {
            return Or(left, rigth);
        }

        if (type == Token.TokenType.Mayor)
        {
            return Mayor(left, rigth);
        }

        if (type == Token.TokenType.MayorIgual)
        {
            return MayorIgual(left, rigth);
        }

        if (type == Token.TokenType.Menor)
        {
            return Menor(left, rigth);
        }

        if (type == Token.TokenType.MenorIgual)
        {
            return MenorIgual(left, rigth);
        }

        return null!;
    }

    private object IgualIgual(object left, object rigth)
    {
        if (left is double && rigth is double)
        {
            return (double)left == (double)rigth;
        }

        if (left is bool && rigth is bool)
        {
            return (bool)left == (bool)rigth;
        }

        if (left is string && rigth is string)
        {
            return (string)left == (string)rigth;
        }

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '==' cannot be used between " + left + " and  " + rigth));
        return null!;
    }

    private object NoIgual(object left, object rigth)
    {
        if (left is double && rigth is double)
        {
            return (double)left != (double)rigth;
        }

        if (left is bool && rigth is bool)
        {
            return (bool)left != (bool)rigth;
        }

        if (left is string && rigth is string)
        {
            return (string)left != (string)rigth;
        }

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '!=' cannot be used between " + left + " and " + rigth));
        return null!;
    }

    private object MenorIgual(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left <= (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '<=' cannot be used between " + left + " and " + rigth));
        return null!;
    }

    private object MayorIgual(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left >= (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '>=' cannot be used between " + left + " and " + rigth));
        return null!;

    }

    private object Menor(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left < (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '<' cannot be used between " + left + " and " + rigth));
        return null!;

    }

    private object Mayor(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left > (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '>' cannot be used between " + left + " and " + rigth));
        return null!;

    }

    private object And(object left, object rigth)
    {
        if (left is bool && rigth is bool)

            return (bool)left && (bool)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '&&' cannot be used between " + left + " and " + rigth));
        return null!;

    }

    private object Or(object left, object rigth)
    {
        if (left is bool && rigth is bool)

            return (bool)left || (bool)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '||' cannot be used between " + left + " and " + rigth));
        return null!;
    }

    private object Suma(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left + (double)rigth;

        if (left is Seq l && rigth is Seq r)
        {
            if (l is null) return null!;
            else
            {
                return l.SumaSeq(r);
            }
        }
        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '+' cannot be used between " + left + " and " + rigth));
        return null!;
    }

    private object Resta(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left - (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '-' cannot be used between " + left + " and " + rigth));
        return null!;
    }

    private object Multiplicacion(object left, object rigth)
    {
        if (left is double  && rigth is double )

            return Convert.ToDouble(left)*Convert.ToDouble(rigth);

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '*' cannot be used between " + left + " and " + rigth));
        return null!;

    }

    private object Division(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left / (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '/' cannot be used between " + left + " and " + rigth));
        return null!;

    }

    private object Modulo(object left, object rigth)
    {
        if (left is double && rigth is double)

            return (double)left % (double)rigth;

        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '%' cannot be used between " + left + " and " + rigth));
        return null!;
    }

}