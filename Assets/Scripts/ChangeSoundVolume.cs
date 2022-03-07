using UnityEngine;

public class ChangeSoundVolume : MonoBehaviour
{
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void SoundSliderOnValueChange(float newSliderValue)
	{
		// 音楽の音量をスライドバーの値に変更
		audioSource.volume = newSliderValue;
	}


}
