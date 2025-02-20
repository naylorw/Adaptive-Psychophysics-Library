namespace AdaptivePsychophysicsLibrary.WeightedUpDown
{
	internal class WeightedUpDown : AdaptiveProcedure
	{
		public readonly double targetProbability;
		public readonly double stepSize;
		public WeightedUpDown(double initialValue, double stepSize, double targetProbability, EndCondition endCondition, int endConditionValue)
		{
			testedValues.Add(initialValue);
			this.stepSize = stepSize;
			this.targetProbability = targetProbability;
			this.endCondition = endCondition;
			this.endConditionValue = endConditionValue;
		}

		public override double NextValue()
		{
			if (testedValues.Count > responses.Count)
			{
				return testedValues[^1];
			}

			double Zn;
			switch (responses[^1])
			{
				case ResponseState.CorrectResp:
					Zn = 1;
					break;
				case ResponseState.WrongResp:
					Zn = 0;
					break;
				case ResponseState.DontKnowResp:
					Zn = 0.5;
					break;
				default:
					throw new ArgumentException();
			}

			double stepModifier = Zn - targetProbability;
			// Rescale the step sizes so the smaller step size is equal to the provided step size
			if (Math.Abs(1-targetProbability) < Math.Abs(0 - targetProbability))
			{
				stepModifier /= Math.Abs(1 - targetProbability);
			}
			else
			{
				stepModifier /= Math.Abs(0 - targetProbability);
			}

			double nextValue = testedValues[^1] - stepSize * stepModifier;

			testedValues.Add(nextValue);

			return nextValue;
		}
	}
}
