using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public delegate void EventHandler_NewScreenshot();

public class Controller_Screenshot : MonoBehaviour
{
    public static event EventHandler_NewScreenshot EVENT_NewScreenshot;
    public float DurationToShow = 2.0f;
    public TextMeshProUGUI myButton;
    public GameObject myPanel;
    Texture2D _texture;
    // Start is called before the first frame update

    void Start()
    {

    }

    void Update()
    {
        //Broken in Unity?
        //if (Input.GetKey(KeyCode.Print))
        if (Input.GetKeyDown(KeyCode.Pause))
        {
            TakeAShot();
        }
    }

    // Update is called once per frame

    public void TakeAShot()
    {
        Core.Ins.AudioManager.PlaySfx("360329__inspectorj__camera-shutter-fast-a");
        
        //Debug.Log("The Screenshot is saved in " + Application.persistentDataPath);
        //"Application.persistentDataPath" is the file path to save the screenshots, you can change it according to your need

        //the screenshot image is name in this format, you can change it according to your need
        var fileName = "ScreenshotX" + System.DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + ".png";
        var pathToSave = Application.persistentDataPath + "/" + fileName;

        UnityEngine.ScreenCapture.CaptureScreenshot(pathToSave);
        _texture = ScreenCapture.CaptureScreenshotAsTexture();
        
        EVENT_NewScreenshot?.Invoke();
        StartCoroutine(ShowAndHide(myPanel, DurationToShow));
    }

    IEnumerator ShowAndHide(GameObject go, float delay)
    {
        myButton.enabled = true;
        myButton.SetText("Screenshot Taken!");
        go.SetActive(true);
        Sprite sp = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height),
                new Vector2(0.5f, 0.5f));
        Image image = go.GetComponent<Image>();
        image.sprite = sp;
        
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
        myButton.enabled = false;
    }

    IEnumerator CaptureIt()
    {
        yield return new WaitForEndOfFrame();
        //Instantiate(blink, new Vector2(0f, 0f), Quaternion.identity);
    }
}
