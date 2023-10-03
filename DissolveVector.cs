using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveVector : MonoBehaviour
{
    public Renderer rend;
    [Range(0.0f, 1.05f)]
    public float dissolve = 0.0f;

    public Transform p0, p1;

    public bool xAxis;
    public bool yAxis;
    public bool zAxis;
    public bool invert;
    public Transform pivot;
    private Vector3 start;
    private Vector3 end;
    private Bounds bounds;
    private Vector3 interpolVec;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();

    }

    private void Update()
    {
        bounds = rend.bounds;
        pivot.position = bounds.center;
        pivot.localScale = Vector3.one;

        if (xAxis)
        {
            rend.material.SetFloat("_xAxis", 1.0f);
            rend.material.SetFloat("_yAxis", 0.0f);
            rend.material.SetFloat("_zAxis", 0.0f);

            start = pivot.InverseTransformPoint(bounds.center + new Vector3(-bounds.extents.x, 0, 0));
            end = pivot.InverseTransformPoint(bounds.center + new Vector3(bounds.extents.x, 0, 0));

            if (invert)
            {
                Vector3 temp = end;
                end = start;
                start = temp;
            }

            rend.material.SetVector("_Start", new Vector4(start.x, start.y, start.z, 1.0f));
            rend.material.SetVector("_End", new Vector4(end.x, end.y, end.z, 1.0f));

            interpolVec = Vector3.Lerp(start, end, dissolve);

            p0.localPosition = start;
            p1.localPosition = end;

            float xlerp = interpolVec.x;
            rend.material.SetFloat("_DissolveAmount", xlerp);
        }
        else if (yAxis)
        {
            rend.material.SetFloat("_yAxis", 1.0f);

            rend.material.SetFloat("_xAxis", 0.0f);
            rend.material.SetFloat("_zAxis", 0.0f);

            start = pivot.InverseTransformPoint(bounds.center + new Vector3(0, -bounds.extents.y, 0));
            end = pivot.InverseTransformPoint(bounds.center + new Vector3(0, bounds.extents.y, 0));


            if (invert)
            {
                Vector3 temp = end;
                end = start;
                start = temp;
            }

            rend.material.SetVector("_Start", new Vector4(start.x, start.y, start.z, 1.0f));
            rend.material.SetVector("_End", new Vector4(end.x, end.y, end.z, 1.0f));

            interpolVec = Vector3.Lerp(start, end, dissolve);

            p0.localPosition = start;
            p1.localPosition = end;

            float ylerp = interpolVec.y;
            rend.material.SetFloat("_DissolveAmount", ylerp);

        }
        else if (zAxis)
        {
            rend.material.SetFloat("_zAxis", 1.0f);

            rend.material.SetFloat("_xAxis", 0.0f);
            rend.material.SetFloat("_yAxis", 0.0f);

            start = pivot.InverseTransformPoint(bounds.center + new Vector3(0, 0, -bounds.extents.z));
            end = pivot.InverseTransformPoint(bounds.center + new Vector3(0, 0, bounds.extents.z));

            if (invert)
            {
                Vector3 temp = end;
                end = start;
                start = temp;
            }

            rend.material.SetVector("_Start", new Vector4(start.x, start.y, start.z, 1.0f));
            rend.material.SetVector("_End", new Vector4(end.x, end.y, end.z, 1.0f));

            interpolVec = Vector3.Lerp(start, end, dissolve);

            p0.localPosition = start;
            p1.localPosition = end;

            float zlerp = interpolVec.z;
            rend.material.SetFloat("_DissolveAmount", zlerp);
        }
    }
}
