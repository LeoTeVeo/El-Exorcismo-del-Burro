using UnityEngine;

[System.Serializable]

public class MaterialGroup
{
    public string type;
    public AudioClip[] clips;
}

[CreateAssetMenu (fileName = "AudioDatabase", menuName = "Audio/Database")]

public class AudioDatabase : MonoBehaviour
{
    public MaterialGroup[] materials;

    public AudioClip GetAudioClip(string typeMaterial)
    {
        foreach (var group in materials)
        {
            if (group.type == typeMaterial)
            {
                return group.clips[Random.Range(0, group.clips.Length)];
            }
        }

        return null;
    }
}
