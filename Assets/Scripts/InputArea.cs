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
        if (nowPhase != PhotonNetwork.CurrentRoom.GetPhase())//�t�F�C�Y���ς�����Ƃ�
        {
            nowPhase = PhotonNetwork.CurrentRoom.GetPhase();

            if (nowPhase == ConstInt.FILLED_PHASE) //���̓t�F�C�Y�ɕς�����Ƃ�
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
            //���͊����Ȃ�InputField�ɐG����Ȃ�����
            canInput = !canInput;
            inputField.interactable = canInput;
        }

        //���͂��������̓����������o��
        if (!(inputField.text == ""))
        {
            char[] cr = inputField.text.ToCharArray();
            inputField.text = cr[0].ToString();
        }
    }

    // �������킹
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

    // �p�l���̏���ύX
    [PunRPC]
    private void UpdateInput(string character, bool isCorrect)
    {
        inputField.text = character;

        if (isCorrect) { inputField.image.color = correctColor; }
        else { inputField.image.color = incorrectColor; }
    }
}
