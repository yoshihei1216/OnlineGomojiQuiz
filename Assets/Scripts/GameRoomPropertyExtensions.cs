using ExitGames.Client.Photon;
using Photon.Realtime;

public static class GameRoomPropertyExtensions
{
    private const string QuizNumberKey = "QuizNumber";
    private const string QuestionCountKey = "QuestionCount";
    private const string PhaseKey = "Phase";
    private static readonly Hashtable propsToSet = new Hashtable();

    /// <summary>
    /// 問題番号を取得
    /// </summary>
    /// <param name="room">部屋</param>
    /// <returns>問題番号</returns>
    public static int GetQuizNumber(this Room room)
    {
        return (room.CustomProperties[QuizNumberKey] is int number) ? number : 0;
    }

    /// <summary>
    /// 問題番号を変更
    /// </summary>
    /// <param name="room">部屋</param>
    /// <param name="quizNumber">問題番号</param>
    public static void SetQuizNumber(this Room room, int quizNumber)
    {
        propsToSet[QuizNumberKey] = quizNumber;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// 出題番号を取得
    /// </summary>
    /// <param name="room">部屋</param>
    /// <returns>出題番号</returns>
    public static int GetQuestionCount(this Room room)
    {
        return (room.CustomProperties[QuestionCountKey] is int number) ? number : 1;
    }

    /// <summary>
    /// 出題番号を変更
    /// </summary>
    /// <param name="room">部屋</param>
    /// <param name="questionCount">出題番号</param>
    public static void SetQuestionCount(this Room room, int questionCount)
    {
        propsToSet[QuestionCountKey] = questionCount;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// フェイズを取得
    /// </summary>
    /// <param name="room">部屋</param>
    /// <returns>0=準備フェイズ,1=入力フェイズ</returns>
    public static int GetPhase(this Room room)
    {
        return (room.CustomProperties[PhaseKey] is int count) ? count : 0;
    }

    /// <summary>
    /// フェイズを変更
    /// </summary>
    /// <param name="room">部屋</param>
    /// <param name="phaseCount">0=準備フェイズ,1=入力フェイズ</param>
    public static void SetPhase(this Room room, int phaseCount)
    {
        propsToSet[PhaseKey] = phaseCount;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
