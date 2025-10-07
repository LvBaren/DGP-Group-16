using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform waypointsParent; // zet hier je 'Waypoints' object
    public float speed = 1.5f;
    public bool loop = true;

    Transform[] points;
    int i = 0;

    void Awake()
    {
        // verzamel alle child-punten in volgorde
        if (waypointsParent != null)
        {
            int n = waypointsParent.childCount;
            points = new Transform[n];
            for (int k = 0; k < n; k++) points[k] = waypointsParent.GetChild(k);
            if (points.Length > 0) transform.position = points[0].position;
        }
    }

    void Update()
    {
        if (points == null || points.Length < 2) return;

        Transform target = points[i];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            i++;
            if (i >= points.Length)
                i = loop ? 0 : points.Length - 1;
        }
    }
}
