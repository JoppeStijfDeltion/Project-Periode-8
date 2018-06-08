using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

	public IEnumerator DestroySelf(float _DestroyTimer) {
		yield return new WaitForSeconds(_DestroyTimer);
		Destroy(gameObject);
	}
}
