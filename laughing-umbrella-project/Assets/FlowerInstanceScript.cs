using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerInstanceScript : MonoBehaviour
{
    int damage = 1;
    float knockbackStrength = 2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues(int damage, float knockbackStrength)
    {
        this.damage = damage;
        this.knockbackStrength = knockbackStrength;
    } 

    public int GetDamage()
    {
        return damage;
    }
    public float GetKnockbackStrength()
    {
        return knockbackStrength;
    }
}
