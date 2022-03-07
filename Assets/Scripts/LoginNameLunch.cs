using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoginNameLunch : MonoBehaviourPunCallbacks
{
    [SerializeField] [Tooltip("新規参加通知テキストプレハブ")] Text notifyNewPlayerPrefab;
    int joinNumber ;

    private void Start()
    {
        joinNumber = 0;
    }

    // プレイヤーが新規参加したときに呼ばれるコールバック
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        float x= transform.position.x;
        float y= transform.position.y;
        Vector2 firstPosition = new Vector2(x,y - 30 * (joinNumber % 3));
        Text notifyNewPlayerText= Instantiate(notifyNewPlayerPrefab, firstPosition, Quaternion.identity);
        notifyNewPlayerText.transform.SetParent(transform);
        notifyNewPlayerText.text = $"{newPlayer.NickName} が参加しました";
        joinNumber++;
    }

}
