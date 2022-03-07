using UnityEngine;
using UnityEngine.UI;

public class ChangeCameraSize : MonoBehaviour
{
    [SerializeField] private Slider cameraSizeSlider;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void OnValueChange()
    {
        mainCamera.orthographicSize = 2*(cameraSizeSlider.maxValue)-cameraSizeSlider.value;
    }
}
