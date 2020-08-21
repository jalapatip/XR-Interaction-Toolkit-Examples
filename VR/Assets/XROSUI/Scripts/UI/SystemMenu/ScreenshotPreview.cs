using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//Screenshot Menu Manager
public class ScreenshotPreview : MonoBehaviour
{
    public Image myImage;
    string[] files = null;
    int currentImageId = 0;

    // Use this for initialization
    private void Start()
    {
        Controller_Screenshot.EVENT_NewScreenshot += GetPictureAndShowIt;
    }

    void Update()
    {
        //if (lastAskTime + coolDown < Time.time)
        //{
        //    GetPictureAndShowIt();
        //    lastAskTime = Time.time;
        //}
    }

    private void GetPictureAndShowIt()
    {
        files = Directory.GetFiles(Application.persistentDataPath + "/", "*.png"); //to get the local files(screenshots)
        if (files.Length > 0)
        {
            string pathToFile = files[currentImageId];
            Texture2D texture = GetScreenshotImage(pathToFile);
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            myImage.sprite = sp;
        }
    }
    
    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;
        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }
        return texture;
    }

    public void NextPicture() //to get the next screenshot
    {
        if (files.Length > 0)
        {
            currentImageId += 1;
            if (currentImageId > files.Length - 1)
            {
                currentImageId = 0;
            }
            GetPictureAndShowIt();
        }
    }

    public void PreviousPicture() //to get the previous screenshot
    {
        if (files.Length > 0)
        {
            currentImageId -= 1;
            //Debug.Log("this is the no. " + whichScreenShotIsShown + "Screenshot");
            if (currentImageId < 0)
            {
                currentImageId = files.Length - 1;
            }
            GetPictureAndShowIt();
        }
    }
}
