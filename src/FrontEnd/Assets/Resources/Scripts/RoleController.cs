using UnityEngine;
using UnityEngine.UI;

public class RoleController : MonoBehaviour
{
    public int PlayerId;
    public KeyCode AttackKeyCode;
    public float speed = 500f;
    [SerializeField] private Rigidbody2D rb;
    private Camera _mainCamera;
    private Animator _animator;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioSource _audioSource;
    private float _objectWidth;
    private float _objectHeight;
    private static readonly int IsWalk = Animator.StringToHash("isWalk");
    private static readonly int Attacked = Animator.StringToHash("attacked");
    private static readonly int Dead = Animator.StringToHash("Dead");
    public string PlayerHp;
    private float _hp;
    private float _maxHp;
    private Image _hpBar;

    private Vector2 _moveInput;
    private Transform _transform;
    private int _rightArrowPressedCount;
    private float _rightArrowPressedFirstTime;
    private int _leftArrowPressedCount;
    private float _leftArrowPressedFirstTime;
    private bool _isRunFacingRight;

    void Start()
    {
        _mainCamera = Camera.main;

        // TODO: Use [SerializeField] 
        _objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        _animator = GetComponentInChildren<Animator>();
        _transform = GetComponent<Transform>();

        _maxHp = 100f;
        _hp = _maxHp;
        _hpBar = GameObject.Find(PlayerHp).GetComponent<Image>();
    }

    void Update()
    {
        RestrictMovementInCameraView();
        Attack();
        Move();
        
        // 按兩下方向鍵跑步, 而且要往朝向的方向跑,而且要控制不同的玩家跑，像是玩家一按右鍵跑，玩家二按左鍵跑
        if (PlayerId == 2 && Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rightArrowPressedCount++;
            if (_rightArrowPressedCount == 1)
            {
                _rightArrowPressedFirstTime = Time.time;
            }
            else if (_rightArrowPressedCount == 2)
            {
                if (Time.time - _rightArrowPressedFirstTime <= 1)
                {
                    _isRunFacingRight = _transform.localScale.x > 0;
                    Run();
                }
        
                _rightArrowPressedCount = 0;
            }
        }
        else if (PlayerId == 2 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _leftArrowPressedCount++;
            if (_leftArrowPressedCount == 1)
            {
                _leftArrowPressedFirstTime = Time.time;
            }
            else if (_leftArrowPressedCount == 2)
            {
                if (Time.time - _leftArrowPressedFirstTime <= 1)
                {
                    _isRunFacingRight = _transform.localScale.x > 0;
                    Run();
                }
                
                _leftArrowPressedCount = 0;
            }
        }
    }

    private void Run()
    {
        _animator.SetBool("Run", true);
        speed = 1000f;
    }

    private void RestrictMovementInCameraView()
    {
        // TODO: 可以用 Cinemachine
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
        if (Input.GetKeyDown(AttackKeyCode))
        {
            // TODO: 要切檔案，集中管理
            _animator.SetTrigger(Attacked);
        }
    }

    public void OnDamage(float damage)
    {
        _hp -= damage;
        var fillAmount = _hp / _maxHp;
        fillAmount = Mathf.Clamp01(fillAmount);
        _hpBar.fillAmount = fillAmount;

        if (_hp <= 0)
        {
            Debug.Log("dead.");
            _animator.SetTrigger(Dead);
        }
        else
        {
            Debug.Log($"血量剩餘{_hp}");
        }
    }

    private void Move()
    {
        var horizontal = Input.GetAxis("PlayerHorizontal" + PlayerId);
        var vertical = Input.GetAxis("PlayerVertical" + PlayerId);
        _moveInput = new Vector2(horizontal, vertical);

        if (_moveInput == Vector2.zero)
        {
            _animator.SetBool(IsWalk, false);
        }
        else
        {
            var audioClip = _audioManager.GetAudioClip("Walk");

            // TODO: 動態產生 AudioSource
            if (_audioSource.isPlaying is false)
            {
                _audioSource.PlayOneShot(audioClip);
            }

            if (_isRunFacingRight && _moveInput.x < 0 ||
                !_isRunFacingRight && _moveInput.x > 0)
            {
                speed = 500f;
                _animator.SetBool("Run", false);
            }

            _animator.SetBool(IsWalk, true);
            var localScale = _transform.localScale;
            _transform.localScale = _moveInput.x > 0
                ? new Vector3(1, localScale.y, localScale.z)
                : new Vector3(-1, localScale.y, localScale.z);
        }
    }

    private void FixedUpdate()
    {
        // 如果是在 Run 的狀態，就會往角色的方向往前移動，不用案方向鍵
        if (_animator.GetBool("Run"))
        {
            var isRight = _transform.localScale.x > 0;
            var vector3 = isRight
                ? _transform.right.normalized
                : -transform.right.normalized;
            rb.AddForce(vector3 * speed);
            return;
        }

        rb.AddForce(_moveInput * speed);
    }
}