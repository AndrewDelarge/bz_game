namespace game.Gameplay {
	public abstract class Resource<T> {
		protected T _value;

		public T value => _value;
		
		public abstract bool Increase(T value);

		public abstract bool Reduce(T value);
	}
}