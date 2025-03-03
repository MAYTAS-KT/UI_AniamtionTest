using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIElementInfo : MonoBehaviour
{
   [SerializeField] Image icon;
   [SerializeField] Image desaturatedIcon;

   public Button elementButton;

    public void SetElementInfo(ElementUI elementUI)
    {
         icon.sprite = elementUI.icon;
         desaturatedIcon.sprite = elementUI.desaturatedIcon;
    }

}
