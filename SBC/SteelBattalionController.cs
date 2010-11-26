/*********************************************************************************
 *   SteelBattalion.NET - A library to access the Steel Battalion XBox           *
 *   controller.  Written by Joseph Coutcher.                                    *
 *                                                                               *
 *   This file is part of SteelBattalion.NET                                     *
 *                                                                               *
 *   SteelBattalion.NET is free software: you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by        *
 *   the Free Software Foundation, either version 3 of the License, or           *
 *   (at your option) any later version.                                         *
 *                                                                               *
 *   SteelBattalion.NET is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of              *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               *
 *   GNU General Public License for more details.                                *
 *                                                                               *
 *   You should have received a copy of the GNU General Public License           *
 *   along with SteelBattalion.NET.  If not, see <http://www.gnu.org/licenses/>. *
 *                                                                               *
 *   While this code is licensed under the LGPL, please let me know whether      *
 *   you're using it.  I'm always interested in hearing about new projects,      *
 *   especially ones that I was able to make a contribution to...in the form of  *
 *   this library.                                                               *
 *                                                                               *
 *   EMail: geekteligence at google mail                                         *
 *                                                                               *
 *   2010-11-26: JC - Initial commit                                             *
 *                                                                               * 
 *********************************************************************************/

using System;
using System.Text;
using System.Text.RegularExpressions;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using System.Timers;
using System.Collections.Generic;

namespace SBC
{
	/// <summary>
	/// Description of SteelBattalionController.
	/// </summary>
	public class SteelBattalionController {
		#region Public and Private variables
		public DateTime LastDataEventDate = DateTime.Now;
		public UsbDevice MyUsbDevice;
		
		public delegate void ButtonStateChangedDelegate(ButtonState[] arr);
		public event ButtonStateChangedDelegate ButtonStateChanged;

		public delegate void RawDataDelegate(byte[] arr);
		public event RawDataDelegate RawData;

		UsbEndpointReader reader = null;
		UsbEndpointWriter writer = null;
		
		Timer pollTimer = new Timer();
		
		// The USB device finder that looks for the Steel Batallion controller
		public UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(0x0A7B, 0xD000);

		/// <summary>
		/// The byte buffer that the raw control data is stored
		/// </summary>
		byte[] rawControlData = new Byte[26];
		
		/// <summary>
		/// The byte buffer that the raw LED data is stored
		/// </summary>
		byte[] rawLEDData = new Byte[34];
		#endregion
		
		/// <summary>
		/// Constructor for the controller.  Does nothing at the moment.
		/// </summary>
		public SteelBattalionController() {
		}
		
		/// <summary>
		/// Sets the intensity of the specified LED in the buffer, and sends the buffer to the controller.
		/// </summary>
		/// <param name="LightId">A ControllerLEDEnum value that specifies which LED to modify</param>
		/// <param name="Intensity">The intensity of the LED, ranging from 0 to 15</param>
		public void SetLEDState(ControllerLEDEnum LightId, int Intensity) {
			SetLEDState(LightId, Intensity, true);
		}

		/// <summary>
		/// Sets the intensity of the specified LED in the buffer, but gives the option on whether you want
		/// to send the buffer to the controller.  This can be useful for updating multiple LED's at the
		/// same time, but not waiting for the LED buffer to transfer to the device after each call.
		/// </summary>
		/// <param name="LightId">A ControllerLEDEnum value that specifies which LED to modify</param>
		/// <param name="Intensity">The intensity of the LED, ranging from 0 to 15</param>
		/// <param name="refreshState">A boolean value indicating whether to refresh the buffer on the device.</param>
		public void SetLEDState(ControllerLEDEnum LightId, int Intensity, bool refreshState) {
			int hexPos = ((int) LightId) % 2;
			int bytePos = (((int) LightId) - hexPos) / 2;
			
			if (Intensity > 0x0f) Intensity = 0x0f;
			
			// Erase the byte position, and set the light intensity
			rawLEDData[bytePos] &= (byte) ((hexPos == 1)?0x0F:0xF0);
			rawLEDData[bytePos] += (byte) (Intensity * ((hexPos == 1)?0x10:0x01));
			
			if (refreshState) {
				RefreshLEDState();
			}
		}
		
		/// <summary>
		/// Refreshes the LED buffer/state on the controller
		/// </summary>
		public void RefreshLEDState() {
			ErrorCode ec = ErrorCode.None;
			int bytesWritten;
			
			ec = writer.Write(rawLEDData, 1000, out bytesWritten);
			if (ec != ErrorCode.None) throw new Exception(UsbDevice.LastErrorString);
		}

