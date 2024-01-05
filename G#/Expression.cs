
using System.Linq.Expressions;

namespace Wall_E
{
    public abstract class Expression : IVisitor
    {
        public virtual object Visit(Dictionary<string, (Type, object)> values)
        {
            return null!;
        }
        public class Binary : Expression
        {
            public Expression rigth { get; private set; }
            public Token operador { get; private set; }
            public Expression left { get; private set; }

            public Binary(Expression rigth, Token operador, Expression left)
            {
                this.rigth = rigth;
                this.operador = operador;
                this.left = left;
            }

            public override object Visit(Dictionary<string, (Type, object)> values)
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

            public object IgualIgual(object left, object rigth)
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

            public object NoIgual(object left, object rigth)
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

            public object MenorIgual(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left <= (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '<=' cannot be used between " + left + " and " + rigth));
                return null!;
            }

            public object MayorIgual(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left >= (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '>=' cannot be used between " + left + " and " + rigth));
                return null!;

            }

            public object Menor(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left < (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '<' cannot be used between " + left + " and " + rigth));
                return null!;

            }

            public object Mayor(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left > (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '>' cannot be used between " + left + " and " + rigth));
                return null!;

            }

            public object And(object left, object rigth)
            {
                if (left is bool && rigth is bool)

                    return (bool)left && (bool)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '&&' cannot be used between " + left + " and " + rigth));
                return null!;

            }

            public object Or(object left, object rigth)
            {
                if (left is bool && rigth is bool)

                    return (bool)left || (bool)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '||' cannot be used between " + left + " and " + rigth));
                return null!;
            }

            public object Suma(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left + (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '+' cannot be used between " + left + " and " + rigth));
                return null!;
            }

            public object Resta(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left - (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '-' cannot be used between " + left + " and " + rigth));
                return null!;
            }

            public object Multiplicacion(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left * (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '*' cannot be used between " + left + " and " + rigth));
                return null!;

            }

            public object Division(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left / (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '/' cannot be used between " + left + " and " + rigth));
                return null!;

            }

            public object Modulo(object left, object rigth)
            {
                if (left is double && rigth is double)

                    return (double)left % (double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '%' cannot be used between " + left + " and " + rigth));
                return null!;
            }

        }

        public class Unary : Expression
        {
            public Token operador { get; private set; }
            public Expression rigth { get; private set; }
            public Unary(Token operador, Expression rigth)
            {
                this.operador = operador;
                this.rigth = rigth;
            }

            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                object rigth = Evaluator.GetValue(this.rigth, values);

                if (rigth is double) return -(double)rigth;

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Operator '-' cannot be before expression " + rigth));
                return null!;
            }
        }

        public class Assign : Expression
        {
            public Token name { get; private set; }
            public Expression value { get; private set; }
            public Assign(Token name, Expression value)
            {
                this.name = name;
                this.value = value;
            }

            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                object value = Evaluator.GetValue(this.value, values);
                Type type = value.GetType();
                values.Add(name.value, (type, value));
                return null!;
            }
        }

        public class Literal : Expression
        {
            public object value { get; private set; }
            public Literal(object value)
            {
                this.value = value;
            }
            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                return value;
            }
        }

        public class Variable : Expression
        {
            public Token name { get; private set; }
            public Variable(Token name)
            {
                this.name = name;
            }
            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                if (values is null)
                {
                    Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name.value + " does not have a value assigned"));
                }

                else
                {
                    if (values.ContainsKey(name.value))
                    {
                        return name.value;
                    }
                }

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name.value + " does not have a value assigned"));
                return null!;
            }
        }

        public class Call : Expression
        {
            public string name { get; private set; }
            public Statement.Function fun { get; private set; }
            public Sequence seq { get; private set; }
            public string message { get; private set; }
            public Call(string name, Sequence seq, Statement.Function fun, string message = null!)
            {
                this.name = name;
                this.fun = fun;
                this.seq = seq;
                this.message = message;
            }

            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                seq.Visit(values);
                List<object> value= seq.list;
                if(name=="point")
                {

                }
            }

            private object FunPoint(Dictionary<string, (Type, object)> values)
            {
                return null!;
            }

            private object FunLine(Dictionary<string, (Type, object)> values)
            {
                return null!;
            }

            private object FunSegment(Dictionary<string, (Type, object)> values)
            {
                return null!;
            }

            private object FunCircle(Dictionary<string, (Type, object)> values)
            {
                return null!;
            }

            private object FunRay(Dictionary<string, (Type, object)> values)
            {
                return null!;
            }
            private object FunArc(Dictionary<string, (Type, object)> values)
            {
                return null!;
            }
        }
        public class Sequence : Expression
        {
            public List<Expression> arguments { get; private set; }
            public List<object> list;
            public Sequence(List<Expression> arguments)
            {
                this.arguments = arguments;
                list=new();
            }

            public override object Visit(Dictionary<string, (Type, object)> values)
            {        
                foreach(var x in arguments)
                {
                    object val=Evaluator.GetValue(x,values);
                    list.Add(val);
                }
                return null!;
            }
        }

    }

}