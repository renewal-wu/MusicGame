using UnityEngine;
using UnityEngine.SceneManagement;

public class CancelWaittingButtonHandler : MonoBehaviour
{
    public GameObject StartContainer;

    public GameObject WaitContainer;

    private int waitingCount;
    
    // Use this for initialization
    void Start()
    {
        waitingCount = GameSceneUtility.Instance.MAX_WAITTING_TIME_SECOND;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSceneUtility.Instance.IsWaittingOtherGamer)
        {
            waitingCount -= 1;

            if (waitingCount <= 0)
            {
                GameSceneUtility.Instance.IsWaittingOtherGamer = false;
                SceneManager.LoadScene("Gaming");
            }
        }
        else
        {
            waitingCount = GameSceneUtility.Instance.MAX_WAITTING_TIME_SECOND;
        }
    }

    public void OnClick()
    {
        WaitContainer.SetActive(false);
        StartContainer.SetActive(true);
        GameSceneUtility.Instance.IsWaittingOtherGamer = false;
        Debug.Log("cancel waitting, and display start now button.");
    }

    private void OnDestroy()
    {
        
    }
}