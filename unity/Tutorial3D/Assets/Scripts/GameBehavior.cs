using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{

    public bool showWinScreen = false;
    public bool showLossScreen = false;
    private int _itemsCollected = 0;
    private int _playerHP = 3;
    const int MaxItemsDefault = 1;
    public int maxItems = MaxItemsDefault;
    public string labelText = $"Collect all {MaxItemsDefault} items and win your freedom!";


    public int Items
    {
        get { return _itemsCollected; }
        set 
        {
            _itemsCollected = value;
            Debug.Log($"Items: {_itemsCollected}");
            if(_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                showWinScreen = true;
                Time.timeScale = 0.0f;
            }
            else
            {
                labelText = "Item found, only " + (maxItems - _itemsCollected) + " more to go!";
            }
        }
    }

    public int HP
    {
        get { return _playerHP; }
        set 
        {
            _playerHP = value; 
            Debug.Log($"HP: {_playerHP}");
            if(_playerHP <= 0)
            {
                showLossScreen = true;
                Time.timeScale = 0.0f;
            }
            else
            {
                labelText = "Ouch... that's gotta hurt!";
            }
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
        showLossScreen = false;
        showWinScreen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(showWinScreen)
        {
            if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50), "YOU WON!"))
            {
                Debug.Log("Player has won the game!");
                RestartLevel();
            }
        }

        if (showLossScreen)
        {

           if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50), "YOU LOSE!"))
            {
                Debug.Log("Player has lost the game!");
                labelText = "You want to try again?";
                RestartLevel();
            }
        }

        //Debug.Log("OnGUI");
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health: " + _playerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected: " + _itemsCollected);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
    }
}
