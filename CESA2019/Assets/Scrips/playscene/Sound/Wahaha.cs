//
// Wahaha.cs
// Actor: Tamamura Shuuki
//

using UnityEngine;

// わはははははははははｈ
public class Wahaha : MonoBehaviour
{
    private AudioSource _audio;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if(_time >= 10)
        {
            _audio.Play();
            _time = 0;
        }
    }
}
