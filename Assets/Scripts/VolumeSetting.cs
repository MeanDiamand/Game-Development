using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    public GameObject volumeMenu;

    private void Start()
    {
        if (PlayerPrefs.HasKey("backgroundmusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetEffectVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("backgroundmusic", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("backgroundmusicVolume", volume);
    }

    public void SetEffectVolume()
    {
        float volume = effectSlider.value;
        audioMixer.SetFloat("effect", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("backgroundmusicVolume");
        effectSlider.value = PlayerPrefs.GetFloat("effectVolume");
        SetMusicVolume();
        SetEffectVolume();
    }

    public void VolumeSettingMenu()
    {
        Time.timeScale = 0;
        volumeMenu.SetActive(true);
    }

    public void Back()
    {
        Time.timeScale = 1;
        volumeMenu.SetActive(false);
        Start();
    }
}
