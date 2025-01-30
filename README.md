I am writing logic and making simple UI with unity tool kit for a turn based text adventure. I have been writing in / learning C# for three or so months.

The game so far has logic for character creation, choosing directions to travel, deciding the result of arriving at different locations and a combat loop which is triggered if a destination is marked "hostile."
//Scripts + description//
DungeonMaster is on an empty game object with other important scripts. It is the MAIN thread and interface between most other scripts.

StatsHandler is a monobehavior attached to all "combatants" which may be enemies, companions, players and enemies.

Map generates a random map, marking locations with different Enums which are used to determine result of arriving at said point.

TravelScript handles travel.

CombatFlow handles the combat cycle.

KragostiosEnums is all of my enums.

PlayerOptions handles the Ui presentation of procedurally generated options, ie travel direction, abilities (attacks, buffs, etc) and target selection.

NarrationScript handles the narration displayed on screen.

Abilities_SO and Item_SO are scriptable objects scripts.

AbilityLibrary is a script that holds Ability scriptable objects are held in a library with related useful functions for finding various abilities or ability categories.

KDebug is my debug script for togglig debug logs on anf off.

//plans/ upcoming branch categories//

I need an inventory system and an inventory screen. The mechanic for looting gold and xp will be expanded easily to hold whatever items enemies or dead companions drop.

I need a merchant system.

TO ALL BADASS PROGRAMMERS: I would like to have two additional layers to my map, both of which were mind-blowingly complex for me to wrap my head around. Currently the map is an array of Vector2s, starting at -mapsizeX,-mapsizeY expanding to +mapsozeX, +mapsizeY. At each integer coordinate there can be 1 of about 12 locationTypes enumerated in my enum script (KragostiosEnums). I would like to generate on top of that random Biome regions that are surfaces instead of points. Additionally and not necessarily coinciding with the biome surfaces I would like to add Kingdome surfaces so that a point can have a village, but be inside of a the whatever kingdom and inside of a jungle. Obviously the code for one is reusable for the other. If anyone would like to do this or give a pseudo code outline, I take off my hat to you.
