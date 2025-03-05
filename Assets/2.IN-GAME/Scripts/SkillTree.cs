
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
   public Image[] skillIcons;
   public Sprite unlockedSkillSprite;
    public Sprite lockedSkillSprite;
    public Sprite darkSkillSprite;


   public void UnlockSkill(int skillUnlocked)
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
   }
}
