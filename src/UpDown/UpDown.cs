namespace AdaptivePsychophysicsLibrary.UpDown
{
	public class UpDown : AdaptiveProcedure
	{
		public readonly double stepSize;

		public UpDown(double initialValue, double stepSize, EndCondition endCondition, int endConditionValue)
		{
			testedValues.Add(initialValue);
			this.stepSize = stepSize;
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

			double stepModifier = 2 * Zn - 1;
			double nextValue = testedValues[^1] - stepSize * stepModifier;

			testedValues.Add(nextValue);

			return nextValue;
		}
	}
}
