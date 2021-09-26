using System;
using System.Collections.Generic;


[Serializable]
public class RASAResult
{
    public string text;
    public Intent intent;
    public List<string> entities;
    public List<Intent> intent_ranking;
}

[Serializable]
public class Intent
{
    public string name;
    public double confidence;
}

[Serializable]
public class ServerResult
{
    public RASAResult result;
}


