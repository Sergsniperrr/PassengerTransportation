using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.View.Windows.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private string _sceneForLoading;
        [SerializeField] private Slider _bar;

        private void Start()
        {
            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneForLoading);

            while (asyncLoad is { isDone: false })
            {
                _bar.value = asyncLoad.progress;

                yield return null;
            }
        }
    }
}