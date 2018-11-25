using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class MyGun : MonoBehaviour {
	private float damage = 20f;
	
	private float range = 50f;
	
	private bool isAutomatic = true;
	
	private float fireRate = 0.1f;

	private const float defaultSpread = 0.01f;

	private float spread;

	private float nextShoot;

	[SerializeField]
	private ParticleSystem parSys;

	[SerializeField]
	private GameObject bulletHole;

	[SerializeField]
	private Camera cam;

	//Recoiling
	private Transform recoilMod;

	private GameObject weapon;

	private float maxRecoil_x = -10f;

	private float recoilSpeed = 6f;

	private float recoil = 0.0f;

	private const float maxRecoil = 1f;

	private Vector3 stdPos; 
	
	//Ammo and reloading

	private int maxAmmo = 30;

	private int curAmmo;

	private float reloadTime = 1f;

	bool isReloading = false;

	[SerializeField]
	Text ammoText;

	[SerializeField]
	AudioClip reloadSound;

	AudioSource audioSource;

	[SerializeField]
	int enemyLayer = 10;

	[SerializeField]
	Transform reticle;

	Transform crossTop;
	Transform crossBottom;
	Transform crossLeft;
	Transform crossRight;

	float reticleStartPoint;

	[SerializeField]
	LayerMask layerMask;

	void Start () {
		nextShoot = Time.time;
		recoilMod = GetComponent<Transform>();
		weapon = gameObject;
		spread = defaultSpread;
		stdPos = gameObject.transform.localPosition;

		crossTop = reticle.transform.Find("Cross/Top").transform;
		crossBottom = reticle.transform.Find("Cross/Bottom").transform;
		crossLeft = reticle.transform.Find("Cross/Left").transform;
		crossRight = reticle.transform.Find("Cross/Right").transform;

		reticleStartPoint = crossTop.localPosition.y;

		curAmmo = maxAmmo;

		ammoText.text = curAmmo + "/" + maxAmmo;

		audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		spread = defaultSpread + 10f * recoil;
		Recoiling();

		if (isReloading)
			return;
		
		if (Input.GetKeyDown(KeyCode.R) && curAmmo < maxAmmo) {
			StartCoroutine(Reload());
		}
	}

	public void Shoot() {

		if (isReloading)
			return;

		if (curAmmo <= 0) {
			StartCoroutine(Reload());
			return;
		}

		if (nextShoot <= Time.time) {
			--curAmmo;
			ammoText.text = curAmmo + "/" + maxAmmo;
			RaycastHit hit;
			Vector3 dir = cam.transform.forward;
			dir.x += (Random.value - 0.5f) * spread;
			dir.y += (Random.value - 0.5f) * spread;
			dir.z += (Random.value - 0.5f) * spread;
			
			if (Physics.Raycast(cam.transform.position, dir, out hit, range)) {
				if (hit.collider.gameObject.layer == enemyLayer) {
					hit.collider.gameObject.GetComponent<Character>().Hit(damage);
				}

				if (bulletHole != null && hit.transform.tag != "Mob") {
					Destroy(Instantiate(bulletHole, hit.point, 
					Quaternion.FromToRotation(Vector3.up, hit.normal)), 60f);
				}
			}

			GetComponent<AudioSource>().Play();
			parSys.Play();
			
			nextShoot = Time.time + fireRate;

			if (recoil < maxRecoil)
				recoil += 0.1f;
		}
	}

	void Recoiling () {
        if (recoil > 0f) {
            Quaternion maxRecoil = Quaternion.Euler (maxRecoil_x, 0f, 0f);
			Vector3 endPos = stdPos - 0.15f * Vector3.forward;
            //transform.localRotation = Quaternion.Slerp (transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
			transform.localPosition = Vector3.Slerp (transform.localPosition, endPos, Time.deltaTime * recoilSpeed);
			CrossRecoiling();
            recoil -= Time.deltaTime;

        } else {
            recoil = 0f;
            //transform.localRotation = Quaternion.Slerp (transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
			transform.localPosition = Vector3.Slerp (transform.localPosition, stdPos, Time.deltaTime * recoilSpeed);
			CrossRecoilingBack();
        }
	}

	void CrossRecoiling () {
		Vector3 topEnd = Vector3.up * spread * 200f;
		Vector3 bottomEnd = Vector3.down * spread * 200f;
		Vector3 leftEnd = Vector3.left * spread * 200f;
		Vector3 rightEnd = Vector3.right * spread * 200f; //crossRight.localPosition + 
		crossTop.localPosition = Vector3.Lerp(crossTop.localPosition, topEnd, recoilSpeed * Time.deltaTime);
		crossBottom.localPosition = Vector3.Lerp(crossBottom.localPosition, bottomEnd, recoilSpeed * Time.deltaTime);
		crossLeft.localPosition = Vector3.Lerp(crossLeft.localPosition, leftEnd, recoilSpeed * Time.deltaTime);
		crossRight.localPosition = Vector3.Lerp(crossRight.localPosition, rightEnd, recoilSpeed * Time.deltaTime);
	}

	void CrossRecoilingBack () {
		Vector3 topEnd = Vector3.up * reticleStartPoint;
		Vector3 bottomEnd = Vector3.down * reticleStartPoint;
		Vector3 leftEnd = Vector3.left * reticleStartPoint;
		Vector3 rightEnd = Vector3.right * reticleStartPoint;
		crossTop.localPosition = Vector3.Lerp(crossTop.localPosition, topEnd, recoilSpeed * Time.deltaTime);
		crossBottom.localPosition = Vector3.Lerp(crossBottom.localPosition, bottomEnd, recoilSpeed * Time.deltaTime);
		crossLeft.localPosition = Vector3.Lerp(crossLeft.localPosition, leftEnd, recoilSpeed * Time.deltaTime);
		crossRight.localPosition = Vector3.Lerp(crossRight.localPosition, rightEnd, recoilSpeed * Time.deltaTime);
	}

	IEnumerator Reload() {
		isReloading = true;

		Debug.Log("Reloading...");
		
		audioSource.PlayOneShot(reloadSound);

		yield return new WaitForSeconds(reloadTime);

		curAmmo = maxAmmo;

		ammoText.text = curAmmo + "/" + maxAmmo;

		isReloading = false;
	}
}
