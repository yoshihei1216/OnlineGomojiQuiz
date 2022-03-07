using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoginNameLunch : MonoBehaviourPunCallbacks
{
    [SerializeField] [Tooltip("�V�K�Q���ʒm�e�L�X�g�v���n�u")] Text notifyNewPlayerPrefab;
    int joinNumber ;

    private void Start()
    {
        joinNumber = 0;
    }

    // �v���C���[���V�K�Q�������Ƃ��ɌĂ΂��R�[���o�b�N
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        float x= transform.position.x;
        float y= transform.position.y;
        Vector2 firstPosition = new Vector2(x,y - 30 * (joinNumber % 3));
        Text notifyNewPlayerText= Instantiate(notifyNewPlayerPrefab, firstPosition, Quaternion.identity);
        notifyNewPlayerText.transform.SetParent(transform);
        notifyNewPlayerText.text = $"{newPlayer.NickName} ���Q�����܂���";
        joinNumber++;
    }

}
