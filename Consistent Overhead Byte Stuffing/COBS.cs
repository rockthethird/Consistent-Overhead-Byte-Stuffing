using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consistent_Overhead_Byte_Stuffing {
	/// <summary>
	/// Consistent Overhead Byte Stuffing is an encoding that removes all 0 
	/// bytes from arbitrary binary data. The encoded data consists only of 
	/// bytes with values from 0x01 to 0xFF. This is useful for preparing data 
	/// for transmission over a serial link (RS-232 or RS-485 for example), as 
	/// the 0 byte can be used to unambiguously indicate packet boundaries. 
	/// 
	/// COBS also has the advantage of adding very little overhead (at least 1 
	/// byte, plus up to an additional byte per 254 bytes of data). For 
	/// messages smaller than 254 bytes, the overhead is constant.
	/// </summary>
	public static class COBS {
		/// <summary>
		/// The encoded data will contain only values from 0x01 to 0xFF.
		/// </summary>
		/// <param name="Input">An array up to 254 bytes in length</param>
		/// <returns>Returns the encoded input</returns>
		public static IEnumerable<byte> Encode(IEnumerable<byte> Input) {
			if (Input == null)
				return null;

			if (Input.Count() > 254)
				throw new ArgumentOutOfRangeException("Input length must not exceed 254 bytes");

			var result = new List<byte>();
			int distanceIndex = 0;
			byte distance = 1;  // Distance to next zero

			foreach (var i in Input) {
				// If we encounter a zero (the frame delimiter)
				if (i == 0) {
					// Write the value of the distance to the next zero back in output where we last saw a zero
					result.Insert(distanceIndex, distance);

					// Set the distance index to the latest index plus one
					distanceIndex = (byte)result.Count;

					// Reset the value which indicates the distance to the next zero (the frame delimiter)
					distance = 1;
				}
				else {
					// Otherwise simply add the next value to the result
					result.Add(i);

					// Increment the distance to the next zero
					distance++;

					// Check for maximum distance value
					if (distance == 0xFF) {
						// Set the distance variable to its maximum value
						result.Insert(distanceIndex, distance);

						// Set the distance index to the latest index plus one
						distanceIndex = (byte)result.Count;

						// Reset the value which indicates the distance to the next zero (the frame delimiter)
						distance = 1;
					}
				}
			}

			// If the packet hasn't reached the maximum size
			if(result.Count != 255 && result.Count > 0)
				// Add the last distance variable
				result.Insert(distanceIndex, distance);

			// Return with the result
			return result;
		}

		/// <summary>
		/// The decoded data will be restored with all zeros which were removed
		/// during the decoding process.
		/// </summary>
		/// <param name="Input">A COBS encoded array</param>
		/// <returns>Returns the decoded input</returns>
		public static IEnumerable<byte> Decode(IEnumerable<byte> Input) {
			if (Input == null)
				return null;

			if (Input.Count() > 255)
				throw new ArgumentOutOfRangeException("Input length must not exceed 254 bytes");

			var input = Input.ToArray();
			var result = new List<byte>();
			int distanceIndex = 0;
			byte distance = 1;  // Distance to next zero

			// Continue decoding which the next index is valid
			while (distanceIndex < input.Length) {
				// Get the next distance value
				distance = input[distanceIndex];

				// Ensure the input is formatted correctly (distanceIndex + distance)
				if (input.Length < distanceIndex + distance || distance < 1) {
					Trace.WriteLine("Consistent Overhead Byte Stuffing failed to parse an input.");
					return new List<byte>();
				}

				// Add the range of byte up to the next zero
				if (distance > 1) {
					for (byte i = 1; i < distance; i++)
						result.Add(input[distanceIndex + i]);
				}

				// Determine the next distance index (doing this here assists the below if)
				distanceIndex += distance;

				// Add the original zero back
				if(distance < 0xFF && distanceIndex < input.Length)
					result.Add(0);
			}

			return result;
		}

		/// <summary>
		/// Stuffs "length" bytes of data at the location pointed to by
		/// "input", writing the output to the location pointed to by
		/// "output". Returns the number of bytes written to "output".
		/// </summary>
		/// <param name="input"></param>
		/// <param name="length"></param>
		/// <param name="output"></param>
		/// <returns></returns>
		public static int cobs_encode(ref byte[] input, int length, ref byte[] output) {
			int read_index = 0;
			int write_index = 1;

			int code_index = 0;
			byte distance = 1;

			while (read_index < length) {
				// If we encounter a zero for the current value of the input
				if (input[read_index] == 0) {
					// Write the value of the distance to the next zero back in output where we last saw a zero
					output[code_index] = distance;

					// Set the distance index to the latest index plus one
					code_index = write_index++;

					// Reset the distance
					distance = 1;

					// Keep the read and write indexes the same
					read_index++;
				}
				else {
					// Simply copy the value over from the input (increment the indexes up one value)
					output[write_index++] = input[read_index++];

					// Increment the distance
					distance++;

					// If the distance reaches maximum valve
					if (distance == 0xFF) {
						// Set the distance variable to its maximum value
						output[code_index] = distance;

						// Set the distance index to the latest index plus one
						code_index = write_index++;

						// Reset the distance
						distance = 1;
					}
				}
			}

			if(code_index != 255 && output.Count() > 0)
				output[code_index] = distance;

			return write_index;
		}

		/// <summary>
		/// Unstuffs "length" bytes of data at the location pointed to by
		/// "input", writing the output * to the location pointed to by
		/// "output". Returns the number of bytes written to "output" if
		/// "input" was successfully unstuffed, and 0 if there was an
		/// error unstuffing "input".
		/// </summary>
		/// <param name="input"></param>
		/// <param name="length"></param>
		/// <param name="output"></param>
		/// <returns></returns>
		public static int cobs_decode(ref byte[] input, int length, ref byte[] output) {
			int read_index = 0;
			int write_index = 0;
			byte distance;
			byte i;

			while (read_index < length) {
				// Copy the current input value to the distance value
				distance = input[read_index];

				// If the index of the next distance value is greater than the length of the input
				// AND the distance is not equal to one
				if (read_index + distance > length && distance != 1) {
					return 0;
				}

				// Increment to the next not zero value
				read_index++;

				// Copy the input to the output for the distance
				for (i = 1; i < distance; i++) {
					output[write_index++] = input[read_index++];
				}

				// Determine if the 
				if (distance != 0xFF && read_index != length) {
					output[write_index++] = Convert.ToByte('\0');
				}
			}

			return write_index;
		}
	}
}