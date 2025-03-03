using UnityEngine.UI;
using UnityEngine;

public class UIElementInfo : MonoBehaviour
{
   [SerializeField] Image icon;
   [SerializeField] Image desaturatedIcon;

   [SerializeField] Button elementButton;

    public void SetElementInfo(ElementUI elementUI)
    {
         icon.sprite = elementUI.icon;
         desaturatedIcon.sprite = elementUI.desaturatedIcon;
    }

}
