using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BalloonGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject balloonPrefab;

    [SerializeField] public List<AudioClip> balloonPopSounds;

    public Sprite[] destrSp;

    float spawnDelay = 0.3f;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        SpawnBalloon();
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(Spawn());
        yield return null;
    }

    void SpawnBalloon()
    {
        Vector2 spawnPosition = new Vector2(Screen.width * Random.Range(0.1f, 0.9f), -200f);
        GameObject obj = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity, transform);
        obj.transform.localScale = Vector3.one;
        Destroy(obj, 5f);
        spawnDelay *= 0.999f;
    }
}