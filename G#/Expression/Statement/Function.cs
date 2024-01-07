namespace Wall_E;
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