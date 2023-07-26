using System;
using System.Collections.Generic;
using game.core.common;

namespace game.Source.core.Common {
	public class GameTimer : ICoreManager {
		private Dictionary<int, Action> _actions = new ();
		private Dictionary<int, float> _timings = new ();
		
		private Dictionary<int, float> _newTimers = new ();
		private List<int> _oldTimers = new ();

		private float _time;
		private int _lastId;
		
		public void Update(float deltaTime) {
			CleanUp();
			AddAll();
			
			_time += deltaTime;

			foreach (var (id, time) in _timings) {
				if (time <= _time) {
					_actions[id].Invoke();
					_oldTimers.Add(id);
				}
			}
		}

		public int SetTimeout(float time, Action callback) {
			_lastId++;
			
			_newTimers.Add(_lastId, _time + time);
			_actions.Add(_lastId, callback);

			return _lastId;
		}

		public void KillTimeout(int timerId) {
			_oldTimers.Add(timerId);
		}

		private void AddAll() {
			foreach (var (id, time) in _newTimers) {
				_timings.Add(id, time);
			}
			
			_newTimers.Clear();
		}

		private void CleanUp() {
			foreach (var oldTimer in _oldTimers) {
				if (_actions.ContainsKey(oldTimer)) {
					_actions.Remove(oldTimer);
				}
				if (_timings.ContainsKey(oldTimer)) {
					_timings.Remove(oldTimer);
				}
			}
		}
	}
}