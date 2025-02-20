namespace AdaptivePsychophysicsLibrary
{
	public abstract class AdaptiveProcedure
	{
		#region Variables
		public List<double> testedValues = new();
		public List<ResponseState> responses = new();
		public bool AllowDontKnowResponses { get { return _allowDontKnowResponses; } set { _allowDontKnowResponses = value; } }
		private bool _allowDontKnowResponses = true;
		public enum ResponseState
		{
			WrongResp = 0,
			CorrectResp = 1,
			DontKnowResp = 2
		}

		public enum EndCondition
		{
			NumberOfTrials,
			NumberOfInversions
		}
		protected EndCondition endCondition;
		protected int endConditionValue;
		#endregion

		#region Functions
		public void AddResult(ResponseState responseState)
		{
			if (testedValues.Count != responses.Count + 1)
			{
				Console.WriteLine("Error adding result when a new result was not expected");
				return;
			}

			if (responseState == ResponseState.DontKnowResp && !AllowDontKnowResponses)
			{
				Console.WriteLine("Response Don't Know provided when these have been prohibited by the AllowDontKnowResponses variable");
				return;
			}

			responses.Add(responseState);
		}

		public void SetTestedValuesHistory(List<double> testedValues, List<ResponseState> responses)
		{
			if (testedValues.Count != responses.Count)
			{
				Console.WriteLine("Error setting pre-solved tested values, testedValues count does not equal responses count. This function should only be used during initialisation of an adaptive procedure.");
				return;
			}
			this.testedValues = testedValues;
			this.responses = responses;
		}

		public abstract double NextValue();

		public bool IsFinished()
		{
			return endCondition switch
			{
				EndCondition.NumberOfTrials => CheckNumberOfTrialsEndCondition(),
				EndCondition.NumberOfInversions => CheckNumberOfInversionsEndCondition(),
				_ => throw new ArgumentException("Unhandled EndCondition: " + endCondition.ToString()),
			};
		}
		private bool CheckNumberOfTrialsEndCondition()
		{
			if (responses.Count >= endConditionValue)
				return true;
			else return false;
		}
		private bool CheckNumberOfInversionsEndCondition()
		{
			if (GetInversionCount() >= endConditionValue) return true;
			else return false;
		}

		protected int GetInversionCount()
		{
			int inversions = 0;
			bool currentDirection = responses[0] == ResponseState.CorrectResp;  // false is up, true is down
			for (int i = 0; i < responses.Count; i++)
			{
				switch (responses[i])
				{
					case ResponseState.WrongResp:
					case ResponseState.DontKnowResp:
						if (currentDirection)
						{
							inversions++;
							currentDirection = false;
						}
						break;
					case ResponseState.CorrectResp:
						if (!currentDirection)
						{
							inversions++;
							currentDirection = true;
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Unexpected response state: " + responses[i].ToString());
				}
			}

			return inversions;
		}
		#endregion

		#region Result Analysis
		public double GetAverageOfLastTrials(int numberOfTrials)
		{
			if (numberOfTrials >= testedValues.Count)
			{
				return testedValues.Average();
			}

			double sum = 0;
			for (int i = 1; i <= numberOfTrials; i++)
			{
				sum += testedValues[^i];
			}

			return sum / numberOfTrials;
		}
		#endregion
	}
}
