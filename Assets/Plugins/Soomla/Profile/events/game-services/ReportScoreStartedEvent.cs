using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class ReportScoreStartedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard Destination;

		public ReportScoreStartedEvent(Provider provider, Leaderboard destination, string payload) : base(provider, payload)
		{
			this.Destination = destination;
		}
	}
}