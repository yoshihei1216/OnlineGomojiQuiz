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
                    if (CountReadyPlayer() == maxPlayers)//�S�����������Ȃ�
                    {
                        PhotonNetwork.CurrentRoom.SetPhase(ConstInt.FILLED_PHASE);//���[������̓t�F�C�Y��
                    }
                }
            }
        }

        if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//�t�F�C�Y���ς�����Ƃ�
        {
            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

            if(nowPhase == ConstInt.READY_PHASE)//�����t�F�C�Y�ɕς�����Ƃ�
            {
                isReadyButton.gameObject.SetActive(true);
            }
            else if (nowPhase == ConstInt.FILLED_PHASE)//���̓t�F�C�Y�ɕς�����Ƃ�
            {
                isReadyButton.gameObject.SetActive(false);
                UpdateIsReady(false);
                if (photonView.IsMine) { PhotonNetwork.LocalPlayer.SetReadyBool(false); }
            }
        }
    }

    //���͏󋵂̐؂�ւ�
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

    // ���������̃����o�[���𐔂���
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
    /// �����󋵂ɍ��킹�ăe�L�X�g�ƐF��ω�
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

    //���v���C���[�����[���֎Q���������ɌĂ΂��R�[���o�b�N
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            //��u�҂��Ă���{�^���̏�Ԃ��X�V  
            if (nowPhase == ConstInt.READY_PHASE) { Invoke("TransmitIsReady", 0.1f); }
        }
    }
}
