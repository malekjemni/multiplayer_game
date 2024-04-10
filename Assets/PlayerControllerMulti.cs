using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GameCreator.Runtime.Characters;
using Cinemachine;
using GameCreator.Runtime.VisualScripting;

public class PlayerControllerMulti : NetworkBehaviour
{
    [SerializeField] private Character character; // R�f�rence au GameObject du personnage
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private Trigger trigger;

    [SerializeField] private ThirdPersonController controller;
    [SerializeField] private CharacterController cc;

    [SerializeField] private GameObject UiPrefab;
    [SerializeField] private GameObject UiChatPrefab;


    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //Test with Game Creator
        //character = GetComponent<Character>();
        //character.enabled = true;
        //trigger.enabled = true;


        //Test with CharacterController
        cc.enabled = true;
        controller.enabled = true;

        // Obtenez la r�f�rence au GameObject du personnage
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");      
        mainCamera.GetComponent<Camera>().enabled = true;
        playerCamera.Priority = 10;
 
    }

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //    if (!isLocalPlayer)
    //    {
    //        character.enabled = true;
    //        trigger.enabled = true;
    //        jump.enabled = true;
    //    }
    //}

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Activer le personnage pour le joueur local
           
        GameObject ui = Instantiate(UiPrefab);
        ui.name = "UiPlayer";
        ui.transform.SetParent(transform);
        GameObject uichat = Instantiate(UiChatPrefab);
        uichat.transform.SetParent(transform);

        GetComponent<ChatBehavior>().chatText = uichat.GetComponent<ChatPopup>().chatText;
        GetComponent<ChatBehavior>().inputField = uichat.GetComponent<ChatPopup>().inputField;
        uichat.GetComponent<ChatPopup>().openChatHub();


    }
}
