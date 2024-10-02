using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int someData = 99;
    
    public void SayHello()
    {
        Debug.LogFormat("Hello, I'm Game Manager! someData value: {0}", someData);
    }
    
}
