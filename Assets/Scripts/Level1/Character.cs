using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private string name;
    private GameObject character, impactFace;
    private Image icon, stamina;
    private Animator animator;

    public Character(string name, GameObject character, GameObject impactFace, Image icon, Image stamina){
        this.name = name;
        this.character = character;
        this.impactFace = impactFace;
        this.icon = icon;
        this.stamina = stamina;
        this.animator = character.GetComponent<Animator>();
    }

    public Character(){
        this.name = "";
        this.character = null;
        this.impactFace = null;
        this.icon = null;
        this.stamina = null;
        this.animator = null;
    }

    public bool CompareNameTo(string n){
        if (name == n)
            return true;
        else 
            return false;
    }

    public GameObject GetCharacter(){
        return character;
    }

    public GameObject GetImpactFace(){
        return impactFace;
    }

    public Image GetIcon(){
        return icon;
    }

    public Animator GetAnimator(){
        return animator;
    }

    public void SetIconColor(Color32 newColor){
        icon.color = newColor;
    }

    public void Hide(){
        character.SetActive(false);
        impactFace.SetActive(false);
        icon.color = new Color32(255, 255, 255, 255);
    }

    public void Show(){
        character.SetActive(true);
        icon.color = new Color32(0, 80, 255, 255);
    }

    public float GetStaminaFillAmount(){
        if (stamina == null)
            return 0;
        else 
            return stamina.fillAmount;
    }

    // Animation

    public void SetBool(string name, bool value){
        animator.SetBool(name, value);
    }

    public bool GetBool(string name){
        return animator.GetBool(name);
    }
}
