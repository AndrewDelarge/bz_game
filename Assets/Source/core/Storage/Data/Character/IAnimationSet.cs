using System;
using UnityEngine;

namespace game.core.Storage.Data.Character {
	public interface IAnimationSet<T> where T : Enum
	{
		public AnimationData<T> GetAnimationData(T id);
	}
}