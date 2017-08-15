using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbortInviteButtonHandler : MonoBehaviour {

    public GameObject StartContainer;

    public GameObject InviteContainer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        InviteContainer.SetActive(false);
        StartContainer.SetActive(true);
    }
}
