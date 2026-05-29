using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public enum MaterialType { Metal, Ceramic, Wood }

    [Header("Configuracion Del Objeto")]
    [SerializeField] private MaterialType miMaterial; 
    [SerializeField] private float velocityThreshold = 2f;
    public AudioDatabase database; // El almacén central

    private void OnCollisionEnter(Collision collision)
    {
        float force = collision.relativeVelocity.magnitude;

        if (force > velocityThreshold && database != null)
        {
            AudioClip clip = database.GetAudioClip(miMaterial.ToString());
            if (clip != null)
            {
                float volume = Mathf.Clamp01(force / 10f);
                float pitch = Random.Range(0.8f, 1.1f);

                AudioSource.PlayClipAtPoint(clip, collision.contacts[0].point, volume);
            }
        }
    }
}