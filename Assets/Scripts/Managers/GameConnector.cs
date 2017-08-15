using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using UnityEngine;
#if NETFX_CORE
using Windows.Security.ExchangeActiveSyncProvisioning;
#endif

public class GameConnector : Singleton<GameConnector>
{
    /// <summary>
    /// 連線已建立
    /// </summary>
    public event EventHandler ConnectionStarted;
    private void OnConnectionStarted()
    {
        ConnectionStarted?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler MessageHandlerInitialed;
    private void OnMessageHandlerInitialed()
    {
        MessageHandlerInitialed?.Invoke(this, EventArgs.Empty);
    }

    public UserData LocalUserData { get; private set; }

    public delegate void MessageCallback(UserData userData, NetworkInMessage msg);
    private Dictionary<CommandType, MessageCallback> messageHandlers = new Dictionary<CommandType, MessageCallback>();
    public Dictionary<CommandType, MessageCallback> MessageHandlers
    {
        get
        {
            return messageHandlers;
        }
    }

    private NetworkConnectionAdapter connectionAdapter;
    private NetworkConnection serverConnection;

    private void Start()
    {
        LocalUserData = new UserData();

#if NETFX_CORE
        EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
        LocalUserData.Name = deviceInfo.FriendlyName;
#endif

        if (SharingStage.Instance.IsConnected)
        {
            Connected();
        }
        else
        {
            SharingStage.Instance.SharingManagerConnected += Connected;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Connected(object sender = null, EventArgs e = null)
    {
        SharingStage.Instance.SharingManagerConnected -= Connected;
        OnConnectionStarted();
        InitializeMessageHandlers();
    }

    private void InitializeMessageHandlers()
    {
        SharingStage sharingStage = SharingStage.Instance;

        if (sharingStage == null)
        {
            Debug.Log("Cannot Initialize CustomMessages. No SharingStage instance found.");
            return;
        }

        serverConnection = sharingStage.Manager.GetServerConnection();
        if (serverConnection == null)
        {
            Debug.Log("Cannot initialize CustomMessages. Cannot get a server connection.");
            return;
        }

        connectionAdapter = new NetworkConnectionAdapter();
        connectionAdapter.MessageReceivedCallback += OnMessageReceived;

        var localUser = SharingStage.Instance.Manager.GetLocalUser();
        LocalUserData.Id = localUser.GetID();
        LocalUserData.Name = localUser.GetName();

        for (byte index = (byte)CommandType.Start; index < (byte)CommandType.UpdateScore; index++)
        {
            if (MessageHandlers.ContainsKey((CommandType)index) == false)
            {
                MessageHandlers.Add((CommandType)index, null);
            }

            serverConnection.AddListener(index, connectionAdapter);
        }

        OnMessageHandlerInitialed();
    }

    private void OnMessageReceived(NetworkConnection connection, NetworkInMessage msg)
    {
        var commandType = (CommandType)msg.ReadByte();
        var userId = msg.ReadInt64();

        if (userId == LocalUserData.Id)
        {
            // 發送者與接收者相同，需忽略此次訊息
            return;
        }

        var userData = ReadUserData(userId, msg);

        MessageHandlers[commandType]?.Invoke(userData, msg);
    }

    private UserData ReadUserData(long userId, NetworkInMessage msg)
    {
        var userData = new UserData();
        userData.Id = userId;
        userData.Name = msg.ReadString();
        userData.Score = msg.ReadInt32();

        return userData;
    }

    public void SendStartGameCommand()
    {
        if (serverConnection != null && serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)CommandType.Start);

            serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.UserMessageChannelStart);
        }
    }

    public void SendAcceptCommand(UserData userData)
    {
        if (serverConnection != null && serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)CommandType.Accept);

            AppendTargetUserId(msg, userData);

            serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.UserMessageChannelStart);
        }
    }

    public void SendBeginCommand(Dictionary<string, int> participants)
    {
        if (serverConnection != null && serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)CommandType.Begin);

            AppendParticipants(msg, participants);

            serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.UserMessageChannelStart);
        }
    }

    public void SendUpdateScore()
    {
        if (serverConnection != null && serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)CommandType.UpdateScore);

            serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.UserMessageChannelStart);
        }
    }

    private NetworkOutMessage CreateMessage(byte commandType)
    {
        NetworkOutMessage msg = serverConnection.CreateMessage(commandType);
        msg.Write(commandType);

        AppendUserData(msg, LocalUserData);

        return msg;
    }

    private void AppendUserData(NetworkOutMessage msg, UserData userData)
    {
        msg.Write(userData.Id);
        msg.Write(userData.Name);
        msg.Write(userData.Score);
    }

    private void AppendTargetUserId(NetworkOutMessage msg, UserData userData)
    {
        msg.Write(userData.Id);
    }

    private void AppendParticipants(NetworkOutMessage msg, Dictionary<string, int> participants)
    {
        var participantsString = JsonUtility.ToJson(participants);
        msg.Write(participantsString);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (serverConnection != null)
        {
            for (byte index = (byte)CommandType.Start; index < (byte)CommandType.UpdateScore; index++)
            {
                serverConnection.RemoveListener((byte)index, connectionAdapter);
            }
            connectionAdapter.MessageReceivedCallback -= OnMessageReceived;
        }
    }
}
