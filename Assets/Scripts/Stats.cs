using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public int hp;
    public int armor;
    public int movement;
    public int damage;

    public Stats(int hp, int armor, int movement, int damage){
        this.hp = hp;
        this.armor = armor;
        this.movement = movement;
        this.damage = damage;
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
}
