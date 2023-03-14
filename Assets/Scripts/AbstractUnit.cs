using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractUnit : MonoBehaviour
{
    private int hp;
    private int armorC;
    private int movement;
    private vector<string> damage;
    private string weapon = 'None';
    private string armor = 'None';

    public AbstractUnit(int hp, int armorC, int movement, vector<string> damage){
        this.hp = hp;
        this.armorC = armorC;
        this.movement = movement;
        this.damage = damage;
    }

    public void addWeapon(string weapon){
        if (weapon.equals('Club')) {
            this.weapon = 'Club';
            damage.pushBack('d4');
        }
        if (weapon.equals('HandAxe')) {
            this.weapon = 'HandAxe';
            damage.pushBack('d6');
        }
        if (weapon.equals('GreatClub')) {
            this.weapon = 'GreatClub';
            damage.pushBack('d8');
        }
    }

    public void addArmor(string armor){
        if (armor.equals('Padded')) {
            this.armor = 'Padded';
            armorC = 11;
        }
        if (armor.equals('Studded Leather')) {
            this.armor = 'Studded Leather';
            armorC = 12;
        }
        if (armor.equals('Leather')) {
            this.armor = 'Leather';
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
