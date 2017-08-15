using UnityEngine;
using UnityEngine.UI;

public class MusicGameUtility
{
    public static bool IsSingleModel { get; set; } = false;

    public static GameObject GenerateSocreItem(Transform parent, float x, float y, string name, int score)
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