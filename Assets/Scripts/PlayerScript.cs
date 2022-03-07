using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    [SerializeField] Text nameText;

    private float timer_s;

    private const float ADJUST_X = 188f;
    private const float ADJUST_Y = -220f;

    void Start()
    {
        timer_s = 0.0f;
        nameText.text = photonView.Owner.NickName;
        if (photonView.IsMine) { SetPlayerPanelNumber(); }
    }
    private void Update()
    {
        //�ʒu����
        if (timer_s < 10.0f)
        {
            SetFirstPosition();

            timer_s+= Time.deltaTime;
        }
    }

    /// <summary>
    /// �v���C���[�̉񓚔ԍ�������
    /// </summary>
    private void SetPlayerPanelNumber()
    {
        var players = PhotonNetwork.PlayerList;
        List<int> UsedPanelNumbers = new List<int>();

        //�g�p����Ă���ԍ������X�g�ɒǉ�
        foreach (var player in players)
        {
            UsedPanelNumbers.Add(player.GetPlayerPanelNumber());
        }

        //�g�p����Ă��Ȃ��ŏ��̔ԍ����񓚔ԍ��Ɍ���
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        for (int i = 0; i < players.Length; i++)
        {
            if (!UsedPanelNumbers.Contains(i))
            {
                PhotonNetwork.LocalPlayer.SetPlayerPanelNumber(i);

                break;
            }
        }
    }

    /// <summary>
    /// �񓚗��������ʒu�ɔz�u
    /// </summary>
    private void SetFirstPosition()
    {
        if (photonView.IsMine)
        {
            //panelNumbe�ɉ����Ĕz�u
            int panelNumber = PhotonNetwork.LocalPlayer.GetPlayerPanelNumber();
            transform.position = new Vector2(ADJUST_X * (panelNumber - 2), ADJUST_Y);
        }
    }
}
