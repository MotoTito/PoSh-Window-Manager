# Add-Type @"

# ////<csscript>



# ////<references>



# ////<reference>System.Windows.Forms</reference>

# ////<reference>System.Drawing.Rectangle</reference>



# ////</references>



# ////</csscript>



# using System;

# using System.Windows.Forms;



# public class ScreenInfo{



# public static void GetScreenInfo(){

#     foreach (var screen in System.Windows.Forms.Screen.AllScreens)

#     {

#         Console.WriteLine("Device Name: " + screen.DeviceName);

#         Console.WriteLine("Bounds: " + 

#             screen.Bounds.ToString());

#         Console.WriteLine("Type: " + 

#             screen.GetType().ToString());

#         Console.WriteLine("Working Area: " + 

#             screen.WorkingArea.ToString());

#         Console.WriteLine("Primary Screen: " + 

#             screen.Primary.ToString());

#     }

# }



# public static void HelloWorld(){

#     Console.WriteLine("Hello World Method");

# }

# }

# "@ -ReferencedAssemblies @("System.Windows.Forms", "System.Drawing")
$CSCODE = [IO.File]::ReadAllText(".\program.cs")

Add-Type -TypeDefinition $CSCODE -Language CSharp -ReferencedAssemblies @("System.Windows.Forms", "System.Drawing")

#Call the ScreenInfo compiled object and the function GetScreenInfo

# [ScreenInfo]::HelloWorld()

# [ScreenInfo]::GetScreenInfo()
[position_windows]::Main()
# [position_windows]::GetFocusableAppsString()