using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAttacks : MonoBehaviour
{
    public ItemList itemList;
    public BattleManager battleManager;
    public GameObject attackButton;
    void Start()
    {
        LoadAttackSkills();
    }

    public void LoadAttackSkills()
    {
        // with the variable "weapon" I can check for how many elements of WeaponSkills there are and add a button for each instance
        // when each instance is being added, I should be assigning the functionality alongside the creation of the button
        //functionality of the skill will be assigned within the weapon scriptableobject itself
        //but i need to chek Players level, with the level of the element to see if it is equal or higher, before the creation of the button

        //i will need to create a one-for-all method in battle manager
        // a PlayerSkill which will take in, skill name, skill damage multiplier, skill element, skill type
        // when the button is created, it will be assigned to that method 

        int id = PlayerPrefs.GetInt("InventorySlotScene" + 18 + "ID", -1);
        Items weapon = null;

        for (int j = 0; j < itemList.items.Length; j++)
        {
            if (itemList.items[j].id == id)
            {

                weapon = itemList.items[j];
                Debug.Log(weapon.itemName);
            }
            if (weapon != null)
            {
                break;
            }
        }
        for (int i = 0; i < weapon.skillName.Length; i++) // for each skill the weapon has, make a button
        {
            if(PlayerPrefs.GetInt("playerlevel") >= weapon.skillLevel[i])
            {
                GameObject buttonGO = Instantiate(attackButton, transform);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonText.text = weapon.skillName[i];
                buttonGO.GetComponent<Button>().onClick.AddListener(() => UseSkill(weapon.skillName[i], weapon.skillPower[i], weapon.skillElement[i]));
            }
            
        }
    }

    public void UseSkill(string skillname, float power, string element)
    {
        if (battleManager.state == BattleState.PLAYERTURN)
        {
            PlayerSkills skill = GetComponent<PlayerSkills>();
            skill.AttackSkill(skillname, element, power);
            battleManager.WeaponSkill();
            Debug.Log("button for the skill was made");
        }
            
    }

}
