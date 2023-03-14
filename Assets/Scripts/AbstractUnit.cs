using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractUnit : MonoBehaviour
{
    private int hp;
    private int armor;
    private int movement;
    private int damage;

    public AbstractUnit(int hp, int armor, int movement, int damage){
        this.hp = hp;
        this.armor = armor;
        this.movement = movement;
        this.damage = damage;
    }

    public void addWeapon(string weapon){
        // add weapon to damage
    }

    public void addArmor(string armor){
        // add armor to AC
    }

    public void takeDamage(int damageTaken){
        hp -= damageTaken;
        if(hp < 0)  hp = 0;
    }

    public int getHp(){
        return hp;
    }

    public int getArmor(){
        return armor;
    }

    public int getMove(){
        return movement;
    }

    public void die(){
        //remove everything about this unit here
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
