using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenuAttribute]
public class CalenStats : ScriptableObject{
	public float defaultGrav, defaultDrag, airDrag;
	public float walkForceGrounded;
	public float footGravForce;
	public float rotateForceGround, rotateForceAir;
	public float maxTorque;

	public float jumpForce, jumpForceBig, bigJumpDelay, groundRaycastDistance, maxJumpAngle;

	public float interactionRange;

	public float shotgunKnockback, shotgunKnockbackUp, shootFreezeTime;
	
}
