using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

using Const;

public class InputArea : MonoBehaviourPunCallbacks
{
    [SerializeField] private Color correctColor;
    [SerializeField] private Color incorrectColor;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    [SerializeField] private InputField inputField;

    private int nowPhase;
    private bool canInput;
    private AudioSource audioSource;

    void Start()
    {
        nowPhase = 0;
        canInput = true;
        inputField.text = "";
        inputField.image.color = incorrectColor;
        inputField.interactable = false;
        audioSource = GameObject.Find("SoundVolumeSlider").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//フェイズが変わったとき
        {
            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

            if (nowPhase == ConstInt.FILLED_PHASE) //入力フェイズに変わったとき
            {
                if (photonView.IsMine)
                {
                    canInput = true;
                    inputField.interactable = true;
                }

                UpdateInput("", false);
            }
        }
    }

    public void OnFilledButtonClick()
    {
        if (photonView.IsMine)
        {
            //入力完了ならInputFieldに触れられなくする
            canInput = !canInput;
            inputField.interactable = canInput;
        }

        //入力した文字の頭文字を取り出す
        if (!(inputField.text == ""))
        {
            char[] cr = inputField.text.ToCharArray();
            inputField.text = cr[0].ToString();
        }
    }

    // 答え合わせ
    public void AnswerCheck(string correctAnswer)
    {
        bool isCorrect= inputField.text == correctAnswer;
        photonView.RPC(nameof(UpdateInput), RpcTarget.All, inputField.text, isCorrect);

        if (isCorrect)
        {
            audioSource.clip = correctSound;

            int correctCount = PhotonNetwork.LocalPlayer.GetCorrectCount();
            PhotonNetwork.LocalPlayer.SetCorrectCount(correctCount + 1);
        }
        else
        {
            audioSource.clip = incorrectSound;
        }

        audioSource.loop = false;
        audioSource.Play();
    }

    // パネルの情報を変更
    [PunRPC]
    private void UpdateInput(string character, bool isCorrect)
    {
        inputField.text = character;

        if (isCorrect) { inputField.image.color = correctColor; }
        else { inputField.image.color = incorrectColor; }
    }
}
