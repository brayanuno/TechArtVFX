using System;
using System.Reflection;
using UnityEngine;


namespace Brayan_CSharpCourse
{
    
    public class Program_Reflection : MonoBehaviour
    {
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            /*GrabFunction();*/
            Exercise exercise = new Exercise();
            Type playerType = typeof(Exercise).GetNestedType("Player");

            MethodInfo myFunctionmethodInfo = playerType.GetMethod("PlayerMethod");
            myFunctionmethodInfo.Invoke(new Exercise.Player(), new object[] { });
            /*foreach (Type type in typeof(Exercise).GetNestedTypes(BindingFlags.NonPublic ))
            {
                Debug.Log(type);
            }*/



        }

        void GrabFunction()
        {    
            Exercise exercise = new Exercise();
            /*exercise.MyFunction();*/
            
            MethodInfo methodInfo = typeof(Exercise).GetMethod("MyFunction");
            methodInfo.Invoke(exercise, new object[] { });

            
        }

        public void w()
        {
          
        }
        
        // Update is called once per frame
        void Update()
        {
       
        }
    }

    public class Exercise
    {
        public class Player
        {
            public void PlayerMethod()
            {
                Debug.Log("PlayerMethod");
            }
        }
        public class Generic
        {
            
        }

        private class Enemy
        {
            
        }
    }
        

}
