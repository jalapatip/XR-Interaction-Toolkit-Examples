using UnityEngine.XR.Interaction.Toolkit;

public class Switch_RecognizeSpeech : Switch_Base
{
    protected override void OnActivated(XRBaseInteractor obj)
    {
        //Core.Ins.Microphone.StartListeningForKeywords();
        Core.Ins.Microphone.ToggleListeningForKeywords();
    }
}
