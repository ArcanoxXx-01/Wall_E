namespace Wall_E;
public class DecSequence : Statement
{
    public Sequence seq { get; private set; }
    public Expression value { get; private set; }
    public DecSequence(Sequence seq, Expression value)
    {
        this.seq = seq;
        this.value = value;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        object value = Evaluator.GetValue(this.value, values);
        List<Expression> seq = this.seq.arguments;

        if (value is Seq s)
        {
            for (int i = 0; i < seq.Count; i++)
            {
                if (seq[i] is Variable v)
                {
                    Token tok = v.name;
                    if (!values.ContainsKey((string)tok.value))
                    {
                        if (tok.type != Token.TokenType.UnderScore)
                        {
                            if (i == seq.Count - 1)
                            {
                                if (s.count > i || s.count == -1)
                                {
                                    values.Add((string)tok.value, s.Rest(i));
                                    continue;
                                }
                                else
                                {
                                    values.Add((string)tok.value, null!);
                                    continue;
                                }
                            }

                            else
                            {
                                if (s.count > i||s.count==-1)
                                {
                                    values.Add((string)tok.value, s[i]);
                                    continue;
                                }

                                else
                                {
                                    values.Add((string)tok.value, null!);
                                    continue;
                                }
                            }
                        }
                        else continue;
                    }
                    else
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + tok.value + " already has been declared"));
                        return null!;
                    }
                }
                else
                {
                    Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression " + seq[i] + " sequences statement only receives variables as arguments "));
                    return null!;
                }

            }
        }

        else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Expected sequence as a value of the expression"));
        return null!;
    }

}