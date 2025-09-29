using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string _sceneForLoading;
    [SerializeField] private Slider _bar;

    private void Awake()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneForLoading);

        while (asyncLoad.isDone == false)
        {
            _bar.value = asyncLoad.progress;

            yield return null;
        }
    }
}
