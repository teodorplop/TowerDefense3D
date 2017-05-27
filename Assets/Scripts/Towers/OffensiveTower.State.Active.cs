﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class OffensiveTower {
	private float _attackTimer;
	private float _timeBetweenAttacks;
	private Monster _target;

	protected IEnumerator Active_EnterState() {
		_attackTimer = _timeBetweenAttacks = 1.0f / _attackSpeed;
		yield return null;
	}

	protected virtual void OnAttack(Monster target) {
		_attackTimer = _timeBetweenAttacks;
	}
	private bool TargetIsStillValid() {
		return _target != null && !_target.IsDead && !_target.ReachedDestination && Vector3.Distance(_target.transform.position, transform.position) <= _radius;
	}
	protected virtual void UpdateTarget() {
		if (!TargetIsStillValid()) {
			_target = null;

			List<Monster> monsters = owner.GetMonstersInRange(transform.position, _radius);
			if (monsters.Count > 0) {
				float distance = Mathf.Infinity;
				foreach (Monster monster in monsters) {
					float monsterDistance = Vector3.Distance(transform.position, monster.transform.position);
					if (monsterDistance < distance) {
						distance = monsterDistance;
						_target = monster;
					}
				}
			}
		}
	}

	protected void Active_FixedUpdate() {
		_attackTimer = Mathf.Max(_attackTimer - Time.deltaTime, 0.0f);
		if (_attackTimer <= 0.0f) {
			UpdateTarget();
			if (_target) {
				OnAttack(_target);
			}
		}
	}

	protected void Active_OnDrawGizmos() {
		if (_debug) {
			Gizmos.DrawWireSphere(transform.position, _radius);
		}
	}
}
