using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    private float _speed;
    [SerializeField] private float _xLimit = 10.0f;
    private float _yLimit;
    Vector2 _direction;
    private AudioSource _source;
    [SerializeField] private AudioClip _wallhit;
    [SerializeField] private AudioClip _paddlehit;
    [SerializeField] private AudioClip _losepoint;
        
    // Start is called before the first frame update
    void Start()
    {
        _yLimit = GameBehavior.Instance.CalculateYLimit(gameObject);
        Debug.Log(_yLimit);
        _source = GetComponent<AudioSource>();
        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehavior.Instance.State == GameBehavior.StateMachine.Play)

        {
            transform.position += (Vector3)_direction * _speed * Time.deltaTime;

            /*
            transform.position += new Vector3(
                _speed * _direction.x,
                _speed * _direction.y,
                0
            ) * Time.deltaTime;
            */

            CheckBounds();
        }
    }

    void CheckBounds()
    {
        if (Mathf.Abs(transform.position.y) > _yLimit)
        {
            _direction.y *= -1;
            transform.position = new Vector3(
                transform.position.x, 
                Mathf.Sign(transform.position.y) * _yLimit, 
                transform.position.z);
            // _source.clip = _wallhit;
            _source.PlayOneShot(_wallhit); // not interrupt for effects
        }

        if (Mathf.Abs(transform.position.x) > _xLimit)
        {
            GameBehavior.Instance.ScorePoint(
                transform.position.x > 0 ? 0 : 1);
            ResetBall();
        }
    }
    void ResetBall()
    {
        transform.position = Vector3.zero;
            
        _direction = new Vector2(
            Random.value > 0.5f ? 1 : -1,       // ternary operator
            Random.value > 0.5f ? 1 : -1
        );          // condition ? pass : fail
        /*
        if (Random.value > 0.5f {
            _direction.x = 1
         } else {
            _direction.x = -1
         }
        */
        _speed = GameBehavior.Instance.InitialBallSpeed;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision!");
        if (other.gameObject.CompareTag("Paddle"))
        {
            _speed *= GameBehavior.Instance.BallSpeedIncrement;
            _direction.x *= -1;
        }
    }
}
