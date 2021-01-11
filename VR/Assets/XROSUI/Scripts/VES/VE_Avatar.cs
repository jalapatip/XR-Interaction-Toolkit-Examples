using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VE_Avatar : VE_EquipmentBase
{
    public Renderer myRenderer;

    public float distanceBeforeShowingDoll = 0.5f;

    RaycastHit _hit;
    private GameObject _unplacedAlternateAvatar;

    private XRRayInteractor _myRayInteractor;

    private VA_AvatarBase _avatarScript;
    // Start is called before the first frame update
    void Start()
    {
        //_myRayInteractor = obj.GetComponent<XRRayInteractor>();
        _myRayInteractor = Core.Ins.XRManager.GetRightRayController().GetComponent<XRRayInteractor>();
        GetNewAltAvatar();
    }

    private void GetNewAltAvatar()
    {
//        print("Got new avatar in VE avatar");
        _unplacedAlternateAvatar = Core.Ins.Avatar.GetNewAlternateAvatar();
        _avatarScript = _unplacedAlternateAvatar.GetComponent<VA_AvatarBase>();
        _avatarScript.EnableColliders(false);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (this.IsSelected() && !_isInSocket && _isEquipped)
        {
//            print(Vector3.Distance(this.transform.position, this.socket.transform.position));
            if (Vector3.Distance(this.transform.position, this.socket.transform.position) < distanceBeforeShowingDoll)
            {
                _WithinGestureSphere = true;
                this.myRenderer.enabled = true;
            }
            else
            {
                this.myRenderer.enabled = false;
                _WithinGestureSphere = false;
            }
        }

        WhileWithinGestureSphere(_WithinGestureSphere);
    }

    private void WhileWithinGestureSphere(bool withinGestureSphere)
    {
        if (!IsSelected())
            return;
        
        if (withinGestureSphere)
        {
            if (_myRayInteractor)
            {
                _unplacedAlternateAvatar.SetActive(false);
            }
        }
        else
        {
            if (_myRayInteractor)
            {
                _unplacedAlternateAvatar.SetActive(true);
//                print("ray interactor");
                _myRayInteractor.GetCurrentRaycastHit(out _hit);
                _unplacedAlternateAvatar.transform.position = _hit.point;
            }
        }
    }

    private bool _WithinGestureSphere = true;

    protected override void OnFirstHoverEnter(XRBaseInteractor obj)
    {
        base.OnFirstHoverEnter(obj);
        Core.Ins.Avatar.ShowAvatarManagementMode(true);
    }


    protected override void OnHoverEnter(XRBaseInteractor obj)
    {
        base.OnHoverEnter(obj);

        //Core.Ins.XRManager.GetRightRayController().GetComponent<XRRayInteractor>().GetCurrentRaycastHit(out _hit);
    }

    protected override void OnLastHoverExit(XRBaseInteractor obj)
    {
        base.OnLastHoverExit(obj);
        Core.Ins.Avatar.ShowAvatarManagementMode(false);
        _unplacedAlternateAvatar.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(_hit.point, Vector3.one);
    }

    protected override void OnActivate(XRBaseInteractor obj)
    {
        base.OnActivate(obj);
        //Toggle On/Off Avatar Management Mode

        if (_WithinGestureSphere)
        {
            Core.Ins.Avatar.ToggleAvatarManagementModeLock();
        }
        else
        {
            if (_myRayInteractor)
            {
                _myRayInteractor.GetCurrentRaycastHit(out _hit);
                _unplacedAlternateAvatar.transform.position = _hit.point;
//                Core.Ins.XRManager.GetRightRayController().GetComponent<XRRayInteractor>().GetCurrentRaycastHit(out _hit);
                
                // GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // go.transform.position = _hit.point;
                // go.name = "PRIMITIVE";
                
                _unplacedAlternateAvatar.SetActive(true);
                _avatarScript.EnableColliders(true);
                _avatarScript.DeployAvatar();
                this.GetNewAltAvatar();
            }
        }
    }

    public override void HandleGesture(ENUM_XROS_EquipmentGesture equipmentGesture, float distance)
    {
        base.HandleGesture(equipmentGesture, distance);

        switch (equipmentGesture)
        {
            case ENUM_XROS_EquipmentGesture.Up:
                break;
            case ENUM_XROS_EquipmentGesture.Down:
                break;
            case ENUM_XROS_EquipmentGesture.Forward:
                break;
            case ENUM_XROS_EquipmentGesture.Backward:
                break;
            case ENUM_XROS_EquipmentGesture.Left:
                Core.Ins.Avatar.PreviousAlternateAvatar();
                break;
            case ENUM_XROS_EquipmentGesture.Right:
                Core.Ins.Avatar.NextAlternateAvatar();
                break;
            case ENUM_XROS_EquipmentGesture.RotateClockwise:
                break;
            case ENUM_XROS_EquipmentGesture.RotateCounterclockwise:
                break;
            default:
                break;
        }
    }
}