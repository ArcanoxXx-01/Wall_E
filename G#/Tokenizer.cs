namespace Wall_E
{
    public class Tokenizer
    {
        public List<Token> tokens { get; private set; }

        public List<ERROR> errores { get; private set; }

        public Tokenizer(string input)
        {
            errores = new();
            tokens = new();
            GetTokens(input);
        }

        private void GetTokens(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ') continue;

                if (input[i] == ';')
                {
                    tokens.Add(new Token(Token.TokenType.PuntoYComa, ""));
                    continue;
                }

                if (input[i] == '_')
                {
                    tokens.Add(new Token(Token.TokenType.UnderScore, ""));
                    continue;
                }

                if (input[i] == ',')
                {
                    tokens.Add(new Token(Token.TokenType.Coma, ""));
                    continue;
                }

                if (input[i] == '\n') continue;

                if (input[i] == '(')
                {
                    tokens.Add(new Token(Token.TokenType.ParentesisAbierto, ""));
                    continue;
                }

                if (input[i] == ')')
                {
                    tokens.Add(new Token(Token.TokenType.ParentesisCerrado, ""));
                    continue;
                }

                if (input[i] == '{')
                {
                    tokens.Add(new Token(Token.TokenType.LlaveAbierta, ""));
                    continue;
                }

                if (input[i] == '}')
                {
                    tokens.Add(new Token(Token.TokenType.LlaveCerrada, ""));
                    continue;
                }

                if (input[i] == '*')
                {
                    tokens.Add(new Token(Token.TokenType.Aster, ""));
                    continue;
                }

                if (input[i] == '/')
                {
                    tokens.Add(new Token(Token.TokenType.Div, ""));
                    continue;
                }

                if (input[i] == '%')
                {
                    tokens.Add(new Token(Token.TokenType.Mod, ""));
                    continue;
                }

                if (input[i] == '+')
                {
                    tokens.Add(new Token(Token.TokenType.Suma, ""));
                    continue;
                }

                if (input[i] == '-')
                {
                    tokens.Add(new Token(Token.TokenType.Menos, ""));
                    continue;
                }


                if (input[i] == '!')
                {
                    if (input[i + 1] == '=')
                    {
                        tokens.Add(new Token(Token.TokenType.Diferente, ""));
                        i++;
                        continue;
                    }

                    else errores.Add(new ERROR(ERROR.ErrorType.LexicalError, input[i] + " is not a valid token"));
                    continue;

                }

                if (input[i] == '|')
                {
                    if (input[i + 1] == '|')
                    {
                        tokens.Add(new Token(Token.TokenType.Or, ""));
                        i++;
                        continue;
                    }

                    else
                    {
                        errores.Add(new ERROR(ERROR.ErrorType.LexicalError, input[i] + " is not a valid token"));
                        continue;
                    }
                }

                if (input[i] == '&')
                {
                    if (input[i + 1] == '&')
                    {
                        tokens.Add(new Token(Token.TokenType.And, ""));
                        i++;
                        continue;
                    }

                    else
                    {
                        errores.Add(new ERROR(ERROR.ErrorType.LexicalError, input[i] + " is not a valid token"));
                        continue;
                    }
                }

                if (input[i] == '>')
                {
                    if (input[i + 1] == '=')
                    {
                        tokens.Add(new Token(Token.TokenType.MayorIgual, ""));
                        i++;
                        continue;
                    }

                    else
                    {
                        tokens.Add(new Token(Token.TokenType.Mayor, ""));
                        continue;
                    }
                }

                if (input[i] == '<')
                {
                    if (input[i + 1] == '=')
                    {
                        tokens.Add(new Token(Token.TokenType.MenorIgual, ""));
                        i++;
                        continue;
                    }

                    else
                    {
                        tokens.Add(new Token(Token.TokenType.Menor, ""));
                        continue;
                    }
                }

                if (input[i] == '=')
                {
                    if (input[i + 1] == '=')
                    {
                        tokens.Add(new Token(Token.TokenType.IgualIgual, ""));
                        i++;
                        continue;
                    }

                    else
                    {
                        tokens.Add(new Token(Token.TokenType.Asign, ""));
                        continue;
                    }
                }

                if (input[i] == '0' || input[i] == '1' || input[i] == '2' || input[i] == '3' || input[i] == '4' || input[i] == '5' || input[i] == '6' || input[i] == '7' || input[i] == '8' || input[i] == '9')
                {
                    string numero = "";
                    numero = numero + input[i];
                    int ContadorDePuntos = 0;
                    bool valido = true;
                    bool Add = false;

                    for (int j = i + 1; j < input.Length; j++)
                    {

                        if (input[j] == '0' || input[j] == '1' || input[j] == '2' || input[j] == '3' || input[j] == '4' || input[j] == '5' || input[j] == '6' || input[j] == '7' || input[j] == '8' || input[j] == '9')
                        {
                            numero += input[j];
                            i = j;
                            continue;
                        }

                        if (input[j] == '.')
                        {
                            if (input.Length > j + 2)
                            {
                                if (input[j + 1] == '.' && input[j + 2] == '.')
                                {
                                    if (ContadorDePuntos != 0)
                                    {
                                        errores.Add(new ERROR(ERROR.ErrorType.LexicalError, " Sequences of type '...' only receives ints like values"));
                                        Add = true;
                                        i = j + 2;
                                        break;
                                    }
                                    tokens.Add(new Token(Token.TokenType.Number, double.Parse(numero)));
                                    tokens.Add(new Token(Token.TokenType.ThreePoints, ""));
                                    Add = true;
                                    i = j + 2;
                                    break;
                                }
                            }

                            numero += input[j];
                            i = j;
                            ContadorDePuntos++;
                            continue;
                        }

                        if (input[j] == 'i')
                        {
                            if (input[j + 1] == 'n')
                            {
                                i = j;
                                break;
                            }

                            numero += input[j];
                            valido = false;
                        }

                        if (input[j] == '_' || input[j] == 'A' || input[j] == 'a' || input[j] == 'B' || input[j] == 'b' || input[j] == 'C' || input[j] == 'c' || input[j] == 'D' || input[j] == 'd' || input[j] == 'E' || input[j] == 'e' || input[j] == 'F' || input[j] == 'f' || input[j] == 'G' || input[j] == 'g' || input[j] == 'H' || input[j] == 'h' || input[j] == 'I' || input[j] == 'J' || input[j] == 'j' || input[j] == 'K' || input[j] == 'k' || input[j] == 'L' || input[j] == 'l' || input[j] == 'M' || input[j] == 'm' || input[j] == 'N' || input[j] == 'n' || input[j] == 'O' || input[j] == 'o' || input[j] == 'P' || input[j] == 'p' || input[j] == 'Q' || input[j] == 'q' || input[j] == 'R' || input[j] == 'r' || input[j] == 'S' || input[j] == 's' || input[j] == 'T' || input[j] == 't' || input[j] == 'U' || input[j] == 'u' || input[j] == 'V' || input[j] == 'v' || input[j] == 'w' || input[j] == 'W' || input[j] == 'x' || input[j] == 'X' || input[j] == 'Y' || input[j] == 'y' || input[j] == 'Z' || input[j] == 'z')
                        {
                            valido = false;
                            numero += input[j];
                            i = j;
                            continue;
                        }

                        break;
                    }

                    if (!valido)
                    {
                        ERROR error = new ERROR(ERROR.ErrorType.LexicalError, numero + " is not a valid token");
                        errores.Add(error);
                        continue;
                    }

                    if (ContadorDePuntos > 1)
                    {
                        ERROR error = new ERROR(ERROR.ErrorType.LexicalError, " Number " + numero + " was declared incorrectly");
                        errores.Add(error);
                        continue;
                    }
                    if (!Add)
                    {
                        tokens.Add(new Token(Token.TokenType.Number, double.Parse(numero)));
                    }
                    continue;
                }

                if (input[i] == '"')
                {
                    string a = "";
                    for (int j = i + 1; j < input.Length; j++)
                    {

                        if (input[j] == '"')
                        {
                            i = j;
                            break;
                        }

                        else
                        {
                            a += input[j];
                        }

                        if (j == input.Length - 1 && input[j] != '"')
                        {
                            ERROR error = new ERROR(ERROR.ErrorType.LexicalError, "String " + a + " was declared incorrectly");
                            errores.Add(error);
                        }

                        continue;
                    }

                    tokens.Add(new Token(Token.TokenType.String, a));
                    continue;
                }

                if (input[i] == 'A' || input[i] == 'a' || input[i] == 'B' || input[i] == 'b' || input[i] == 'C' || input[i] == 'c' || input[i] == 'D' || input[i] == 'd' || input[i] == 'E' || input[i] == 'e' || input[i] == 'F' || input[i] == 'f' || input[i] == 'G' || input[i] == 'g' || input[i] == 'H' || input[i] == 'h' || input[i] == 'I' || input[i] == 'i' || input[i] == 'J' || input[i] == 'j' || input[i] == 'K' || input[i] == 'k' || input[i] == 'L' || input[i] == 'l' || input[i] == 'M' || input[i] == 'm' || input[i] == 'N' || input[i] == 'n' || input[i] == 'O' || input[i] == 'o' || input[i] == 'P' || input[i] == 'p' || input[i] == 'Q' || input[i] == 'q' || input[i] == 'R' || input[i] == 'r' || input[i] == 'S' || input[i] == 's' || input[i] == 'T' || input[i] == 't' || input[i] == 'U' || input[i] == 'u' || input[i] == 'V' || input[i] == 'v' || input[i] == 'w' || input[i] == 'W' || input[i] == 'x' || input[i] == 'X' || input[i] == 'Y' || input[i] == 'y' || input[i] == 'Z' || input[i] == 'z')
                {
                    string a = "";
                    a += input[i];

                    for (int j = i + 1; j < input.Length; j++)
                    {
                        if (input[j] == 'A' || input[j] == 'a' || input[j] == 'B' || input[j] == 'b' || input[j] == 'C' || input[j] == 'c' || input[j] == 'D' || input[j] == 'd' || input[j] == 'E' || input[j] == 'e' || input[j] == 'F' || input[j] == 'f' || input[j] == 'G' || input[j] == 'g' || input[j] == 'H' || input[j] == 'h' || input[j] == 'I' || input[j] == 'i' || input[j] == 'J' || input[j] == 'j' || input[j] == 'K' || input[j] == 'k' || input[j] == 'L' || input[j] == 'l' || input[j] == 'M' || input[j] == 'm' || input[j] == 'N' || input[j] == 'n' || input[j] == 'O' || input[j] == 'o' || input[j] == 'P' || input[j] == 'p' || input[j] == 'Q' || input[j] == 'q' || input[j] == 'R' || input[j] == 'r' || input[j] == 'S' || input[j] == 's' || input[j] == 'T' || input[j] == 't' || input[j] == 'U' || input[j] == 'u' || input[j] == 'V' || input[j] == 'v' || input[j] == 'w' || input[j] == 'W' || input[j] == 'x' || input[j] == 'X' || input[j] == 'Y' || input[j] == 'y' || input[j] == 'Z' || input[j] == 'z' || input[j] == '0' || input[j] == '1' || input[j] == '2' || input[j] == '3' || input[j] == '4' || input[j] == '5' || input[j] == '6' || input[j] == '7' || input[j] == '8' || input[j] == '9')
                        {
                            a += input[j];
                            i = j;
                            continue;
                        }

                        break;
                    }

                    if (a == "rest")
                    {
                        tokens.Add(new Token(Token.TokenType.UnderScore, ""));
                        continue;
                    }

                    if (a == "if")
                    {
                        tokens.Add(new Token(Token.TokenType.If, ""));
                        continue;
                    }

                    if (a == "then")
                    {
                        tokens.Add(new Token(Token.TokenType.Then, ""));
                        continue;
                    }

                    if (a == "else")
                    {
                        tokens.Add(new Token(Token.TokenType.Else, ""));
                        continue;
                    }

                    if (a == "let")
                    {
                        tokens.Add(new Token(Token.TokenType.Let, ""));
                        continue;
                    }

                    if (a == "in")
                    {
                        tokens.Add(new Token(Token.TokenType.In, ""));
                        continue;
                    }

                    if (a == "color")
                    {
                        tokens.Add(new Token(Token.TokenType.Color, ""));
                        continue;
                    }

                    if (a == "restore")
                    {
                        tokens.Add(new Token(Token.TokenType.Restore, ""));
                        continue;
                    }

                    if(a=="import")
                    {
                        tokens.Add(new Token(Token.TokenType.Import, ""));
                        continue; 
                    }

                    tokens.Add(new Token(Token.TokenType.Identificador, a));
                    continue;
                }

                errores.Add(new ERROR(ERROR.ErrorType.LexicalError, " '" + input[i] + "' is not a valid token" + i));

            }

            tokens.Add(new Token(Token.TokenType.EOF, ""));
        }
    }
}