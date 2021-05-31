using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloater : MonoBehaviour
{

	private void OnTriggerStay(Collider coll)
	{
		if (!coll.isTrigger && coll.attachedRigidbody != null)
		{
			coll.attachedRigidbody.AddForce(Physics.gravity * -1.1f);
			coll.attachedRigidbody.AddForce(coll.attachedRigidbody.velocity * -1f);
		}
	}

}
