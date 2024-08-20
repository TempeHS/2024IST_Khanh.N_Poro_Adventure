using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
   [SerializeField] private int health = 100;

   private int Max_HEALTH = 100;
   
   //Update is called once per frame
   void Update ()
   {

   }

   public void Damage(int amount)
   {
    if(amount < 0)
    {
        throw new System.AurgumentOutOfRangeExeception("Cannot have negative Damage")
    }

    this.health -= amount;

      
   }

   public void Heal(int amount)
   {
    if (amount < 0)
    { 
       throw new System.AurgumentOutOfRangeExeception("Cannot have negative healing")
    }

    bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;
    if(health + amount > MAX_HEALTH)
    {
        this.health = MAX_HEALTH;
    }
    else
    {
         this.health += amount;
    }
  }
}
