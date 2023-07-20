using System.Collections;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // מערך הקליפים השומעים
    private AudioSource audioSource; // מקור השמע

    private int currentIndex = 0; // האינדקס של הקליפ הנוכחי

    // פעולת ההתחלה
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;



    }

    // פונקציה לערבוב הקליפים
    private void ShuffleClips()
    {
        // שימוש באלגוריתם פישר-ייטס לערבוב המערך
        for (int i = 0; i < audioClips.Length - 1; i++)
        {
            int randomIndex = Random.Range(i, audioClips.Length);
            AudioClip tempClip = audioClips[randomIndex];
            audioClips[randomIndex] = audioClips[i];
            audioClips[i] = tempClip;
        }
    }

    // פונקציה להפעלת קליפ אודיו
    private void PlayNextClip()
    {
        if (currentIndex >= audioClips.Length)
        {
            // ערבוב הקליפים כאשר כולם נשמעו
            ShuffleClips();
            currentIndex = 0;
        }

        audioSource.clip = audioClips[currentIndex];
        audioSource.Play();

        currentIndex++;
    }

    // פונקציה שמופעלת בעת תנגשות אוביקט
    private void OnCollisionEnter(Collision collision)
    {
        PlayNextClip();
    }
}
