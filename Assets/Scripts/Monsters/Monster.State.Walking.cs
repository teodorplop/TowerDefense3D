﻿using UnityEngine;
using Pathfinding;

public partial class Monster {
	[SerializeField]
	private float _movementSpeed;
	[SerializeField]
	private float _turnSpeed;
	
	private int _waypointIndex;
	private Path _waypointsPath;

	void Walking_FixedUpdate() {
		Vector3 position = transform.position - owner.WorldOffset;
		Vector2 position2D = new Vector2(position.x, position.z);

		if (_waypointsPath.turnBoundaries[_waypointIndex].HasCrossedLine(position2D)) {
			++_waypointIndex;

			if (_waypointIndex == _waypointsPath.turnBoundaries.Length) {
				++_pathIndex;
				SetState(MonsterState.Idle);
				return;
			}
		}
		
		Quaternion targetRotation = Quaternion.LookRotation(_waypointsPath.waypoints[_waypointIndex] - position);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _turnSpeed);
		transform.Translate(Vector3.forward * Time.fixedDeltaTime * _movementSpeed);
	}

	void Walking_OnDrawGizmos() {
		if (_debug) {
			for (int i = _waypointIndex; i < _waypointsPath.waypoints.Length; ++i) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(_waypointsPath.waypoints[i], Vector3.one);

				if (i == _waypointIndex) {
					Gizmos.DrawLine(transform.position, _waypointsPath.waypoints[i] + owner.WorldOffset);
				} else {
					Gizmos.DrawLine(_waypointsPath.waypoints[i - 1] + owner.WorldOffset, _waypointsPath.waypoints[i] + owner.WorldOffset);
				}
			}
		}
	}
}
