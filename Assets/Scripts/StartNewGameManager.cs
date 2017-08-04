using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://onedrive.live.com/view.aspx?resid=E5D8C3F4EFAEC162!2592141&ithint=file%2cdocx&app=Word&authkey=!AKyw0BzT-t2zUv8
/// </summary>
public class StartNewGameManager : MonoBehaviour {

    public GameObject StartContainer;

    public GameObject WaitContainer;

    public GameObject AcceptIniteContainer;

    void Awake()
    {
        StartContainer.SetActive(true);        
        WaitContainer.SetActive(false);
        AcceptIniteContainer.SetActive(false);
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
