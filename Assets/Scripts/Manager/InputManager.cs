using System.Collections;
using System.Collections.Generic;
using nicorueda.Player;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public void OnAttackMelee(InputAction.CallbackContext context)
    {
        if (context.started) PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.AttackMelee);
    }

    public void OnAttackDistance(InputAction.CallbackContext context)
    {
        if (context.started)
        {

                PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Pointing);
            
        }

        if (context.canceled)
            {
                
                PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.AttackDistance);
            }
    }


    public void OnMove(InputAction.CallbackContext value){
        Vector2 movement = value.ReadValue<Vector2>();
        PlayerMovement.instance.MoveLogicMethod(movement);
        //if (value.started)
        

            //PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Running);

            if ((movement.magnitude > 0.5))
            {
                if (PlayerAnimation.m_State != PlayerAnimation.AnimationState.Running)
                PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Walking);
            } 

        else PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Idle);
    }

    public void OnRun(InputAction.CallbackContext context){
        if (context.started)
        {

            PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Running);
        }
            //PlayerManager.instance.DuplicateVelocity();

            if (context.canceled)
            {

                PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Walking);
            }
            //PlayerManager.instance.SplitVelocity();
        
    }
    
    
    public void OnSprint(InputAction.CallbackContext context){
        if(context.started)  PlayerAnimation.instance.changeState(PlayerAnimation.AnimationState.Sprint);
    }
    

    /*public void OnGodMode(InputAction.CallbackContext context){
        Debug.Log("God mode");
        if (context.started)
        {
            Debug.Log("GOD MODE");
            PlayerManager.instance.GodMode();
        }
    }*/
    
    


}
