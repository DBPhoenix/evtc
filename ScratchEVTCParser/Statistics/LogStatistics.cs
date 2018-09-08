using System.Collections.Generic;
using System.Linq;
using ScratchEVTCParser.Model.Agents;

namespace ScratchEVTCParser.Statistics
{
	public class LogStatistics
	{
		public long FightTimeMs { get; }
		public float BossDps { get; }
		public float BossConditionDps { get; }
		public float BossPhysicalDps { get; }

		public LogStatistics(long fightTimeMs, float bossDps, float bossConditionDps, float bossPhysicalDps)
		{
			FightTimeMs = fightTimeMs;
			BossDps = bossDps;
			BossConditionDps = bossConditionDps;
			BossPhysicalDps = bossPhysicalDps;
		}
	}
}