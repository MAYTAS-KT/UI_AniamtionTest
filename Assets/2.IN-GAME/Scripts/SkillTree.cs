
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
   public Image[] skillIcons;
   public Sprite unlockedSkillSprite;
    public Sprite lockedSkillSprite;
    public Sprite darkSkillSprite;

    public Image FillImage;

   public void UnlockSkill(int skillUnlocked, float targetFill)
   {
       for (int i = 0; i < skillIcons.Length; i++)
       {
           if (i < skillUnlocked)
           {
               skillIcons[i].sprite = unlockedSkillSprite;
           }
           else if(i==skillUnlocked)
           {
                skillIcons[i].sprite = darkSkillSprite;
           }
           else
           {
               skillIcons[i].sprite = lockedSkillSprite;
           }
       }

        StopAllCoroutines(); // Stop any existing animation
        StartCoroutine(AnimateFill(0, targetFill, 0.5f));
    }

    private IEnumerator AnimateFill(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;
        FillImage.fillAmount = startValue; // Start from 0

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            FillImage.fillAmount = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            yield return null;
        }

        FillImage.fillAmount = endValue; // Ensure it reaches the exact target value

        
    }
}
