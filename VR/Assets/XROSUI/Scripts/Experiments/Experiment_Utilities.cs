public enum Gesture {HeadUp, HeadDown, HeadForward, HeadBackward}

[System.Serializable]
public class Scaler
{
    // Note: Variables must be public for the JSONUtility to load correctly
    public string type;
    public float min;
    public float scale;
    public float data_min;
    public float data_max;
    public float data_range;
    public int n_samples_seen;

    public float Transform(float input)
    {
        return input * scale + min;
    }

    public float InverseTransform(float input)
    {
        return (input - min) / scale;
    }
}

[System.Serializable]
public class Scalers
{
    public Scaler[] scalers;
}

[System.Serializable]
public class GestureKey
{
    public string key;
    public string gesture;

    public Gesture GetGesture()
    {
        switch (gesture)
        {
            case "HeadDown":
                return Gesture.HeadDown;
            case "HeadUp":
                return Gesture.HeadUp;
            case "HeadForward":
                return Gesture.HeadForward;
            case "HeadBackward":
                return Gesture.HeadBackward;
        }

        return Gesture.HeadUp;
    }
}

[System.Serializable]
public class GestureKeys
{
    public GestureKey[] gestureKeys;
}
