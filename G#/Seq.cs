using System.Collections;

namespace Wall_E;
public class Seq<T> : IEnumerable<T>
{
    private bool IsInfinite;
    List<T> values;

    public int count { get; private set; }

    public Seq(List<T> values, bool IsInfinite)
    {
        this.values = values;
        this.IsInfinite = IsInfinite;
        if (IsInfinite) count = -1;
        else count = values.Count;
    }
    

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}