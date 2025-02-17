using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MaskPosition : MonoBehaviour
{
    public Transform maskObject;
    public List<GameObject> objectTarget = new List<GameObject>();
    public List<Renderer> shaderTarget = new List<Renderer>();
    public List<ObjectInformation> objectInformation = new List<ObjectInformation>();
    public string shaderPropertyX, shaderPropertyY, shaderPropertySphere, shaderPropertyFrontierColor, shaderPropertyFrontierWidth, shaderPropertyFrontierSize, shaderPropertyVisibility;
    public Color frontierColor;
    public Vector2 frontierWidth, visibility;
    public float frontierColorIntensity, frontierSize, radious;
    public Vector3 maskPosition;

    private void Start()
    {
        foreach (GameObject obj in objectTarget)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (!shaderTarget.Contains(renderer))
            {
                var property = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(property);
                if (property == null)
                    property = new MaterialPropertyBlock(); // Ensure it's never null
                objectInformation.Add(new ObjectInformation { obj = obj, rend = renderer, prop = property });
                shaderTarget.Add(renderer);
            }
        }
    }

    void Update()
    {
        if (shaderTarget.Count != objectTarget.Count)
        {
            foreach (GameObject obj in objectTarget)
            {
                var renderer = obj.GetComponent<Renderer>();
                if (!shaderTarget.Contains(renderer))
                {
                    var property = new MaterialPropertyBlock();
                    renderer.GetPropertyBlock(property);
                    if (property == null)
                        property = new MaterialPropertyBlock();
                    objectInformation.Add(new ObjectInformation { obj = obj, rend = renderer, prop = property });
                    shaderTarget.Add(renderer);
                }
            }
        }

        maskPosition = maskObject.position;

        foreach (var objInfo in objectInformation)
        {
            if (!objectTarget.Contains(objInfo.obj))
            {
                shaderTarget.Remove(objInfo.rend);
                objectInformation.Remove(objInfo);
            }
            else if (!shaderTarget.Contains(objInfo.rend))
            {
                objectTarget.Remove(objInfo.obj);
                objectInformation.Remove(objInfo);
            }
            var objPos = objInfo.obj.transform.position;
            if (objInfo.prop == null)
                objInfo.prop = new MaterialPropertyBlock();
            var property = objInfo.prop;
            property.SetFloat(shaderPropertyX, (objPos - maskPosition).x);
            property.SetFloat(shaderPropertyY, (objPos - maskPosition).y);
            property.SetColor(shaderPropertyFrontierColor, frontierColor * frontierColorIntensity);
            property.SetVector(shaderPropertyFrontierWidth, frontierWidth);
            property.SetFloat(shaderPropertyFrontierSize, frontierSize);
            property.SetVector(shaderPropertySphere, new Vector4(maskPosition.x, maskPosition.y, maskPosition.z, radious));
            property.SetVector(shaderPropertyVisibility, visibility);
            objInfo.obj.GetComponent<Renderer>().SetPropertyBlock(property);
        }
    }
}

[System.Serializable]
public class ObjectInformation
{
    public GameObject obj;
    public Renderer rend;
    public MaterialPropertyBlock prop;
}