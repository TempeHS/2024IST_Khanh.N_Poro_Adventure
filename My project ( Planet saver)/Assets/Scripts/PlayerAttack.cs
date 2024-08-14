using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies; 
    public float attackRange; 
    public int damage;

    Void Update(){

        if(timeBtwAttack <= 0){
            // then you can attack
            if(Input. Getkey(KeyCode.Space)){
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleA11( attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++) {
                    enemiesToDamage[i].GetComponent<Enemy>().health -= damage;
                }
            }

            timeBtwAttack = startTimeBtwAttack;
        } else {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected(){

        
    }     