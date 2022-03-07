using ExitGames.Client.Photon;
using Photon.Realtime;

public static class GameRoomPropertyExtensions
{
    private const string QuizNumberKey = "QuizNumber";
    private const string QuestionCountKey = "QuestionCount";
    private const string PhaseKey = "Phase";
    private static readonly Hashtable propsToSet = new Hashtable();

    /// <summary>
    /// ���ԍ����擾
    /// </summary>
    /// <param name="room">����</param>
    /// <returns>���ԍ�</returns>
    public static int GetQuizNumber(this Room room)
    {
        return (room.CustomProperties[QuizNumberKey] is int number) ? number : 0;
    }

    /// <summary>
    /// ���ԍ���ύX
    /// </summary>
    /// <param name="room">����</param>
    /// <param name="quizNumber">���ԍ�</param>
    public static void SetQuizNumber(this Room room, int quizNumber)
    {
        propsToSet[QuizNumberKey] = quizNumber;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// �o��ԍ����擾
    /// </summary>
    /// <param name="room">����</param>
    /// <returns>�o��ԍ�</returns>
    public static int GetQuestionCount(this Room room)
    {
        return (room.CustomProperties[QuestionCountKey] is int number) ? number : 1;
    }

    /// <summary>
    /// �o��ԍ���ύX
    /// </summary>
    /// <param name="room">����</param>
    /// <param name="questionCount">�o��ԍ�</param>
    public static void SetQuestionCount(this Room room, int questionCount)
    {
        propsToSet[QuestionCountKey] = questionCount;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    /// <summary>
    /// �t�F�C�Y���擾
    /// </summary>
    /// <param name="room">����</param>
    /// <returns>0=�����t�F�C�Y,1=���̓t�F�C�Y</returns>
    public static int GetPhase(this Room room)
    {
        return (room.CustomProperties[PhaseKey] is int count) ? count : 0;
    }

    /// <summary>
    /// �t�F�C�Y��ύX
    /// </summary>
    /// <param name="room">����</param>
    /// <param name="phaseCount">0=�����t�F�C�Y,1=���̓t�F�C�Y</param>
    public static void SetPhase(this Room room, int phaseCount)
    {
        propsToSet[PhaseKey] = phaseCount;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
