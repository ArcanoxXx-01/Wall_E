
using System.Diagnostics;

namespace Wall_E
{
    public abstract class Statement : Expression
    {
        public class VarDeclaration : Statement
        {
            public string type { get; private set; }
            public string name { get; private set; }

            public VarDeclaration(string type, string name)
            {
                this.type = type;
                this.name = name;
            }
            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                if (type == "point")
                {
                    Point f1 = Evaluator.random.GeneratePoint();
                    if (values.ContainsKey(name))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
                    }
                    values.Add(name, (f1.GetType(), f1));
                }
                if (type == "line")
                {
                    Line f1 = Evaluator.random.GenerateLine();
                    if (values.ContainsKey(name))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
                    }
                    values.Add(name, (f1.GetType(), f1));
                }
                if (type == "segment")
                {
                    Segment f1 = Evaluator.random.GenerateSegment();
                    if (values.ContainsKey(name))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
                    }
                    values.Add(name, (f1.GetType(), f1));
                }
                if (type == "ray")
                {
                    Ray f1 = Evaluator.random.GenerateRay();
                    if (values.ContainsKey(name))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
                    }
                    values.Add(name, (f1.GetType(), f1));
                }
                if (type == "circle")
                {
                    Circle f1 = Evaluator.random.GenerateCircle();
                    if (values.ContainsKey(name))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
                    }
                    values.Add(name, (f1.GetType(), f1));
                }
                if (type == "arc")
                {
                    Arc f1 = Evaluator.random.GenerateArc();
                    if (values.ContainsKey(name))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
                    }
                    else values.Add(name, (f1.GetType(), f1));
                }
                // if (type == "sequence")
                // {

                // }
                // if (type == "point sequence")
                // {

                // }
                // if (type == "line sequence")
                // {

                // }
                // if (type == "segment sequence")
                // {

                // }
                // if (type == "ray sequence")
                // {

                // }
                // if (type == "circle sequence")
                // {

                // }
                // if (type == "arc sequence")
                // {

                // }
                return null!;
            }

        }

        public class DecSequence : Statement
        {
            public Sequence seq { get; private set; }
            public Expression value { get; private set; }
            public DecSequence(Sequence seq, Expression value)
            {
                this.seq = seq;
                this.value = value;
            }

            // public override object Visit(Dictionary<string, (Type, object)> values)
            // {
            //     object value= Evaluator.GetValue(this.value,values);
            // }

        }

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

            public override object Visit(Dictionary<string, (Type, object)> values)
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

                Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Expected bool as a value of condicion if"));
                return null!;
            }
        }

        public class LetIn : Statement
        {
            public List<Assign> letBody { get; private set; }
            public Expression inBody { get; private set; }
            public Dictionary<string, (Type, object)> variables;
            public LetIn(List<Assign> letBody, Expression inBody)
            {
                this.letBody = letBody;
                this.inBody = inBody;
                variables = new();
            }

            public override object Visit(Dictionary<string, (Type, object)> values)
            {
                foreach (Assign a in letBody)
                {
                    if (variables.ContainsKey(a.name.value) || values.ContainsKey(a.name.value))
                    {
                        Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + a.name.value + " already has been declared "));
                    }
                    object value = Evaluator.GetValue(a.value, values);
                    variables.Add(a.name.value, (value.GetType(), value));
                    values.Add(a.name.value, (value.GetType(), value));
                }

                object letbody = Evaluator.GetValue(inBody, values);
                foreach (var x in variables)
                {
                    values.Remove(x.Key);
                }

                return letbody;
            }
        }

        public class Function : Statement
        {
            public string name { get; private set; }
            public List<Token> arguments { get; private set; }
            public Expression funBody { get; private set; }

            public Function(string name, List<Token> arguments, Expression funBody)
            {
                this.name = name;
                this.arguments = arguments;
                this.funBody = funBody;
            }

        }
    }
}