using System;
using System.Collections.Generic;
using ScratchEVTCParser.Events;
using ScratchEVTCParser.Model.Agents;

namespace ScratchEVTCParser.Statistics.Encounters
{
	public class PhaseSplitter
	{
		private readonly Agent boss;
		private readonly IPhaseTrigger[] triggers;

		public PhaseSplitter(Agent boss, params IPhaseTrigger[] triggers)
		{
			if (triggers.Length == 0)
			{
				throw new ArgumentException("At least one phase trigger must be provided", nameof(triggers));
			}

			this.boss = boss;
			this.triggers = triggers;
		}

		/// <summary>
		/// Split events into phases, each phase starting with a trigger.
		/// Events prior to the first trigger are not put into any phase.
		/// </summary>
		/// <param name="events"></param>
		/// <returns>The detected phases</returns>
		public IEnumerable<Phase> GetEventsByPhases(IReadOnlyList<Event> events)
		{
			var eventsInPhase = new List<Event>();
			IPhaseTrigger previousTrigger = null;

			int currentTriggerIndex = 0;
			var currentTrigger = triggers[currentTriggerIndex];

			for (int i = 0; i < events.Count; i++)
			{
				var e = events[i];

				if (currentTrigger != null && currentTrigger.IsTrigger(e))
				{
					if (previousTrigger != null)
					{
						var currentPhaseStart = eventsInPhase[0]; // The previous triggering event
						yield return new Phase(currentPhaseStart.Time, e.Time, currentTriggerIndex,
							previousTrigger.PhaseName, eventsInPhase);
					}

					previousTrigger = currentTrigger;
					eventsInPhase.Clear();
					if (currentTriggerIndex + 1 < triggers.Length)
					{
						currentTrigger = triggers[currentTriggerIndex++];
					}
					else
					{
						currentTrigger = null;
					}
				}

				eventsInPhase.Add(e);
			}

			if (previousTrigger != null)
			{
				var currentPhaseStart = eventsInPhase[0]; // The previous triggering event
				yield return new Phase(currentPhaseStart.Time, events[events.Count - 1].Time, currentTriggerIndex,
					previousTrigger.PhaseName, eventsInPhase);
			}
		}
	}
}