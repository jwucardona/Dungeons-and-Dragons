using System.Collections;
using System.Collections.Generic;
using UnityEngine;

import "System.String.Format";

public class AbstractUnit : MonoBehaviour
{

    private int hp;
    private int armorC;
    private int movement;
    private string type;
    private List<string> damage;
    private string weapon = "None";
    private string armor = "None";

    // types of units:
    // Wizard = Wiz, Cleric = Cle, Skeleton = Sk, Skeleton Horse = SkH
    public AbstractUnit(int hp, int armorC, int movement, string type){
        this.hp = hp;
        this.armorC = armorC;
        this.movement = movement;
        this.type = type;
    }

    public void addWeapon(string weapon){
        if (String.Equals("Club", weapon)) {
            this.weapon = "Club";
            damage.add("d4");
        }
        if (String.Equals("HandAxe", weapon)) {
            this.weapon = "HandAxe";
            damage.add("d6");
        }
        if (String.Equals("GreatClub", weapon)) {
            this.weapon = "GreatClub";
            damage.add("d8");
        }
    }

    public void addArmor(string armor){
        if (String.Equals("Padded", armor)) {
            this.armor = "Padded";
            armorC = 11;
        }
        if (String.Equals("Studded Leather", armor)) {
            this.armor = "Studded Leather";
            armorC = 12;
        }
        if (String.Equals("Leather", armor)) {
            this.armor = "Leather";
            armorC = 11;
        }
    }

    public void takeDamage(int damageTaken){
        hp -= damageTaken;
        if(hp < 0){
            hp = 0;
            die();
        }
    }

    public int getHp(){
        return hp;
    }

    public int getArmor(){
        return armorC;
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
