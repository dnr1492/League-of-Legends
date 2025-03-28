using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Button btnQuit;
    private Text title;

    private void Awake()
    {
        btnQuit = GetComponentInChildren<Button>();
        btnQuit.onClick.AddListener(() => {
            SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
            Time.timeScale = 1;
        });

        title = GetComponentInChildren<Text>();
        title.text = "게임 오버";

        Time.timeScale = 0;
    }
}
