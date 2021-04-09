using UnityEngine;
using UnityEngine.UI;

public class OptionScript : PopUpScaleScript
{
    [SerializeField]
    private Slider soundSlider = null;

    private void Start()
    {
       PlusStart();
    }
    private void Update()
    {
        gameManager.SetSoundValue(soundSlider.value);
        SetScale();
    }
    
}
