using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WaveformScene : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent <AudioSource>();
        source.time = Random.value * source.clip.length;
        source.Play();
        Invoke(nameof(backToCity), 30);
    }

    void backToCity()
    {
        SceneManager.LoadScene("main");
    }

}
