namespace Wall_E
{

    public class Token
    {
        public enum TokenType
        {
            Number,
            String,
            Identificador,
            Restore,
            Rest,
            UnderScore, //"_"
            Undefined,

            //Separadores
            Coma,
            PuntoYComa,
            EOF,
            ParentesisAbierto,
            ParentesisCerrado,
            LlaveAbierta,
            LlaveCerrada,

            //Operadores Aritmeticos
            Suma, //"+"
            Menos, //"-"
            Aster, //"*"
            Div, //"/"
            Mod, //"%"

            //Operadores Comparadores
            Menor,
            MenorIgual,
            Mayor,
            MayorIgual,
            IgualIgual,
            Diferente,
            Asign,

            //Operadores Binarios
            Or,
            And,
            
            //Palabras Clave
            If,
            Then,
            Else,
            Let,
            In,
            Color,
            ThreePoints,
            Import,
        }

        public TokenType type { get; private set; }
        public object value { get; private set; }

        public Token(TokenType type, object value)
        {
            this.type = type;

            this.value = value;
        }
    }
}