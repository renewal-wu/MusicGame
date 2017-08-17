using System;
using HUX.Buttons;
using HUX.Interaction;
using HUX.Receivers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// https://onedrive.live.com/view.aspx?resid=E5D8C3F4EFAEC162!2592141&ithint=file%2cdocx&app=Word&authkey=!AKyw0BzT-t2zUv8
/// </summary>
public class StartGameSceneManager : InteractionReceiver
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

        var startNowButton = GameObject.Find("StartNowButton");
        //var buttonTarget = startNowButton.GetComponent<HUX.Buttons.Button>() as HUX.Buttons.Button;

        //if (MusicGameUtility.IsSingleModel)
        //{
        //    buttonTarget.ButtonState = HUX.Buttons.Button.ButtonStateEnum.Interactive;
        //    //buttonTarget.interactable = true;
        //}
        //else
        //{
        //    // start 的按鈕要跟著是否連線成功才可以使用
        //    //buttonTarget.interactable = GameManager.Instance.IsConnectionStarted;
        //    if (GameManager.Instance.IsConnectionStarted)
        //    {
        //        buttonTarget.ButtonState = HUX.Buttons.Button.ButtonStateEnum.Interactive;
        //    }
        //    else
        //    {
        //        buttonTarget.ButtonState = HUX.Buttons.Button.ButtonStateEnum.Disabled;
        //    }
        //}
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

    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        base.OnTapped(obj, eventArgs);

        switch (obj.name)
        {
            case "StartNowButton":
                OnStartNowButtonClick();
                break;
            case "CancelWaittingButton":
                OnCancelWaittingButtonClick();
                break;
            case "AcceptButton":
                OnAcceptInviteButtonClick();
                break;
            case "AbortButton":
                OnAbortInviteButtonClick();
                break;
            default:
                break;
        }
    }

    private void OnStartNowButtonClick()
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

    private void OnCancelWaittingButtonClick()
    {
        // 取消回到開始畫面
        WaitContainer.SetActive(false);
        StartContainer.SetActive(true);
        Debug.Log("cancel waitting, and display start now button.");
    }

    private void OnAcceptInviteButtonClick()
    {
        // 送出 accept 等待收到 GameBegun
        GameManager.Instance.AcceptGame(GameManager.Instance.LocalUserData);
        WaitContainer.SetActive(true);
        InviteContainer.SetActive(false);
    }

    private void OnAbortInviteButtonClick()
    {
        InviteContainer.SetActive(false);
        StartContainer.SetActive(true);
    }
}