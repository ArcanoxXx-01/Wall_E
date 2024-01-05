namespace Wall_E
{
    public class Evaluator
    {
        public Evaluator(IPaint Paint, IAplication App, string Code)
        {
            this.Code = Code;
            this.Paint = Paint;
            this.App = App;
            expressions = GetExpressions(Code);
            values = new();
            if (expressions is not null)
            {
                GetValues(expressions, values);
            }
        }
        public static Randoms random =new Randoms();
        public static List<ERROR> errors = new();
        public Dictionary<string, (Type, object)> values;
        public string Code { get; private set; }
        public List<Expression>? expressions { get; private set; }
        public IPaint Paint { get; private set; }
        public IAplication App { get; private set; }

        private List<Expression> GetExpressions(string Code)
        {
            Functions.Fun();
            Tokenizer tokenizer = new Tokenizer(Code);
            if (tokenizer.errores.Count != 0) App.PrintErrors(tokenizer.errores);
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


        private void GetValues(List<Expression> expressions, Dictionary<string, (Type, object)> variables)
        {
            foreach (Expression expr in expressions)
            {
                GetValue(expr, variables);
            }
        }

        public static object GetValue(Expression expr, Dictionary<string, (Type, object)> variables)
        {
            return null!;
        }
    }
}