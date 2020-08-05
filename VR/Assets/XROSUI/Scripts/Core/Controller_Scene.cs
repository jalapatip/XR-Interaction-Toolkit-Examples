using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Scene : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DebugUpdate();
    }

    //Track Debug Inputs here
    //https://docs.google.com/spreadsheets/d/1NMH43LMlbs5lggdhq4Pa4qQ569U1lr_O7HSHESEantU/edit#gid=0
    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene(3);
        }        
    }

    //This loads the scene in File/Build Settings
    public void LoadSceneById(int i)
    {
        SceneManager.LoadScene(i);
    }
}