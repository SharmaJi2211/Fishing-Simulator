using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] Slider volumeSlider;

    [Header("Quality")]
    [SerializeField] TMP_Dropdown qualityDropdown;

    const string VOLUME_KEY  = "MasterVolume";
    const string QUALITY_KEY = "QualityLevel";

    void Start()
    {
        ApplySavedSettings();
        PopulateQualityDropdown();
    }

    void ApplySavedSettings()
    {
        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        AudioListener.volume = savedVolume;
        if (volumeSlider != null) volumeSlider.value = savedVolume;

        // Load saved quality
        int savedQuality = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(savedQuality);
        if (qualityDropdown != null) qualityDropdown.value = savedQuality;
    }

    void PopulateQualityDropdown()
    {
        if (qualityDropdown == null) return;
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    // Called by volume slider OnValueChanged
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    // Called by quality dropdown OnValueChanged
    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(QUALITY_KEY, index);
        PlayerPrefs.Save();
    }
}
