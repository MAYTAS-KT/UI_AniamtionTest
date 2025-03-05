using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ElementUIData elementUIData;

    public UIElementInfo elementUIPrefab;
    public GameObject elementParent;

    public TextMeshProUGUI elementName;
   
    private float elapsedTime = 0f;

    public SkillTree skilltreeIcon;

    public static UIManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InstantiateElementUI();
    }

    private void InstantiateElementUI()
    {
        for (int i = 0; i < elementUIData.UIelements.Count; i++)
        {
            UIElementInfo element = Instantiate(elementUIPrefab, elementParent.transform);
            element.SetElementInfo(elementUIData.UIelements[i]);
            string name = elementUIData.UIelements[i].ElementType.ToString();
            element.elementButton.onClick.AddListener(() => elementName.text = name);
        }
    }

    public void checkElementPanelOn()
    {
    
    }
}
