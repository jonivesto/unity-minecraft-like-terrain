
public class Custom : Item
{
    public string itemName;
    public string prefabPath;
    public bool displaced = false;
    public bool collision = true;

    public override string GetName()
    {
        return itemName;
    }
}
