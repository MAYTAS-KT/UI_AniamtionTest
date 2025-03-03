using System.Collections.Generic;
using UnityEngine;


public enum ElementType
{
    none,
    Fire,
    Water,
    wind,
    Light,
    Dark
}


[System.Serializable]
public class ElementUI 
{
   public ElementType ElementType;
   public Sprite icon;
   public Sprite desaturatedIcon;
}


[CreateAssetMenu(fileName = "ElementUIData", menuName = "Scriptable Objects/ElementUIData")]
public class ElementUIData : ScriptableObject
{
    public List<ElementUI> UIelements;
}
