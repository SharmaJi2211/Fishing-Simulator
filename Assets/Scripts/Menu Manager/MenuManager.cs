using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainPanel;       // the root panel with Play/Settings/Quit buttons
    [SerializeField] GameObject settingsPanel;   // settings popup
    [SerializeField] GameObject loadingPanel;    // shown during async load

    [Header("Loading")]
    [SerializeField] Slider     loadingBar;
    [SerializeField] TMP_Text   loadingText;

    // Called by Play button
    public void OnPlayPressed()
    {
        mainPanel.SetActive(false);
        loadingPanel.SetActive(true);
        StartCoroutine(LoadFishingScene());
    }

    IEnumerator LoadFishingScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextIndex);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f); 

            if (loadingBar  != null) loadingBar.value  = progress;
            if (loadingText != null) loadingText.text  = Mathf.RoundToInt(progress * 100f) + "%";

            if (op.progress >= 0.9f)
            {
                if (loadingText != null) loadingText.text = "Press any key...";
                if (Input.anyKeyDown) op.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // Called by Settings button
    public void OnSettingsPressed()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // Called by the Close button inside settings
    public void OnSettingsClose()
    {
        settingsPanel.SetActive(false);
    }

    // Called by Quit button
    public void OnQuitPressed()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
