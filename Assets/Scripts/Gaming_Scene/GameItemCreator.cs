using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemCreator : MonoBehaviour
{
    private string[] MaterialSets = new string[] { "Red", "Blue", "Green", "Brown" };

    private DateTime GameEndTime { get; set; }

    private DateTime LastGenerateTime { get; set; }

    [Tooltip("每一個項目產生的間隔")]
    public float DelaySecondTime = 15;

    [Tooltip("物件使用的 Material")]
    public string MaterialName = "Red";

    [Tooltip("預設物件分數")]
    public int DefaultScore = 3;

    [Tooltip("最小重力指數")]
    public int MinDrag = 10;

    [Tooltip("最大重力指數")]
    public int MaxDrag = 20;

    // Use this for initialization
    void Start()
    {
        GameEndTime = DateTime.UtcNow.AddSeconds(GameManager.GameCountdownSeconds);
        LastGenerateTime = DateTime.UtcNow;
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.UtcNow >= GameEndTime)
        {
            return;
        }

        var diff = (DateTime.UtcNow - LastGenerateTime).TotalSeconds;

        if (diff >= DelaySecondTime)
        {
            LastGenerateTime = DateTime.UtcNow;

            UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
            //GenerateGameItem(this.transform);
            StartCoroutine(DelayGenerate());
        }
    }

    private IEnumerator DelayGenerate()
    {
        yield return new WaitForSeconds(60);
    }

    private GameObject GenerateGameItem(Transform parentTransform)
    {
        var newItem = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // set postiion
        Vector3 position = transform.position;
        position.x = UnityEngine.Random.Range(-3f, 3f);
        position.z = UnityEngine.Random.Range(3f, 8f);
        newItem.transform.position = position;
        newItem.transform.parent = parentTransform;

        // set rigidbody
        newItem.AddComponent(typeof(Rigidbody));
        var rigid = newItem.GetComponent<Rigidbody>();
        rigid.drag = UnityEngine.Random.Range(MinDrag, MaxDrag);        

        // set material 
        int score = UnityEngine.Random.Range(0, 3);
        string materialName = MaterialName; //MaterialSets[score];
        var material = Resources.Load($"Materials/{materialName}", typeof(Material)) as Material;
        var render = newItem.GetComponent<MeshRenderer>();
        render.material = material;        

        // set controller
        newItem.AddComponent(typeof(GameItemController));
        var gameController = newItem.GetComponent<GameItemController>();
        gameController.DefaultScore = DefaultScore;

        return newItem;
    }
}