		/// <summary>
		/// Retrieves the LED state from the internal buffer.  This does not return the actual
		/// intensity from the controller itself.  But, if this is the only library accessing
		/// the device, you can assume that the LED state is the same as what's in the buffer.
		/// </summary>
		/// <param name="LightId"></param>
		/// <returns></returns>
		public int GetLEDState(ControllerLEDEnum LightId) {
			int hexPos = ((int) LightId) % 2;
			int bytePos = (((int) LightId) - hexPos) / 2;
			
			return (((int) rawLEDData[bytePos]) & ((hexPos == 1)?0xF0:0x0F)) / ((hexPos == 1)?0x10:0x01);
		}

		/// <summary>
		/// Initializes the device via LibUSB, and sets the refresh interval of the controller data
		/// </summary>
		/// <param name="Interval">The interval that the device is polled for information.</param>
		public void Init(int Interval) {
			//ErrorCode ec = ErrorCode.None;

			// Find and open the usb device.
			MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
			if (MyUsbDevice == null) throw new Exception("Device Not Found.");

			IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
			if (!ReferenceEquals(wholeUsbDevice, null)) {
				wholeUsbDevice.SetConfiguration(1);
				wholeUsbDevice.ClaimInterface(0);
			}
			
			reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
			writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
			
			ButtonMasks.InitializeMasks();
			
			SetPollingInterval(Interval);
			pollTimer.Elapsed += new ElapsedEventHandler(pollTimer_Elapsed);
			pollTimer.Start();

			TestLEDs();
			RefreshLEDState();
		}
		
		/// <summary>
		/// This private function is triggered upon initialization of the controller.  It flashes all of the LED
		/// lights 5 times...just as a sanity check to make sure I coded all of the enumerator values for the LED's :-)
		/// </summary>
		private void TestLEDs() {
			for (int j = 0; j < 5; j++) {
				for (int intensity = 0; intensity <= 0x0f; intensity++) {
					foreach(string value in Enum.GetNames(typeof(ControllerLEDEnum))) {
						ControllerLEDEnum LightId = (ControllerLEDEnum) Enum.Parse(typeof(ControllerLEDEnum), value);
						
						int hexPos = ((int) LightId) % 2;
						int bytePos = (((int) LightId) - hexPos) / 2;
						
						if (intensity > 0x0f) intensity = 0x0f;
						
						rawLEDData[bytePos] &= (byte) ((hexPos == 1)?0x0F:0xF0);
						rawLEDData[bytePos] += (byte) (intensity * ((hexPos == 1)?0x10:0x01));
					}
					
					RefreshLEDState();
				}
				for (int intensity = 0x0f; intensity >= 0; intensity--) {
					foreach(string value in Enum.GetNames(typeof(ControllerLEDEnum))) {
						ControllerLEDEnum LightId = (ControllerLEDEnum) Enum.Parse(typeof(ControllerLEDEnum), value);
						
						int hexPos = ((int) LightId) % 2;
						int bytePos = (((int) LightId) - hexPos) / 2;
						
						if (intensity > 0x0f) intensity = 0x0f;
						
						rawLEDData[bytePos] &= (byte) ((hexPos == 1)?0x0F:0xF0);
						rawLEDData[bytePos] += (byte) (intensity * ((hexPos == 1)?0x10:0x01));
					}
					
					RefreshLEDState();
				}
			}
		}

		/// <summary>
		/// When the poll timer elapses, this function retrieves data from the controller, and passes
		/// the raw data to both the raw data event (so applications can analyze the raw data if needed),
		/// and passes the data to the private CheckStateChanged function.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// Please note - the poll timer is stopped until all events have been processed...so
		/// keep your event handlers optimized for analyzing the controller data.
		/// </remarks>
		private void pollTimer_Elapsed(object sender, ElapsedEventArgs e) {
			pollTimer.Stop();
			int readByteCount = 0;
			byte[] buf = new byte[64];
			reader.Read(buf, 0, 64, 1000, out readByteCount);
			
			if (this.RawData != null) {
				RawData(buf);
			}
			
			CheckStateChanged(buf);
			
			Array.Copy(buf, 0, rawControlData, 0, readByteCount);
			//Console.WriteLine(ConvertToHex(rawControlData, rawControlData.Length));
			pollTimer.Start();
		}
		
