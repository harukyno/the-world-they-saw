using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace War_System{
    
    public class Combat_Unit_Definition : MonoBehaviour
    {//変数の用意
    int Unit_Number;
    int[] Unit_Attack_Vector;
    int[] Unit_Siz;
    float Unit_Width;
    int Unit_Type;
    string[] Unit_Skill;
    int[] Unit_Status;
    int[] Unit_Status_Plus;
    string [] Unit_Have_Unit;
    int[] Unit_Have_Unit_Number;
    string[] Unit_Have_Ammunition;
    int[] Unit_Have_Ammunition_Number;
    float [] Unit_Resistance;
    float [] Unit_Resistance_Alpha;
    string Unit_Name;
    float [] Spiritual;
    short Control;
    int master;
    float[] inventory;
    int[] Unit_Skill_Num;
    float density;

    int Power_S;
    
    


    public void Unit_Setup(int[] Summon_Unit_Siz,int Summon_Number,float Summon_Width,int[] Summon_Status,string[] Summon_Have_Unit,int[] Summon_Have_Unit_Number,string[] Summon_Have_Ammunition,
    int[] Summon_Have_Ammunition_Number,int Summon_Unit_Type,string[] Summon_Skill,float[] Summon_Resistance,string Summon_Name,float[] Summon_Spiritual,short Summon_Control,float[] Summon_Inventory,int Summon_Master) //召喚機構
        {
      
        //召喚時のユニット数
        Unit_Number = Summon_Number.DeepCopy();
        
        //召喚するときの大きさ
        Unit_Siz = Summon_Unit_Siz.DeepCopy();
        //召喚するときの単体幅
        Unit_Width = Summon_Width.DeepCopy();
        //召喚されるときのステータス
        Unit_Status = Summon_Status.DeepCopy();
        Array.Resize(ref Unit_Status_Plus,109);

        Unit_Status[5] *= Unit_Number;
        Debug.Log("SAN" + Summon_Status[6].ToString());
        Unit_Status[6] *= Unit_Number;
        Unit_Status[7] *= Unit_Number;
        Unit_Status[8] *= Unit_Number;
        Unit_Status[70] *= Unit_Number;

        Unit_Status_Plus[5] *= Unit_Number;
        Unit_Status_Plus[6] *= Unit_Number;
        Unit_Status_Plus[7] *= Unit_Number;
        Unit_Status_Plus[8] *= Unit_Number;
        Unit_Status_Plus[70] *= Unit_Number;


        //ユニットの内包ユニット
        Unit_Have_Unit = Summon_Have_Unit.DeepCopy();
        //ユニットの内包ユニットの数
        Unit_Have_Unit_Number = Summon_Have_Unit_Number.DeepCopy();
        //ユニットの内包弾薬
        Unit_Have_Ammunition = Summon_Have_Ammunition.DeepCopy();
        //ユニットの内包弾薬数
        Unit_Have_Ammunition_Number = Summon_Have_Ammunition_Number.DeepCopy();
        //ユニットタイプ
        Unit_Type = Summon_Unit_Type.DeepCopy();
        //スキル
        Unit_Skill = Summon_Skill.DeepCopy();
        //耐性
        Unit_Resistance = Summon_Resistance.DeepCopy();
        //名前を付ける
        Unit_Name = Summon_Name.DeepCopy();
        //価値観
        Spiritual = Summon_Spiritual.DeepCopy();
        //制御
        Control = Summon_Control.DeepCopy();
        //インベントリ内容
        inventory = Summon_Inventory.DeepCopy();
        //制御権
        master = Summon_Master.DeepCopy();

        //密度計算
        density = (float)Unit_Number/((float)Summon_Width*(float)Unit_Siz[0]*(float)Unit_Siz[1]*(float)Unit_Siz[2]);


        Debug.Log("Summon " + Unit_Name + "数" + Unit_Number.ToString());
        

        }

    // Start is called before the first frame update
    void Start()
    {
        
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //ダメージ処理
    public void Damage(int[] Damage_Power,int[] Damage_Type,Vector3 Attack_Position,int Attack_Speed,int Attack_Aiming,int Attack_Num,int[] Penetration,int[] FPenetration)
    {
        Debug.Log("===ダメージ処理===");
        //戦闘距離を計算
        int Distance;
        Distance = Convert.ToInt32(Math.Pow(
        ( Math.Pow((transform.position.x-Attack_Position.x),2)) +
        ( Math.Pow((transform.position.y-Attack_Position.y),2)) + 
        ( Math.Pow((transform.position.z-Attack_Position.z),2)),0.5)) ;
        //攻撃到着までの時間を計算
        int Damage_Time;
        Damage_Time = Mathf.Clamp((Distance / Attack_Speed) / 6,1,2147483647);
        Debug.Log("所望時間" + Damage_Time.ToString());
        
        


        float morale;
        Debug.Log("数" + Unit_Number.ToString());
        
        morale = (float)(Unit_Status[6]+Unit_Status_Plus[6])/(float)((Unit_Status[2]+Unit_Status_Plus[2])*Unit_Number);
        Debug.Log("士気率" + morale.ToString());

        //命中数を計算
        int Hit_Num;
        //爆発の場合は命中を補正
        float Aim_Puls = 0.0F;
        if(Damage_Type[0] == 3 ){
            Aim_Puls = (float)Math.Pow(Damage_Power[0],1.0/3.0);

        }

        float Aim =  (float)(Aim_Puls + ((float)(Attack_Aiming - (Damage_Time-1)*(Unit_Status[32]+Unit_Status[34]+Unit_Status_Plus[32]+Unit_Status_Plus[34])) * (float)Attack_Num ));
        float Avoidance = (float)((Unit_Status[32]+Unit_Status[34]+Unit_Status_Plus[32]+Unit_Status_Plus[34]) * morale);
        
        Debug.Log("攻撃回数" + Attack_Num.ToString());
        Debug.Log("命中式" + Aim.ToString());
        Debug.Log("回避式" + Avoidance.ToString());
        Hit_Num = (int)(Aim/Avoidance);
        Hit_Num = System.Math.Min(Hit_Num,Attack_Num);
        Debug.Log("命中数" + Hit_Num.ToString());

        //防御の分類と計算
        //防御値の初期化
        int [] Calculation_DEF ={0,0,0,0,0,0,0,0};
        
        //ダメージの準備
        int Power　= 0;
        int SANdmg = 0;
        
        int Damage_Cup = 1;
        

        for(int Wave = 0; Wave < (Damage_Power.Length); Wave++)
        {
            if (Unit_Type == 1){
            //これから書く(車両の処理)

            }else if(Unit_Type == 0){
            //歩兵のユニット

            if(Damage_Type[Wave]  < 5){
            //物理防御
            Calculation_DEF[0] = (Unit_Status[9]+Unit_Status_Plus[9]);
            Calculation_DEF[1] = (Unit_Status[9]+Unit_Status_Plus[9]);
            }else if(Damage_Type[Wave] == 6){
            //超常防御
            Calculation_DEF[0] = (Unit_Status[21]+Unit_Status_Plus[21]);
            Calculation_DEF[1] = (Unit_Status[21]+Unit_Status_Plus[21]);
            }//未実装

            //傾斜装甲値
            Calculation_DEF[2] = (0);
            Calculation_DEF[3] = (0);
            //対流体防御
            Calculation_DEF[4] = (0);
            Calculation_DEF[5] = (0);
            }
            

            Power += Damage_Power[Wave]; 
            Debug.Log(Power);

            
            //ダメージタイプによる火力キャップ
        
        if      (Damage_Type[Wave] == 0 ){
            //打撃
            Damage_Cup = Convert.ToInt32((Unit_Status[1]+Unit_Status_Plus[1])*1);
        }else if(Damage_Type[Wave] == 1 ){
            //斬撃
            Damage_Cup = Convert.ToInt32((Unit_Status[1]+Unit_Status_Plus[1])*0.75);
        }else if(Damage_Type[Wave] == 2 ){
            //貫通
            Damage_Cup = Convert.ToInt32((Unit_Status[1]+Unit_Status_Plus[1])*2);
        }else if(Damage_Type[Wave] == 3 ){
            //衝撃
            Damage_Cup = Convert.ToInt32((Unit_Status[1]+Unit_Status_Plus[1])*0.5);
        }else if(Damage_Type[Wave] == 4 ){
            //圧迫
            Damage_Cup = Convert.ToInt32(Calculation_DEF[0]);
        }else if(Damage_Type[Wave] >= 5 ){
            //エネルギー以上
            Damage_Cup = Power;
        }

        //火力キャップを適応
        if(Power > Damage_Cup){
            Power = Convert.ToInt32(Math.Pow(Power - Damage_Cup,0.5) + Power);
            Debug.Log("火力CUPカバー発生　CUP値" + Damage_Cup.ToString() + "適応値" + Power.ToString());
        }

        //流体貫徹力の処理
        if(FPenetration[Wave] > 1){
             Calculation_DEF[0] = Convert.ToInt32(System.Math.Min(Calculation_DEF[0] - Math.Pow(System.Math.Min((FPenetration[Wave] - Calculation_DEF[4]),0),2),0));
             }

        //跳弾
        if(Penetration[Wave] != 0 & Penetration[Wave]*2 < Calculation_DEF[0] + Calculation_DEF[7]){
            Debug.Log("[跳弾] 貫徹力" + Penetration[Wave].ToString());
            int[] Penetration_Next = {0};
            Penetration_Next[0] = Penetration[Wave]/2;
            int[] Penetration_Next2 = new int[1];
            Penetration_Next2[0] = Penetration[Wave]*3;
            int[] Damage_Type_Next = {0};
            Damage_Type_Next[0] = 0; 
            int[] FPenetration_2 = {0};
            FPenetration_2[0] = 0;
            
            
            Damage(Penetration_Next,Damage_Type_Next,transform.position,1,(Unit_Status[34]+Unit_Status_Plus[34]+Unit_Status[33]+Unit_Status_Plus[34]),1,Penetration_Next2,FPenetration_2);
            break;
            

        }
        

         //過貫通判定
        if(Calculation_DEF[0]*2 < Penetration[Wave]){
            Debug.Log("[過貫通] 貫徹力" + Penetration[Wave].ToString() + "装甲"+ Calculation_DEF[0].ToString() );
            int[] Penetration_Next = {0};
            Penetration_Next[0] = Convert.ToInt32(Math.Pow(Penetration[Wave],2));
            int[] Penetration_Next2 = new int[1];
            Penetration_Next2[0] = Calculation_DEF[0];
            int[] Damage_Type_Next = {0};
            Damage_Type_Next[0] = 3;
            int[] FPenetration_2 = {0};
            FPenetration_2 [0] = 0;
            
            Damage(Penetration_Next,Damage_Type_Next,transform.position,1,(Unit_Status[32]+Unit_Status_Plus[32]+Unit_Status[35]+Unit_Status_Plus[35]),Hit_Num,Penetration_Next2,FPenetration_2);
            break;
            }
        //貫通判定
        Debug.Log("貫徹力" + Penetration[Wave].ToString() + "装甲"+ Calculation_DEF[0].ToString() );
        Calculation_DEF[0] = Mathf.Clamp(Calculation_DEF[0] - Penetration[Wave],0,2147483647);

        Power -= Calculation_DEF[0];

        Debug.Log("威力" + Power.ToString());

        SANdmg += (int)(System.Math.Max((int)(Math.Pow(Power/(Unit_Status[5]/Unit_Number*Spiritual[0])*1000,0.5)) - (Unit_Status[31] + Unit_Status_Plus[31]) ,0.0F));
       
        Debug.Log("精神威力" + SANdmg.ToString());

        
        Debug.Log("密度" + density.ToString());
        Power_S = Power;
        Power *= (int)((float)Hit_Num * Math.Pow(density,0.5)*0.5);
        SANdmg *= (int)((float)Hit_Num * Math.Pow(density,0.5)*0.5);

        Debug.Log("総合威力" + Power.ToString());
        Debug.Log("総合精神威力" + SANdmg.ToString());
        
        
       
        
        }
        Unit_Status[5] -= Power;
        Unit_Status[5] = Mathf.Clamp(Unit_Status[5],0,(Unit_Status[1] + Unit_Status_Plus[1])*Unit_Number);
        Unit_Status[6] -= SANdmg;
        Unit_Status[6] = Mathf.Clamp(Unit_Status[6],0,(Unit_Status[2] + Unit_Status_Plus[2])*Unit_Number);
        

        Debug.Log("残り体力" + Unit_Status[5].ToString() + "/" + ((Unit_Status[1] + Unit_Status_Plus[1])*Unit_Number).ToString());
        Debug.Log("残り精神力" + Unit_Status[6].ToString() + "/" + ((Unit_Status[2] + Unit_Status_Plus[2])*Unit_Number).ToString());
        float die_score = ((float)(Power/(Unit_Status[1] + Unit_Status_Plus[1]))/(float)(4+ Math.Abs(Math.Log(Power_S/Unit_Status[1],4))) * (float)((float)((Unit_Status[1] + Unit_Status_Plus[1])*Unit_Number)/(float)Unit_Status[5])*(float)((float)((Unit_Status[2] + Unit_Status_Plus[2])*Unit_Number)/(float)Unit_Status[6]));
        Debug.Log("死傷数" + die_score.ToString());
        Unit_Number -= (int)die_score;
        Debug.Log("残存数" + Unit_Number.ToString());
        //暫定で書いておく　後で密度変更のやつは書く。
        density = (float)Unit_Number/((float)Unit_Width*(float)Unit_Siz[0]*(float)Unit_Siz[1]*(float)Unit_Siz[2]);
        



    }
    
    }
}