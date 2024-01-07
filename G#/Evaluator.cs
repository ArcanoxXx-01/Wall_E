namespace Wall_E
{
    public class Evaluator
    {
        public Evaluator(IPaint Paint, IAplication App, string Code)
        {   
            Functions f=new Functions();
            this.Code = Code;
            this.Paint = Paint;
            this.App = App;
            expressions = GetExpressions(Code);
            values = new();
            Dibuja = new();
            errors = new();
            colors = new();
            colors.Push(Color.Black);
            if (expressions is not null)
            {
                GetValues(expressions, values);
            }
        }

        public static Stack<Color> colors = new();
        public static Randoms random = new Randoms();
        public static List<(IDrawable, Color)> Dibuja = new();
        public static List<ERROR> errors = new();
        public Dictionary<string, object> values { get; set; }
        public string Code { get; private set; }
        public List<Expression>? expressions { get; private set; }
        public IPaint Paint = null!;
        public IAplication App { get; private set; }

        private List<Expression> GetExpressions(string Code)
        {
            Functions.Fun();

            Tokenizer tokenizer = new Tokenizer(Code);
            if (tokenizer.errores.Count != 0)
            {
                App.PrintErrors(tokenizer.errores);
                return null!;
            }
            else
            {
                Parser parser = new Parser(tokenizer.tokens);
                try
                {
                    parser.Parse();
                }

                catch (ERROR error)
                {
                    App.PrintError(error);
                    return null!;
                }

                return parser.exprs;
            }
        }

        private void GetValues(List<Expression> expressions, Dictionary<string, object> variables)
        {
            foreach (Expression expr in expressions)
            {
                GetValue(expr, variables);
            }
        }

        public static object GetValue(Expression expr, Dictionary<string, object> variables)
        {
            if (expr is Call call)
            {
                return call.Visit(variables);
            }
            if (expr is Variable variable)
            {
                return variable.Visit(variables);
            }
            if (expr is Literal literal)
            {
                return literal.Visit(variables);
            }
            if (expr is IfStatement ifStatement)
            {
                return ifStatement.Visit(variables);
            }
            if (expr is VarDeclaration varDeclaration)
            {
                return varDeclaration.Visit(variables);
            }
            if (expr is Binary binary)
            {
                return binary.Visit(variables);
            }
            if (expr is Assign assign)
            {
                return assign.Visit(variables);
            }
            if (expr is Unary unary)
            {
                return unary.Visit(variables);
            }
            if (expr is DecSequence decseq)
            {
                return decseq.Visit(variables);
            }
            if (expr is Sequence seq)
            {
                return seq.Visit(variables);
            }
            if (expr is Colors color)
            {
                return color.Visit(variables);
            }
            if (expr is Restore restore)
            {
                return restore.Visit(variables);
            }
            if (expr is LetIn let)
            {
                return let.Visit(variables);
            }
            if(expr is Rango rango)
            {
                return rango.Visit(variables);
            }
            return null!;
        }
    }
}