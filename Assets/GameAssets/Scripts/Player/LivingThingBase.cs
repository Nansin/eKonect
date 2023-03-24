using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThingBase : MonoBehaviour
{
    protected GameController gameController;
    protected int hp;

    private bool isDie;

    public bool IsDie { get => isDie; set => isDie = value; }

    // Start is called before the first frame update
    private void Start()
    {
        gameController = GameController.Instance;
    }

}
