using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerMotor playerMotor;
    public MouseScript mouseScript;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeInHierarchy == true)
            {
                pauseMenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerMotor.enabled = true;
                mouseScript.enabled = true;
            }
            else
            {
                pauseMenu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playerMotor.enabled = false;
                mouseScript.enabled = false;
            }
        }
    }

    public void Button_Resume()
    {
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMotor.enabled = true;
        mouseScript.enabled = true;
    }

    public void Button_Settings()
    {
        //Settings in game
    }

    public void Button_MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
