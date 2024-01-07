
namespace Wall_E
{
    public abstract class Expression : IVisitor
    {
        public virtual object Visit(Dictionary<string, object> values)
        {
            return null!;
        }
    }
}