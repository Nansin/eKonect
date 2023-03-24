using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThingBase : MonoBehaviour
{
    protected GameController gameController;
    protected int hp;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
