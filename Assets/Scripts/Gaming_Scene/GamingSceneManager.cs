using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

public class GamingSceneManager : MonoBehaviour
{
    public GameObject ParticipantsScoreArea;

    private Dictionary<UserData, GameObject> ParticipantsObjects;

    private float MergeHeight = -160;

    private DateTime lastDateTime;

    // Use this for initialization
    void Start()
    {
        MusicGameController.Instance.IsWaittingOtherGamer = false;

        GameManager.Instance.GameEnded += Instance_GameEnded;
        GameManager.Instance.ScoreUpdated += Instance_ScoreUpdated;
        GameManager.Instance.ParticipantsUpdated += Instance_ParticipantsUpdated;

        ParticipantsObjects = new Dictionary<UserData, GameObject>();

        // 測試資料
        for (int i = 0; i < 5; i++)
        {
            UserData user = new UserData
            {
                Id = i,
                Name = $"pou{i}",
                Score = i
            };

            Instance_ScoreUpdated(null, user);
        }

        GameManager.Instance.StartGame();

        lastDateTime = DateTime.UtcNow.AddSeconds(GameManager.GameCountdownSeconds);
    }

    private void Instance_ParticipantsUpdated(object sender, System.EventArgs e)
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
                var newItem = MusicGameController.Instance.GenerateSocreItem(ParticipantsScoreArea.transform, -300, MergeHeight, item.Key, 0);
                ParticipantsObjects.Add(newUser, newItem);
            }
        }
    }

    private void Instance_ScoreUpdated(object sender, UserData e)
    {
        // 分數更新
        var existUser = ParticipantsObjects.Keys.Where(x => x.Id == e.Id).FirstOrDefault();

        if (existUser == null)
        {
            // 測試資料
            //GenerateNewUser(e);
            return;
        }

        var existItem = ParticipantsObjects[existUser];
        UpdateUserScore(existItem, e);
    }

    private void Instance_GameEnded(object sender, System.EventArgs e)
    {
        var users = ParticipantsObjects.Keys.ToList();
        users.Add(GameManager.Instance.LocalUserData);
        MusicGameController.Instance.Participants = users;

        SceneManager.LoadScene("GameEnded");
    }

    // Update is called once per frame
    void Update()
    {
        var text = GameObject.Find("SocreText").GetComponent<Text>() as Text;

        if (text != null && GameManager.Instance.LocalUserData != null)
        {
            text.text = $"Self Score: {GameManager.Instance.LocalUserData.Score}";
        }

        if (DateTime.UtcNow >= lastDateTime)
        {
            Instance_GameEnded(null, EventArgs.Empty);
            lastDateTime = DateTime.UtcNow.AddDays(1);
        }
    }

    private void GenerateNewUser(UserData e)
    {
        // identify user data to update score
        var newItem = new GameObject();
        newItem.layer = 5;
        newItem.transform.parent = ParticipantsScoreArea.transform;
        MergeHeight -= 25f;

        newItem.AddComponent(typeof(RectTransform));
        var rectTransform = newItem.GetComponent<RectTransform>();
        newItem.transform.localPosition = new Vector3(-300, MergeHeight, 0);
        rectTransform.localPosition = new Vector3(-300, MergeHeight, 0);
        rectTransform.sizeDelta = new Vector2(200, 50);
        rectTransform.localScale = new Vector3(1, 1, 1);

        newItem.AddComponent(typeof(Text));
        var textObject = newItem.GetComponent<Text>() as Text;
        // 要給 font 才能顯示
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        textObject.font = ArialFont;
        textObject.material = ArialFont.material;
        textObject.text = $"{e.Name}: {e.Score}";
        textObject.fontSize = 20;
        textObject.lineSpacing = 5;

        newItem.SetActive(true);

        ParticipantsObjects.Add(e, newItem);
    }

    private void UpdateUserScore(GameObject gameObject, UserData e)
    {
        var textObject = gameObject.GetComponent<Text>() as Text;
        textObject.text = $"{e.Name}: {e.Score}";
    }
}