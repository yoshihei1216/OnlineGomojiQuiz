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
		// ���y�̉��ʂ��X���C�h�o�[�̒l�ɕύX
		audioSource.volume = newSliderValue;
	}


}
