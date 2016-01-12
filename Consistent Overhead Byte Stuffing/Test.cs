using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consistent_Overhead_Byte_Stuffing {
	[TestClass]
	public class COBSTesting {
		public static byte[] decoded1 = new byte[] { 0 };
		public static byte[] decoded2 = new byte[] { 0, 0 };
		public static byte[] decoded3 = new byte[] { 11, 22, 0, 33 };
		public static byte[] decoded4 = new byte[] { 11, 22, 33, 44 };
		public static byte[] decoded5 = new byte[] { 11, 0, 0, 0 };
		public static byte[] decoded6 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254 };

		public static byte[] encoded1 = new byte[] { 1, 1 };
		public static byte[] encoded2 = new byte[] { 1, 1, 1 };
		public static byte[] encoded3 = new byte[] { 3, 11, 22, 2, 33 };
		public static byte[] encoded4 = new byte[] { 5, 11, 22, 33, 44 };
		public static byte[] encoded5 = new byte[] { 2, 11, 1, 1, 1 };
		public static byte[] encoded6 = new byte[] { 255, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254 };

		[TestMethod]
		public void TestEncoding() {
			var encodeTest = new byte[1000];

			// Jacques Fortier implementation (slight modifications for C#)
			COBS.cobs_encode(ref decoded1, decoded1.Count(), ref encodeTest);
			Assert.IsTrue(encoded1.SequenceEqual(encodeTest.Take(encoded1.Count())));

			COBS.cobs_encode(ref decoded2, decoded2.Count(), ref encodeTest);
			Assert.IsTrue(encoded2.SequenceEqual(encodeTest.Take(encoded2.Count())));

			COBS.cobs_encode(ref decoded3, decoded3.Count(), ref encodeTest);
			Assert.IsTrue(encoded3.SequenceEqual(encodeTest.Take(encoded3.Count())));

			COBS.cobs_encode(ref decoded4, decoded4.Count(), ref encodeTest);
			Assert.IsTrue(encoded4.SequenceEqual(encodeTest.Take(encoded4.Count())));

			COBS.cobs_encode(ref decoded5, decoded5.Count(), ref encodeTest);
			Assert.IsTrue(encoded5.SequenceEqual(encodeTest.Take(encoded5.Count())));

			COBS.cobs_encode(ref decoded6, decoded6.Count(), ref encodeTest);
			Assert.IsTrue(encoded6.SequenceEqual(encodeTest.Take(encoded6.Count())));

			// IEnumerable method
			Assert.IsTrue(encoded1.SequenceEqual(COBS.Encode(decoded1)));
			Assert.IsTrue(encoded2.SequenceEqual(COBS.Encode(decoded2)));
			Assert.IsTrue(encoded3.SequenceEqual(COBS.Encode(decoded3)));
			Assert.IsTrue(encoded4.SequenceEqual(COBS.Encode(decoded4)));
			Assert.IsTrue(encoded5.SequenceEqual(COBS.Encode(decoded5)));
			Assert.IsTrue(encoded6.SequenceEqual(COBS.Encode(decoded6)));
		}

		[TestMethod]
		public void TestEncodingNull() {
			Assert.AreEqual(null, COBS.Encode(null));
		}

		[TestMethod]
		public void TestEncodingEmpty() {
			Assert.IsTrue(new List<byte>().SequenceEqual(COBS.Encode(new List<byte>())));
		}

		[TestMethod]
		public void TestEncodingMaxSize() {
			// Create an array of maximum allowed size
			var maxSizeArray = new byte[254];

			// Attempt to encode the array
			var result = COBS.Encode(maxSizeArray);

			// Assert the array was created correctly
			Assert.IsTrue(result.All(c => c == 1));
		}

		[TestMethod]
		public void TestEncodingTooLarge() {
			try {
				// Create an array one byte too large
				var outOfRangeArray = new byte[255];

				// Attempt to encode the array
				COBS.Encode(outOfRangeArray);

				// If the code does NOT throw the exception the method failed
				Assert.Fail();
			}
			catch (ArgumentOutOfRangeException) {
				// If the exception was caught then the test passed
			}
		}

		[TestMethod]
		public void TestDecoding() {
			var decodeTest = new byte[1000];

			// Jacques Fortier implementation (slight modifications for C#)
			COBS.cobs_decode(ref encoded1, encoded1.Count(), ref decodeTest);
			Assert.IsTrue(decoded1.SequenceEqual(decodeTest.Take(decoded1.Count())));

			COBS.cobs_decode(ref encoded2, encoded2.Count(), ref decodeTest);
			Assert.IsTrue(decoded2.SequenceEqual(decodeTest.Take(decoded2.Count())));

			COBS.cobs_decode(ref encoded3, encoded3.Count(), ref decodeTest);
			Assert.IsTrue(decoded3.SequenceEqual(decodeTest.Take(decoded3.Count())));

			COBS.cobs_decode(ref encoded4, encoded4.Count(), ref decodeTest);
			Assert.IsTrue(decoded4.SequenceEqual(decodeTest.Take(decoded4.Count())));

			COBS.cobs_decode(ref encoded5, encoded5.Count(), ref decodeTest);
			Assert.IsTrue(decoded5.SequenceEqual(decodeTest.Take(decoded5.Count())));

			COBS.cobs_decode(ref encoded6, encoded6.Count(), ref decodeTest);
			Assert.IsTrue(decoded6.SequenceEqual(decodeTest.Take(decoded6.Count())));

			Assert.IsTrue(decoded1.SequenceEqual(COBS.Decode(encoded1)));
			Assert.IsTrue(decoded2.SequenceEqual(COBS.Decode(encoded2)));
			Assert.IsTrue(decoded3.SequenceEqual(COBS.Decode(encoded3)));
			Assert.IsTrue(decoded4.SequenceEqual(COBS.Decode(encoded4)));
			Assert.IsTrue(decoded5.SequenceEqual(COBS.Decode(encoded5)));
			Assert.IsTrue(decoded6.SequenceEqual(COBS.Decode(encoded6)));
		}

		[TestMethod]
		public void TestDecodingNull() {
			Assert.AreEqual(null, COBS.Decode(null));
		}

		[TestMethod]
		public void TestDecodingEmpty() {
			Assert.IsTrue(new List<byte>().SequenceEqual(COBS.Decode(new List<byte>())));
		}

		[TestMethod]
		public void TestDecodingMaxSize() {
			// Create an array of maximum allowed size
			var maxSizeArray = new byte[255];

			// Attempt to decode the array
			var result = COBS.Decode(maxSizeArray);

			// Assert that an all zero array returns an empty list
			Assert.IsTrue(result.Count() == 0);
		}

		[TestMethod]
		public void TestDecodingTooLarge() {
			try {
				// Create an array one byte too large
				var outOfRangeArray = new byte[256];

				// Attempt to decode the array
				COBS.Decode(outOfRangeArray);

				// If the code does NOT throw the exception the method failed
				Assert.Fail();
			}
			catch (ArgumentOutOfRangeException) {
				// If the exception was caught then the test passed
			}
		}

		[TestMethod]
		public void TestEncodingThenDecoding() {
			var encodeTest = new byte[1000];
			var decodeTest = new byte[1000];

			// Jacques Fortier implementation (slight modifications for C#)
			COBS.cobs_encode(ref decoded1, decoded1.Count(), ref encodeTest);
			COBS.cobs_decode(ref encodeTest, encoded1.Count(), ref decodeTest);
			Assert.IsTrue(decoded1.SequenceEqual(decodeTest.Take(decoded1.Count())));

			COBS.cobs_encode(ref decoded2, decoded2.Count(), ref encodeTest);
			COBS.cobs_decode(ref encodeTest, encoded2.Count(), ref decodeTest);
			Assert.IsTrue(decoded2.SequenceEqual(decodeTest.Take(decoded2.Count())));

			COBS.cobs_encode(ref decoded3, decoded3.Count(), ref encodeTest);
			COBS.cobs_decode(ref encodeTest, encoded3.Count(), ref decodeTest);
			Assert.IsTrue(decoded3.SequenceEqual(decodeTest.Take(decoded3.Count())));

			COBS.cobs_encode(ref decoded4, decoded4.Count(), ref encodeTest);
			COBS.cobs_decode(ref encodeTest, encoded4.Count(), ref decodeTest);
			Assert.IsTrue(decoded4.SequenceEqual(decodeTest.Take(decoded4.Count())));

			COBS.cobs_encode(ref decoded5, decoded5.Count(), ref encodeTest);
			COBS.cobs_decode(ref encodeTest, encoded5.Count(), ref decodeTest);
			Assert.IsTrue(decoded5.SequenceEqual(decodeTest.Take(decoded5.Count())));

			COBS.cobs_encode(ref decoded6, decoded6.Count(), ref encodeTest);
			COBS.cobs_decode(ref encodeTest, encoded6.Count(), ref decodeTest);
			Assert.IsTrue(decoded6.SequenceEqual(decodeTest.Take(decoded6.Count())));

			Assert.IsTrue(decoded1.SequenceEqual(COBS.Decode(COBS.Encode(decoded1))));
			Assert.IsTrue(decoded2.SequenceEqual(COBS.Decode(COBS.Encode(decoded2))));
			Assert.IsTrue(decoded3.SequenceEqual(COBS.Decode(COBS.Encode(decoded3))));
			Assert.IsTrue(decoded4.SequenceEqual(COBS.Decode(COBS.Encode(decoded4))));
			Assert.IsTrue(decoded5.SequenceEqual(COBS.Decode(COBS.Encode(decoded5))));
			Assert.IsTrue(decoded6.SequenceEqual(COBS.Decode(COBS.Encode(decoded6))));
		}

		/// <summary>
		/// THESE TESTS ARE INVALID AND ABANDONED AND REMAIN FOR REFERENCE ONLY
		/// 
		/// An unidentified anomaly exists in which the order of the tests or
		/// the surround code (i.e. other test changes) affects the timing.  
		/// For example, if you move the block of code for the second test 
		/// above the first, the times will move dramatically outside expected 
		/// variances.  GC.Collect() was added as an attempt to remedy this 
		/// anomaly with little to no effect.
		/// 
		/// The order of the tests were chosen so that the time for the Jacques
		/// Fortier implementation is at its fastest.
		/// 
		/// Any insight for why this happens would be appreciated.
		/// </summary>
		//[TestMethod]
		public void TestEncodeSpeed() {
			var duration1 = new Stopwatch();
			var duration2 = new Stopwatch();

			// Prepare for performance testing
			GC.Collect();

			// Time the IEnumerable version
			IEnumerable<byte> result1;
			duration1.Start();
			result1 = COBS.Encode(decoded6);
			duration1.Stop();

			// Time the Jacques Fortier implementation (slight modifications for C#)
			var encodeTest = new byte[255];
			duration2.Start();
			COBS.cobs_encode(ref decoded6, decoded6.Count(), ref encodeTest);
			duration2.Stop();

			// Display the results
			Console.WriteLine(String.Format("IEnumerable: {0} ticks ({1}) | Jacques Fortier: {2} ticks ({3})", duration1.ElapsedTicks, duration1.Elapsed, duration2.ElapsedTicks, duration2.Elapsed));

			// Ensure IEnumerable encode/decode succeeded
			Assert.IsTrue(encoded6.SequenceEqual(result1));

			// Ensure Jacques Fortier encode/decode succeeded
			Assert.IsTrue(encoded6.SequenceEqual(encodeTest));

			// Test if the IEnumerable friendly implementation is faster
			Assert.IsTrue(duration1.ElapsedTicks < duration2.ElapsedTicks);
		}

		/// <summary>
		/// THESE TESTS ARE INVALID AND ABANDONED AND REMAIN FOR REFERENCE ONLY
		/// 
		/// An unidentified anomaly exists in which the order of the tests or
		/// the surround code (i.e. other test changes) affects the timing.  
		/// For example, if you move the block of code for the second test 
		/// above the first, the times will move dramatically outside expected 
		/// variances.  GC.Collect() was added as an attempt to remedy this 
		/// anomaly with little to no effect.
		/// 
		/// The order of the tests were chosen so that the time for the Jacques
		/// Fortier implementation is at its fastest.
		/// 
		/// Any insight for why this happens would be appreciated.
		/// </summary>
		//[TestMethod]
		public void TestDecodeSpeed() {
			var duration1 = new Stopwatch();
			var duration2 = new Stopwatch();

			// Prepare for performance testing
			GC.Collect();

			// Time the IEnumerable version
			IEnumerable<byte> result1;
			duration1.Restart();
			result1 = COBS.Decode(encoded6);
			duration1.Stop();

			// Time the Jacques Fortier implementation (slight modifications for C#)
			var decodeTest = new byte[254];
			duration2.Restart();
			COBS.cobs_decode(ref encoded6, encoded6.Count(), ref decodeTest);
			duration2.Stop();

			// Display the results
			Console.WriteLine(String.Format("IEnumerable: {0} ticks ({1}) | Jacques Fortier: {2} ticks ({3})", duration1.ElapsedTicks, duration1.Elapsed, duration2.ElapsedTicks, duration2.Elapsed));

			// Ensure IEnumerable encode/decode succeeded
			Assert.IsTrue(decoded6.SequenceEqual(result1));

			// Ensure Jacques Fortier encode/decode succeeded
			Assert.IsTrue(decoded6.SequenceEqual(decodeTest.Take(decoded6.Count())));

			// Test if the IEnumerable friendly implementation is faster
			Assert.IsTrue(duration1.ElapsedTicks < duration2.ElapsedTicks);
		}
	}
}
