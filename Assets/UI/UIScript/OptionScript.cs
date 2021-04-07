using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    [SerializeField]
    private Slider soundSlider = null;
    private GameManager gameManager = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void Update()
    {
        gameManager.SetSoundValue(soundSlider.value);
    }
}
