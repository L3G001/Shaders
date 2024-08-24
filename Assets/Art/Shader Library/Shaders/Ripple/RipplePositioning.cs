using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePositioning : MonoBehaviour
{
    private Coroutine rippleRoutine;
    [SerializeField] private float _rippleTime = 1.5f;
    [SerializeField] private float _maxRippleStrength = 0.75f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    var mat = hit.transform.GetComponent<MeshRenderer>().material;
                    mat.SetVector("_Ripple_Center", hit.point);
                    if (rippleRoutine != null) { StopCoroutine(rippleRoutine); }
                    rippleRoutine = StartCoroutine(DoRipple(mat));
                }
            }
        }
    }

    private IEnumerator DoRipple(Material mat)
    {
        for (float t = 0.0f; t < _rippleTime; t += Time.deltaTime)
        {
            var strenght = 0.0f;
            if (t <= _rippleTime / 2) { if (strenght <= _maxRippleStrength) { strenght = t / _rippleTime; } }
            else { strenght = _maxRippleStrength * (1.0f - t / _rippleTime); }
            mat.SetFloat("_Ripple_Strength", strenght);
            yield return null;
        }
    }
}
