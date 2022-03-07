using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{
    private const string IsReadyKey = "IsReady";
    private const string IsFilledKey = "IsFilled";
    private const string PlayerPanelNumberKey = "PlayerPanelNumber";
    private const string CorrectCountKey = "CorrectCount";
    private static readonly Hashtable propsToSet = new Hashtable();

    /// <summary>
    /// 準備状況を取得
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <returns>T:済、F:未</returns>
    public static bool GetReadyBool(this Player player)
    {
        return (player.CustomProperties[IsReadyKey] is bool isReady) ? isReady : false;
    }

    /// <summary>
    /// 準備状況を変更
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="isReady">T:済、F:未</param>
    public static void SetReadyBool(this Player player, bool isReady)
    {
        propsToSet[IsReadyKey] = isReady;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// 入力状況を取得
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <returns>T:済、F:未</returns>
    public static bool GetFilledBool(this Player player)
    {
        return (player.CustomProperties[IsFilledKey] is bool isFilled) ? isFilled : false;
    }

    /// <summary>
    /// 入力状況を変更
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="isFilled">T:済、F:未</param>
    public static void SetFilledBool(this Player player, bool isFilled)
    {
        propsToSet[IsFilledKey] = isFilled;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// プレイヤーの順番を取得
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <returns>順番</returns>
    public static int GetPlayerPanelNumber(this Player player)
    {
        return (player.CustomProperties[PlayerPanelNumberKey] is int panelNumber) ? panelNumber : -1;
    }

    /// <summary>
    /// プレイヤーの順番を変更
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="panelNumber">順番</param>
    public static void SetPlayerPanelNumber(this Player player, int panelNumber)
    {
        propsToSet[PlayerPanelNumberKey] = panelNumber;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// 正解数を取得
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <returns>正解数</returns>
    public static int GetCorrectCount(this Player player)
    {
        return (player.CustomProperties[CorrectCountKey] is int correctCount) ? correctCount : 0;
    }

    /// <summary>
    /// 正解数を変更
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="correctCount">正解数</param>
    public static void SetCorrectCount(this Player player, int correctCount)
    {
        propsToSet[CorrectCountKey] = correctCount;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
