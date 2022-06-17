using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SceneTransition : MonoBehaviour
    {
        public Text LoadingPercentage;
        public Image LoadingProgressBar;

        private static SceneTransition instance;
      //  private static bool shouldPlayOpeningAnimation = false;

        private bool start = false;
        private float progress=0;
       // private float step = 0.05f;

        private Animator componentAnimator;
        private AsyncOperation loadingSceneOperation;

        public static void SwitchToScene(int sceneName)
        {
            
            instance.componentAnimator.SetTrigger("sceneStart");

            instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            // Чтобы сцена не начала переключаться пока играет анимация closing:
            instance.loadingSceneOperation.allowSceneActivation = false;
           
            // instance.LoadingProgressBar.fillAmount = 0;
        }

        private void ToNull()
        {
            instance.LoadingPercentage.text =0 + "%";
            instance.LoadingProgressBar.fillAmount = 0;
        }
        private void Start()
        {
           /* instance = this;

            componentAnimator = GetComponent<Animator>();

            if (shouldPlayOpeningAnimation)
            {
                componentAnimator.SetTrigger("sceneEnd");
                instance.LoadingProgressBar.fillAmount = 1;

                // Чтобы если следующий переход будет обычным SceneManager.LoadScene, не проигрывать анимацию opening:
                shouldPlayOpeningAnimation = false;
            }*/
           
           
           instance = this;
           componentAnimator = GetComponent<Animator>();
           componentAnimator.SetTrigger("sceneEnd");
           
           Load();
           
           
        }

        private void Load()
        {
            if (loadingSceneOperation != null)
            {
                LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount,
                    loadingSceneOperation.progress,
                    Time.deltaTime * 5);
            }
        }

        private void Update()
        {
            
            if (start)
            {
                LoadingPercentage.text = Mathf.Lerp(0, 1f, progress) * 100 + "%";
                LoadingProgressBar.fillAmount = Mathf.Lerp(0, 1f, progress);
                progress += 0.05f;
                if (progress >= 1)
                {
                    start = false;
                    componentAnimator.SetTrigger("sceneEnd");
                    instance.loadingSceneOperation.allowSceneActivation = true;

                }

            }

        }

        public void OnAnimationOver()
        {
            start = true;
        }

    }
}
