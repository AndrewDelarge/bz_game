namespace game.Gameplay {
	public struct HealthChange<T> {
		public T type { get; }
		public float value { get; private set; }

		public HealthChange(float value, T type) {
			this.type = type;
			this.value = value;
		}

		public void ChangeValue(float value) {
			this.value = value;
		}
	}
}