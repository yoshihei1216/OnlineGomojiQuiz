using UnityEngine;

public class LoginName : MonoBehaviour
{
    [SerializeField] [Tooltip("�Q�����O������鑬�x")] private float velocity;
    [SerializeField] [Tooltip("�Q�����O��������܂ł̎���")] private float destoryTime_s;

    float timer_s;

    private void Start()
    {
        timer_s= 0.0f;
    }

    private void Update()
    {
        transform.Translate(-velocity * Time.deltaTime, 0, 0);//�Q�����O�𗬂�

        //���Ԍo�߂ŏ���
        if (timer_s > destoryTime_s) { Destroy(gameObject); }
        else { timer_s += Time.deltaTime; }
    }
}
