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

    // スコアを更新
    [PunRPC]
    public void ScoreUpdate(int correct,int question)
    {
        scoreText.text = $"{correct} / {question}";

        correctCount = correct;
        questionCount = question;
    }

    //他プレイヤーがルームへ参加した時に呼ばれるコールバック
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            //一瞬待ってからスコアを更新  
             Invoke("ScoreTransmit", 0.1f); 
        }
    }
}
