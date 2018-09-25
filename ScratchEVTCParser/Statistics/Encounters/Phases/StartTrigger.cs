using ScratchEVTCParser.Events;

namespace ScratchEVTCParser.Statistics.Encounters.Phases
{
	public class StartTrigger : IPhaseTrigger
	{
		public string PhaseName { get; }

		public StartTrigger(string phaseName)
		{
			PhaseName = phaseName;
		}

		public bool IsTrigger(Event e) => e is LogStartEvent;
	}
}