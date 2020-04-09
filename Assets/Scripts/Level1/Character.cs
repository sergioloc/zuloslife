using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character
{
    private string alias;
    private GameObject character, impactFace;
    private Image icon, stamina;
    private Animator animator;

    public Character(string alias, GameObject character, GameObject impactFace, Image icon, Image stamina){
        this.alias = alias;
        this.character = character;
        this.impactFace = impactFace;
        this.icon = icon;
        this.stamina = stamina;
        this.animator = character.GetComponent<Animator>();
    }

    public bool CompareNameTo(string n){
        if (alias == n)
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

    public void SetIconColor(Color32 blue){
        icon.color = blue;
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

    public bool IsPlayingAnimation(string anim){
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(anim)){
            return true;
        }
        return false;
    }

    public bool IsImpactFaceActive(){
        if (impactFace.activeSelf)
            return true;
        else
            return false;   
    }

    public void SetImpactFaceActive(bool state){
        impactFace.SetActive(state);
    }
}
