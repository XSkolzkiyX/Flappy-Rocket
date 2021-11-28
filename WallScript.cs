using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    GameObject Player, ScoreSound;
    int Score = 1;

    void Start()
    {
        Player = GameObject.Find("Player");
        ScoreSound = GameObject.Find("ScoreSound");
    }

    void Update()
    {
        if(transform.position.x <= -30)
        {
            Player.GetComponent<MainScript>().Generation();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if(Object.tag == "Player")
        {
            Player.GetComponent<MainScript>().Score += Score;
            Score = 0;
            ScoreSound.GetComponent<AudioSource>().Play();
        }
    }
}
