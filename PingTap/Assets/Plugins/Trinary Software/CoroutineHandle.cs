namespace MEC
{
	/// <summary>
	/// A handle for a MEC coroutine.
	/// </summary>
	public readonly struct CoroutineHandle : System.IEquatable<CoroutineHandle>
	{
		private const byte ReservedSpace = 0x0F;
		private static readonly int[] NextIndex = { ReservedSpace + 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		private readonly int _id;

		public byte Key { get { return (byte)(_id & ReservedSpace); } }

		public CoroutineHandle(byte ind)
		{
			if (ind > ReservedSpace)
				ind -= ReservedSpace;

			_id = NextIndex[ind] + ind;
			NextIndex[ind] += ReservedSpace + 1;
		}

		public bool Equals(CoroutineHandle other)
		{
			return _id == other._id;
		}

		public override bool Equals(object other)
		{
			if (other is CoroutineHandle)
				return Equals((CoroutineHandle)other);
			return false;
		}

		public static bool operator ==(CoroutineHandle a, CoroutineHandle b)
		{
			return a._id == b._id;
		}

		public static bool operator !=(CoroutineHandle a, CoroutineHandle b)
		{
			return a._id != b._id;
		}

		public override int GetHashCode()
		{
			return _id;
		}

		/// <summary>
		/// Is true if this handle may have been a valid handle at some point. (i.e. is not an uninitialized handle, error handle, or a key to a coroutine lock)
		/// </summary>
		public bool IsValid
		{
			get { return Key != 0; }
		}
	}
}
