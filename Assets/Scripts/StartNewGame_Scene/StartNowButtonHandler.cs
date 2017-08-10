using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartNowButtonHandler : MonoBehaviour
{
    public Button StartNowButton;

    public GameObject StartContainer;

    public GameObject WaitContainer;

    private void Awake()
    {
        StartNowButton.interactable = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //StartNowButton.interactable = GameManager.Instance.IsConnectionStarted;
    }

    private void OnDestroy()
    {
    }

    public void OnClick()
    {
        StartContainer.SetActive(false);
        WaitContainer.SetActive(true);
        GameSceneUtility.Instance.IsWaittingOtherGamer = true;
        //GameManager.Instance.StartGame();

        Debug.Log("start now game button be clicked, need display waiting other user ....");
    }
}