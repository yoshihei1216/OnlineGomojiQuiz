using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Score : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text scoreText;

    private int correctCount ;
    private int questionCount ;

    void Start()
    {
        correctCount = 0;
        questionCount = 0;
        scoreText.text = "0 / 0";
    }

    private void ScoreTransmit()
    {
        photonView.RPC(nameof(ScoreUpdate), RpcTarget.All,correctCount,questionCount);
    }

    // �X�R�A���X�V
    [PunRPC]
    public void ScoreUpdate(int correct,int question)
    {
        scoreText.text = $"{correct} / {question}";

        correctCount = correct;
        questionCount = question;
    }

    //���v���C���[�����[���֎Q���������ɌĂ΂��R�[���o�b�N
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            //��u�҂��Ă���X�R�A���X�V  
             Invoke("ScoreTransmit", 0.1f); 
        }
    }
}
