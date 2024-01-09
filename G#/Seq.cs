using System.Collections;

namespace Wall_E;
public class Seq : IEnumerable, IDrawable
{
    private bool IsInfinite;
    public List<object> values
    { get; private set; }

    public int count { get; private set; }

    public Seq(List<object> values, bool IsInfinite)
    {
        this.values = values;
        this.IsInfinite = IsInfinite;
        if (IsInfinite) count = -1;
        else count = values.Count;
    }

    public object this[int index]
    {
        get { return values[index]; }
    }

    public Seq Rest(int index)
    {
        List<object> seq = new();
        for (int i = index; i < values?.Count; i++)
        {
            seq.Add(values[i]);
        }
        return new Seq(seq, IsInfinite);
    }

    public Seq SumaSeq(Seq seq)
    {
        List<object> obj = new();
        foreach (var x in this)
        {
            obj.Add(x);
        }
        foreach (var t in seq)
        {
            obj.Add(t);
        }

        return new Seq(obj, IsInfinite||seq.IsInfinite);
    }
    public IEnumerator GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return values.GetEnumerator();
    }
    public void Draw(IPaint paint, Color color, string? message)
    {
        paint.DrawSeq(this, color, message);
    }
}