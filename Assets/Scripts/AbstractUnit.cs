using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class AbstractUnit : MonoBehaviour
{
    
    private int hp;
    private int maxHp;
    private int armorC;
    private int movement;
    private string type;
    private List<string> damage;
    private string weapon = "None";
    private string armor = "None";
    private string className = "None";
    public UnitStatsHud hud;


    public RollScript rs = new RollScript();

    // types of units:
    // Wizard = Wiz, Cleric = Cle, Skeleton = Sk, Skeleton Horse = SkH
    public AbstractUnit(int hp, int armorC, int movement, string type){
        this.hp = hp;
        maxHp = hp;
        this.armorC = armorC;
        this.movement = movement;
        this.type = type;
        damage = new List<string>();
        setDamage(type);
    }

    public void addWeapon(string weapon){
        if (weapon.Equals("Club")) {
            this.weapon = "Club";
            damage.Add("d4");
        }
        if (weapon.Equals("HandAxe")) {
            this.weapon = "HandAxe";
            damage.Add("d6");
        }
        if (weapon.Equals("GreatClub")) {
            this.weapon = "GreatClub";
            damage.Add("d8");
        }
    }

    public void addArmor(string armor){
        if (armor.Equals("Padded")) {
            this.armor = "Padded";
            armorC = 11;
        }
        if (armor.Equals("Studded Leather")) {
            this.armor = "Studded Leather";
            armorC = 12;
        }
        if (armor.Equals("Leather")) {
            this.armor = "Leather";
            armorC = 11;
        }
    }

    public void takeDamage(int damageTaken){
        hp -= damageTaken;

        // healthBar.SetProgress(hp / maxHealth, 3);
        if(hp < 0){
            hp = 0;
            die();
        }
    }

    private void getReferences(){
        hud = GetComponent<UnitStatsHud>();
    }

    public void checkHealth(){
        hud.UpdateHealth(hp, maxHp);
    }

    public void setDamage(string type){
        if (type.Equals("Wiz")){
            damage.Add("d4");
            className = "Wizard";
        }
            
        if (type.Equals("Cle")) {
            damage.Add("d6");
            damage.Add("d6");
            className = "Cleric";
        }
        if (type.Equals("Sk")) {
            damage.Add("d6");
            damage.Add("2");
            className = "Skeleton";
        }
        if (type.Equals("SkH")) {
            damage.Add("d6");
            damage.Add("d6");
            damage.Add("4");
            className = "Skel Horse";
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
    // override in children
    public virtual void die(){
    }
    
    public void rollForAttack()
    {
        //iterate through damage list
        for(int i = 0; i < damage.Count; i++)
        {
            int totalDam = 0;
            if(damage[i].Equals("d4"))
                totalDam += rs.rollD("D4");
            if(damage[i].Equals("d6"))
                totalDam += rs.rollD("D6");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        getReferences();
        hud.createStats(className, armorC, movement);
        
    }

    // Update is called once per frame
    void Update()
    {
        checkHealth();
    }

}
