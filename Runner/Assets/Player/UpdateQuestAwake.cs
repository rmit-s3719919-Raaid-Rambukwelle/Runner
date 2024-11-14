using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateQuestAwake : MonoBehaviour
{
    public string input;
    private void Awake()
    {
        PlayerManager.current.UpdateQuest(input);
    }
}
