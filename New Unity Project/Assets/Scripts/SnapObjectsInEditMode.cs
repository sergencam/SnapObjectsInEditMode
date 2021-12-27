using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SnapObjectsInEditMode
{
    static SnapObjectsInEditMode()
    {
        SceneView.duringSceneGui -= Update;
        SceneView.duringSceneGui += Update;
    }
    public static void Update(SceneView sceneView)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (Event.current.shift)
            {
                var selectedTransform = Selection.activeTransform;
                if (Event.current.type == EventType.KeyDown)
                {
                    switch (Event.current.keyCode)
                    {
                        case KeyCode.UpArrow:
                            SnapObject(selectedTransform, Vector3.up);
                            break;
                        case KeyCode.DownArrow:
                            SnapObject(selectedTransform, Vector3.down);
                            break;
                        case KeyCode.RightArrow:
                            SnapObject(selectedTransform, Vector3.right);
                            break;
                        case KeyCode.LeftArrow:
                            SnapObject(selectedTransform, Vector3.left);
                            break;
                    }
                }
            }
        }
#endif
    }

    public static void SnapObject(Transform snapObjectTransform, Vector3 _direction)
    {
        Vector3 _scale = snapObjectTransform.localScale;
        RaycastHit hit;

        if (Physics.Raycast(snapObjectTransform.position, _direction, out hit, 500f))
        {
            snapObjectTransform.transform.rotation = Quaternion.LookRotation(hit.normal);
            snapObjectTransform.position = hit.point;

            Vector3 penetrationDirection;
            float penetrationDistance;

            bool overlapped = Physics.ComputePenetration(
            snapObjectTransform.GetComponent<Collider>(), snapObjectTransform.transform.position, snapObjectTransform.transform.rotation,
            hit.collider, hit.point, hit.transform.rotation,
            out penetrationDirection, out penetrationDistance
            );

            var targetPos = snapObjectTransform.position;
            targetPos += penetrationDirection / 2f;
            snapObjectTransform.position = targetPos;
        }
    }
}
