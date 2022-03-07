using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Const;

public class Ready : MonoBehaviourPunCallbacks
{
    [SerializeField] private string readyText;
    [SerializeField] private string unreadyText;
    [SerializeField] private Color readyTextColor;
    [SerializeField] private Color unreadyTextColor;
    [SerializeField] private Text isReadyText;
    [SerializeField] private Button isReadyButton;

    private int maxPlayers;
    private int nowPhase;

    void Start()
    {
        maxPlayers=PhotonNetwork.CurrentRoom.MaxPlayers;
        nowPhase = 0;
        isReadyText.text = unreadyText;
        isReadyText.color = unreadyTextColor;

        if (photonView.IsMine)
        {
            PhotonNetwork.LocalPlayer.SetReadyBool(false);
        }
        else 
        { 
            isReadyButton.interactable = false; 
        }
    }

    private void Update()
    {
        if (nowPhase == ConstInt.READY_PHASE)
        {
            if (photonView.IsMine)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (CountReadyPlayer() == maxPlayers)//全員準備完了なら
                    {
                        PhotonNetwork.CurrentRoom.SetPhase(ConstInt.FILLED_PHASE);//ルームを入力フェイズへ
                    }
                }
            }
        }

        if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//フェイズが変わったとき
        {
            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

            if(nowPhase == ConstInt.READY_PHASE)//準備フェイズに変わったとき
            {
                isReadyButton.gameObject.SetActive(true);
            }
            else if (nowPhase == ConstInt.FILLED_PHASE)//入力フェイズに変わったとき
            {
                isReadyButton.gameObject.SetActive(false);
                UpdateIsReady(false);
                if (photonView.IsMine) { PhotonNetwork.LocalPlayer.SetReadyBool(false); }
            }
        }
    }

    //入力状況の切り替え
    public void SwitchIsReady()
    {
        bool isReady = PhotonNetwork.LocalPlayer.GetReadyBool();
        photonView.RPC(nameof(UpdateIsReady), RpcTarget.All, !isReady);
        PhotonNetwork.LocalPlayer.SetReadyBool(!isReady);
    }

    private void TransmitIsReady()
    {
        bool isReady = PhotonNetwork.LocalPlayer.GetReadyBool();
        photonView.RPC(nameof(UpdateIsReady), RpcTarget.All, isReady);
    }

    // 準備完了のメンバー数を数える
    private int CountReadyPlayer()
    {
        int readyPlayerNumber = 0;
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        { 
            if (player.GetReadyBool()) { readyPlayerNumber++; } 
        }

        return readyPlayerNumber;
    }

    /// <summary>
    /// 準備状況に合わせてテキストと色を変化
    /// </summary>
    [PunRPC]
    private void UpdateIsReady(bool isReady)
    {
        if (isReady)
        {
            isReadyText.text = readyText;
            isReadyText.color = readyTextColor;
        }
        else
        {
            isReadyText.text = unreadyText;
            isReadyText.color = unreadyTextColor;
        }
    }

    //他プレイヤーがルームへ参加した時に呼ばれるコールバック
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            //一瞬待ってからボタンの状態を更新  
            if (nowPhase == ConstInt.READY_PHASE) { Invoke("TransmitIsReady", 0.1f); }
        }
    }
}
