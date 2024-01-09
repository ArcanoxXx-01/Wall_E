
namespace Wall_E
{
    public class Parser
    {
        public List<Token> tokens { get; private set; }
        public List<Import> imports { get; private set; }
        int current = 0;
        int currentLine = 1;
        public List<Expression> exprs { get; private set; }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            imports = new();
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

                            Function exp = new Function(name, arguments, body);

                            Functions.AddFun(name, exp);

                            exprs.Add(exp);

                            if (match(Token.TokenType.EOF)) return;

                            Parse();
                            return;
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
            Expression expr = range();

            if (match(Token.TokenType.Coma))
            {
                if (expr is Statement)
                {
                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the sequence in line " + currentLine + " in " + current);
                }

                List<Expression> args = new() { expr };

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

                Sequence seq = new Sequence(args);

                if (match(Token.TokenType.Asign))
                {
                    advance();

                    int count = current;

                    Expression value = expression();

                    if (value is Statement)
                    {
                        throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as value of the sequence in line " + currentLine + " in " + current);
                    }

                    if (value is Sequence)
                    {
                        if (tokens[current - 1].type != Token.TokenType.LlaveCerrada && tokens[count].type != Token.TokenType.LlaveAbierta)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Sequence must be inclosed in brances , in line " + currentLine);
                        }
                    }

                    return new DecSequence(seq, value);
                }

                return seq;

            }

