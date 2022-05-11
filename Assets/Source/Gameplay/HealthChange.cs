namespace game.Source.Gameplay {
	public struct HealthChange<T> {
		public T type { get; }
		public float value { get; }

		public HealthChange(float value, T type) {
			this.type = type;
			this.value = value;
		}
	}
}