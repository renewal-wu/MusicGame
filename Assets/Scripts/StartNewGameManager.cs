using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNewGameManager : MonoBehaviour {

    public GameObject StartContainer;

    public GameObject WaitContainer;

    void Awake()
    {
        StartContainer.SetActive(true);        
        WaitContainer.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
