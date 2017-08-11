using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AcceptInviteButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        MusicGameController.Instance.IsWaittingOtherGamer = false;
        GameManager.Instance.AcceptGame(GameManager.Instance.LocalUserData);
        SceneManager.LoadScene(1);
    }
}