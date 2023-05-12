using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   public static LevelManager main;
   public Transform startPoint;
   public Transform[] path;
   public int startCurrency = 100;
   
   public int currency;

   public void Start(){
      currency = startCurrency;
   }

   private void Awake(){
        main = this;
   }

   public void IncreaseCurrency(int amount){
      currency +=amount;
   }
   public bool SpendCurrency(int amount){
      if(amount <= currency){
         currency -= amount;
         return true;
      }else{
         Debug.Log("U haven`t money");
         return false;
      }
   }
}
