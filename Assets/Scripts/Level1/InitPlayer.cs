using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitPlayer : MonoBehaviour
{
    [Header("Characters")]
    public GameObject pandaGameObject;
    public GameObject keroGameObject;
    public GameObject cinamonGameObject;
    public GameObject kutterGameObject;
    public GameObject triskyGameObject;

    [Header("Impact Faces")]
    public GameObject pandaImpactFace;
    public GameObject keroImpactFace;
    public GameObject cinamonImpactFace;
    public GameObject kutterImpactFace;
    public GameObject triskyImpactFace;

    [Header("Icons")]
    public Image fillPanda;
    public Image fillKero;
    public Image fillCinamon;
    public Image fillKutter;
    public Image fillTrisky;
    private Image fillCurrent;

    [Header("Stamina")]
    public Image staminaPanda;
    public Image staminaKero;
    public Image staminaCinamon;
    public Image staminaKutter;
    public Image staminaTrisky;

    void Awake()
    {
        LevelOneValues.characters = new Character[5];
        LevelOneValues.characters[0] = new Character("Panda", pandaGameObject, pandaImpactFace, fillPanda, staminaPanda);
        LevelOneValues.characters[1] = new Character("Kero", keroGameObject, keroImpactFace, fillKero, staminaKero);
        LevelOneValues.characters[2] = new Character("Cinamon", cinamonGameObject, cinamonImpactFace, fillCinamon, staminaCinamon);
        LevelOneValues.characters[3] = new Character("Kutter", kutterGameObject, kutterImpactFace, fillKutter, staminaKutter);
        LevelOneValues.characters[4] = new Character("Trisky", triskyGameObject, triskyImpactFace, fillTrisky, staminaTrisky);
    }
}
