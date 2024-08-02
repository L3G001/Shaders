using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HideElements : ShaderGUI
{
    bool showCategorySurfaceInput = true;
    bool showCategoryAdvancedOptions = true;

    Texture2D lineTexture;
    GUIStyle sectionStyle;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        ElementListCreator RelatedProperties = Resources.Load<ElementListCreator>("CustomGUIElementList");
        Material targetMat = materialEditor.target as Material;

        if (lineTexture == null)
        {
            lineTexture = new Texture2D(1, 1);
            lineTexture.SetPixel(0, 0, Color.gray); // Set the color of the line
            lineTexture.Apply();
        }

        if (sectionStyle == null)
        {
            sectionStyle = new GUIStyle(GUI.skin.box);
            sectionStyle.normal.background = MakeTex(1, 1, new Color(0.18f, 0.18f, 0.18f, 0.72f));
            sectionStyle.stretchHeight = true;
            sectionStyle.stretchWidth = true;
        }

        EditorGUILayout.Separator();

        showCategorySurfaceInput = EditorGUILayout.BeginFoldoutHeaderGroup(showCategorySurfaceInput, "Surface Inputs",sectionStyle);
        if (showCategorySurfaceInput)
        {
            EditorGUILayout.BeginVertical(sectionStyle);
            foreach (MaterialPropertiesList i in RelatedProperties.RelatedPropertiesList)
            {
                GUILayout.Space(5);
                Rect lineRect = EditorGUILayout.GetControlRect(false, 1);
                EditorGUI.DrawPreviewTexture(lineRect, lineTexture, null, ScaleMode.StretchToFill);
                GUILayout.Space(5);

                string Bool = i.Bool;
                List<string> PropertiesON = i.PropertiesToShow;
                List<string> PropertiesOFF = i.PropertiesToHide;

                MaterialProperty BoolProperty = FindProperty(Bool, properties);
                materialEditor.ShaderProperty(BoolProperty, BoolProperty.displayName);

                if (BoolProperty.floatValue == 1)
                {
                    foreach (string s in PropertiesON)
                    {
                        MaterialProperty property = FindProperty(s, properties);
                        EditorGUI.indentLevel++;
                        materialEditor.ShaderProperty(property, property.displayName);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (PropertiesOFF.Count > 0)
                    {
                        foreach (string s in PropertiesOFF)
                        {
                            MaterialProperty property = FindProperty(s, properties);
                            EditorGUI.indentLevel++;
                            materialEditor.ShaderProperty(property, property.displayName);
                            EditorGUI.indentLevel--;
                        }
                    }
                    else { continue; }
                }
                GUILayout.Space(5);
                EditorGUI.DrawPreviewTexture(lineRect, lineTexture, null, ScaleMode.StretchToFill);
                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Separator();

        showCategoryAdvancedOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showCategoryAdvancedOptions, "Advanced Options",sectionStyle);
        if (showCategoryAdvancedOptions)
        {
            EditorGUILayout.BeginVertical(sectionStyle);
            materialEditor.RenderQueueField();
            materialEditor.EnableInstancingField();
            materialEditor.DoubleSidedGIField();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}

