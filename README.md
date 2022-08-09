
# LD48 Source

## Orientation

This is the source code for TwoWolf's LD48. This includes everything in the Assets folder including external plugins, packages, sprites, and external files such as Photoshop/Aesprite. All this does not go over 20mb so I did not bother.

Originally the raw source was on another repository. This was before I knew how to use git so I just compressed it and uploaded the entire thing. I managed to create another account, cleanup the source a bit, and create and push to another repository. The version control therefore starts from v2.0. There are still a lot of bugs to fix and code to maintain but the game is complete and runs just fine as it is

## Project Setup

Since this only contains the Assets folder, you need to setup the project manually yourself. There's a custom script within the folder which helps create Layers and manages misc project settings automatically (in the Logo Scene) but that may not be enough. I'm thinking of finding a way to set all my project settings this way, but for now you must go through the following process:

- Create project in Unity 2021.3.6f1, Core 2D
- Delete everything in the Assets folder and clone/pull
- Open Logo Scene. Please, read this and do this the first thing you clone this repository. This will setup your layer settings automatically
- Goto Edit -> Project Settings -> Physics 2D. Change Y gravity to -35. Change default material to SimplePlayerMaterial (use the explorer button, easy to find)
- Goto File -> Build Settings. Add all Scenes in this order: Logo, Game, and Stage Buffer
- In the Package Manager install the ShaderGraph package (for Curtains)


## Bugs to Fix
- Screen transition does not fit the screen
- Bomb # text does not show (only in web build)
- Amount of breakable stones does not reset upon death
- Tearing lines between tiles
- Camera will sometimes not pan with the character (gets stuck in either side of the map)
- Map pieces do not fit with each other sometimes
- Often a single map piece repeats itself indefinitely for no apparent reason
- Incorrect ceiling death physics and animation (physics fixed at v2.0)
- May increase screen size
- May decrease all item costs drastically
- May improve initial and/or elemetnary upgrade stat

