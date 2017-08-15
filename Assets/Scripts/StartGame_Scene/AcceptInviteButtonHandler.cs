using UnityEngine;

public class AcceptInviteButtonHandler : MonoBehaviour {

    public GameObject WaitContainer;

    public GameObject InviteContainer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        // 送出 accept 等待收到 GameBegun
        GameManager.Instance.AcceptGame(GameManager.Instance.LocalUserData);
        WaitContainer.SetActive(true);
        InviteContainer.SetActive(false);
    }
}