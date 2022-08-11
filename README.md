
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
- Rearrange sorting order for all game elements including items and UI
- Screen transition (i.e. curtain) now fits the screen
- UI is now up to scale in fullscreen (currently 1024 x 768)
- Bomb # text now shows in web build
- Fixed tearing lines between tiles
- Fixed bug where camera gets stuck in gets stuck in either side of the map
- Prevent Mole controls before and after Overworld/Playing state
- Correct ceiling death physics and animation
- Map generation now works as intended (does not repeat, almost always fits)
- Tiles and items are now optimized and garbage collected offscreen (FPS fix)
- Stop dual bgm sources from overlapping when returning out of window focus
- Breakable stones resets count upon death with Game digLevel = 3, as intended
- Prevented coins from spawning in hard blocks in the beginning
- Prevented occasional late map generation breaking immersion
  
### Changes
- Increased screen size (1024 x 768)
- Optimized map procedural generation algorithm from O(logN) to O(N) 
- Disable items back to pools so they can be recycled
- Created a Garbage Collector above game camera which disables pool items and erase tiles
  (This also fixed FPS and Mole from occasionally bouncing off the low end of the camera)

### Changes to Make (TEMP)
- May decrease all item costs drastically
- May improve initial and/or elementary upgrade stat

