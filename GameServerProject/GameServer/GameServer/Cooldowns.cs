using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
	
    public class Cooldowns
    {
		Dictionary<string, DateTime> _cooldowns = new Dictionary<string, DateTime>();

		public void AddCooldownInSeconds(string cd_name, double seconds)
		{
			if (_cooldowns.ContainsKey(cd_name))
				return;

			_cooldowns.Add(cd_name, DateTime.Now + TimeSpan.FromSeconds(seconds));
		}

		public void SetCooldownInSeconds(string cd_name, double seconds)
		{
			if (_cooldowns.ContainsKey(cd_name))
				_cooldowns.Remove(cd_name);

			//Console.WriteLine("Setting cooldown");
			_cooldowns.Add(cd_name, DateTime.Now + TimeSpan.FromSeconds(seconds));
		}

		public bool isCooldownReady(string cd_name)
		{

			if (!_cooldowns.ContainsKey(cd_name) || (_cooldowns[cd_name] - DateTime.Now).TotalMilliseconds <= 0)
			{
				//cooldown is finnished
				_cooldowns.Remove(cd_name);
				return true;
			}
			return false;
		}

		public double GetCdInSeconds(string cd_name)
		{
			double left = 0;
			if (_cooldowns.ContainsKey(cd_name))
			{
				left = (_cooldowns[cd_name] - DateTime.Now).TotalSeconds;
			}

			return left;
		}

		public void removeCooldown(string cd_name)
		{
			_cooldowns.Remove(cd_name);
		}
	}
}
