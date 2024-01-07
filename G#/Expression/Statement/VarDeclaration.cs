namespace Wall_E;
public class VarDeclaration : Statement
{
    public string type { get; private set; }
    public string name { get; private set; }

    public VarDeclaration(string type, string name)
    {
        this.type = type;
        this.name = name;
    }
    public override object Visit(Dictionary<string, object> values)
    {
        if (values.ContainsKey(name))
        {
            Evaluator.errors.Add(new ERROR(ERROR.ErrorType.SemanticError, " Variable " + name + " already has been declared"));
            return null!;
        }
        if (type == "point")
        {
            Point f1 = Evaluator.random.GeneratePoint();
            values.Add(name, f1);
        }
        if (type == "line")
        {
            Line f1 = Evaluator.random.GenerateLine();
            values.Add(name, f1);
        }
        if (type == "segment")
        {
            Segment f1 = Evaluator.random.GenerateSegment();
            values.Add(name, f1);
        }
        if (type == "ray")
        {
            Ray f1 = Evaluator.random.GenerateRay();
            values.Add(name, f1);
        }
        if (type == "circle")
        {
            Circle f1 = Evaluator.random.GenerateCircle();
            values.Add(name, f1);
        }
        if (type == "arc")
        {
            Arc f1 = Evaluator.random.GenerateArc();
            values.Add(name, f1);
        }
        // if (type == "sequence")
        // {

        // }
        if (type == "point sequence")
        {
            Seq seq = Evaluator.random.PointSeq();
            values.Add(name, seq);
        }
        if (type == "line sequence")
        {
            Seq seq = Evaluator.random.LineSeq();
            values.Add(name, seq);
        }
        if (type == "segment sequence")
        {
            Seq seq = Evaluator.random.SegmentSeq();
            values.Add(name, seq);
        }
        if (type == "ray sequence")
        {
            Seq seq = Evaluator.random.RaySeq();
            values.Add(name, seq);
        }
        if (type == "circle sequence")
        {
            Seq seq = Evaluator.random.CircleSeq();
            values.Add(name, seq);
        }
        if (type == "arc sequence")
        {
            Seq seq = Evaluator.random.ArcSeq();
            values.Add(name, seq);
        }

        return null!;
    }

}