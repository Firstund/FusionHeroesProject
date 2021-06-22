using UnityEngine;
using UnityEngine.UI;

public class SoundSliderScript : MonoBehaviour
{
    [SerializeField]
    private ChangeSpriteScript changeSpirteScript = null;
    public void OnValueChnaged()
    {
        if(GetComponent<Slider>().value <=0f)
        {
            changeSpirteScript.ChangeImage();
        }
        else{
            changeSpirteScript.ChangeToFirst();
        }
    }
  
}
