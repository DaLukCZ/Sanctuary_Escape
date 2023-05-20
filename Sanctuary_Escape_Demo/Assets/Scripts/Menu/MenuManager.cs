using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Slider LoadingSlider;
    public GameObject panel;
    public void Button_Start()
    {
        SceneManager.LoadScene(1);
    }

    public void Button_Settings()
    {
        SceneManager.LoadScene(2);
    }

    public void Button_Exit()
    {
        Application.Quit();
    }

}
