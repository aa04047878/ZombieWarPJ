using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientData 
{
    /*
    ClientData : �C�������ܼƪ����
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
