using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch_StartPrediction : Switch_Base
{
    public DataCollection_Exp2Predict predictModule;
    public Canvas labelCanvas;
    public Renderer cubeRenderer;
    
    public GameObject PF_SlotVisualization;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    protected override void OnActivated(XRBaseInteractor obj)
    {
//        Dev.Log("StartPrediction: Activate");
        var slot = predictModule.PredictSlot();
        
        ///Create a new object and provide a color based on slot location
        GameObject go = GameObject.Instantiate(PF_SlotVisualization, this.transform.position, this.transform.rotation);
        Renderer r = go.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        Color c = Color.white;
        //c = new Color(255f, 165f, 0f);
        c = Experiment2_PeripersonalSlotHelper.GetSlotColor(slot);
        block.SetColor(BaseColor, c);
        block.SetColor(EmissionColor, c);
        r.SetPropertyBlock(block);
    }
    
    protected override void OnSelectedEnter(XRBaseInteractor arg0)
    {
        labelCanvas.enabled = false;
        cubeRenderer.enabled = false;
    }
    
    protected override void OnSelectedExit(XRBaseInteractor arg0)
    {
        labelCanvas.enabled = true;
        cubeRenderer.enabled = true;
    }
}
