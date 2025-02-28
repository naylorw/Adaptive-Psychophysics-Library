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

			AllowDontKnowResponses = false;
		}

		public override double NextValue()
		{
			if (testedValues.Count > responses.Count)
			{
				return testedValues[^1];
			}

			double Zn = responses[^1] switch
			{
				ResponseState.CorrectResp => 1,
				ResponseState.WrongResp => 0,
				_ => throw new ArgumentException(),
			};
			double stepModifier = 2 * Zn - 1;
			double nextValue = testedValues[^1] - stepSize * stepModifier;

			testedValues.Add(nextValue);

			return nextValue;
		}
	}
}
