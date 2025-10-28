using UnityEngine;
public class GearRotator : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip gearSound; // AudioClip voor rollen
    [SerializeField] private float maxVolume = 1f; // Volume dicht bij speler
    [SerializeField] private float minVolumeDistance = 10f; // Afstand waar volume minimaal is
    [SerializeField] private float maxVolumeDistance = 2f;  // Afstand waar volume maximaal is

    public Vector3 axis = Vector3.forward; // Z-as
    public float speed = 90f;
    public bool spinning = true;

    private AudioSource audioSource;
    private Transform player;

    void Start()
    {
        // Audio setup
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gearSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D geluid
        audioSource.minDistance = maxVolumeDistance;
        audioSource.maxDistance = minVolumeDistance;
        audioSource.volume = 0f;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (gearSound != null)
            audioSource.Play(); // Start looping
    }
    void Update()
    {
        if (spinning) transform.Rotate(axis, speed * Time.deltaTime, Space.Self);

        // Past volume aan op afstand tot speler
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            float vol = Mathf.Clamp01(1 - (distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
            audioSource.volume = vol * maxVolume;
        }
    }
}
