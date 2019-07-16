
public class ThinBirchTree : TreeType
{
    public ThinBirchTree()
    {
        height = 5;

        woodBlock = 15;
        leafBlock = 14;

        leafModel = new int[] {
            0, 0, 0,

            1, 0, 0,
            0, 0, 1,
            0, 0, -1,
            -1, 0, 0,

            1, -1, 0,
            0, -1, 1,
            0, -1, -1,
            -1, -1, 0,
            -1, -1, -1,
            -1, -1, 1,
            1, -1, -1,
            1, -1, 1,

        };

    }
}
