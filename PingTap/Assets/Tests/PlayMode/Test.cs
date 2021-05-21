using System.Collections;

namespace Tests
{
	public class Test
	{
		public IEnumerator TestWithEnumeratorPasses()
		{
			// Use the Assert class to test conditions.
			// Use yield to skip a frame.
			yield return null;
		}
	}
}