		/// <summary>
		/// Checks the button state based on the raw data returned, and if it has, the ButtonStateChanged event is raised
		/// </summary>
		/// <param name="buf">Raw data buffer retrieved from the controller</param>
		private void CheckStateChanged(byte[] buf) {
			ButtonEnum[] values = (ButtonEnum[]) Enum.GetValues(typeof(ButtonEnum));
			ButtonState[] stateChangedArray = new ButtonState[values.Length];
			
			for(int i = 0; i < values.Length; i++) {
				ButtonMasks.ButtonMask mask = ButtonMasks.MaskList[(int) values[i]];
				
				ButtonState state = new ButtonState();
				state.button = (ButtonEnum) values[i];
				state.currentState = ((buf[mask.bytePos] & mask.maskValue) > 0);
				state.changed = isStateChanged(buf, mask.bytePos, mask.maskValue);
				stateChangedArray[(int) values[i]] = state;
			}
			
			if ((stateChangedArray.Length > 0) && (this.ButtonStateChanged != null)) {
				ButtonStateChanged(stateChangedArray);
			}
		}
		
		/// <summary>
		/// Checks to see if the buton state has changed
		/// </summary>
		/// <param name="buf">The raw data buffer</param>
		/// <param name="bytePos">The byte position to check</param>
		/// <param name="maskValue">The mask value for that button/switch</param>
		/// <returns></returns>
		private bool isStateChanged(byte[] buf, int bytePos, int maskValue) {
			return ((buf[bytePos] & maskValue) != (rawControlData[bytePos] & maskValue));
		}

		// De-initialize the controller
		public void UnInit() {
			// Always disable and unhook event when done.
			//reader.DataReceivedEnabled = false;
			//reader.DataReceived -= (OnRxEndPointData);
			
			pollTimer.Stop();
			pollTimer.Elapsed -= (pollTimer_Elapsed);

			if (MyUsbDevice != null) {
				if (MyUsbDevice.IsOpen) {
					IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
					if (!ReferenceEquals(wholeUsbDevice, null)) {
						// Release interface #0.
						wholeUsbDevice.ReleaseInterface(0);
					}
					MyUsbDevice.Close();
				}
			}
			
			MyUsbDevice = null;
		}
		
		/// <summary>
		/// Allows you to set the polling interval while the controller is initialized
		/// </summary>
		/// <param name="Interval"></param>
		public void SetPollingInterval(int Interval) {
			pollTimer.Interval = Interval;
		}
		
		/// <summary>
		/// Corresponds to the "Rotation Lever" joystick on the left.
		/// </summary>
		public int RotationLever {
			get { return (int) rawControlData[13]; }
		}

		/// <summary>
		/// Corresponds to the "Sight Change" analog stick on the "Rotation Lever" joystick.  X Axis value.
		/// </summary>
		public int SightChangeX {
			get { return (int) rawControlData[15]; }
		}

		/// <summary>
		/// Corresponds to the "Sight Change" analog stick on the "Rotation Lever" joystick.  Y Axis value.
		/// </summary>
		public int SightChangeY {
			get { return (int) rawControlData[17]; }
		}

		/// <summary>
		/// Corresponds to the "Aiming Lever" joystick on the right.  X Axis value.
		/// </summary>
		public int AimingX {
			get { return (int) rawControlData[9]; }
		}

		/// <summary>
		/// Corresponds to the "Aiming Lever" joystick on the right.  Y Axis value.
		/// </summary>
		public int AimingY {
			get { return (int) rawControlData[11]; }
		}

		/// <summary>
		/// Corresponds to the left pedal on the pedal block
		/// </summary>
		public int LeftPedal {
			get { return (int) rawControlData[19]; }
		}

		/// <summary>
		/// Corresponds to the middle pedal on the pedal block
		/// </summary>
		public int MiddlePedal {
			get { return (int) rawControlData[21]; }
		}

		/// <summary>
		/// Corresponds to the right pedal on the pedal block
		/// </summary>
		public int RightPedal {
			get { return (int) rawControlData[23]; }
		}

		/// <summary>
		/// Corresponds to the tuner dial position.  The 9 o'clock postion is 0, and the 6 o'clock position is 12.
		/// The blank area between the 6 and 9 o'clock positions is 13, 14, and 15 clockwise.
		/// </summary>
		public int TunerDial {
			get { return (int) rawControlData[24] & 0x0F; }
		}
		
		/// <summary>
		/// Corresponds to the gear lever on the left block.
		/// </summary>
		public int GearLever {
			get { return (int) rawControlData[25]; }
		}

		/// <summary>
		/// Function to convert a byte array to a hex string
		/// </summary>
		/// <param name="asciiString">Byte array containing the actual byte values to convert</param>
		/// <param name="count">the count of the bytes to convert</param>
		/// <returns></returns>
		public string ConvertToHex(byte[] asciiString, int count) {
			StringBuilder hex = new StringBuilder();
			int i = 0;
			
			foreach (byte c in asciiString) {
				int tmp = c;
				hex.Append(String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString())));
				i++;
				if(i >= count) break;
			}
			
			return hex.ToString();
		}
	}
}
