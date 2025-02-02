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
    //private List<string> damage;
    protected string damageDice;
    private string weapon = "None";
    private string armor = "None";
    private string className = "None";
    //public UnitStatsHud hud;
    private int numLoc;
    private int ss1, ss2, ss3;

    public RollScript rs = new RollScript();

    // types of units:
    // Wizard = Wiz, Cleric = Cle, Skeleton = Sk, Skeleton Horse = SkH
    public AbstractUnit(int hp, int armorC, int movement, string type, int ss1, int ss2, int ss3){
        this.hp = hp;
        maxHp = hp;
        this.armorC = armorC;
        this.movement = movement;
        this.type = type;
        this.ss1 = ss1;
        this.ss2 = ss2;
        this.ss3 = ss3;
        //damage = new List<string>();
        setDamage(type);
    }
    public void addWeapon(string weapon){
        if (weapon.Equals("Club")) {
            this.weapon = "Club";
            damageDice = "D4";
            //damage.Add("d4");
        }
        if (weapon.Equals("HandAxe")) {
            this.weapon = "HandAxe";
            damageDice = "D6";
            //damage.Add("d6");
        }
        if (weapon.Equals("GreatClub")) {
            this.weapon = "GreatClub";
            damageDice = "D8";
            //damage.Add("d8");
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

    public IEnumerator takeDamage(int damageTaken){
        yield return new WaitForSeconds(1f);
        hp -= damageTaken;

        // checkHealth();
        if(hp < 0){
            hp = 0;
            die();
        }
    }

    public void addHealth(int amountHealed) {
        hp += amountHealed;
        if(hp > maxHp)  hp = maxHp;
        // checkHealth();
    }

    public void setDamage(string type){
        if (type.Equals("Wiz")){
            damageDice = "D4";
            //damage.Add("d4");
            className = "Wizard";
        }
            
        if (type.Equals("Cle"))
        {
            damageDice = "D6";
            //damage.Add("d6");
            //damage.Add("d6");
            className = "Cleric";
        }
        if (type.Equals("Sk"))
        {
            damageDice = "D6";
            //damage.Add("d6");
            //damage.Add("2");
            className = "Skeleton";
        }
        if (type.Equals("SkH"))
        {
            damageDice = "D6";
            //damage.Add("d6");
            //damage.Add("d6");
            //damage.Add("4");
            className = "Skel Horse";
        }
    }

    public string getDamageDice()
    {
        return damageDice;
    }

    public int getHp(){
        return hp;
    }

    public int getMaxHp(){
        return maxHp;
    }

    public int getArmorC(){
        return armorC;
    }

    public string getWeapon(){
        return weapon;
    }

    public string getArmor(){
        return armor;
    }

    public int getMove(){
        return movement;
    }

    public string getClass(){
        return className;
    }

    public int getSS1()
    {
        return ss1;
    }
    public void setSS1(int input)
    {
        ss1 = input;
    }

    public int getSS2()
    {
        return ss2;
    }
    public void setSS2(int input)
    {
        ss2 = input;
    }

    public int getSS3()
    {
        return ss3;
    }
    public void setSS3(int input)
    {
        ss3 = input;
    }
    // override in children
    public virtual void die(){
    }
    
    public virtual int rollForAttack()
    {
        return 0;
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
