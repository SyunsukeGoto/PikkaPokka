using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Command : MonoBehaviour
{
    [SerializeField, Header("コマンド")]
    private string command;
    
    public UnityAction Function;

    private int commandLenght;

    private int currentNum;

    private UnityEvent ue;

    // Start is called before the first frame update
    void Start()
    {
        ue = new UnityEvent();
        SetAction(Message);

        commandLenght = command.Length;

        currentNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(command[currentNum].ToString()))
        {
            currentNum++;
        }

        if(currentNum == commandLenght)
        {
            ue.Invoke();
            currentNum = 0;
        }
    }

    private void Message()
    {
        Debug.Log("コマンドが入力されました");
    }

    public void SetAction(UnityAction ua)
    {
        ue.AddListener(ua);
    }
}
