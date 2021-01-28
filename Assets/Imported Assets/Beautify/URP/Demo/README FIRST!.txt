Beautify for Universal Rendering Pipeline!

Requirements:
- Unity 2019.3 or later
- Universal RP 7.2.0 or later (install it from Package Manager)

Read the PDF quick start guide for details or check this video: https://youtu.be/6fpeiysj6KM

To use Beautify, add Beautify override to a Post Processing Volume and customize!


Change log:


Version 1.6
- Added "Vignetting Blink Style" option
- Added "Vignetting Center" option
- Added "Bloom Near Attenuation" option
- Added "Anamorphic Flares Near Attenuation" option
- Added new debug layers to Debug View

Version 1.5
- Added Depth Of Field Transparent Support option

Version 1.4
- Added Sun Flares "Occlusion Layer Mask" option
- Added Sun Flares "Attenuation Speed" (works with Occlusion Layer Mask option)
- [Fix] Fixed an issue that could produce Beautify to use a disabled camera when computing Sun Flares effect

Version 1.3.1 15/NOV/2020
- Improved compatibility with URP 10.1
- [Fix] Fixed an issue that prevents correct shader keyword stripping (ie. cloud build)

Version 1.3 18/OCT/2020
- Added "Bloom Exclusion Mask" option
- Added new demo scene "LUT Blending"
- [Fix] Fixed regression which disabled sharpen in build

Version 1.2.2 23/SEP/2020
- Optimized scriptable render pass initialization

Version 1.2.1 31/AGO/2020
- Support for LUT textures of 256x8 size
- [Fix] Fixed DoF material memory leak

Version 1.2 24/JUL/2020
- Added bloom color tint option under "Customize Bloom" section
- [Fix] Inspector fixes

Version 1.1 28/MAY/2020
- Added "Downscaling" option to Optimizations section

Version 1.0.2 19/MAY/2020
- Added Depth of Field "Distance Shift" parameter

Version 1.0.1 1/MAY/2020
- [Fix] Fixed max clamp values for some sharpen parameters

Version 1.0 April/2020
- Tested on Windows, Mac, Android.
- Added VR Single Pass Stereo support (tested with Oculus Quest)
- Added Beautify and Unity Post Processing build optimization options
- Added Best Performance Mode
- Added Final Blur effect
- Added White Balance color grading option
- Added Night Vision effect
- Added "Direct Write To Camera" option in Performance section


