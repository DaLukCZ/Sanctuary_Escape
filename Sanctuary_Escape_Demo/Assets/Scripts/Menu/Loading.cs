 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public int sceneID;
    public Slider LoadingSlider;
    void Start()
    {
        StartCoroutine(LoadScene(sceneID));
    }

    IEnumerator LoadScene(int sceneID)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneID);
        while (!op.isDone)
        {
            float value = Mathf.Clamp01(op.progress / .9f);
            LoadingSlider.value = value;
            yield return null;
        }
    }
}
