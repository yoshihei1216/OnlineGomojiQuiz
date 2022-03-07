using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Const;

public class Filled : MonoBehaviourPunCallbacks
{
    [SerializeField] private string filledText;
    [SerializeField] private string unfilledText;
    [SerializeField] private Color filledTextColor;
    [SerializeField] private Color unfilledTextColor;
    [SerializeField] private Text isFilledText;
    [SerializeField] private Button isFilledButton;
    [SerializeField] private AudioClip answerCheckSound;

    private int maxPlayers;
    private int nowPhase;
    private AudioSource audioSource;

    void Start()
    {
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        nowPhase = 0;
        isFilledText.text = unfilledText;
        isFilledText.color = unfilledTextColor;
        audioSource = GameObject.Find("SoundVolumeSlider").GetComponent<AudioSource>();

        if (photonView.IsMine) 
        { 
            PhotonNetwork.LocalPlayer.SetFilledBool(false);
        }
        else 
        { 
            isFilledButton.interactable = false; 
        }
    }

    private bool one = false;

    private void Update()
    {
        if (nowPhase == ConstInt.FILLED_PHASE)
        {
            if (photonView.IsMine)
            {
                if (CountFilledPlayer() == maxPlayers)//全員準備完了なら
                {
                    if(!one)
                    {
                        audioSource.clip = answerCheckSound;
                        audioSource.loop = false;
                        audioSource.Play();
                        one = true;
                    }

                    if (PhotonNetwork.IsMasterClient)
                    {
                        StartCoroutine(ToReadyPhase());//ルームを準備フェイズへ
                    }
                }
            }
        }

        if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//フェイズが変わったとき
        {
            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

            if (nowPhase == ConstInt.READY_PHASE)//準備フェイズに変わったとき
            {
                isFilledButton.gameObject.SetActive(false);
                UpdateIsFilled(false);
                if (photonView.IsMine) { PhotonNetwork.LocalPlayer.SetFilledBool(false); }

                one = false;
            }
            else if (nowPhase == ConstInt.FILLED_PHASE)//入力フェイズに変わったとき
            {
                isFilledButton.gameObject.SetActive(true);
            }
        }
    }
    
    public void SwitchIsFilled()//入力状況の切り替え
    {
        bool isFilled = PhotonNetwork.LocalPlayer.GetFilledBool();
        photonView.RPC(nameof(UpdateIsFilled), RpcTarget.All, !isFilled);
        PhotonNetwork.LocalPlayer.SetFilledBool(!isFilled);
    }

    private void TransmitIsFilled()
    {
        bool isFilled = PhotonNetwork.LocalPlayer.GetFilledBool();
        photonView.RPC(nameof(UpdateIsFilled), RpcTarget.All, isFilled);
    }

    private int CountFilledPlayer()// 入力完了のメンバー数を数える
    {
        int filledPlayerNumber = 0;
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        { 
            if (player.GetFilledBool()) { filledPlayerNumber++; } 
        }

        return filledPlayerNumber;
    }

    //ルームを準備フェイズへ
    private IEnumerator ToReadyPhase()
    {
        yield return new WaitForSeconds(1.0f);

        PhotonNetwork.CurrentRoom.SetPhase(ConstInt.READY_PHASE);
    }

    /// <summary>
    /// 入力状況に合わせてテキストと色を変化
    /// </summary>
    [PunRPC]
    private void UpdateIsFilled(bool isFilled)
    {
        if (isFilled)
        {
            isFilledText.text = filledText;
            isFilledText.color = filledTextColor;
        }
        else
        {
            isFilledText.text = unfilledText;
            isFilledText.color = unfilledTextColor;
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)//他プレイヤーがルームへ参加した時に呼ばれるコールバック
    {
        if (photonView.IsMine)
        {
            //一瞬待ってからボタンの状態を更新
            if (nowPhase == ConstInt.FILLED_PHASE) { Invoke("TransmitIsFilled", 0.1f); }
        }
    }
}

