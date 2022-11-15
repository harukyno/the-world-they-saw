using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using War_System;
using System;

public class Summoner : MonoBehaviour
{
    public Combat_Unit_Definition Combat_Unit;
    public Combat_Unit_Definition[] Unit_List = new Combat_Unit_Definition[3];
    // Start is called before the first frame update
    void Start()
    {
        Summon_T();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Summon_T()
    {
        int[] Summon_central_coordinates = {0,0,0};
        int[] Summon_Size ={50,40,1};
        int Summon_Number = 20;
        int[] Summon_vector = {1,1,1};
        float Summon_Width = 1;
        int[] Summon_Status = {0};
        Array.Resize(ref Summon_Status,109);
        Summon_Status[0] = 0;
        Summon_Status[1] = 25;
        Summon_Status[2] = 25;
        Summon_Status[3] = 25;
        Summon_Status[4] = 15;
        Summon_Status[5] = 25;
        Summon_Status[6] = 25;
        Summon_Status[7] = 25;
        Summon_Status[8] = 15;

        Summon_Status[9] = 6;
        Summon_Status[10] = 5;
        Summon_Status[20] = 6;
        Summon_Status[21] = 5;

        Summon_Status[31] = 6;
        Summon_Status[32] = 6;
        Summon_Status[33] = 0;
        Summon_Status[34] = 0;
        Summon_Status[37] = 0;

        string[] Summon_Have_Unit = {""};
        int[] Summon_Have_Unit_Number = {0};
        string[] Summon_Have_Ammunition = {""};
        int[] Summon_Have_Ammunition_Number = {0};

        int Summon_Unit_Type = 0;
        string[] Summon_Skill = {""};
        float[] Summon_Resistance = {1,1,1,1,1,1,1,1,1};
        float[] Summon_Spiritual =  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        float[] inventity = {0.5F,0.0F,0.0F};
        short Summon_Control = 1;
        int master = 1;

        for (int i = 0; i < 10; i++)
        {
            Unit_List[i] = Instantiate(Combat_Unit,transform.position,Quaternion.identity);
            Unit_List[i].name = "Combat_Unit" + i;
            Array.Resize(ref Unit_List,Unit_List.Length + 1);
            Unit_List[i].Unit_Setup(Summon_Size,Summon_Number,Summon_Width,Summon_Status,Summon_Have_Unit ,Summon_Have_Unit_Number,Summon_Have_Ammunition,Summon_Have_Ammunition_Number,Summon_Unit_Type,Summon_Skill,Summon_Resistance,i.ToString(),Summon_Spiritual,Summon_Control,inventity,master);
        }


        int[] power = {30};
        int[] type ={0};
        int[] Penetration = {5};
        int[] FPenetration = {0};
        //(弾速,命中式,攻撃回数)
        Unit_List[5].Damage(power,type,transform.position,7,7,200,Penetration,FPenetration);
        Unit_List[5].Damage(power,type,transform.position,7,7,200,Penetration,FPenetration);
        Unit_List[5].Damage(power,type,transform.position,7,7,200,Penetration,FPenetration);
   


    }
}
