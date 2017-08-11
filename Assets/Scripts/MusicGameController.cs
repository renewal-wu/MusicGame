using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicGameController
{
    #region singleton
    private static object lockObject = new object();

    private static MusicGameController instance;
    public static MusicGameController Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new MusicGameController();
                    }
                }
            }

            return instance;
        }
    }
    #endregion

    public bool IsWaittingOtherGamer { get; set; } = false;

    public int MAX_WAITTING_TIME_SECOND { get; private set; } = 300;

    public List<UserData> Participants { get; set; } = new List<UserData>();

    public MusicGameController()
    {
        //GameManager.Instance.GameEnded -= Instance_GameEnded;
        //GameManager.Instance.GameEnded += Instance_GameEnded;
    }

    public GameObject GenerateSocreItem(Transform parent, float x, float y, string name, int score)
    {
        var newItem = new GameObject();
        newItem.layer = 5;
        newItem.transform.parent = parent;

        newItem.AddComponent(typeof(RectTransform));
        var rectTransform = newItem.GetComponent<RectTransform>();
        newItem.transform.localPosition = new Vector3(x, y, 0);
        rectTransform.localPosition = new Vector3(x, y, 0);
        rectTransform.sizeDelta = new Vector2(200, 50);
        rectTransform.localScale = new Vector3(1, 1, 1);

        newItem.AddComponent(typeof(Text));
        var textObject = newItem.GetComponent<Text>() as Text;
        // 要給 font 才能顯示
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        textObject.font = ArialFont;
        textObject.material = ArialFont.material;
        textObject.text = $"{name}: {score.ToString()}";
        textObject.fontSize = 20;
        textObject.lineSpacing = 5;

        return newItem;
    }
}