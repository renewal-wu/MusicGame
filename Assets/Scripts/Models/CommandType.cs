using HoloToolkit.Sharing;

public enum CommandType : byte
{
    /// <summary>
    /// 開始遊戲
    /// </summary>
    Start = MessageID.UserMessageIDStart,

    /// <summary>
    /// 接受遊戲
    /// </summary>
    Accept,

    /// <summary>
    /// 啟動遊戲
    /// </summary>
    Begin,

    /// <summary>
    /// 更新分數
    /// </summary>
    UpdateScore
}