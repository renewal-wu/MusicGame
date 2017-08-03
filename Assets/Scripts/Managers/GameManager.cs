using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 開始遊戲的倒數計時秒數限制
    /// </summary>
    public const int GameStartCountdownSeconds = 10;

    /// <summary>
    /// 遊戲時間總長
    /// </summary>
    public const int GameCountdownSeconds = 60;

    /// <summary>
    /// 遊戲時間倒數
    /// </summary>
    public int CurrentGameCountdownSeconds = GameCountdownSeconds;

    /// <summary>
    /// 連線已建立
    /// </summary>
    public event EventHandler ConnectionStarted;

    /// <summary>
    /// 收到他人的開始遊戲指令
    /// </summary>
    public event EventHandler GameStartCommandReceived;

    /// <summary>
    /// 開始遊戲倒數計時
    /// </summary>
    public event EventHandler GameStartCountdown;

    /// <summary>
    /// 啟動遊戲
    /// </summary>
    public event EventHandler GameBegun;

    /// <summary>
    /// 參與者名單更新
    /// </summary>
    public event EventHandler ParticipantsUpdated;

    /// <summary>
    /// 得分更新
    /// </summary>
    public event EventHandler ScoreUpdated;

    /// <summary>
    /// 遊戲結束
    /// </summary>
    public event EventHandler GameEnded;

    public bool IsConnectionStarted { get; private set; } = false;
    public bool IsOwner { get; private set; } = false;
    public bool IsGameStarting { get; private set; } = false;

    /// <summary>
    /// 參與者列表
    /// </summary>
    public Dictionary<string, int> Participants { get; set; } = new Dictionary<string, int>();

    public void StartGame()
    {
        IsOwner = true;
    }

    public void AcceptGame(int starterId)
    {
        if (IsOwner == true || IsGameStarting == true)
        {
            return;
        }
    }

    public void BeginGame()
    {
        if (IsOwner == true || IsGameStarting == true)
        {
            return;
        }

        IsGameStarting = true;

        // 倒數計時

        // 倒數完後
        // 把目前的 Participants 丟給大家
        // 
    }

    /// <summary>
    /// 更新自己的得分
    /// </summary>
    public void UpdateScore(int score)
    {
    }
}
