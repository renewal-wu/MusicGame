using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// https://onedrive.live.com/view.aspx?resid=E5D8C3F4EFAEC162!2592141&ithint=file%2cdocx&app=Word&authkey=!AKyw0BzT-t2zUv8
/// </summary>
public class StartGameSceneManager : MonoBehaviour
{
    public GameObject StartContainer;

    public GameObject WaitContainer;

    public GameObject InviteContainer;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        StartContainer.SetActive(true);
        WaitContainer.SetActive(false);
        InviteContainer.SetActive(false);

        GameManager.Instance.GameStartCountdown += GameManager_GameStartCountdown;
        GameManager.Instance.GameStartCommandReceived += GameManager_GameStartCommandReceived;
        GameManager.Instance.GameBegun += GameManager_GameBegun;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GameManager_GameStartCommandReceived(object sender, UserData e)
    {
        if (GameManager.Instance.IsOwner)
        {
            // 如果是自己開的游戲，忽略
            return;
        }

        // 收到別人開游戲的指令，顯示 接受或是取消
        StartContainer.SetActive(false);
        WaitContainer.SetActive(false);
        InviteContainer.SetActive(true);
    }

    private void GameManager_GameStartCountdown(object sender, float e)
    {
        // 更新倒數
        var gameObject = GameObject.Find("CountdownSeconds");
        var textObject = gameObject.GetComponent<Text>() as Text;
        textObject.text = $"{e.ToString()}s";
    }

    private void GameManager_GameBegun(object sender, EventArgs e)
    {
        // 進入游戲
        SceneManager.LoadScene(1);
    }
}