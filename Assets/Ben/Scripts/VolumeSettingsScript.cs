using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsScript : MonoBehaviour
{
    public Slider effectSlider;
    public Slider musicSlider;
    public FloatSO effectSO;
    public FloatSO musicSO;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EffectVolumeChange() 
    {
        effectSO.value = effectSlider.value;
    }
    public void MusicVolumeChange() 
    {
        musicSO.value = musicSlider.value;
    }
}
