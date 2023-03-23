using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonComponent<CameraController>
{
    [SerializeField] private Vector3 offset;

    private Transform target;
    private Vector3 camTarget;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.15f;
    private bool isFirstMoveCam = true;
    private List<Tween> tweens = new List<Tween>();
    private Tween firstTween;

    private float shakeAmount = 5f;
    private int count = 0;

    private void Start()
    {
        SetTarget(PlayerController.Instance.transform);
    }

    private void FixedUpdate()
    {
        if (!isFirstMoveCam && target != null && target.hasChanged)
        {
            camTarget = target.position + offset;
        }
        if (!isFirstMoveCam)
        {
            transform.position = Vector3.SmoothDamp(transform.position, camTarget, ref velocity, smoothTime);
        }
    }

    public void SetTarget(Transform target, float timeChange = 0.5f)
    {
        //PlayerController.Instance.AllowMove = false; // lock move player when camera move to other target
        CancelInvoke();
        if (firstTween != null)
            firstTween.Kill();
        this.target = target;
        if (this.target == null)
            return;
        Vector3 newPos = target.position + offset;
        Tween tween = this.transform.DOMove(newPos, timeChange).SetEase(Ease.Linear).OnComplete(() =>
        {
            isFirstMoveCam = false;
        });
        tweens.Add(tween);
    }
}