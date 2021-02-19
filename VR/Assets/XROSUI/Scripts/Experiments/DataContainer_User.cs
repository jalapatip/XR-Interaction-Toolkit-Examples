using System;

[Serializable]
public class DataContainer_User
{
    public float height = -1;
    public float armLength = -1;
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
            _headerString = nameof(DataContainer_User.height) + "," +
                            nameof(DataContainer_User.armLength);
            
        }
        return _headerString;
    }
}