using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElementInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image desaturatedIcon;
    public Button elementButton;

    private float targetFill;
    private float animationDuration = 0.5f; // Adjust duration for speed
    private int skillUnlocked = 1;
    private float skillTreeTargetfill = 0.5f;

    private void Start()
    {
        elementButton.onClick.AddListener(OnElementClick);
    }

    public void SetElementInfo(ElementUI elementUI)
    {
        icon.sprite = elementUI.icon;
        desaturatedIcon.sprite = elementUI.desaturatedIcon;
        icon.fillAmount = elementUI.initialfillAmount;
        targetFill = elementUI.initialfillAmount; // Store target fill amount
        skillUnlocked = elementUI.skillUnlocked;
        skillTreeTargetfill=elementUI.skillTreeTargetfill;
    }

    public void OnElementClick()
    {
        UIManager.instance.skilltreeIcon.UnlockSkill(skillUnlocked,skillTreeTargetfill);
        StopAllCoroutines(); // Stop any existing animation
        StartCoroutine(AnimateFill(0, targetFill, animationDuration));
    }

    private IEnumerator AnimateFill(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;
        icon.fillAmount = startValue; // Start from 0

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            icon.fillAmount = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            yield return null;
        }

        icon.fillAmount = endValue; // Ensure it reaches the exact target value
    }
}