            return expr;
        }

        private Expression range()
        {
            Expression expr = logical();

            if (match(Token.TokenType.ThreePoints))
            {
                advance();

                if (match(Token.TokenType.LlaveCerrada))
                {
                    return new Rango(expr);
                }

                else
                {
                    Expression final = logical();
                    if (match(Token.TokenType.LlaveCerrada))
                    {
                        return new Rango(expr, final);
                    }

                    else
                    {
                        throw new ERROR(ERROR.ErrorType.SyntaxError, " Sequences must be inclosed in brances , in line " + currentLine);
                    }
                }
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

                expr = new Binary(left, operador, rigth);
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

                expr = new Binary(left, operador, rigth);
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

                expr = new Binary(left, operador, rigth);
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

                expr = new Binary(left, operador, rigth);
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

                expr = new Binary(left, operador, rigth);
            }

            return expr;
        }

        private Expression unary()
        {
            if (match(Token.TokenType.Menos))
            {
                Token operador = advance();

                Expression rigth = logical();

                return new Unary(operador, rigth);
            }

            return import();
        }

        private Expression import()
        {
            if (match(Token.TokenType.Import))
            {
                advance();
                string path = (string)tokens[current].value;
                consume(Token.TokenType.String, " Expected string path after expression in line " + currentLine + " in " + current);
                Import import = new Import(path);
                imports.Add(import);
                return import;
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

                return new IfStatement(condicion, thenBody, elseBody);
            }

            return letStatement();
        }

        private Expression letStatement()
        {

            if (match(Token.TokenType.Let))
            {
                advance();

                List<Expression> letBody = Variables();

                consume(Token.TokenType.In, " Expected 'in' after expression in line " + currentLine + " in " + current);

                Expression inBody = expression();

                return new LetIn(letBody, inBody);
            }

            return color();
        }

        private List<Expression> Variables()
        {
            List<Expression> answer = new List<Expression>();

            while (!match(Token.TokenType.In))
            {
                Expression expr = expression();
                if (expr is DecSequence s)
                {
                    answer.Add(s);
                    consume(Token.TokenType.PuntoYComa, " Expect ';' or 'in' after expression in line " + currentLine + " in " + current);
                    continue;
                }

                else if (expr is Assign a)
                {
                    answer.Add(a);
                    consume(Token.TokenType.PuntoYComa, " Expect ';' or 'in' after expression in line " + currentLine + " in " + current);
                    continue;
                }
                else if (expr is VarDeclaration v)
                {
                    answer.Add(v);
                    consume(Token.TokenType.PuntoYComa, " Expect ';' or 'in'  after expression in line " + currentLine + " in " + current);
                    continue;
                }
                else
                {
                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Expected variable declaration as argument in line " + currentLine + " in " + current);
                }
                // Token name = new Token(Token.TokenType.Identificador, "");

                // if (match(Token.TokenType.Identificador))
                // {
                //     name = advance();
                // }

                // else
                // {
                //     throw new ERROR(ERROR.ErrorType.SyntaxError, " Expect a variable name after expression in line " + currentLine + " in " + current);
                // }

                // consume(Token.TokenType.Asign, " Expect '=' after variable name in line " + currentLine + " in " + current);

                // Expression expr = expression();

                // if (expr is Statement)
                // {
                //     throw new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression as assign of value in line " + currentLine + " in " + current);
                // }
            }

            return answer;
        }


        private Expression color()
        {
            if (match(Token.TokenType.Color))
            {
                advance();
                string color = (string)tokens[current].value;
                consume(Token.TokenType.Identificador, " Expected identify as a color in line " + currentLine + " in " + current);
                return new Colors(color);
            }

            if (match(Token.TokenType.Restore))
            {
                advance();
                return new Restore();
            }

            return call();
        }

        private Expression call()
        {
            if (match(Token.TokenType.Identificador))
            {
                if (Functions.Contains((string)tokens[current].value))
                {
                    string name = (string)advance().value;

                    Function function = Functions.Get(name);

                    if (name == "draw")
                    {
                        if (match(Token.TokenType.LlaveAbierta))
                        {
                            Expression auxseq = primary();

                            if (auxseq is Statement)
                            {
                                throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the function call in line " + currentLine + " in " + current);
                            }

                            if (auxseq is Sequence)
                            {
                                Sequence seq = (Sequence)auxseq;

                                if (match(Token.TokenType.Asign))
                                {
                                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                                }

                                return new Call(name, seq, function);
                            }

                            if (auxseq is not null)
                            {
                                List<Expression> list = new() { auxseq };
                                Sequence seq = new Sequence(list);

                                if (match(Token.TokenType.Asign))
                                {
                                    throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                                }

                                return new Call(name, seq, function);
                            }
                        }

                        int count = current;

                        Expression aux = expression();

                        if (aux is Statement)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the function call in line " + currentLine + " in " + current);
                        }

                        if (aux is Sequence)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Expected '{' before expression in line " + currentLine + " in " + count);
                        }

                        List<Expression> list1 = new() { aux };

                        Sequence sequence = new Sequence(list1);

                        string message = "";

                        if (match(Token.TokenType.String))
                        {
                            message += advance().value;
                        }

                        if (match(Token.TokenType.Asign))
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                        }

                        return new Call(name, sequence, function, message);

                    }

                    if (match(Token.TokenType.ParentesisAbierto))
                    {
                        Expression expr = primary();

                        if (expr is Statement)
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Cannot make a statement as arguments of the function call in line " + currentLine + " in " + current);
                        }

                        if (expr is Sequence s)
                        {
                            if (match(Token.TokenType.Asign))
                            {
                                throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                            }

                            return new Call(name, s, function);
                        }

                        List<Expression> list2 = new() { expr };

                        Sequence seq = new Sequence(list2);

                        if (match(Token.TokenType.Asign))
                        {
                            throw new ERROR(ERROR.ErrorType.SyntaxError, " Functions cannot be redefined");
                        }

                        return new Call(name, seq, function);


                    }

                    current--;
                }
            }
            return stm();
        }
        Expression stm()
        {
            if (match(Token.TokenType.Identificador))
            {
                string[] b = { "sequence" };

                string[] a = { "point", "line", "segment", "circle", "ray", "arc", "color" };

                if (checkValue(a))
                {
                    string type = (string)advance().value;

                    if (checkValue(b))
                    {
                        type += " " + b[0];
                        advance();
                    }

                    string name = (string)tokens[current].value;

                    consume(Token.TokenType.Identificador, " Expected identifier as a variable name in line " + currentLine + " in " + current);

                    return new VarDeclaration(type, name);
                }

            }

            return var();
        }

        private Expression var()
        {
            if (match(Token.TokenType.Identificador))
            {
                Token name = advance();

                if ((current == 1 || tokens[current - 2].type == Token.TokenType.PuntoYComa || tokens[current - 2].type == Token.TokenType.Let) && match(Token.TokenType.Asign))
                {
                    advance();

                    Expression value = expression();

                    if (value is DecSequence || value is VarDeclaration || value is Import)
                    {
                        throw new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression as assign of value in line " + currentLine + " in " + current);
                    }

                    return new Assign(name, value);
                }

                return new Variable(name);
            }

            return primary();
        }



        private Expression primary()
        {
            Token.TokenType[] a = { Token.TokenType.Number, Token.TokenType.String };

            if (match(a))
            {
                return new Literal(advance().value);
            }

            if (match(Token.TokenType.UnderScore))
            {
                return new Variable(advance());
            }

            if (match(Token.TokenType.ParentesisAbierto))
            {
                advance();

                if (match(Token.TokenType.ParentesisCerrado))
                {
                    advance();
                    List<Expression> args = new();
                    return new Sequence(args);
                }

                Expression expr = expression();

                consume(Token.TokenType.ParentesisCerrado, " Missing ')' after expression in line " + currentLine + " in " + current);

                return expr;
            }

            if (match(Token.TokenType.LlaveAbierta))
            {
                advance();

                if (match(Token.TokenType.LlaveCerrada))
                {
                    advance();
                    List<Expression> args = new();
                    return new Sequence(args);
                }

                Expression expr = expression();

                consume(Token.TokenType.LlaveCerrada, " Missing '}' after expression in line " + currentLine + " in " + current);

                return expr;
            }

            throw new ERROR(ERROR.ErrorType.SyntaxError, " Invalid expression " + advance().type + " at line " + currentLine + " in " + current);
        }
    }
}