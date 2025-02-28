namespace AdaptivePsychophysicsLibrary.AcceleratedStochasticApproximation
{
	internal class AcceleratedStochasticApproximation : AdaptiveProcedure
	{
		public readonly double initialStepSize;
		public readonly double targetProbability;
		public AcceleratedStochasticApproximation(double initialValue, double initialStepSize, double targetProbability, EndCondition endCondition, int endConditionValue)
		{
			testedValues.Add(initialValue);
			this.initialStepSize = initialStepSize;
			this.targetProbability = targetProbability;
			this.endCondition = endCondition;
			this.endConditionValue = endConditionValue;
		}

		public override double NextValue()
		{
			double _Zn;
			switch (responses[^1])
			{
				case ResponseState.WrongResp:
					_Zn = 0;
					break;
				case ResponseState.CorrectResp:
					_Zn = 1;
					break;
				case ResponseState.DontKnowResp:
					_Zn = 0.5;
					break;
				default:
					_Zn = -100;
					Console.WriteLine($"Unknown ResponseState: {responses[^1]}");
					break;
			}

			double _shiftFactor;
			if (responses.Count > 2)
			{
				_shiftFactor = initialStepSize / (2 + GetInversionCount());
			}
			else
			{
				_shiftFactor = initialStepSize / responses.Count;
			}

			double nextValue = testedValues[^1] - _shiftFactor * (_Zn - targetProbability);

			testedValues.Add(nextValue);

			return nextValue;
		}
	}
}
