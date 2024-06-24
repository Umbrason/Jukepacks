namespace MinecraftJukeboxPackCreator;

/*
    what does this do?
    - takes a bunch of audio files in any format thats supported by ffmpeg?
    - take in an album name
    - save them as a minecraft data & resourcepack
        Datapack:
        - generate jukebox_song jsons
        - generate ogg files in correct locations <- mono files seem to be important?!
        - modify (creeper) loot-table(s) to contain new music discs
            - wandering trader sells music discs?
        Resourcepack:
        - generate translation files
        - generate sound event json
        https://bugs.mojang.com/browse/MC-272786 good example of a config

*/
static class MinecraftJukeboxPackCreator
{
    public static MinecraftJukeboxPackCreatorSettings Settings = new();
    [STAThread]
    static void Main()
    {
        /*
        whats going in, whats coming out of this?    
        INPUT:
         - MP3/OGG/WAV files         
        OUTPUT:
         - Datapack containing JSON files
         - Resourcepack containing Mono-OGG files and JSON for sound events & translation entry
        */

        ApplicationConfiguration.Initialize();
        Application.Run(new MainGUI());
        Settings.Save();
    }
}
