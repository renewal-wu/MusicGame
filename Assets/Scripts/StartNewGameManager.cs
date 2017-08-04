using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNewGameManager : MonoBehaviour {

    public GameObject StartContainer;

    public GameObject WaitContainer;

    public GameObject AcceptIniteContainer;

    void Awake()
    {
        StartContainer.SetActive(true);        
        WaitContainer.SetActive(false);
        AcceptIniteContainer.SetActive(false);

        GameManager.Instance.GameStartCommandReceived += Instance_GameStartCommandReceived;
    }

    private void Instance_GameStartCommandReceived(object sender, System.EventArgs e)
    {
        StartContainer.SetActive(false);
        WaitContainer.SetActive(false);
        AcceptIniteContainer.SetActive(true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
