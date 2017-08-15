using UnityEngine;
using UnityEngine.SceneManagement;

public class CancelWaittingButtonHandler : MonoBehaviour
{
    public GameObject StartContainer;

    public GameObject WaitContainer;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        // 取消回到開始畫面
        WaitContainer.SetActive(false);
        StartContainer.SetActive(true);
        Debug.Log("cancel waitting, and display start now button.");
    }
}