﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public FloatReference maxHP;
	public FloatReference currentHP;
	public FloatReference attack;
	public FloatReference defense;

	public System.Action Die;

	public Animator animator;

	public Spell[] spells;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
		/* 
		if (Input.GetKeyDown(KeyCode.D)){
			animator.SetBool("Dead", true);
		}*/

	}

	public void HandleSpell(Spell spell, Character caster) {

		float attackBoost = caster.attack.value;
		float defenseBoost = caster.defense.value;
		float toDeal = spell.baseDamage + attackBoost - defenseBoost;
		if (toDeal < 0) toDeal = 0;
		StartCoroutine (DamageAfterSeconds(toDeal, spell.getDuration()));
	}

	// At this point, all calculations are done
	public void TakeDamage(float dmg){
		currentHP.variable.value -= dmg;
		if (currentHP.value < 0){
			currentHP.variable.value = 0; //minimum is zero
		}
	}

	IEnumerator DamageAfterSeconds(float dmg, float secs){
		yield return new WaitForSeconds(secs);
		TakeDamage(dmg);
		CheckIfDead();
		yield break;
	}

	public void CheckIfDead(){
		//Reminder: Action != null checks if there are listeners
		if (currentHP.value <= 0 && Die != null){
			Die();
			animator.SetBool("Dead", true);
		}
	}
}
