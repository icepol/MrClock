using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Rigidbody2D _targetBody;
    private Transform _target;
    private Vector2 _velocity;

    [SerializeField] float smoothX;
    [SerializeField] float smoothY;
    [SerializeField] float overflow;

    float _xMin;
    float _xMax;

    float _yMin;
    float _yMax;

    private float _width;
    private float _height;
    
    float z;

    private bool _isFollowing;

    private void Awake()
    {
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.AddListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.AddListener(Events.CAMERA_START_FOLLOWING, OnCameraStartFollowing);
        EventManager.AddListener(Events.CAMERA_STOP_FOLLOWING, OnCameraStopFollowing);
    }

    private void Start()
    {
        _height = Camera.main.orthographicSize * 2f;
        _width = _height * Camera.main.aspect;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        _target = player.transform;
        _targetBody = player.GetComponent<Rigidbody2D>();

        z = transform.position.z;

        _isFollowing = false;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.RemoveListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.RemoveListener(Events.CAMERA_START_FOLLOWING, OnCameraStartFollowing);
        EventManager.RemoveListener(Events.CAMERA_STOP_FOLLOWING, OnCameraStopFollowing);
    }

    private void Update() {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (!_isFollowing)
            return;
        
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = _target.position;

        Vector2 velocity = _targetBody.velocity;

        float targetX = targetPosition.x + velocity.x * 0.5f;
        float targetY = targetPosition.y + velocity.y * 0.5f;

        float x = Mathf.Clamp(
            Mathf.SmoothDamp(currentPosition.x, targetX, ref _velocity.x, smoothX),
            _xMin,
            _xMax
        );
        
        float y = Mathf.Clamp(
            Mathf.SmoothDamp(currentPosition.y, targetY, ref _velocity.y, smoothY),
            _yMin,
            _yMax
        );

        transform.position = new Vector3(Round(x), Round(y), z);
    }

    float Round(float value)
    {
        return (int) (value * 50) / 50f;
    }

    void OnPlayerDied()
    {
        _isFollowing = false;
    }

    void OnBoundariesBottomLeft(Vector3 vector)
    {
        _xMin = vector.x + _width * 0.5f + overflow;
        _yMin = vector.y + _height * 0.5f + overflow;
    }

    void OnBoundariesTopRight(Vector3 vector)
    {
        _xMax = vector.x - _width * 0.5f - overflow;
        _yMax = vector.y - _height * 0.5f - overflow;
    }

    void OnCameraStartFollowing()
    {
        _isFollowing = true;
    }
    
    void OnCameraStopFollowing()
    {
        _isFollowing = false;
    }
}