using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject titleView;
    [SerializeField] private GameObject quizView;
    [SerializeField] private Button JoinRoomButton;
    [SerializeField] private Text nametext;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private  byte maxPlayer = 5;

    private CanvasGroup titleCanvas;    

    private const int TITLE_SCENE = 0;
    private const int QUIZ_SCENE = 1;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//マスターサーバーへ接続する
        
        JoinRoomButton.interactable = false;
        
        titleCanvas = titleView.GetComponent<CanvasGroup>();
    }

    //シーン変更
    private void SetScene(int scene)
    {
        switch (scene)
        {
            case TITLE_SCENE:
                titleView.SetActive(true);
                quizView.SetActive(false);
                break;

            case QUIZ_SCENE:
                titleView.SetActive(false);
                quizView.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void OnJoinRoomButtonClick()
    {
        titleCanvas.interactable = false;// ルーム参加処理中は、入力できないようにする

        PhotonNetwork.NickName = nametext.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnRoomLeftButtonClick()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;//ルームへの参加を許可する

        PhotonNetwork.LocalPlayer.SetReadyBool(false);
        PhotonNetwork.LocalPlayer.SetFilledBool(false);
        PhotonNetwork.LocalPlayer.SetCorrectCount(0);
        PhotonNetwork.LocalPlayer.SetPlayerPanelNumber(-1);

        audioSource.clip = null;

        PhotonNetwork.LeaveRoom();
    }

    //マスターサーバーへの接続が成功したとき
    public override void OnConnectedToMaster()
    {
        JoinRoomButton.interactable = true;
        titleCanvas.interactable = true;
    }

    //ルームに参加したとき
    public override void OnJoinedRoom()
    { 
        SetScene(QUIZ_SCENE);

        // Resources内から自身のパネル（ネットワークオブジェクト）を生成する
        PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);

        //ルームが満員になったら、以降そのルームへの参加を不許可にする
        if(PhotonNetwork.CurrentRoom.PlayerCount==PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    //ランダムなルームへの入室が失敗したとき
    public override void OnJoinRandomFailed(short returnCode, string nessage)
    {
        //人数制限を設定し、ルームを作成
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    //ルームを離れたとき
    public override void OnLeftRoom()
    {
        SetScene(TITLE_SCENE);
    }

    //同じルームのプレイヤーが退出したとき
    public override void OnPlayerLeftRoom(Player player)
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }
}