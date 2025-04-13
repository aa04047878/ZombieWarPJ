using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientData 
{
    /*
    ClientData : 遊戲全局變數的資料
    */

    public string curUserName;
    public override string ToString()
    {
        return $"curUserName : {curUserName}";
    }

    public ClientData(string curUserName)
    {
        this.curUserName = curUserName;
    }
}
