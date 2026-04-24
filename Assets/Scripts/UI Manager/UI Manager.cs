using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("State UI")]
    [SerializeField] TMP_Text stateText;
    [SerializeField] TMP_Text messageText;

    [Header("Reel Minigame UI")]
    [SerializeField] GameObject    reelPanel;
    [SerializeField] RectTransform trackPanel;    // the background
    [SerializeField] RectTransform catchingBar;   // bar that player controls
    [SerializeField] RectTransform fishIcon;      // fish sprite
    [SerializeField] Slider progressSlider;


    void HandleStateChanged(FishingState state)
    {
        stateText.text = state switch
        {
            FishingState.Idle    => "Press LMB to Cast",
            FishingState.Casting => "Casting...",
            FishingState.Waiting => "Waiting for a bite...",
            FishingState.Bite    => "FISH ON! Press LMB",
            FishingState.Reeling => "Hold Space to keep fish in safe zone!",
            FishingState.Result  => "",
            _                   => ""
        };

        stateText.color = state == FishingState.Bite ? Color.yellow : Color.white;
        reelPanel.SetActive(state == FishingState.Reeling);
    }

    void HandleBite()
    {
        ShowMessage("!! FISH ON !!", Color.yellow);
    }

    void HandleFishEscaped()
    {
        ShowMessage("The fish got away...", Color.red);
    }

    void HandleFishCaught(FishData fish)
    {
        string label = fish.rarity == Rarity.Rare ? "RARE! " : "";
        ShowMessage(label + fish.fishName + " caught!", Color.green);
    }

    void HandleFishIconPosition(float pos)
    {
        float h = trackPanel.rect.height;
        fishIcon.anchoredPosition = new Vector2(fishIcon.anchoredPosition.x, pos * h);
    }

    void HandleZonePosition(float min, float max)
    {
        float h = trackPanel.rect.height;
        catchingBar.anchoredPosition = new Vector2(0f, min * h);
        catchingBar.sizeDelta        = new Vector2(catchingBar.sizeDelta.x, (max - min) * h);
    }

    void HandleProgress(float progress)
    {
        progressSlider.value = progress;
    }

    void ShowMessage(string msg, Color color)
    {
        messageText.text  = msg;
        messageText.color = color;
        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), 2f);
    }

    void ClearMessage() => messageText.text = "";
    void HandleNotifyMessage(string msg, Color color) => ShowMessage(msg, color);
    void OnEnable()
    {
        GameEvents.OnFishingStateChanged += HandleStateChanged;
        GameEvents.OnBite += HandleBite;
        GameEvents.OnFishEscaped += HandleFishEscaped;
        GameEvents.OnFishCaught += HandleFishCaught;
        GameEvents.OnFishIconPositionChanged += HandleFishIconPosition;
        GameEvents.OnZonePositionChanged += HandleZonePosition;
        GameEvents.OnProgressChanged += HandleProgress;
        GameEvents.OnNotifyMessage += HandleNotifyMessage;
    }

    void OnDisable()
    {
        GameEvents.OnFishingStateChanged -= HandleStateChanged;
        GameEvents.OnBite -= HandleBite;
        GameEvents.OnFishEscaped -= HandleFishEscaped;
        GameEvents.OnFishCaught -= HandleFishCaught;
        GameEvents.OnFishIconPositionChanged -= HandleFishIconPosition;
        GameEvents.OnZonePositionChanged -= HandleZonePosition;
        GameEvents.OnProgressChanged -= HandleProgress;
        GameEvents.OnNotifyMessage -= HandleNotifyMessage;
    }
}