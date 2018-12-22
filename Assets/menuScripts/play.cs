using UnityEngine;
using System.Collections;

public class play : MonoBehaviour {



    public int choice;
    void OnMouseDown()
    {
        if (choice == 0)
        {
            // play
           // Application.LoadLevel(0);
            print("hello");
			Application.LoadLevel("Game");

        }
        else if (choice == 1)
        {
            // credits
            print("credits");
            Application.LoadLevel("Credits");
        }
        else if( choice == 2 )
        {
            // exit
            print("quit");
            Application.Quit();
        }
        else if (choice == 3)
        { 
            //back
            print("back");
            Application.LoadLevel("GUI");
        }
       
    }
}
