using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainScript : MonoBehaviour
{
    public static bool MuteSound = false;
    public Sprite MuteSprite, UnMuteSprite;
    public ParticleSystem System;
    public Animator Transition;
    public bool Dead = true, NeedToShowAds = true;
    public GameObject Level, WallsPrefab, ScoreText, 
        StartButton, RestartButton, MuteButton, TutorialText,
        BGPrefab, GroundPrefab, JumpSound, DeathSound, Explosion;
    public float Score, Speed, JumpForce;
    Rigidbody2D Rb;
    GameObject CurWalls;
    int Spacing = 25, NeedToGenerate = 10;
    bool NeedToPlaySound = true;
    
    void Start()
    {
        BGGeneration(BGPrefab, -12, -7, 0);
        BGGeneration(BGPrefab, 12, -7, 0);
        BGGeneration(BGPrefab, 12, 7, 0);
        BGGeneration(BGPrefab, -12, 7, 0);
        BGGeneration(BGPrefab, 36, -7, 0);
        BGGeneration(BGPrefab, 36, 7, 0);

        BGGeneration(GroundPrefab, -12, -12, 0);
        BGGeneration(GroundPrefab, 12, -12, 0);
        BGGeneration(GroundPrefab, 12, 12, 180);
        BGGeneration(GroundPrefab, -12, 12, 180);
        BGGeneration(GroundPrefab, 36, -12, 0);
        BGGeneration(GroundPrefab, 36, 12, 180);
        StartButton.transform.localScale = new Vector2((Screen.height / 100) * 20, (Screen.height / 100) * 20);
        MuteButton.transform.localScale = new Vector2((Screen.height / 100) * 15, (Screen.height / 100) * 15);
        RestartButton.transform.localScale = new Vector2((Screen.height / 100) * 15, (Screen.height / 100) * 15);
        Rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < NeedToGenerate; i++)
        {
            Generation();
        }
    }

    void Update()
    {
        ScoreText.transform.position = new Vector2(Screen.width / 2, (Screen.height / 10) * 8);
        StartButton.transform.position = new Vector2(Screen.width / 2, (Screen.height / 10) * 4);
        RestartButton.transform.position = new Vector2(Screen.width / 2, (Screen.height / 10) * 4);
        MuteButton.transform.position = new Vector2(Screen.width / 2, (Screen.height / 10) * 2);
        if (Dead != true)
        {
            ScoreText.SetActive(true);
            ScoreText.GetComponent<Text>().text = "Score: " + Score;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                //Rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                if (touch.phase == TouchPhase.Began)
                {
                    Rb.gravityScale = -10f;
                    if (NeedToPlaySound != false)
                    {
                        JumpSound.GetComponent<AudioSource>().Play();
                        NeedToPlaySound = false;
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    Rb.gravityScale = 1f;
                    NeedToPlaySound = true;
                }
            }
            if (Score > Speed * 10)
            {
                Speed = Score / 10;
            }
            Level.transform.Translate(Vector2.left * Speed * Time.deltaTime, 0);
        }

        if (MuteSound)
        {
            AudioListener.volume = 0f;
            MuteButton.GetComponent<Image>().sprite = UnMuteSprite;
        }
        else
        {
            MuteButton.GetComponent<Image>().sprite = MuteSprite;
            AudioListener.volume = 1f;
        }
    }

    public void Generation()
    {
        float Y = Random.Range(-5, 5);
        Vector2 Spawn = new Vector2(Spacing * 2, Y);
        if (CurWalls != null)
            Spawn = new Vector2(CurWalls.transform.position.x + Spacing, Y);
        CurWalls = Instantiate(WallsPrefab);
        CurWalls.transform.position = Spawn;
        CurWalls.transform.parent = Level.transform;
    }

    void BGGeneration(GameObject Prefab, float x, float y, float r)
    {
        GameObject CurBG = Instantiate(Prefab);
        CurBG.transform.position = new Vector2(x, y);
        CurBG.transform.Rotate(new Vector3(0, 0, r));
        CurBG.transform.parent = Level.transform;
    }

    public void StartBTN()
    {
        Dead = false;
        StartButton.SetActive(false);
        MuteButton.SetActive(false);
        TutorialText.SetActive(true);
        Rb.isKinematic = false;
        System.Play();
    }

    public void Restart()
    {
        ScoreText.SetActive(false);
        StartCoroutine(LoadLevel());
        //SceneManager.LoadScene(0);
    }

    public void Mute()
    {
        MuteSound = !MuteSound;
    }

    IEnumerator LoadLevel()
    {
        Transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D Object)
    {
        if(Object.tag == "Respawn")
        {
            if (Dead != true)
            {
                DeathSound.GetComponent<AudioSource>().Play();
                Instantiate(Explosion, transform.position, Quaternion.identity);
                Dead = true;
                System.Stop();
                RestartButton.SetActive(true);
            }
        }
    }
}
