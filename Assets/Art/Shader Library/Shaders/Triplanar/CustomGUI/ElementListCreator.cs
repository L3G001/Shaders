using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomGUIElementList", menuName = "Shader Graph/Extras/CustomGUI Element List", order = 1)]

public class ElementListCreator : ScriptableObject
{
    [Tooltip("List of bools and properties that are related.")]
    public List<MaterialPropertiesList> RelatedPropertiesList = new List<MaterialPropertiesList>();
}

[System.Serializable]
public class MaterialPropertiesList
{
    [Tooltip("You need to input the refenrence name (usually starts with _ in Shader Graph) of the bool variable which you want to control what properties are shown.")]
    public string Bool;
    [Tooltip("You need to input the refenrence name (usually starts with _ in Shader Graph) of the property which you want to be shown by the Bool.")]
    public List<string> PropertiesToShow = new List<string>();
    [Tooltip("You need to input the refenrence name (usually starts with _ in Shader Graph) of the property which you want to be hidden by the Bool.")]
    public List<string> PropertiesToHide = new List<string>();
}
