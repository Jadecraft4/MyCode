using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    //publics 
    public GameObject PauseUI;
    //privates
    private bool isOn = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }
    public void Toggle()
    {
        if (isOn)
        {
            PauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            isOn = false;
        }
        else
        {
            PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
            Time.timeScale = 1;
            isOn = true;
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
