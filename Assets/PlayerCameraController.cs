using UnityEngine;
using Mirror;


public class PlayerCameraController : NetworkBehaviour
{
    public Camera playerCamera;


    public override void OnStartClient()
    {
        base.OnStartClient();

        // D�sactiver le personnage pour les clients qui ne sont pas le joueur local
        if (!isLocalPlayer)
        {
            Debug.Log("local");
           playerCamera.enabled = false;

        }
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Activer le personnage pour le joueur local
        playerCamera.enabled = true;
    }


}
