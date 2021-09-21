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
    

    protected override void OnActivated(XRBaseInteractor obj)
    {
//        Dev.Log("StartPrediction: Activate");
        var slot = predictModule.PredictSlot();
        
        predictModule.CreateVisualization(obj.transform.position, obj.transform.rotation, slot);
        
        ///Create a new object and provide a color based on slot location
        // var go = GameObject.Instantiate(PF_SlotVisualization, obj.transform.position, obj.transform.rotation);
        // //var go = GameObject.Instantiate(PF_SlotVisualization, Core.Ins.XRManager.GetXrCamera().gameObject.transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
        // var r = go.GetComponent<Renderer>();
        // var block = new MaterialPropertyBlock();
        // var c = Color.white;
        // //c = new Color(255f, 165f, 0f);
        // c = Experiment2_PeripersonalSlotHelper.GetSlotColor(slot);
        // block.SetColor(BaseColor, c);
        // block.SetColor(EmissionColor, c);
        // r.SetPropertyBlock(block);
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
