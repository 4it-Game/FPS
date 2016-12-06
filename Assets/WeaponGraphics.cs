using UnityEngine;

public class WeaponGraphics : MonoBehaviour {

	public ParticleSystem muzzleFlash;
	public GameObject hitEfectPrefab;
	[Header("Weapon Efects")]
	public Transform shell;
	public Transform shellEjection;
	[Header("Weapon Bullet Efects")]
	public Transform[] projectileSpawn;
	public Projectile projectile;
}
