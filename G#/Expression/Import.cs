namespace Wall_E;
public class Import : Expression
{
    public string path{get ; private set;}
    public Import(string path)
    {
        this.path=path;
    }
}