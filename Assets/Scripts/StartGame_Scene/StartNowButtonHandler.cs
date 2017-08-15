using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartNowButtonHandler : MonoBehaviour
{
    public Button StartNowButton;

    public GameObject StartContainer;

    public GameObject WaitContainer;

    private void Awake()
    {
        //StartNowButton.interactable = false;
    }

    // Use this for initialization
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicGameUtility.IsSingleModel)
        {
            StartNowButton.interactable = true;
        }
        else
        {
            // start 的按鈕要跟著是否連線成功才可以使用
            StartNowButton.interactable = GameManager.Instance.IsConnectionStarted;
        }
    }

    public void OnClick()
    {
        if (MusicGameUtility.IsSingleModel)
        {
            SceneManager.LoadScene(1);
            return;
        }

        StartContainer.SetActive(false);
        WaitContainer.SetActive(true);

        // 發出開啓一個游戲活動
        GameManager.Instance.StartGame();
    }
}