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
using System.Collections.Generic;

namespace SBC{
	public enum ButtonEnum {
		RightJoyMainWeapon,
		RightJoyFire,
		RightJoyLockOn,
		CockpitHatch,
		Ignition,
		Start,
		Eject,
		MultiMonOpenClose,
		MultiMonMapZoomInOut,
		MultiMonModeSelect,
		MultiMonSubMonitor,
		MainMonZoomIn,
		MainMonZoomOut,
		Washing,
		Extinguisher,
		Chaff,
		WeaponConMain,
		WeaponConSub,
		WeaponConMagazine,
		FunctionFSS,
		FunctionManipulator,
		FunctionLineColorChange,
		FunctionTankDetach,
		FunctionOverride,
		FunctionNightScope,
		FunctionF1,
		FunctionF2,
		FunctionF3,
		Comm1,
		Comm2,
		Comm3,
		Comm4,
		Comm5,
		LeftJoySightChange,
		ToggleFilterControl,
		ToggleOxygenSupply,
		ToggleFuelFlowRate,
		ToggleBuffreMaterial,
		ToggleVTLocation,
		TunerDialStateChange,
		GearLeverStateChange
	}
	
	public class ButtonMasks {
		public static ButtonMask[] MaskList = new ButtonMask[50];
		public struct ButtonMask {
			public int bytePos;
			public int maskValue;
			
			public ButtonMask(int bytePos, int maskValue) {
				this.bytePos = bytePos;
				this.maskValue = maskValue;
			}
		}
		
		public static void InitializeMasks() {
			MaskList[(int) ButtonEnum.RightJoyMainWeapon]        = new ButtonMask( 2, 0x01);
			MaskList[(int) ButtonEnum.RightJoyFire]              = new ButtonMask( 2, 0x03);
			MaskList[(int) ButtonEnum.RightJoyLockOn]            = new ButtonMask( 2, 0x04);
			MaskList[(int) ButtonEnum.CockpitHatch]              = new ButtonMask( 2, 0x10);
			MaskList[(int) ButtonEnum.Ignition]                  = new ButtonMask( 2, 0x20);
			MaskList[(int) ButtonEnum.Start]                     = new ButtonMask( 2, 0x40);
			MaskList[(int) ButtonEnum.Eject]                     = new ButtonMask( 2, 0x08);
			MaskList[(int) ButtonEnum.MultiMonOpenClose]         = new ButtonMask( 2, 0x80);
			MaskList[(int) ButtonEnum.MultiMonMapZoomInOut]      = new ButtonMask( 3, 0x01);
			MaskList[(int) ButtonEnum.MultiMonModeSelect]        = new ButtonMask( 3, 0x02);
			MaskList[(int) ButtonEnum.MultiMonSubMonitor]        = new ButtonMask( 3, 0x04);
			MaskList[(int) ButtonEnum.MainMonZoomIn]             = new ButtonMask( 3, 0x08);
			MaskList[(int) ButtonEnum.MainMonZoomOut]            = new ButtonMask( 3, 0x10);
			MaskList[(int) ButtonEnum.Washing]                   = new ButtonMask( 4, 0x01);
			MaskList[(int) ButtonEnum.Extinguisher]              = new ButtonMask( 4, 0x02);
			MaskList[(int) ButtonEnum.Chaff]                     = new ButtonMask( 4, 0x04);
			MaskList[(int) ButtonEnum.WeaponConMain]             = new ButtonMask( 5, 0x02);
			MaskList[(int) ButtonEnum.WeaponConSub]              = new ButtonMask( 5, 0x04);
			MaskList[(int) ButtonEnum.WeaponConMagazine]         = new ButtonMask( 5, 0x08);
			MaskList[(int) ButtonEnum.FunctionFSS]               = new ButtonMask( 3, 0x20);
			MaskList[(int) ButtonEnum.FunctionManipulator]       = new ButtonMask( 3, 0x40);
			MaskList[(int) ButtonEnum.FunctionLineColorChange]   = new ButtonMask( 3, 0x80);
			MaskList[(int) ButtonEnum.FunctionTankDetach]        = new ButtonMask( 4, 0x08);
			MaskList[(int) ButtonEnum.FunctionOverride]          = new ButtonMask( 4, 0x10);
			MaskList[(int) ButtonEnum.FunctionNightScope]        = new ButtonMask( 4, 0x20);
			MaskList[(int) ButtonEnum.FunctionF1]                = new ButtonMask( 4, 0x40);
			MaskList[(int) ButtonEnum.FunctionF2]                = new ButtonMask( 4, 0x80);
			MaskList[(int) ButtonEnum.FunctionF3]                = new ButtonMask( 5, 0x01);
			MaskList[(int) ButtonEnum.Comm1]                     = new ButtonMask( 5, 0x10);
			MaskList[(int) ButtonEnum.Comm2]                     = new ButtonMask( 5, 0x20);
			MaskList[(int) ButtonEnum.Comm3]                     = new ButtonMask( 5, 0x40);
			MaskList[(int) ButtonEnum.Comm4]                     = new ButtonMask( 5, 0x80);
			MaskList[(int) ButtonEnum.Comm5]                     = new ButtonMask( 6, 0x01);
			MaskList[(int) ButtonEnum.LeftJoySightChange]        = new ButtonMask( 6, 0x02);
			MaskList[(int) ButtonEnum.ToggleFilterControl]       = new ButtonMask( 6, 0x04);
			MaskList[(int) ButtonEnum.ToggleOxygenSupply]        = new ButtonMask( 6, 0x08);
			MaskList[(int) ButtonEnum.ToggleFuelFlowRate]        = new ButtonMask( 6, 0x10);
			MaskList[(int) ButtonEnum.ToggleBuffreMaterial]      = new ButtonMask( 6, 0x20);
			MaskList[(int) ButtonEnum.ToggleVTLocation]          = new ButtonMask( 6, 0x40);
			MaskList[(int) ButtonEnum.TunerDialStateChange]      = new ButtonMask(24, 0x0F);
			MaskList[(int) ButtonEnum.GearLeverStateChange]      = new ButtonMask(25, 0xFF);
		}
	}
	
	public class ButtonState {
		public ButtonEnum button;
		public bool currentState;
		public bool changed;
	}
}
