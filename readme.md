Note from the writer:
----------------------

This project initially started out as a dare from my good friend Travis.  He had the controller laying around, and wanted to see if there was some way we could work it into our podcast.  So, I sat out to see if there was a driver written for Windows 7...most notably...64-bit.  If you're reading this text file, you obviously realized there wasn't one (until now.)

So, I sat out to write something.  Unfortunately, I don't have driver writing skills...and loading the Windows DDK is a pain in the butt.  So I decided to write a library that utilizes the wonderful open-source project LibUSB.  A few weeks of coding, and out came SteelBattalion.NET.

We currently use this library with a custom application I wrote for answering Skype calls and switching video feeds within UStream.  The Communication Tuner buttons control which camera is selected, the Start and Eject buttons are used for answering and hanging up Skype calls, and the toggle switches are used for starting and ending broadcasting/recording.

While I had a ton of fun writing the library, interfacing with LibUSB, and figuring out which bytes/bits represent which buttons on the controller, I couldn't stop there.  I decided to release this as open source, under the LGPL license, for others to utilize in their own projects.

Since this is under the LGPL, you're not compelled to release that this library was used with your code.  But I would ask that you at least let me know you're using it...and if you feel compelled to...a blurb somewhere in your project that you're using SteelBattalion.NET.  I'm always interested in learning about new projects, especially ones that I contributed to...in the form of this library.

Thanks for using this project, and let me know if you have any questions.

Sincerely,
Joe Coutcher
http://www.geekteligence.com/



Setting up the machine to access the Steel Battalion controller:
-------------------------------------------------------------------

Install the LibUSB Filter Driver on your computer.  It is available here:
http://sourceforge.net/projects/libusb-win32/

Both 32 and 64-bit versions should be within the libusb-win32 download.  Install the version appropriate for your platform.

Plug in your Steel Battalion controller.  For Windows XP, Vista and 7, go to the Control Panel->Administrative Tools->Computer Management.  Go to the Device Manager, right click the unknown device that appeared, and click Update Drivers.  Select drivers from a specific location, and point it to the drivers folder in this project.  It will automatically install the appropriate LibUSB stub drivers for your platform.

Once it's installed, you can now use any program that utilizes SteelBattalion.NET.


Development libraries that need to be installed:
--------------------------------------------------

This project uses references from LibUSBDotNet, which you will need to download and install:
http://sourceforge.net/projects/libusbdotnet/

The first time you compile SteelBattalion.NET, it will probably mention that a reference is missing.  Once LibUSBDotNet is installed, add the DLL as a reference in the "SBC" project.

You can use either Visual Studio.NET as a development environment, or my favorite, SharpDevelop.  You can download SharpDevelop from:

http://icsharpcode.net/


Usage of SteelBattalion.NET:
-----------------------------

The sample project included demonstrates how to use the library.  If you have any additional questions, please post a message on the Discussion tab at the CodePlex site:

http://steelbattalionnet.codeplex.com/
