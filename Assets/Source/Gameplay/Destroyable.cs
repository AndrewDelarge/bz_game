using System;
using UnityEngine;

namespace game.Gameplay {
	public class Destroyable : Healthable {
		[SerializeField] protected Collider _baseCollider;
		[SerializeField] protected Rigidbody _baseRigidbody;

		[SerializeField] protected GameObject _baseObject;
		[SerializeField] protected GameObject _destroyedObject;
		
		public override void Init() {
			if (_baseCollider == null) {
				_baseCollider = GetComponent<Collider>();
			}
			
			if (_baseRigidbody == null) {
				_baseRigidbody = GetComponent<Rigidbody>();
			}
			
			_baseObject.SetActive(true);
			_destroyedObject.SetActive(false);
			
			base.Init();
			
			die.Add(OnDie);
		}

		private void OnDestroy() {
			die.Remove(OnDie);
		}

		private void OnDie() {
			_baseObject.SetActive(false);
			_destroyedObject.SetActive(true);
			
			_baseCollider.enabled = false;
			_baseRigidbody.isKinematic = true;
		}
	}
}