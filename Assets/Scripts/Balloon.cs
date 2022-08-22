using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    [SerializeField] private List<Color> colors;
    [SerializeField] private float balloonSpeed = 20f;

    Image image;
    BalloonGenerator bl;

    bool isTouched = false;

    System.Random random;

    void Start()
    {
        bl = FindObjectOfType<BalloonGenerator>();
        image = GetComponent<Image>();
        // get random color
        random = new System.Random();
        int index = random.Next(colors.Count);
        Color color = colors[index];
        image.color = color;
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + (balloonSpeed * Time.deltaTime));    
    }

    public void OnTouch()
    {
        if (isTouched) return;
        isTouched = true;

        // pick random pop sound to play
        int index = random.Next(bl.balloonPopSounds.Count);
        AudioSource.PlayClipAtPoint(bl.balloonPopSounds[index], Camera.main.transform.position, 0.5f);

        StartCoroutine(ShowAnim());
        Destroy(gameObject, 0.8f);
    }

    IEnumerator ShowAnim()
    {
        //image.sprite = bl.destrSp[spriteNumber];
        //spriteNumber++;
        //if (spriteNumber == bl.destrSp.Length)
        //{
        //    image.enabled = false;
        //    yield break;
        //}
        yield return new WaitForSeconds(0.1f);
        image.sprite = bl.destrSp[0];
        //StartCoroutine(ShowAnim());
        //yield return null;
    }
}
