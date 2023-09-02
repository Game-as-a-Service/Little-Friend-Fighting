using UnityEngine;
// CommentByLineComment

public class RoleController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D _rb;
    private bool _isFlipped;
    private Camera _mainCamera;
    private float _objectWidth;
    private float _objectHeight;
    private Animator _animator;
    private static readonly int IsWalk = Animator.StringToHash("isWalk");
    private AudioManager _audioManager;
    private static readonly int Attacked = Animator.StringToHash("attacked");


    void Start()
    {
        _mainCamera = Camera.main;
        
        _rb = GetComponent<Rigidbody2D>();
        _objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        
        _animator = GetComponentInChildren<Animator>();
        _audioManager = GetComponentInChildren<AudioManager>();
    }

    void Update()
    {
        RestrictMovementInCameraView();
        Attack();
        Move();
    }

    // private void RestrictMovementInCameraView()
    // {
    //     // 取得攝影機的可見範圍
    //     var cameraLeftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).x;
    //     var cameraRightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane)).x;
    //     var cameraBottomEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).y;
    //     var cameraTopEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, _mainCamera.nearClipPlane)).y;
    //
    //     // 限制角色在攝影機的可見範圍內移動
    //     var currentPosition = transform.position;
    //     currentPosition.x = Mathf.Clamp(currentPosition.x, cameraLeftEdge + _objectWidth / 2, cameraRightEdge - _objectWidth / 2);
    //     currentPosition.y = Mathf.Clamp(currentPosition.y, cameraBottomEdge + _objectHeight / 2, cameraTopEdge - _objectHeight / 2);
    //     transform.position = currentPosition;
    // }
    
    private void RestrictMovementInCameraView()
    {
        // 取得攝影機的可見範圍
        var cameraLeftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).x;
        var cameraRightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane)).x;
        var cameraBottomEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane)).y;
        var cameraTopEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, _mainCamera.nearClipPlane)).y;

        // 計算ui
        var uiLeftEdge = -400f/*UI左边缘的X坐标*/;
        var uiRightEdge = 400f/*UI右边缘的X坐标*/;
        var uiBottomEdge = -38.78078f/*UI底边缘的Y坐标*/;
        var uiTopEdge = 46.74922f/*UI顶边缘的Y坐标*/;

        // 限制角色在摄像机的可见范围内移动，同时考虑UI元素的边界
        var currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, cameraLeftEdge + _objectWidth / 2, cameraRightEdge - _objectWidth / 2);
        currentPosition.y = Mathf.Clamp(currentPosition.y, cameraBottomEdge + _objectHeight / 2, cameraTopEdge - _objectHeight / 2);

        // 限制角色在攝影機的可見範圍內移動
        currentPosition.x = Mathf.Clamp(currentPosition.x, uiLeftEdge + _objectWidth / 2, uiRightEdge - _objectWidth / 2);
        currentPosition.y = Mathf.Clamp(currentPosition.y, uiBottomEdge + _objectHeight / 2, uiTopEdge - _objectHeight / 2);

        transform.position = currentPosition;
    }
    
    
    private void Attack()
    {
        if (!Input.GetKeyDown(KeyCode.Z)) return;
        _audioManager.PlaySoundFromTo(12f, 12.095192743764173f);
        _animator.SetTrigger(Attacked);
    }
    
    private void Move()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector2(moveHorizontal, moveVertical);
        _rb.velocity = movement * speed;
        
        var isMoving = (Mathf.Abs(moveHorizontal) > 0.1f) || (Mathf.Abs(moveVertical) > 0.1f);
        _animator.SetBool(IsWalk, isMoving);
        if (IsPlayWalkSound(isMoving)) _audioManager.PlaySoundFromTo(4.0f, 4.420272108843537f);
        HandleFlip(moveHorizontal);
    }

    private bool IsPlayWalkSound(bool isMoving)
    {
        return isMoving && !_audioManager.IsPlaying;
    }

    private void HandleFlip(float moveHorizontal)
    {
        switch (moveHorizontal)
        {
            case < 0 when !_isFlipped:
            case > 0 when _isFlipped:
                Flip();
                break;
        }
    }

    private void Flip()
    {
        _isFlipped = !_isFlipped;
        var t = transform;
        var scale = t.localScale;
        scale.x *= -1;
        t.localScale = scale;
    }
}
