using System;
using UnityEngine;

namespace game.Source.Gameplay {
	public class Destroyable : Healthable {
		[SerializeField] protected GameObject _baseObject;
		[SerializeField] protected GameObject _destroyedObject;


		public override void Init() {
			_baseObject.SetActive(true);
			_destroyedObject.SetActive(false);
			
			base.Init();
			
			die += OnDie;
		}

		private void OnDestroy() {
			die -= OnDie;
		}

		private void OnDie() {
			_baseObject.SetActive(false);
			_destroyedObject.SetActive(true);
		}
	}
}