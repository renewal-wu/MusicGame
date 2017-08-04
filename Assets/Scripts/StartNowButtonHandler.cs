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
        
        GameManager.Instance.ConnectionStarted += Instance_ConnectionStarted;
    }

    private void Instance_ConnectionStarted(object sender, System.EventArgs e)
    {
        if (StartNowButton != null)
        {
            StartNowButton.interactable = GameManager.Instance.IsConnectionStarted;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        GameManager.Instance.ConnectionStarted -= Instance_ConnectionStarted;
    }

    public void OnClick()
    {
        StartContainer.SetActive(false);
        WaitContainer.SetActive(true);

        Debug.Log("start now game button be clicked, need display waiting other user ....");
    }
}