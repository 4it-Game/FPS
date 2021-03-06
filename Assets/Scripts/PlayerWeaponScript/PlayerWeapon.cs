﻿using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

	public string name = "AK83";
	public int damage = 1;
	public float range = 200f;
	public enum FireMode{ Auto, Burst, Single};
	public FireMode fireMode;
	public int burstCount;
	public float nextShotTime;
	public GameObject weaponModel;

}
