using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator aimator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Play an attack animation
        aimator.SetTrigger("Attack");

        // Detect enemies in range of attack
        // Damage them
    }

}
