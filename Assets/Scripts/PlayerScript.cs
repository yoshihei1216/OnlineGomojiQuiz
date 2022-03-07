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
        //位置調整
        if (timer_s < 10.0f)
        {
            SetFirstPosition();

            timer_s+= Time.deltaTime;
        }
    }

    /// <summary>
    /// プレイヤーの回答番号を決定
    /// </summary>
    private void SetPlayerPanelNumber()
    {
        var players = PhotonNetwork.PlayerList;
        List<int> UsedPanelNumbers = new List<int>();

        //使用されている番号をリストに追加
        foreach (var player in players)
        {
            UsedPanelNumbers.Add(player.GetPlayerPanelNumber());
        }

        //使用されていない最小の番号を回答番号に決定
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
    /// 回答欄を初期位置に配置
    /// </summary>
    private void SetFirstPosition()
    {
        if (photonView.IsMine)
        {
            //panelNumbeに応じて配置
            int panelNumber = PhotonNetwork.LocalPlayer.GetPlayerPanelNumber();
            transform.position = new Vector2(ADJUST_X * (panelNumber - 2), ADJUST_Y);
        }
    }
}
