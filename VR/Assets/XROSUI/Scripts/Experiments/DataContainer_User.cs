using System;

[Serializable]
public class DataContainer_User
{
    public float height;
    public float armLength;
    public override string ToString()
    {
        return "\n" + this.height + "," +
               this.armLength;

    }
    private static string _headerString;
    public static string HeaderToString()
    {
        if (_headerString == null)
        {
            _headerString = nameof(DataContainer_Exp0.height) + "," + // new edit
                            nameof(DataContainer_Exp0.timestamp);
        }
        return _headerString;
    }
}