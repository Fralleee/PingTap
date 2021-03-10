using System;

namespace Fralle.Core
{
	public class DelegateExamples
	{
		public void Run()
		{
			Action action1 = delegate
			action1();
			FunctionWithOnComplete(action1);


			Action<int> action2 = delegate (int param1)
			{ /*Do something*/ };
			action2(1);

			Action<int, float> action3 = delegate (int param1, float param2)
			{ /*Do something*/ };
			action3(1, 2.1f);

			Action<int, float> action3B = ActionExampleFunction;
			action3B(1, 2.1f);



			// Action that can return a value
			Func<int> func1 = delegate
			{ return 1; };
			int a = func1();

			Func<int, int> func2 = delegate (int param1)
			{ return param1 + 1; };
			int b = func2(1);

			Func<int, float, int> func3 = delegate (int param1, float param2)
			{ return param1 + ((int)param2) + 1; };
			int c = func3(1, 1.1f);

			Func<int, float, int> func3B = FuncExampleFunction;
			int d = func3B(1, 1.1f);


			// Basically a func but can only take one parameter and always returns a bool (Func<int, bool>)
			Predicate<int> pred1 = delegate (int param1)
			{ return true; };
			bool b1 = pred1(1);


			int result = a + b + c + d;
		}

		private void FunctionWithOnComplete(Action onComplete)
		{
			//Do actual work

			//onComplete();
			onComplete?.Invoke();
		}

		private void ActionExampleFunction(int param1, float param2)
		{

		}

		private int FuncExampleFunction(int param1, float param2)
		{
			return 1;
		}
	}
}

