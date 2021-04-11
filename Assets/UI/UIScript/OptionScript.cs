using UnityEngine;
using UnityEngine.UI;

public class OptionScript : PopUpScaleScript // PopUpScaleScript는 해당 창이 뜰 때 나타나는 효과를 적용시켜주는 스크립트
{
    [SerializeField]
    private Slider soundSlider = null;

    private void Start()
    {
       PlusStart();
       gameManager.SetSoundValue(soundSlider.value);
    }
    private void Update()
    {
        gameManager.SetSoundValue(soundSlider.value);
        SetScale();
    }
    
}
