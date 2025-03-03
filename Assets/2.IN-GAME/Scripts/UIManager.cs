using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ElementUIData elementUIData;

    public UIElementInfo elementUIPrefab;
    public GameObject elementParent;

    public UICircularLayoutGroup circularLayoutGroup;
   
    private float elapsedTime = 0f;

    

    public void Start()
    {
        InstantiateElementUI();

    }

    private void InstantiateElementUI()
    {
        for (int i = 0; i < elementUIData.UIelements.Count; i++)
        {
            UIElementInfo element = Instantiate(elementUIPrefab, elementParent.transform);
             element.SetElementInfo(elementUIData.UIelements[i]);
        }
    }

    // void Update()
    // {
    //     if (elapsedTime < 5)
    //     {
    //         // Calculate the interpolation factor
    //         float t = elapsedTime / 5;

    //         // Lerp between the start and target angles
    //         circularLayoutGroup.startAngle = Mathf.Lerp(360f,180f, t);

    //         // Increment the elapsed time
    //         elapsedTime += Time.deltaTime;
    //         circularLayoutGroup.UpdateGrid();
    //     }
    //     else
    //     {
    //         // Ensure the final angle is set to the target angle
    //         circularLayoutGroup.startAngle = 180;
    //     }
    // }
}
