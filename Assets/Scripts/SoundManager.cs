using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] GameObject soundObjectPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, Transform playPos)
    {
        GameObject soundObject = Instantiate(soundObjectPrefab, playPos.position, Quaternion.identity);
        AudioSource soundSource = soundObject.GetComponent<AudioSource>();

        soundSource.clip = clip;
        soundSource.pitch = Random.Range(0.75f, 1.5f);
        soundSource.Play();

        Destroy(soundObject, clip.length);

    }

}
