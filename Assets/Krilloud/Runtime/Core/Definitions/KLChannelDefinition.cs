using System;
using Newtonsoft.Json;

namespace KrillAudio.Krilloud.Definitions
{
	[Serializable]
	public sealed class KLChannelDefinition
	{
		[JsonProperty("channel_name")]
		public string name;

		[JsonProperty("parent_id")]
		public int parent;

		[JsonProperty("volume")]
		public float volume;

		public KLChannelDefinition()
		{
		}

		public KLChannelDefinition(KLChannelDefinition other)
		{
			this.name = other.name;
			this.parent = other.parent;
			this.volume = other.volume;
		}
	}
}