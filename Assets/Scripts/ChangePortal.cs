using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePortal : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private PlayerMovement _player;

    float rotationSpeed = 60;
    Vector3 currentAngles;
    void Start()
    {
        _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    private void Update()
    {
        //rotate the star along the x axis continuously
        currentAngles += new Vector3(-1, 0, 0) * Time.deltaTime * rotationSpeed;
        transform.localEulerAngles = currentAngles;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "FinishLine")
        {
            //stop moving the fragment
            _player = GameObject.Find("Player").GetComponent<PlayerMovement>();
            _player.hitFragment = false;
            //add intensity to the particle system in the main portal
            ParticleSystem ps = collision.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = _scoreManager.score * 2;
            //destory the star
            Destroy(gameObject);
        }
    }
}
