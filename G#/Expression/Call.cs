
namespace Wall_E;
public class Call : Expression
{
    public string name { get; private set; }
    public Function fun { get; private set; }
    public Sequence seq { get; private set; }
    public string message { get; private set; }
    public Call(string name, Sequence seq, Function fun, string message = null!)
    {
        this.name = name;
        this.fun = fun;
        this.seq = seq;
        this.message = message;
    }

    public override object Visit(Dictionary<string, object> values)
    {
        object obj = seq.Visit(values);

        List<object> value = ((Seq)obj).values;
        if (name == "point")
        {
            return FunPoint(value);
        }
        if (name == "line")
        {
            return FunLine(value);
        }
        if (name == "segment")
        {
            return FunSegment(value);
        }
        if (name == "circle")
        {
            return FunCircle(value);
        }
        if (name == "ray")
        {
            return FunRay(value);
        }
        if (name == "arc")
        {
            return FunArc(value);
        }
        if (name == "draw")
        {
            return FunDraw(value);
        }
        if (name == "measure")
        {
            return Measure(value);
        }
        if (name == "intersect")
        {
            return Intersect(value);
        }
        if (name == "points")
        {
            return FunPoints(value);
        }
        else return VisitFuncion(value, Functions.Get(name));

    }

    private object FunPoint(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'point' only receives two params as argument "));
            return null!;
        }

        else
        {
            if (args[0] is double && args[1] is double)
            {
                return new Point((int)(double)args[0], (int)(double)args[1]);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'point' only receives floats as arguments"));
            return null!;
        }
    }

    private object FunLine(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'line' only receives two params as argument "));
            return null!;
        }

        else
        {
            if (args[0] is Point && args[1] is Point)
            {
                return new Line((Point)args[0], (Point)args[1]);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'line' only receives points as arguments"));
            return null!;
        }
    }

    private object FunSegment(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'segment' only receives two params as argument "));
            return null!;
        }

        else
        {
            if (args[0] is Point && args[1] is Point)
            {
                return new Segment((Point)args[0], (Point)args[1]);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'segment' only receives points as arguments"));
            return null!;
        }
    }

    private object FunCircle(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'circle' only receives two params as argument "));
            return null!;
        }

        else
        {
            if (args[0] is Point && args[1] is double)
            {
                return new Circle((Point)args[0], (int)(double)args[1]);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'circle' only receives a Point and a float as arguments"));
            return null!;
        }
    }

    private object FunRay(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'ray' only receives two params as argument "));
            return null!;
        }

        else
        {
            if (args[0] is Point && args[1] is Point)
            {
                return new Ray((Point)args[0], (Point)args[1]);

            }
            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'ray' only receives points as arguments"));
            return null!;
        }
    }

    private object FunArc(List<object> args)
    {
        if (args.Count != 4)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'arc' only receives four params as argument "));
            return null!;
        }

        else
        {
            if (args[0] is Point && args[1] is Point && args[2] is Point && args[3] is double)
            {
                return new Arc((Point)args[0], (Point)args[1], (Point)args[2], (int)(double)args[3]);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'arc' only receives three points and a float as arguments"));
            return null!;
        }
    }

    private object FunDraw(List<object> args)
    {
        foreach (object obj in args)
        {
            if (obj is Seq seq)
            {
                if (seq.count == -1)
                {
                    Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Object " + seq + " is not drawable. Infinites sequences are not drawables "));
                }
                else
                {
                    for (int i = 0; i < seq.count; i++)
                    {
                        if (seq[i] is IDrawable idraw)
                        {
                            continue;
                        }
                        else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Object " + seq[i] + " in sequence " + obj + " is not drawable"));
                        return null!;
                    }
                    Evaluator.Dibuja.Add((seq, Evaluator.colors.Peek()));
                }
            }
            if (obj is IDrawable a)
            {
                Evaluator.Dibuja.Add((a, Evaluator.colors.Peek()));
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Object " + obj + " is not drawable"));
        }
        return null!;
    }

    private object Intersect(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'intersect' only receives two params as arguments "));
            return null!;
        }
        else
        {
            if (args[0] is Figure a && args[1] is Figure b)
            {
                List<object> obj = new();
                foreach (var x in a.Intersect(b))
                {
                    obj.Add(x);
                }
                return new Seq(obj, false);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " It's not possible to intersect object " + args[0] + " with " + args[1]));
            return null!;
        }
    }

    private object Measure(List<object> args)
    {
        if (args.Count != 2)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'measure' only receives two params as arguments "));
            return null!;
        }
        else
        {
            if (args[0] is Point a && args[1] is Point b)
            {
                return a.Measure(b);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'measure' only receives points as arguments "));
            return null!;
        }

    }

    private object FunPoints(List<object> args)
    {
        if (args.Count != 1)
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'points' only receives one param as arguments "));
            return null!;
        }
        else
        {
            if (args[0] is Figure f)
            {
                return Evaluator.random.Points(f);
            }

            else Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Function 'points' only receives a figure as argument "));
            return null!;
        }
    }

    public object VisitFuncion(List<object> valores, Function funcion)
    {
        Dictionary<string, object> value = new Dictionary<string, object>();

        if (funcion.arguments.Count == valores.Count)
        {
            for (int i = 0; i < valores.Count; i++)
            {
                value.Add((string)funcion.arguments[i].value, valores[i]);
            }

            return Evaluator.GetValue(funcion.funBody, value);
        }

        else
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " function " + funcion.name + " only receives " + funcion.arguments.Count + " parameter as arguments"));
            return null!;
        }
    }
}