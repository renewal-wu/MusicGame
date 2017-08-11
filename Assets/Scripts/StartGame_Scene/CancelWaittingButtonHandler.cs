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
        waitingCount = MusicGameController.Instance.MAX_WAITTING_TIME_SECOND;
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicGameController.Instance.IsWaittingOtherGamer)
        {
            waitingCount -= 1;

            if (waitingCount <= 0)
            {
                MusicGameController.Instance.IsWaittingOtherGamer = false;
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            waitingCount = MusicGameController.Instance.MAX_WAITTING_TIME_SECOND;
        }
    }

    public void OnClick()
    {
        WaitContainer.SetActive(false);
        StartContainer.SetActive(true);
        MusicGameController.Instance.IsWaittingOtherGamer = false;
        Debug.Log("cancel waitting, and display start now button.");
    }
}