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
                if (CountFilledPlayer() == maxPlayers)//�S�����������Ȃ�
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
                        StartCoroutine(ToReadyPhase());//���[���������t�F�C�Y��
                    }
                }
            }
        }

        if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//�t�F�C�Y���ς�����Ƃ�
        {
            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

            if (nowPhase == ConstInt.READY_PHASE)//�����t�F�C�Y�ɕς�����Ƃ�
            {
                isFilledButton.gameObject.SetActive(false);
                UpdateIsFilled(false);
                if (photonView.IsMine) { PhotonNetwork.LocalPlayer.SetFilledBool(false); }

                one = false;
            }
            else if (nowPhase == ConstInt.FILLED_PHASE)//���̓t�F�C�Y�ɕς�����Ƃ�
            {
                isFilledButton.gameObject.SetActive(true);
            }
        }
    }
    
    public void SwitchIsFilled()//���͏󋵂̐؂�ւ�
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

    private int CountFilledPlayer()// ���͊����̃����o�[���𐔂���
    {
        int filledPlayerNumber = 0;
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        { 
            if (player.GetFilledBool()) { filledPlayerNumber++; } 
        }

        return filledPlayerNumber;
    }

    //���[���������t�F�C�Y��
    private IEnumerator ToReadyPhase()
    {
        yield return new WaitForSeconds(1.0f);

        PhotonNetwork.CurrentRoom.SetPhase(ConstInt.READY_PHASE);
    }

    /// <summary>
    /// ���͏󋵂ɍ��킹�ăe�L�X�g�ƐF��ω�
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
    
    public override void OnPlayerEnteredRoom(Player newPlayer)//���v���C���[�����[���֎Q���������ɌĂ΂��R�[���o�b�N
    {
        if (photonView.IsMine)
        {
            //��u�҂��Ă���{�^���̏�Ԃ��X�V
            if (nowPhase == ConstInt.FILLED_PHASE) { Invoke("TransmitIsFilled", 0.1f); }
        }
    }
}

