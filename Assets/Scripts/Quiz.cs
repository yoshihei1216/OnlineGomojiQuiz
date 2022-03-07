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
    private List<string[]> csvDatas = new List<string[]>();//csv�̒��g�����郊�X�g
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
                DecideQuizNumber();//���ԍ�������
            }

            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();
            if (nowPhase == ConstInt.READY_PHASE)
            {
                //�����e�L�X�g
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
            if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//�t�F�C�Y���ς�����Ƃ�
            {
                nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

                if (nowPhase == ConstInt.READY_PHASE)//�����t�F�C�Y�ɕς�����Ƃ�
                {
                    UpdateQuizAnswer();

                    //�������킹
                    int quizNumber = PhotonNetwork.CurrentRoom.GetQuizNumber();
                    int playerPanelNumber = PhotonNetwork.LocalPlayer.GetPlayerPanelNumber();
                    string correctAnswer = csvDatas[quizNumber][ANSWER_COLUMN][playerPanelNumber].ToString();

                    inputArea.AnswerCheck(correctAnswer);
                    
                    if (PhotonNetwork.IsMasterClient)
                    {   
                        if (LeftQuizNumber.Count == 0) { ResetQuizNumber(); }//���ԍ����X�g����ɂȂ����烊�X�g�����Z�b�g
                        DecideQuizNumber();//���̖��ԍ�������

                        int questionCount = PhotonNetwork.CurrentRoom.GetQuestionCount() ;
                        PhotonNetwork.CurrentRoom.SetQuestionCount(questionCount+1);
                    }
                    
                }
                else if (nowPhase == ConstInt.FILLED_PHASE) //���̓t�F�C�Y�ɕς�����Ƃ�
                {
                    int correctCount= PhotonNetwork.LocalPlayer.GetCorrectCount(); 
                    int questionCount= PhotonNetwork.CurrentRoom.GetQuestionCount();
                    photonView.RPC(nameof(score.ScoreUpdate), RpcTarget.All,correctCount,questionCount-1);

                    StartCoroutine(Question(PhotonNetwork.CurrentRoom.GetQuestionCount())); 
                }
            }
        }
    }

    // csv�f�[�^�����X�g�ɓǂݍ���
    private void CsvLoad()
    {
        StringReader reader = new StringReader(csvFile.text);

        // ��s���ǂݍ���, �ŕ��������X�g�ɒǉ�����
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }
    }

    /// ���ԍ����X�g�����Z�b�g
    private void ResetQuizNumber()
    {
        for (int i = 0; i < csvDatas.Count; i++)
        {
            LeftQuizNumber.Add(i);
        }
    }

    // ���ԍ�������
    private void DecideQuizNumber()
    {
        //��肩�Ԃ�������
        int randomNumber = Random.Range(0, LeftQuizNumber.Count);
        int quizNumber = LeftQuizNumber[randomNumber];
        LeftQuizNumber.RemoveAt(randomNumber);

        PhotonNetwork.CurrentRoom.SetQuizNumber(quizNumber);
    }

    //�o�莞�̏���
    private IEnumerator Question(int questionCount)
    {
        questionCountBoard.color = new Color32(255, 255, 255, 255);
        questionCountBoardText.text = $"�� { questionCount.ToString()} ��";

        audioSource.clip = questionSound;
        audioSource.Play();

        yield return new WaitForSeconds(1.0f);

        questionCountBoardText.text = "";
        questionCountBoard.color = new Color32(255, 255, 255, 0);

        audioSource.clip = thinkingSound;
        audioSource.loop = true;
        audioSource.Play();

        UpdateQuizSentence();//���̖����o��
    }

    // ��蕶��������
    private void UpdateQuizSentence()
    {
        quizSentence.text = csvDatas[PhotonNetwork.CurrentRoom.GetQuizNumber()][SENTENCE_COLUMN];
        quizWord.text = csvDatas[PhotonNetwork.CurrentRoom.GetQuizNumber()][WORD_COLUMN];
        quizAnswer.text = "";
    }

    // ������������
    private void UpdateQuizAnswer()
    {
        quizAnswer.text = $"�����́u{ csvDatas[PhotonNetwork.CurrentRoom.GetQuizNumber()][ANSWER_COLUMN]}�v";
    }
}
