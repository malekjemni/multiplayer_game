using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TasskNederManager : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text goldText;

    void Start()
    {
        // R�cup�rer les donn�es du joueur � partir de PlayerPrefs
        string username = PlayerPrefs.GetString("Username");
        int gold = PlayerPrefs.GetInt("Gold");

        // Mettre � jour les �l�ments de la sc�ne avec les donn�es du joueur
        usernameText.text = "Username: " + username;
        goldText.text = "Gold: " + gold.ToString();
    }
}
