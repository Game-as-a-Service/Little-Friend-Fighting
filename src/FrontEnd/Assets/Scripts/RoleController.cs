using System;
using System.Linq;
using UnityEngine;

// CommentByLineComment

public class RoleController : MonoBehaviour
{
    public float speed = 500f;
    private Rigidbody2D _rb;
    private bool _isFlipped;
    private Camera _mainCamera;
    private float _objectWidth;
    private float _objectHeight;
    private Animator _animator;
    private static readonly int IsWalk = Animator.StringToHash("isWalk");
    private static readonly int Attacked = Animator.StringToHash("attacked");
    private AudioManager _audioManager;
    private SpriteRenderer _spriteRenderer;

    private Vector2 moveInput;

    void Start()
    {
        _mainCamera = Camera.main;

        _rb = GetComponent<Rigidbody2D>();
        _objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        _animator = GetComponentInChildren<Animator>();
        _audioManager = GetComponentInChildren<AudioManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RestrictMovementInCameraView();
        Attack();
        Move();
    }

    private void RestrictMovementInCameraView()
    {
        // 取得攝影機的可見範圍
        var cameraLeftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).x;
        var cameraRightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane)).x;
        var cameraBottomEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).y;
        var cameraTopEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, _mainCamera.nearClipPlane)).y;

        // 計算ui
        var uiLeftEdge = -400f /*UI左边缘的X坐标*/;
        var uiRightEdge = 400f /*UI右边缘的X坐标*/;
        var uiBottomEdge = -38.78078f /*UI底边缘的Y坐标*/;
        var uiTopEdge = 46.74922f /*UI顶边缘的Y坐标*/;

        // 限制角色在摄像机的可见范围内移动，同时考虑UI元素的边界
        var currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, cameraLeftEdge + _objectWidth / 2,
            cameraRightEdge - _objectWidth / 2);
        currentPosition.y = Mathf.Clamp(currentPosition.y, cameraBottomEdge + _objectHeight / 2,
            cameraTopEdge - _objectHeight / 2);

        // 限制角色在攝影機的可見範圍內移動
        currentPosition.x =
            Mathf.Clamp(currentPosition.x, uiLeftEdge + _objectWidth / 2, uiRightEdge - _objectWidth / 2);
        currentPosition.y = Mathf.Clamp(currentPosition.y, uiBottomEdge + _objectHeight / 2,
            uiTopEdge - _objectHeight / 2);

        transform.position = currentPosition;
    }


    private void Attack()
    {
        if (!Input.GetKeyDown(KeyCode.Z))
        {
            return;
        }

        _audioManager.PlaySoundFromTo(12f, 12.095192743764173f);
        _animator.SetTrigger(Attacked);
    }

    private void Move()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveInput == Vector2.zero)
        {
            _animator.SetBool(IsWalk, false);
        }
        else
        {
            _animator.SetBool(IsWalk, true);
            if (moveInput.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }

            if (!_audioManager.IsPlaying)
            {
                _audioManager.PlaySoundFromTo(4.0f, 4.420272108843537f);
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(moveInput * speed);
    }
}