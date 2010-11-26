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

namespace SBC {
	public enum ControllerLEDEnum {
		EmergencyEject = 4,
		CockpitHatch = 5,
		Ignition = 6,
		Start = 7,
		OpenClose = 8,
		MapZoomInOut = 9,
		ModeSelect = 10,
		SubMonitorModeSelect = 11,
		MainMonitorZoomIn = 12,
		MainMonitorZoomOut = 13,
		
		Gear5 = 41,
		Gear4 = 40,
		Gear3 = 39,
		Gear2 = 38,
		Gear1 = 37,
		GearN = 36,
		GearR = 35,
		
		Comm5 = 33,
		Comm4 = 32,
		Comm3 = 31,
		Comm2 = 30,
		Comm1 = 29,
		MagazineChange = 28,
		
		SubWeaponControl = 27,
		MainWeaponControl = 26,
		F3 = 25,
		F2 = 24,
		F1 = 23,
		NightScope = 22,
		Override = 21,
		TankDetach = 20,
		Chaff = 19,
		Extinguisher = 18,
		Washing = 17,
		LineColorChange = 16,
		Manipulator = 15,
		ForecastShootingSystem = 14,
	}
}
