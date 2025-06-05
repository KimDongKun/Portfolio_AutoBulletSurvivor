using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingManager : MonoBehaviour
{
    public SaveJSonData saveJSonData;

    public Slider bgmUI;
    public Slider fireUI;
    public Slider effectUI;

    public AudioSource stageAudio;
    public AudioSource fireAudio;
    public GameObject effectAudioObject;

    private List<AudioSource> effectAudio;
    private List<AudioSource> enemyAudio;

    public void Stage_AudioSetting(float value)
    {
        stageAudio.volume = value * 0.45f;
    }
    public void Fire_AudioSetting(float value)
    {
        fireAudio.volume = value * 0.3f;
    }
    public void Effect_AudioSetting(float value)
    {
        if(effectAudioObject.TryGetComponent(out AudioLimit effectSounds))
        {
            var effectAudio = effectAudioObject.GetComponent<AudioLimit>().audioSounds.ToList();
            for (int i = 0; i < effectAudio.Count; i++)
            {
                effectAudio[i].volume = value * 0.2f;
            }
        }
       
        var enemyList = GameObject.FindGameObjectsWithTag("EnemyUnit");
        enemyAudio = enemyList.Select(obj => obj.GetComponent<AudioSource>()).Where(audio => audio != null).ToList();
        enemyAudio.Select(audio => audio.volume = value * 0.4f);
    }
    public void Volume_Setting(float bgm, float fire, float effect)
    {
        bgmUI.value = bgm;
        fireUI.value = fire;
        effectUI.value = effect;

        //Effect_AudioSetting(effectUI.value);
    }
    public void SaveButton() 
    {
        saveJSonData.audioSettingData = new SettingData(bgmUI.value, fireUI.value, effectUI.value);
        saveJSonData.Save_AudioSetting();
    }
}
