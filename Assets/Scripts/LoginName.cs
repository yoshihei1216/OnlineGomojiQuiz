using UnityEngine;

public class LoginName : MonoBehaviour
{
    [SerializeField] [Tooltip("参加ログが流れる速度")] private float velocity;
    [SerializeField] [Tooltip("参加ログが消えるまでの時間")] private float destoryTime_s;

    float timer_s;

    private void Start()
    {
        timer_s= 0.0f;
    }

    private void Update()
    {
        transform.Translate(-velocity * Time.deltaTime, 0, 0);//参加ログを流す

        //時間経過で消去
        if (timer_s > destoryTime_s) { Destroy(gameObject); }
        else { timer_s += Time.deltaTime; }
    }
}
