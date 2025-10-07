using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Rigidbody))]
public class GearAutoColliders : MonoBehaviour
{
    [Header("Ring instellingen")]
    public int teeth = 16;          // aantal blokjes rond de rand
    public float radius = 0.55f;    // afstand vanaf midden (pas aan je model)
    public float toothWidth = 0.12f;
    public float toothHeight = 0.05f;
    public float thickness = 0.2f;  // diepte naar voren/achter

    [Header("Hulp")]
    public Transform collidersParent;

    [ContextMenu("Rebuild Colliders")]
    public void Rebuild()
    {
        if (collidersParent == null)
        {
            var go = new GameObject("Colliders");
            go.transform.SetParent(transform, false);
            collidersParent = go.transform;
        }

        // oude kinderen wissen
        for (int i = collidersParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(collidersParent.GetChild(i).gameObject);

        // maak ring
        for (int i = 0; i < teeth; i++)
        {
            float t = (i / (float)teeth) * Mathf.PI * 2f;
            Vector3 localPos = new Vector3(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius, 0f);

            var tooth = new GameObject("Tooth_" + i);
            tooth.transform.SetParent(collidersParent, false);
            tooth.transform.localPosition = localPos;
            tooth.transform.localRotation = Quaternion.Euler(0, 0, t * Mathf.Rad2Deg);

            var box = tooth.AddComponent<BoxCollider>();
            box.size = new Vector3(toothWidth, toothHeight, thickness);
            box.center = new Vector3(0f, -toothHeight * 0.5f, 0f); // onderkant raakt het “loopvlak”
        }

        // midden-schijf voor extra steun
        var hub = new GameObject("Hub");
        hub.transform.SetParent(collidersParent, false);
        var hubCol = hub.AddComponent<CapsuleCollider>();
        hubCol.direction = 2; // Z
        hubCol.radius = Mathf.Max(0.01f, radius * 0.5f);
        hubCol.height = thickness;
    }

    void OnValidate() { Rebuild(); }
}
