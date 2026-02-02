using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimationController : MonoBehaviour
{
 enum AnimationState{
        idle,
        moveSword,
        punch,
        attackWithSword,
        danceSword,
        punchEarth,
        tPose,
        restingFromPunch,
        attackWithSwordNoEffect,
        rest,
    }


    Animator m_animator;
    SpriteRenderer m_sprite;
    

    public static BodyAnimationController sharedInstance;
        GameObject m_sword;


    private void Awake() {
        sharedInstance = this;
        m_animator=GetComponent<Animator>();
        m_sprite = GetComponent<SpriteRenderer>();

        m_sword = GameObject.FindGameObjectWithTag("Sword"); 
    }


    private void PlayState(AnimationState state)
    {
        string animName = string.Empty;
        switch (state)
        {

            case AnimationState.moveSword:
                animName = "moveSword" ;   
                break;
            case AnimationState.punch:
                animName = "punch";   
                break;
            case AnimationState.attackWithSword:
                animName = "attackWithSword";
                break;
            case AnimationState.danceSword:
                animName = "danceSword";
                break;
            case AnimationState.punchEarth:
                animName = "punchEarth";
                break;            
            case AnimationState.tPose:
                animName = "tPose";
                break;
            case AnimationState.restingFromPunch:
                animName = "RrestingFrompunchEarth";
                break;
            case AnimationState.attackWithSwordNoEffect:
                animName = "attackWithSwordNoEffect";
                break;
            case AnimationState.rest:
                animName = "Rest";
                break;
            default:
                animName = "idle";
                break;

        }
        m_animator.Play(animName);
    }

    public void animPlayAttackSwordNoEffect()
    {
        PlayState(AnimationState.attackWithSwordNoEffect);
    }
    
    
    public void animPlaDanceSword(){
        PlayState(AnimationState.danceSword);
    }

    public void animRestAttackPunchEarth()
    {
        PlayState(AnimationState.restingFromPunch);
    }

    public void animPlayMoveSword(){
        PlayState(AnimationState.moveSword);
    }

    public void animPlayPunch(){
        PlayState(AnimationState.punch);
    }

    
    /*public void animPlaySecondPunch(){
        //PlayState(AnimationState.punch);
        m_sprite.flipX = true;
        StartCoroutine(GoIdle());
    }*/

    public void animPlayAttackSword(){
        PlayState(AnimationState.attackWithSword);
    }

    public void animPlayAttackPunchEarth(){
        PlayState(AnimationState.punchEarth);
    }

    public void animPlaytPose(){
        PlayState(AnimationState.tPose);
    }



    public void animPlayIdle(){
        PlayState(AnimationState.idle);
    }
    
    public void animPlayRest(){
        PlayState(AnimationState.rest);
    }

}
