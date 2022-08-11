
# LD48 Source - Title: JUST DIG

## Orientation
This is the source code for TwoWolf's Ludum Dare 48th Entry. This includes everything in the Assets folder including external plugins, packages, sprites, and external files such as Photoshop/Aesprite. All this does not go over 20mb so I did not bother

Originally the raw source was on another repository. This was before I knew how to use git so I just compressed it and uploaded the entire thing. I managed to create another account, cleanup the source a bit, and create and push to another repository. The version control therefore starts from v2.0. There are still a lot of bugs to fix and code to maintain but the game is complete and runs just fine as it is

## Project Setup
Since this only contains the Assets folder, you need to setup the project manually yourself. There's a custom script within the folder which helps create Layers and manages misc project settings automatically (in the Logo Scene) but that may not be enough. I'm thinking of finding a way to set all my project settings this way, but for now you must go through the following process:

- Create project in Unity 2021.3.6f1, Core 2D
- Delete everything in the Assets folder and clone/pull
- Open Logo Scene. Please, read this and do this the first thing you clone this repository. This will setup your layer settings automatically
- Goto Edit -> Project Settings -> Physics 2D. Change Y gravity to -45. Change default material to SimplePlayerMaterial (use the explorer button, easy to find)
- Goto File -> Build Settings. Add all Scenes in this order: Logo, Game, and Stage Buffer
- In the Package Manager install the ShaderGraph package (for Curtains)
- Default setup for WebGL build (turn off compression, lightmap data, auto webgl version target, etc)

## Fixed Bugs & Changes
Since the original was made in 2 days it was full of bugs. Here's a list of bugs I managed to fix and other changes made after the jam

### Bugs
- Screen transition (i.e. curtain) now fits the screen
- UI now up to scale in fullscreen mode 
- Bomb # text now shows in web build
- Fixed tearing lines between tiles
- Fixed bug where camera gets stuck in gets stuck in either side of the map
- Correct ceiling death physics and animation
- Map generation now works as intended (does not repeat, almost always fits)
- Tiles and items are now optimized and garbage collected offscreen (FPS fix)

### Changes
- Increased screen size

### Bugs to Fix & Changes to Make (TEMP)
- May decrease all item costs drastically
- May improve initial and/or elementary upgrade stat
- Audio overlapping when returning out of window focus (important)

