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

    private bool isInvoke = false;

    public bool IsInvoke
    {
        get { return isInvoke; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        ue = new UnityEvent();
        //ue.AddListener(Message);

        commandLenght = command.Length;

        currentNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.anyKeyDown)
		{
        		if(Input.GetKeyDown(command[currentNum].ToString()))
        		{
        		    currentNum++;
        		}
			else
			{
				currentNum = 0;
			}
		}

        if(currentNum == commandLenght)
        {
            ue.Invoke();
            currentNum = 0;
            isInvoke = true;
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
