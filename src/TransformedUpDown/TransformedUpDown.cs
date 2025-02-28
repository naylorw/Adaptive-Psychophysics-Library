namespace AdaptivePsychophysicsLibrary.TransformedUpDown
{
	internal class TransformedUpDown : AdaptiveProcedure
	{
		public readonly double stepSize;
		public readonly int stepsToGoUp, stepsToGoDown;
		public TransformedUpDown(double initialEstimate, double stepSize, int stepsToGoUp, int stepsToGoDown)
		{
			testedValues.Add(initialEstimate);

			if (stepSize == 0) { throw new ArgumentException("Transformed Up Down cannot have a step size of 0"); }
			this.stepSize = stepSize;

			if (stepSize <= 0) { throw new ArgumentException("The steps to go up must be an integer greater than or equal to 1"); }
			this.stepsToGoUp = stepsToGoUp;

			if (stepSize <= 0) { throw new ArgumentException("The steps to go down must be an integer greater than or equal to 1"); }
			this.stepsToGoDown = stepsToGoDown;

			AllowDontKnowResponses = false;
		}
		public override double NextValue()
		{
			ResponseState lastResponse = responses[^1];
			double nextValue = testedValues[^1];

			if (responses.Count >= stepsToGoUp)
			{
				bool allIncorrect = true;
				for (int i = 1; i <= stepsToGoUp; i++)
				{
					if (responses[^i] == lastResponse)
					{
						allIncorrect &= true;
					}
				}

				if (allIncorrect)
				{
					nextValue += stepSize;
					testedValues.Add(nextValue);
					return nextValue;
				}
			}

			if (responses.Count >= stepsToGoDown)
			{
				bool allCorrect = true;
				for (int i = 1; i <= stepsToGoDown; i++)
				{
					if (responses[^i] == lastResponse)
					{
						allCorrect &= true;
					}
				}

				if (allCorrect)
				{
					nextValue -= stepSize;
					testedValues.Add(nextValue);
					return nextValue;
				}
			}

			// Did not qualify to step up or down so test the same value again
			testedValues.Add(nextValue);
			return nextValue;
		}
	}
}
