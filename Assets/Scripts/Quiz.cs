using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

using Const;

public class Quiz : MonoBehaviourPunCallbacks
{   
    [SerializeField] private string firstQuizSentence;
    [SerializeField] private string firstQuizWord;
    [SerializeField] private string firstQuizAnswer;
    [SerializeField] private InputArea inputArea;
    [SerializeField] private TextAsset csvFile;
    [SerializeField] private AudioClip questionSound;
    [SerializeField] private AudioClip thinkingSound;
    [SerializeField] private Score score;

    private int nowPhase;
    private List<string[]> csvDatas = new List<string[]>();//csvの中身を入れるリスト
    private List<int> LeftQuizNumber = new List<int>();
    private Text quizSentence;
    private Text quizWord;
    private Text quizAnswer;
    private Image questionCountBoard;
    private Text questionCountBoardText;
    private AudioSource audioSource;

    private const int SENTENCE_COLUMN = 1;
    private const int WORD_COLUMN = 2;
    private const int ANSWER_COLUMN = 3;


    void Start()
    {
        if (photonView.IsMine)
        {
            nowPhase = 0;
            quizSentence = GameObject.Find("QuizSentenceText").GetComponent<Text>();
            quizWord = GameObject.Find("QuizWordsText").GetComponent<Text>();
            quizAnswer = GameObject.Find("QuizAnswerText").GetComponent<Text>();
            audioSource = GameObject.Find("SoundVolumeSlider").GetComponent<AudioSource>();

            CsvLoad();
            ResetQuizNumber();

            questionCountBoard = GameObject.Find("QuestionCountBoard").GetComponent<Image>();
            questionCountBoardText = questionCountBoard.transform.Find("QuestionCountBoardText").GetComponent<Text>();

            questionCountBoardText.text = "";
            questionCountBoard.color = new Color32(255, 255, 255, 0);

            if (PhotonNetwork.IsMasterClient)
            {
                DecideQuizNumber();//問題番号を決定
            }

            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();
            if (nowPhase == ConstInt.READY_PHASE)
            {
                //初期テキスト
                quizSentence.text = firstQuizSentence;
                quizWord.text = firstQuizWord;
                quizAnswer.text = firstQuizAnswer;
            }
            else if (nowPhase == ConstInt.FILLED_PHASE)
            {
                UpdateQuizSentence();

                //if(PhotonNetwork.CurrentRoom.GetQuestionCount()>0)
                {
                    audioSource.clip = thinkingSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//フェイズが変わったとき
            {
                nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

                if (nowPhase == ConstInt.READY_PHASE)//準備フェイズに変わったとき
                {
                    UpdateQuizAnswer();

                    //答え合わせ
                    int quizNumber = PhotonNetwork.CurrentRoom.GetQuizNumber();
                    int playerPanelNumber = PhotonNetwork.LocalPlayer.GetPlayerPanelNumber();
                    string correctAnswer = csvDatas[quizNumber][ANSWER_COLUMN][playerPanelNumber].ToString();

                    inputArea.AnswerCheck(correctAnswer);
                    
                    if (PhotonNetwork.IsMasterClient)
                    {   
                        if (LeftQuizNumber.Count == 0) { ResetQuizNumber(); }//問題番号リストが空になったらリストをリセット
                        DecideQuizNumber();//次の問題番号を決定

                        int questionCount = PhotonNetwork.CurrentRoom.GetQuestionCount() ;
                        PhotonNetwork.CurrentRoom.SetQuestionCount(questionCount+1);
                    }
                    
                }
                else if (nowPhase == ConstInt.FILLED_PHASE) //入力フェイズに変わったとき
                {
                    int correctCount= PhotonNetwork.LocalPlayer.GetCorrectCount(); 
                    int questionCount= PhotonNetwork.CurrentRoom.GetQuestionCount();
                    photonView.RPC(nameof(score.ScoreUpdate), RpcTarget.All,correctCount,questionCount-1);

                    StartCoroutine(Question(PhotonNetwork.CurrentRoom.GetQuestionCount())); 
                }
            }
        }
    }

    // csvデータをリストに読み込み
    private void CsvLoad()
    {
        StringReader reader = new StringReader(csvFile.text);

        // 一行ずつ読み込み, で分割しつつリストに追加する
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }
    }

    /// 問題番号リストをリセット
    private void ResetQuizNumber()
    {
        for (int i = 0; i < csvDatas.Count; i++)
        {
            LeftQuizNumber.Add(i);
        }
    }

    // 問題番号を決定
    private void DecideQuizNumber()
    {
        //問題かぶりを避ける
        int randomNumber = Random.Range(0, LeftQuizNumber.Count);
        int quizNumber = LeftQuizNumber[randomNumber];
        LeftQuizNumber.RemoveAt(randomNumber);

        PhotonNetwork.CurrentRoom.SetQuizNumber(quizNumber);
    }

    //出題時の処理
    private IEnumerator Question(int questionCount)
    {
        questionCountBoard.color = new Color32(255, 255, 255, 255);
        questionCountBoardText.text = $"第 { questionCount.ToString()} 問";

        audioSource.clip = questionSound;
        audioSource.Play();

        yield return new WaitForSeconds(1.0f);

        questionCountBoardText.text = "";
        questionCountBoard.color = new Color32(255, 255, 255, 0);

        audioSource.clip = thinkingSound;
        audioSource.loop = true;
        audioSource.Play();

        UpdateQuizSentence();//次の問題を出題
    }

    // 問題文書き換え
    private void UpdateQuizSentence()
    {
        quizSentence.text = csvDatas[PhotonNetwork.CurrentRoom.GetQuizNumber()][SENTENCE_COLUMN];
        quizWord.text = csvDatas[PhotonNetwork.CurrentRoom.GetQuizNumber()][WORD_COLUMN];
        quizAnswer.text = "";
    }

    // 答え書き換え
    private void UpdateQuizAnswer()
    {
        quizAnswer.text = $"正解は「{ csvDatas[PhotonNetwork.CurrentRoom.GetQuizNumber()][ANSWER_COLUMN]}」";
    }
}
