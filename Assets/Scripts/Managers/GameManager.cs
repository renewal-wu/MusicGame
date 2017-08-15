using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using UnityEngine;

public class GameManager : MonoBehaviour // Singleton<GameManager>
{
    /// <summary>
    /// 開始遊戲的倒數計時秒數限制
    /// </summary>
    public const float GameStartCountdownSeconds = 10f;

    /// <summary>
    /// 遊戲時間總長
    /// </summary>
    public const float GameCountdownSeconds = 60f;

    /// <summary>
    /// 遊戲時間倒數
    /// </summary>
    public float CurrentGameCountdownSeconds = GameCountdownSeconds;

    /// <summary>
    /// 連線已建立
    /// </summary>
    public event EventHandler ConnectionStarted;
    private void OnConnectionStarted()
    {
        ConnectionStarted?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 收到他人的開始遊戲指令
    /// EventArgs 是來源者
    /// </summary>
    public event EventHandler<UserData> GameStartCommandReceived;
    private void OnGameStartCommandReceived(UserData userData)
    {
        GameStartCommandReceived?.Invoke(this, userData);
    }

    /// <summary>
    /// 開始遊戲倒數計時
    /// </summary>
    public event EventHandler<float> GameStartCountdown;
    private void OnGameStartCountdown(float countdownSeconds)
    {
        GameStartCountdown?.Invoke(this, countdownSeconds);
    }

    /// <summary>
    /// 啟動遊戲
    /// </summary>
    public event EventHandler GameBegun;
    private void OnGameBegun()
    {
        GameBegun?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 參與者名單更新
    /// </summary>
    public event EventHandler ParticipantsUpdated;
    private void OnParticipantsUpdated()
    {
        ParticipantsUpdated?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 得分更新
    /// </summary>
    public event EventHandler<UserData> ScoreUpdated;
    private void OnScoreUpdated(UserData userData)
    {
        ScoreUpdated?.Invoke(this, userData);
    }

    public event EventHandler<float> GameCountdown;
    private void OnGameCountdown(float countdown)
    {
        GameCountdown?.Invoke(this, countdown);
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    public event EventHandler GameEnded;
    private void OnGameEnded()
    {
        GameEnded?.Invoke(this, EventArgs.Empty);
    }

    public bool IsConnectionStarted { get; private set; } = false;
    public bool IsOwner { get; private set; } = false;
    public bool IsGameStarting { get; private set; } = false;

    /// <summary>
    /// 參與者列表
    /// </summary>
    public Dictionary<string, int> Participants { get; set; } = new Dictionary<string, int>();

    public UserData LocalUserData
    {
        get
        {
            return GameConnector.Instance.LocalUserData;
        }
    }

    private bool isGameStartCountdowning = false;
    private float gameStartCountdown = 0;

    private bool isGameBegun = false;

    private static GameManager instance = null;

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        GameConnector.Instance.ConnectionStarted += Instance_ConnectionStarted;
        GameConnector.Instance.MessageHandlerInitialed += Instance_MessageHandlerInitialed;
    }

    private void Update()
    {
        if (isGameStartCountdowning)
        {
            gameStartCountdown += Time.deltaTime;
            OnGameStartCountdown(gameStartCountdown);

            if (gameStartCountdown >= GameStartCountdownSeconds)
            {
                gameStartCountdown = 0;
                isGameStartCountdowning = false;

                BeginGame();
            }
        }
        else if (isGameBegun)
        {
            CurrentGameCountdownSeconds -= Time.deltaTime;
            OnGameCountdown(CurrentGameCountdownSeconds);

            if (CurrentGameCountdownSeconds <= 0)
            {
                CurrentGameCountdownSeconds = GameCountdownSeconds;
                isGameBegun = false;

                // 遊戲結束
                OnGameEnded();
            }
        }
    }

    private void Instance_ConnectionStarted(object sender, EventArgs e)
    {
        IsConnectionStarted = true;
        OnConnectionStarted();
    }

    private void Instance_MessageHandlerInitialed(object sender, EventArgs e)
    {
        GameConnector.Instance.MessageHandlers[CommandType.Start] = StartGameCommandReceived;
        GameConnector.Instance.MessageHandlers[CommandType.Accept] = AcceptCommandReceived;
        GameConnector.Instance.MessageHandlers[CommandType.Begin] = BeginGameCommandReceived;
        GameConnector.Instance.MessageHandlers[CommandType.UpdateScore] = UpdateScoreCommandReceived;
    }

    public void StartGame()
    {
        if (IsConnectionStarted == false || IsOwner == true || IsGameStarting == true)
        {
            return;
        }

        IsOwner = true;
        GameConnector.Instance.SendStartGameCommand();
    }

    public void AcceptGame(UserData userData)
    {
        if (IsConnectionStarted == false || IsOwner == true || IsGameStarting == true)
        {
            return;
        }

        GameConnector.Instance.SendAcceptCommand(userData);
    }

    /// <summary>
    /// 遊戲啟動
    /// </summary>
    private void BeginGame()
    {
        if (IsConnectionStarted == false || IsGameStarting == true)
        {
            return;
        }

        IsGameStarting = true;

        if (IsOwner)
        {
            GameConnector.Instance.SendBeginCommand(Participants);
        }

        OnGameBegun();
    }

    /// <summary>
    /// 更新自己的得分
    /// </summary>
    public void UpdateScore(int score)
    {
        LocalUserData.Score = score;
        GameConnector.Instance.SendUpdateScore();
    }

    /// <summary>
    /// 收到 StartGame 指令
    /// </summary>
    /// <param name="msg"></param>
    private void StartGameCommandReceived(UserData userData, NetworkInMessage msg)
    {
        if (IsOwner == true || IsGameStarting == true)
        {
            return;
        }

        OnGameStartCommandReceived(userData);
    }

    /// <summary>
    /// 收到 Accept 指令
    /// </summary>
    /// <param name="msg"></param>
    private void AcceptCommandReceived(UserData userData, NetworkInMessage msg)
    {
        var targetUserId = msg.ReadInt64();
        if (GameConnector.Instance.LocalUserData.Id == targetUserId)
        {
            return;
        }

        if (isGameBegun)
        {
            return;
        }

        AddUser(userData);

        if (gameStartCountdown == 0 && isGameStartCountdowning == false)
        {
            // 開始倒數計時
            isGameStartCountdowning = true;
        }
    }

    /// <summary>
    /// 收到 BeginGame 指令
    /// </summary>
    /// <param name="msg"></param>
    private void BeginGameCommandReceived(UserData userData, NetworkInMessage msg)
    {
        var participantsString = msg.ReadString();
        Participants = JsonUtility.FromJson<Dictionary<string, int>>(participantsString);
        OnParticipantsUpdated();
        BeginGame();
    }

    /// <summary>
    /// 收到 UpdateScore 指令
    /// </summary>
    /// <param name="msg"></param>
    private void UpdateScoreCommandReceived(UserData userData, NetworkInMessage msg)
    {
        if (IsGameStarting == false)
        {
            return;
        }

        var userName = userData.Name;
        if (Participants.ContainsKey(userName))
        {
            Participants[userName] = userData.Score;
        }

        OnScoreUpdated(userData);
    }

    private void AddUser(UserData userData)
    {
        var userName = userData.Name;
        var score = userData.Score;

        if (Participants.ContainsKey(userName) == false)
        {
            Participants.Add(userName, 0);
        }

        Participants[userName] = score;

        OnParticipantsUpdated();
    }

    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();
    //    GameConnector.Instance.ConnectionStarted -= Instance_ConnectionStarted;
    //    GameConnector.Instance.MessageHandlerInitialed -= Instance_MessageHandlerInitialed;
    //}
}