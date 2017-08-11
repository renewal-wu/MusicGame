using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// <para>1. 抓出資料 + Sort 最高分</para>
/// <para>2. 顯示粒子系統</para>
/// <para>3. 回到 Scene 1</para>
/// </summary>
class GameEndedSceneManager : MonoBehaviour
{
    public GameObject ScoreCanvas;

    private float MergeHeight = 2;

    private void Start()
    {
        if (MusicGameController.Instance.Participants == null || MusicGameController.Instance.Participants.Count == 0)
        {
            return;
        }

        var users = MusicGameController.Instance.Participants.OrderByDescending(x => x.Score);

        foreach (var item in users)
        {
            MergeHeight -= 0.25f;
            var newItem = MusicGameController.Instance.GenerateSocreItem(ScoreCanvas.transform, 0, MergeHeight, item.Name, item.Score);
        }
    }

    private void Update()
    {
        
    }
}