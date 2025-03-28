using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Image progressBar;
    [SerializeField] Text txtLoading;

    private void Awake()
    {
        btn.onClick.AddListener(LoadScene);

        progressBar.fillAmount = 0;
        txtLoading.gameObject.SetActive(false);
    }

    private void LoadScene()
    {
        StartCoroutine(LoadScene("PlayScene"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        txtLoading.gameObject.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress < 0.9f) progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    txtLoading.text = "Press Space Bar to Start";
                    if (Input.GetKeyDown(KeyCode.Space)) asyncOperation.allowSceneActivation = true;
                }
            }

            timer += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
}
