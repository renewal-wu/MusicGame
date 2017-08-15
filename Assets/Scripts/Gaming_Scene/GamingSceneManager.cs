using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamingSceneManager : MonoBehaviour
{
    public GameObject ParticipantsScoreArea;

    /// <summary>
    /// 玩家與物件的集合。
    /// </summary>
    private Dictionary<UserData, GameObject> ParticipantsObjects { get; set; }

    private float MergeHeight = -160;

    private DateTime lastDateTime;

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.GameEnded += GameManager_GameEnded;
        GameManager.Instance.ScoreUpdated += GameManager_ScoreUpdated;

        ParticipantsObjects = new Dictionary<UserData, GameObject>();
        GenerateUserScoreGameObject();

        // 測試資料
        if (MusicGameUtility.IsSingleModel)
        {
            lastDateTime = DateTime.UtcNow.AddSeconds(GameManager.GameCountdownSeconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var instanceObject = FindObjectOfType(typeof(GameManager));

        // 更新自己的分數
        var text = GameObject.Find("SocreText").GetComponent<Text>() as Text;

        if (text != null && GameManager.Instance.LocalUserData != null)
        {
            text.text = $"Self Score: {GameManager.Instance.LocalUserData.Score}";
        }

        if (MusicGameUtility.IsSingleModel && DateTime.UtcNow >= lastDateTime)
        {
            GameManager_GameEnded(null, EventArgs.Empty);
            lastDateTime = DateTime.UtcNow.AddDays(1);
        }
    }

    private void GameManager_ScoreUpdated(object sender, UserData e)
    {
        // 收到別人的分數更新
        var existUser = ParticipantsObjects.Keys.Where(x => x.Id == e.Id).FirstOrDefault();

        if (existUser == null)
        {
            return;
        }

        var existItem = ParticipantsObjects[existUser];
        UpdateUserScore(existItem, e);
    }

    private void GameManager_GameEnded(object sender, System.EventArgs e)
    {
        SceneManager.LoadScene(2);
    }

    private void GenerateUserScoreGameObject()
    {
        // 名單更新
        foreach (var item in GameManager.Instance.Participants)
        {
            var existUser = ParticipantsObjects.Keys.Where(x => x.Id == item.Value).FirstOrDefault();

            if (existUser == null)
            {
                var newUser = new UserData
                {
                    Id = item.Value,
                    Name = item.Key
                };

                MergeHeight -= 25f;
                var newItem = MusicGameUtility.GenerateSocreItem(ParticipantsScoreArea.transform, -300, MergeHeight, item.Key, 0);
                ParticipantsObjects.Add(newUser, newItem);
            }
        }
    }

    private void UpdateUserScore(GameObject gameObject, UserData e)
    {
        var textObject = gameObject.GetComponent<Text>() as Text;
        textObject.text = $"{e.Name}: {e.Score}";
    }
}