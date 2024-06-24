# Jukeþσcks
Resource & Datapack authoring tool for minecraft 1.21

## What are Jukeþσcks?
A Jukeþσck represents a collection of custom music discs and consists of two parts:
### resourcepack
The generated resourcepack (rp) is required for each player to hear the new music discs.  
if a player does not have the resource pack installed, they will still be able to join your server and collect the new discs,  
but they will not hear the new music when the disc is playing.
### datapack
The datapack (dp) is required only on the server. The server handles registering the new songs and just tells clients which sound event to play.  
Due to a [bug](https://bugs.mojang.com/browse/MC-273807) in minecraft 1.21 you will have to restart the server after adding a new datapack for the new jukebox songs to work properly.  
(for singleplayer just going back to the main menu is enough, as that stops the worlds server)

## Requirements
Jukeþσcks requires [ffmpeg](https://ffmpeg.org/) to convert audio files to .ogg  
The first time you use Jukeþσcks you'll need to set the path to a [ffmpeg](https://ffmpeg.org/) executable via the Burger Menu (☰).

## How To: Create a Jukeþσck
To create a new Jukeþσck open the Jukeþσcks application and just drop the audiofiles you want to be part of your pack into the application.
The music disc should lower itself into the jukebox and start spinning. Jukeþσck then crawls through any subdirectories and finds all audio files in your dropped files.
Once it has finished searching, you'll be prompted to pick an empty folder to save the data-/resourcepacks into.

## How To: Install a Jukeþσck
Once you've created a Jukeþσck you can install it into a world by dragging the generated datapack (dp) into the worlds ``datapacks/`` folder. 
Then you'll have to restart the server-instance once due to a [bug](https://bugs.mojang.com/browse/MC-273807) in the ``jukebox_song`` registry.
Once the bug is fixed it should be enough to enable the datapack via ``/datapack enable <datapack-name>``

## How To: Acquire your Custom Discs
#### via wandering trader:
After installing the datapack part of your Jukeþσck(s) wandering traders going to have a new trade offering a bundle, containing all the discs from one Jukeþσck.
These Bundles only cost a single emerald as of now, but I'll probably increase the price and/or make it configurable via the Burger Menu (☰).

#### via commands:
If you want to get a music disc playing your custom song via commands you can use the ``jukebox_playable`` item component introduced in version 1.21:  
``/give @s music_disc_wait[jukebox_playable={song:"<pack_name>:<song_name>"}]``  
where ``pack_name`` is the name of the folder you chose to save your Jukeþσck into in [snake_case](https://en.wikipedia.org/wiki/Snake_case)  
and ``song_name`` is the name of the song's source file also written in [snake_case](https://en.wikipedia.org/wiki/Snake_case).

#### via loot tables:
Sadly, due to datapack loot-tables overwriting each other it is (as of now, to the best of my knowledge) not possible / feasible to have the new music discs drop from traditional sources, 
as it would ruin compatibility with other datapacks, including other Jukeþσcks.

If compatibility is not an issue for your usecase you can write your own [loot_tables](https://minecraft.fandom.com/wiki/Loot_table) and add the custom discs manually.

## Compatibility
All packs generated by Jukeþσcks are compatible with other Jukeþσcks.
Other datapacks should also not interfere with these packs, as Jukeþσcks dont use loot_tables to optain new music discs.
