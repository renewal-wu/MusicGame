using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine.SceneManagement;

public class GameSceneUtility : Singleton<GameSceneUtility>
{
    public bool IsWaittingOtherGamer { get; set; } = false;

    public int MAX_WAITTING_TIME_SECOND { get; private set; } = 300;

    public GameSceneUtility()
    {
        //GameManager.Instance.GameEnded -= Instance_GameEnded;
        //GameManager.Instance.GameEnded += Instance_GameEnded;
    }
}