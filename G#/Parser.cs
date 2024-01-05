
namespace Wall_E
{
    public class Parser
    {
        public List<Token> tokens { get; private set; }
        int current = 0;
        int currentLine = 1;
        public List<Expression> exprs { get; private set; }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            exprs = new();
        }

        private Token advance()
        {
            if (tokens[current].type != Token.TokenType.EOF)
                current++;

            return tokens[current - 1];
        }

        private bool check(Token.TokenType type)
        {
            if (tokens[current].type == type) return true;

            else return false;
        }

        private bool match(Token.TokenType type)
        {
            if (check(type)) return true;

            else return false;
        }

        private bool match(Token.TokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (check(types[i])) return true;
            }

            return false;
        }

        private bool checkValue(string[] token)
        {
            foreach (var tok in token)
            {
                if (tok == (string)tokens[current].value) return true;
            }

            return false;
        }

        private Token consume(Token.TokenType type, string mensaje)
        {
            if (check(type)) return advance();

            throw new ERROR(ERROR.ErrorType.SyntaxError, mensaje);
        }

        public void Parse()
        {
            if (match(Token.TokenType.Identificador))
            {
                int aux = current;

                string name = (string)advance().value;

                if (match(Token.TokenType.ParentesisAbierto))
                {
                    advance();

                    List<Token> arguments = new();

                    if (!Functions.Contains(name))
                    {
                        while (match(Token.TokenType.Identificador))
                        {
                            Token arg = advance();

                            arguments.Add(arg);

                            if (!match(Token.TokenType.ParentesisCerrado))
                                consume(Token.TokenType.Coma, " Expected ',' or ')' after expression in line " + currentLine + " " + current);
                        }
                        consume(Token.TokenType.ParentesisCerrado, " Missing ')' after expression in line " + currentLine + " " + current);

                        if (match(Token.TokenType.Asign))
                        {
                            consume(Token.TokenType.Asign, " Expected '=' after expression in line " + currentLine + " " + current);

                            if (Functions.Contains(name))
                            {
                                throw new ERROR(ERROR.ErrorType.SyntaxError, "Functions cannot be redefined");
                            }
                            Functions.AddFun(name);

                            Expression body = expression();

                            consume(Token.TokenType.PuntoYComa, " Expected ';' after expression in line " + currentLine + " " + current);

                            Statement.Function exp = new Statement.Function(name, arguments, body);

                            Functions.AddFun(name, exp);

                            exprs.Add(exp);

                            if (match(Token.TokenType.EOF)) return;

                            Parse();
                        }
                    }
                }

                current = aux;
            }

            Expression expr = expression();

            consume(Token.TokenType.PuntoYComa, " Expected ';' after expression in line " + currentLine + " " + current);

            exprs.Add(expr);

            if (match(Token.TokenType.EOF)) return;

            currentLine++;

            Parse();
        }

        private Expression expression()
        {
            return seq();
        }

        private Expression seq()
        {
            Expression expr = logical();

            if (match(Token.TokenType.Coma))
            {
                if (expr is Statement)
                {
                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the sequence in line " + currentLine + " in " + current);
                }

                List<Expression> args = new()
            {
                expr
            };
                while (match(Token.TokenType.Coma))
                {
                    advance();

                    Expression arg = logical();

                    if (arg is Statement)
                    {
                        throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the sequence in line " + currentLine + " in " + current);
                    }

                    args.Add(arg);
                }

                Expression.Sequence seq = new Expression.Sequence(args);

                if (match(Token.TokenType.Asign))
                {
                    advance();

                    int count = current;

                    Expression value = expression();

                    if (value is Statement)
                    {
                        throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as value of the sequence in line " + currentLine + " in " + current);
                    }

                    if (value is Expression.Sequence)
                    {
                        if (tokens[current - 1].type != Token.TokenType.LlaveCerrada && tokens[count].type != Token.TokenType.LlaveAbierta)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Sequence must be inclosed in brances , in line " + currentLine);
                        }
                    }

                    return new Statement.DecSequence(seq, value);
                }

                return seq;
            }

            return expr;
        }

        private Expression logical()
        {
            Expression expr = comparison();

            Token.TokenType[] a = { Token.TokenType.Or, Token.TokenType.And };

            if (match(a))
            {
                Token operador = advance();

                Expression left = expr;

                Expression rigth = comparison();

                expr = new Expression.Binary(left, operador, rigth);
            }

            return expr;
        }

        private Expression comparison()
        {
            Expression expr = term();

            Token.TokenType[] a = { Token.TokenType.Mayor, Token.TokenType.MayorIgual, Token.TokenType.Menor, Token.TokenType.MenorIgual, Token.TokenType.Diferente, Token.TokenType.IgualIgual };

            if (match(a))
            {
                Token operador = advance();

                Expression left = expr;

                Expression rigth = term();

                expr = new Expression.Binary(left, operador, rigth);
            }

            return expr;
        }

        private Expression term()
        {
            Expression expr = factor();

            Token.TokenType[] a = { Token.TokenType.Menos, Token.TokenType.Suma };

            if (match(a))
            {
                Token operador = advance();

                Expression left = expr;

                Expression rigth = factor();

                expr = new Expression.Binary(left, operador, rigth);
            }

            return expr;
        }

        private Expression factor()
        {
            Expression expr = mod();

            Token.TokenType[] a = { Token.TokenType.Aster, Token.TokenType.Div };

            if (match(a))
            {
                Token operador = advance();

                Expression left = expr;

                Expression rigth = mod();

                expr = new Expression.Binary(left, operador, rigth);
            }

            return expr;
        }

        private Expression mod()
        {
            Expression expr = unary();

            if (match(Token.TokenType.Mod))
            {
                Token operador = advance();

                Expression left = expr;

                Expression rigth = unary();

                expr = new Expression.Binary(left, operador, rigth);
            }

            return expr;
        }

        private Expression unary()
        {
            if (match(Token.TokenType.Menos))
            {
                Token operador = advance();

                Expression rigth = expression();

                return new Expression.Unary(operador, rigth);
            }

            return ifStatement();
        }

        private Expression ifStatement()
        {
            if (match(Token.TokenType.If))
            {
                advance();

                Expression condicion = expression();

                if (condicion is Statement)
                {
                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as an 'if' condicion in line " + currentLine + " in " + current);
                }

                consume(Token.TokenType.Then, " Expected 'then' after expression in line " + currentLine + " in " + current);

                Expression thenBody = expression();

                consume(Token.TokenType.Else, " Expected 'else' after if statement in line " + currentLine + " in " + current);

                Expression elseBody = expression();

                return new Statement.IfStatement(condicion, thenBody, elseBody);
            }

            return letStatement();
        }

        private Expression letStatement()
        {

            if (match(Token.TokenType.Let))
            {
                advance();

                List<Expression.Assign> letBody = Variables();

                consume(Token.TokenType.In, " Expected 'in' after expression in line " + currentLine + " in " + current);

                Expression inBody = expression();

                return new Statement.LetIn(letBody, inBody);
            }

            return call();
        }

        private List<Expression.Assign> Variables()
        {
            List<Expression.Assign> answer = new List<Expression.Assign>();

            while (!match(Token.TokenType.In))
            {
                Token name = new Token(Token.TokenType.Identificador, "");

                if (match(Token.TokenType.Identificador))
                {
                    name = advance();
                }

                else
                {
                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Expect a variable name after expression in line " + currentLine + " in " + current);
                }

                consume(Token.TokenType.Asign, " Expect '=' after variable name in line " + currentLine + " in " + current);

                Expression expr = expression();

                if (expr is Statement)
                {
                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression as assign of value in line " + currentLine + " in " + current);
                }

                consume(Token.TokenType.PuntoYComa, " Expect ';'  after expression in line " + currentLine + " in " + current);

                answer.Add(new Expression.Assign(name, expr));
            }

            return answer;
        }

        private Expression call()
        {
            if (match(Token.TokenType.Identificador))
            {
                if (Functions.Contains((string)tokens[current].value))
                {
                    string name = (string)advance().value;

                    Statement.Function function = Functions.Get(name);

                    if (name == "draw")
                    {
                        if (match(Token.TokenType.LlaveAbierta))
                        {
                            Expression auxseq = primary();

                            if (auxseq is Statement)
                            {
                                throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the function call in line " + currentLine + " in " + current);
                            }

                            if (auxseq is Expression.Sequence)
                            {
                                Expression.Sequence seq = (Expression.Sequence)auxseq;

                                if (match(Token.TokenType.Asign))
                                {
                                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                                }

                                return new Expression.Call(name, seq, function);
                            }

                            if (auxseq is not null)
                            {
                                List<Expression> list = new() { auxseq };
                                Expression.Sequence seq = new Expression.Sequence(list);

                                if (match(Token.TokenType.Asign))
                                {
                                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                                }

                                return new Expression.Call(name, seq, function);
                            }
                        }

                        int count = current;

                        Expression aux = expression();

                        if (aux is Statement)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the function call in line " + currentLine + " in " + current);
                        }

                        if (aux is Expression.Sequence)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Expected '{' before expression in line " + currentLine + " in " + count);
                        }

                        List<Expression> list1 = new() { aux };

                        Expression.Sequence sequence = new Expression.Sequence(list1);

                        string message = "";

                        if (match(Token.TokenType.String))
                        {
                            message += advance().value;
                        }

                        if (match(Token.TokenType.Asign))
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                        }

                        return new Expression.Call(name, sequence, function, message);

                    }

                    if (match(Token.TokenType.ParentesisAbierto))
                    {
                        Expression expr = primary();

                        if (expr is Statement)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the function call in line " + currentLine + " in " + current);
                        }

                        if (expr is Expression.Sequence)
                        {
                            Expression.Sequence seq = (Expression.Sequence)expr;

                            if (match(Token.TokenType.Asign))
                            {
                                throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                            }

                            return new Expression.Call(name, seq, function);
                        }

                        if (expr is not null)
                        {
                            List<Expression> list2 = new() { expr };

                            Expression.Sequence seq = new Expression.Sequence(list2);

                            if (match(Token.TokenType.Asign))
                            {
                                throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                            }

                            return new Expression.Call(name, seq, function);
                        }

                    }

                    current--;
                }
            }
            return stm();
        }

        private Expression var()
        {
            if (match(Token.TokenType.Identificador))
            {
                Token name = advance();

                if ((tokens[current - 2].type == Token.TokenType.PuntoYComa || current == 1) && match(Token.TokenType.Asign))
                {
                    advance();

                    Expression value = expression();

                    if (value is Statement)
                    {
                        throw new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression as assign of value in line " + currentLine + " in " + current);
                    }

                    return new Expression.Assign(name, value);
                }

                return new Expression.Variable(name);
            }

            return primary();
        }

        Expression stm()
        {
            if (match(Token.TokenType.Identificador))
            {
                string[] b = { "sequence" };

                string[] a = { "point", "line", "segment", "circle", "ray", "arc", "color" };

                if (checkValue(a))
                {
                    string type = advance().value;

                    if (checkValue(b))
                    {
                        type += " " + b[0];
                        advance();
                    }

                    string name = tokens[current].value;

                    consume(Token.TokenType.Identificador, " Expected identifier as a variable name in line " + currentLine + " in " + current);

                    return new Statement.VarDeclaration(type, name);
                }

            }

            return var();
        }

        private Expression primary()
        {
            Token.TokenType[] a = { Token.TokenType.Number, Token.TokenType.String };

            if (match(a))
            {
                return new Expression.Literal(advance().value);
            }

            if (match(Token.TokenType.UnderScore))
            {
                return new Expression.Variable(advance());
            }

            if (match(Token.TokenType.ParentesisAbierto))
            {
                advance();

                Expression expr = expression();

                consume(Token.TokenType.ParentesisCerrado, " Missing ')' after expression in line " + currentLine + " in " + current);

                return expr;
            }

            if (match(Token.TokenType.LlaveAbierta))
            {
                advance();

                Expression expr = expression();

                consume(Token.TokenType.LlaveCerrada, " Missing '}' after expression in line " + currentLine + " in " + current);

                return expr;
            }

            throw new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression " + advance().type + " at line " + currentLine + " in " + current);
        }
    }
}