using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Stop,
    Moving
}

public class PlayerController : MonoBehaviour
{
    private Vector3 direction = new Vector3();
    private float defaultMovementSpeed = 3f;
    private float movementSpeed = 3f;
    private float movementAnimSpeed = 1f;
    private bool isAdIncreaseSpeed = false;
    private float animationSpeed;
    private Rigidbody rigidBody;
    private bool allowMove;
    //private GameController gameController;
    private PlayerState playerState;

    #region Cheat

    //private List<MainCharSpeedData> mainCharSpeedDatas;
    private int level;

    #endregion Cheat

    public static PlayerController Instance;

    public float MovementAnimSpeed
    {
        get => movementAnimSpeed;
        set => movementAnimSpeed = value;
    }

    public float MovementSpeed
    {
        get => movementSpeed;
        private set
        {
            movementSpeed = value;
        }
    }

    //public int MovementSpeedLevel
    //{
    //    get => Prefs.MainCharSpeedLevel;
    //    set
    //    {
    //        Prefs.MainCharSpeedLevel = value;
    //        float coef = /*isAdIncreaseSpeed ? ConfigManager.Instance.AdRewardDatabase.adRewardData.increaseMainCharSpeed :*/ 1f;
    //        MovementSpeed = isAdIncreaseSpeed ? ConfigManager.Instance.AdRewardDatabase.adRewardData.maxMovementSpeed :
    //                                            ConfigManager.Instance.MainCharDatabase.GetSpeedByLevel(MovementSpeedLevel).speed * coef;
    //        Debug.Log("[INFO] MovementSpeed : " + MovementSpeed);
    //    }
    //}

    public bool IsAdIncreaseSpeed
    {
        get => isAdIncreaseSpeed;
        set
        {
            isAdIncreaseSpeed = value;
            //MovementSpeedLevel = MovementSpeedLevel;
        }
    }

    public bool AllowMove { get => allowMove; set => allowMove = value; }
    public PlayerState PlayerState { get => playerState; set => playerState = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AllowMove = true;
        //defaultMovementSpeed = ConfigManager.Instance.MainCharDatabase.GetSpeedByLevel(1).speed;
        //MovementSpeedLevel = MovementSpeedLevel;
    }

    public void Move(float speed, Vector2 dicrection)
    {
        playerState = PlayerState.Moving;
        if (AllowMove)
        {
            direction = new Vector3(dicrection.x, 0, dicrection.y);
            //this.animationSpeed = speed;

            //this.animator.SetFloat("Speed", animationSpeed);
            //this.animator.speed = MovementAnimSpeed;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
        //else
        //    this.animator.SetFloat("Speed", 0);
    }

    public void Stop()
    {
        //isStop = true;
        playerState = PlayerState.Stop;
        direction = Vector3.zero;
        StartCoroutine(ChangeSpeed(animationSpeed, 0, 0.08f));
    }

    private IEnumerator ChangeSpeed(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            animationSpeed = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            //this.animator.SetFloat("Speed", animationSpeed);
            //this.animator.speed = MovementAnimSpeed;
            yield return null;
        }
        animationSpeed = v_end;
        //this.animator.SetFloat("Speed", 0);
        //this.animator.speed = MovementAnimSpeed;
    }

    private void FixedUpdate()
    {
        if (AllowMove)
        {
            rigidBody.AddForce(direction * MovementSpeed, ForceMode.VelocityChange);
        }
    }
}