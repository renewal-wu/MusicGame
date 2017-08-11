using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://onedrive.live.com/view.aspx?resid=E5D8C3F4EFAEC162!2592141&ithint=file%2cdocx&app=Word&authkey=!AKyw0BzT-t2zUv8
/// </summary>
public class StartGameSceneManager : MonoBehaviour
{
    public GameObject StartContainer;

    public GameObject WaitContainer;

    public GameObject InviteContainer;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        StartContainer.SetActive(true);
        WaitContainer.SetActive(false);
        InviteContainer.SetActive(false);
        GameManager.Instance.GameStartCommandReceived += GameManager_GameStartCommandReceived;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GameManager_GameStartCommandReceived(object sender, UserData e)
    {
        StartContainer.SetActive(false);
        WaitContainer.SetActive(false);
        InviteContainer.SetActive(true);
    }
}