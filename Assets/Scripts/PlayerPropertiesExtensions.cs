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
    /// �����󋵂��擾
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <returns>T:�ρAF:��</returns>
    public static bool GetReadyBool(this Player player)
    {
        return (player.CustomProperties[IsReadyKey] is bool isReady) ? isReady : false;
    }

    /// <summary>
    /// �����󋵂�ύX
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <param name="isReady">T:�ρAF:��</param>
    public static void SetReadyBool(this Player player, bool isReady)
    {
        propsToSet[IsReadyKey] = isReady;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// ���͏󋵂��擾
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <returns>T:�ρAF:��</returns>
    public static bool GetFilledBool(this Player player)
    {
        return (player.CustomProperties[IsFilledKey] is bool isFilled) ? isFilled : false;
    }

    /// <summary>
    /// ���͏󋵂�ύX
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <param name="isFilled">T:�ρAF:��</param>
    public static void SetFilledBool(this Player player, bool isFilled)
    {
        propsToSet[IsFilledKey] = isFilled;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// �v���C���[�̏��Ԃ��擾
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <returns>����</returns>
    public static int GetPlayerPanelNumber(this Player player)
    {
        return (player.CustomProperties[PlayerPanelNumberKey] is int panelNumber) ? panelNumber : -1;
    }

    /// <summary>
    /// �v���C���[�̏��Ԃ�ύX
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <param name="panelNumber">����</param>
    public static void SetPlayerPanelNumber(this Player player, int panelNumber)
    {
        propsToSet[PlayerPanelNumberKey] = panelNumber;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// ���𐔂��擾
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <returns>����</returns>
    public static int GetCorrectCount(this Player player)
    {
        return (player.CustomProperties[CorrectCountKey] is int correctCount) ? correctCount : 0;
    }

    /// <summary>
    /// ���𐔂�ύX
    /// </summary>
    /// <param name="player">�v���C���[</param>
    /// <param name="correctCount">����</param>
    public static void SetCorrectCount(this Player player, int correctCount)
    {
        propsToSet[CorrectCountKey] = correctCount;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
