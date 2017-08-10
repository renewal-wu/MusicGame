using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

/// <summary>
/// 控制每一個項目是否消失跟被點擊到。
/// </summary>
public class GameItemController : MonoBehaviour, IInputClickHandler, IInputHandler, IFocusable
{
    private float Underline = 0f;

    private bool Focused { get; set; } = false;

    private string Id { get; set; }

    public int DefaultScore { get; set; } = 1;

    public void OnFocusEnter()
    {
        Focused = true;
    }

    public void OnFocusExit()
    {
        Focused = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
    }

    public void OnInputDown(InputEventData eventData)
    {
    }

    public void OnInputUp(InputEventData eventData)
    {
    }

    private void Awake()
    {
        Id = Guid.NewGuid().ToString();
    }

    private void Update()
    {
        if (transform.position.y <= Underline)
        {
            // fast move
            if (Focused)
            {
                Debug.Log($"got value: {Id}");
                GameManager.Instance.LocalUserData.Score += DefaultScore;
                GameManager.Instance.UpdateScore(GameManager.Instance.LocalUserData.Score);
            }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}