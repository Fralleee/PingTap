using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.PingTap
{
	[RequireComponent(typeof(Weapon))]
	public class RecoilAddon : MonoBehaviour
	{
		[Header("Kickback")]
		[SerializeField] float kickbackForce = 0.15f;
		[FormerlySerializedAs("recoverTime")] [SerializeField] float kickbackRecoverTime = 20f;

		[Header("RecoilAddon")]
		[SerializeField] bool randomizeRecoil = false;
		[SerializeField] float recoilSpeed = 15f;
		[SerializeField] float recoilRecoverTime = 10f;
		[SerializeField] Vector3 randomRecoilConstraints = Vector3.zero;
		[SerializeField] Vector3[] recoilPattern = new Vector3[0];

		int recoilPatternStep;
		Weapon weapon;
		Vector3[] startPositions;
		Vector3 recoil;
		int nextMuzzle;
		float recoilStatMultiplier = 1f;

		void Awake()
		{
			weapon = GetComponent<Weapon>();

			startPositions = weapon.Muzzles.Select(x => x.parent.localPosition).ToArray();
		}

		void Update()
		{
			if (!weapon.IsEquipped)
				return;
			if (kickbackForce <= 0)
				return;

			for (var i = 0; i < startPositions.Length; i++)
			{
				weapon.Muzzles[i].parent.localPosition = Vector3.Lerp(weapon.Muzzles[i].parent.localPosition, startPositions[i], kickbackRecoverTime * Time.deltaTime);
			}

			var toRotation = Quaternion.Euler(recoil.y, recoil.x, recoil.z);
			weapon.Combatant.AimTransform.localRotation = Quaternion.RotateTowards(weapon.Combatant.AimTransform.localRotation, toRotation, recoilSpeed * Time.deltaTime);
			recoil = Vector3.Lerp(recoil, Vector3.zero, recoilRecoverTime * Time.deltaTime);
		}

		public void AddRecoil()
		{

			AddKickback();

			if (randomizeRecoil)
			{
				var xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
				var yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);
				var zRecoil = Random.Range(-randomRecoilConstraints.z, randomRecoilConstraints.z);
				recoil += new Vector3(xRecoil, yRecoil, zRecoil) * recoilStatMultiplier;
			}
			else if (recoilPattern.Length > 0)
			{
				if (recoilPatternStep > recoilPattern.Length - 1)
					recoilPatternStep = 0;
				recoil += recoilPattern[recoilPatternStep] * recoilStatMultiplier;
				recoilPatternStep++;
			}
		}

		void AddKickback()
		{
			if (kickbackForce <= 0)
				return;

			var muzzle = GetMuzzle();
			muzzle.parent.localPosition -= new Vector3(0, 0, kickbackForce);
		}

		Transform GetMuzzle()
		{
			var muzzle = weapon.Muzzles[nextMuzzle];
			if (weapon.Muzzles.Length <= 1)
				return muzzle;

			nextMuzzle++;
			if (nextMuzzle > weapon.Muzzles.Length - 1)
				nextMuzzle = 0;

			return muzzle;
		}
	}
}
