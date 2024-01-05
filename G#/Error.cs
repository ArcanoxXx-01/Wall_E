namespace Wall_E
{
    public class ERROR : Exception
    {
        public enum ErrorType
        {
            LexicalError,
            SyntaxError,
            SemanticError,
        }

        public string Mensaje { get; private set; }
        public ErrorType Type { get; private set; }
        public static bool hadError = false;

        public ERROR(ErrorType type, string mensaje)
        {
            Type = type;
            Mensaje = mensaje;
            hadError = true;
            // if (type == ErrorType.SyntaxError)
            // {
            //     System.Console.WriteLine(type + mensaje);
            // }
        }
        public override string ToString()
        {
            return this.Type + " " + this.Mensaje;
        }
    }
}