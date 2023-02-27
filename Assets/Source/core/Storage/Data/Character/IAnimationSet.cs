using System;
using UnityEngine;

namespace game.core.Storage.Data.Character {
	public interface IAnimationSet<T, T2> where T : Enum
	{
		public IAnimationData<T, T2> GetAnimationData(T id);
	}
}