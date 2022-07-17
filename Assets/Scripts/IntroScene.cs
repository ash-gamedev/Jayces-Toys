using System.Collections;
using UnityEngine;

public class IntroScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AudioManager.instance.PlaySoundEffect(EnumSoundName.IntroSoundEffect);
    }
        

    // Update is called once per frame
    void Update()
    {

    }
}