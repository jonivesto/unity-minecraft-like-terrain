
public abstract class Liquid : Item
{
    protected string liquidName;
    public bool isStill = true;

    public override string GetName()
    {
        return liquidName;
    }
}
