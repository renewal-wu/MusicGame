using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HUX.Interaction;
using HUX.Receivers;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>1. 抓出資料 + Sort 最高分</para>
/// <para>2. 顯示粒子系統</para>
/// <para>3. 回到 Scene 1</para>
/// </summary>
class GameEndedSceneManager : InteractionReceiver
{
    public GameObject ScoreCanvas;

    private float MergeHeight = 2;

    private void Start()
    {
        if (MusicGameUtility.IsSingleModel)
        {
            var selfItem = GameManager.Instance.LocalUserData;
            MergeHeight -= 0.25f;
            var newItem = MusicGameUtility.GenerateSocreItem(ScoreCanvas.transform, 10, MergeHeight, selfItem.Name, selfItem.Score);
            return;
        }

        if (GameManager.Instance.Participants == null || GameManager.Instance.Participants.Count == 0)
        {
            return;
        }
        
        // key: user name; value: score
        var users = GameManager.Instance.Participants.OrderByDescending(x => x.Value);

        foreach (var item in users)
        {
            MergeHeight -= 0.25f;
            var newItem = MusicGameUtility.GenerateSocreItem(ScoreCanvas.transform, 10, MergeHeight, item.Key, item.Value);
        }
    }

    private void Update()
    {
        
    }

    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        base.OnTapped(obj, eventArgs);

        if (obj.name == "EndButton")
        {
            SceneManager.LoadScene(0);
        }
    }
}