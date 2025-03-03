using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetMainLightDirection : MonoBehaviour
{
    [SerializeField] private Material _skyboxMaterial;
    [SerializeField] private Light _mainLight;
    private void Update()
    {
        _skyboxMaterial.SetVector("_MainLightDirection", _mainLight.transform.forward);
        _skyboxMaterial.SetVector("_MainLightUp", _mainLight.transform.up);
        _skyboxMaterial.SetVector("_MainLightRight", _mainLight.transform.right);
    }
}
