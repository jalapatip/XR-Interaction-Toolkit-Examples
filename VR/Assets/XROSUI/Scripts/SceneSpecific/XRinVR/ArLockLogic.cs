using UnityEngine;
using TMPro;

public class ArLockLogic : MonoBehaviour
{
    public Animator AC_LeftDoor;
    public Animator AC_RightDoor;
    public TMP_Text Text_Authentication;
    public MeshRenderer _renderer;
    public Color ColorError = Color.red;
    public Color ColorWarning = Color.yellow;
    public Color ColorSuccess = Color.green;
    public Color ColorNormal = Color.blue;
    private MaterialPropertyBlock _propBlock;

    private void Start()
    {
        _renderer = this.GetComponent<MeshRenderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private void OnTriggerEnter(Collider other)
    {
        var vre = other.GetComponent<VrUserCredential>();
        
        if (vre)
        {
            var cred = vre.Credential;
            if (Core.Ins.Account.CheckAuthentication(cred))
            {
                AuthenticationSuccessful();
            }
            else
            {
                AuthenticationFailed(cred);
            }
        }
        else
        {
            AuthenticationMissing();
        }
    }

    private float _lastChangedTime;

    [TooltipAttribute("Customize using inspector")]
    public float duration = 0.5f;

    private bool _hasMaterialChanged = false;

    //https://thomasmountainborn.com/2016/05/25/materialpropertyblocks/
    private void ChangeMaterial(Color c)
    {
        _lastChangedTime = Time.time;

        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign our new value.
        _propBlock.SetColor("_BaseColor", c);
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
        _hasMaterialChanged = true;
    }

    #region Authentication Responses

    private void AuthenticationSuccessful()
    {
        AC_LeftDoor.SetBool("openLeftDoor", true);
        AC_RightDoor.SetBool("openRightDoor", true);
        Core.Ins.ScenarioManager.SetFlag("OpenDoor", true);
        Text_Authentication.text = "Authentication successful. \nWelcome " + Core.Ins.Account.GetUserName();
        Core.Ins.AudioManager.PlaySfx("511484__mattleschuck__success-bell");
        ChangeMaterial(ColorSuccess);
    }

    private void AuthenticationFailed(string s)
    {
        Text_Authentication.text = "Authentication failed. " + s + " is not authorized!";
        Core.Ins.AudioManager.PlaySfx("467882__samsterbirdies__beep-warning");

        ChangeMaterial(ColorError);
    }

    private void AuthenticationMissing()
    {
        Text_Authentication.text = "Unable to authenticate with presented object. Please authenticate with your ID";

        ChangeMaterial(ColorWarning);
    }

    #endregion Authentication Responses

    public void Update()
    {
        DebugUpdate();

        if (_hasMaterialChanged && _lastChangedTime + duration < Time.time)
        {
            ChangeMaterial(ColorNormal);
            _hasMaterialChanged = false;
        }
    }

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AuthenticationSuccessful();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            AuthenticationFailed("Luffy");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            AuthenticationMissing();
        }
    }
}