using MinecraftJukeboxPackCreator.util;

namespace MinecraftJukeboxPackCreator.packcontents
{
    public class HardcodedDatapackFiles
    {
        string Name { get; }
        List<JukeboxSong> Songs { get; }

        public HardcodedDatapackFiles(string name, List<JukeboxSong> songs)
        {
            this.Name = name;
            this.Songs = songs;
        }

        public Dictionary<string, string> files => new()
    {
        //functions
        {@$"{Name.SnakeCase()}/function/umbrason_custom_album_shared/add_album_trade_self.mcfunction",
@$"
function {Name.SnakeCase()}:umbrason_custom_album_shared/invoke_modify_wandering_trader_with_random_album with storage albums:wandering_trader_offers macro_count
tag @s add album_trade_added
"},
        {@$"{Name.SnakeCase()}/function/umbrason_custom_album_shared/bootstrap.mcfunction", @$"
data modify storage albums:wandering_trader_offers macro_count set value {{value:-1}}
data remove storage albums:wandering_trader_offers macros
data remove storage albums:wandering_trader_offers index
"},
        {@$"{Name.SnakeCase()}/function/umbrason_custom_album_shared/invoke_modify_wandering_trader_with_random_album.mcfunction", @$"
$execute store result storage albums:wandering_trader_offers index int 1 run random value 0..$(value)
function {Name.SnakeCase()}:umbrason_custom_album_shared/invoke_modify_wandering_trades_from_storage_at_index with storage albums:wandering_trader_offers
"},
        {@$"{Name.SnakeCase()}/function/umbrason_custom_album_shared/invoke_modify_wandering_trades_from_storage_at_index.mcfunction", @$"
$function {Name.SnakeCase()}:umbrason_custom_album_shared/modify_wandering_trader_offers with storage albums:wandering_trader_offers macros[$(index)]
"},
        {@$"{Name.SnakeCase()}/function/umbrason_custom_album_shared/modify_wandering_trader_offers.mcfunction", @$"
$data modify entity @s Offers.Recipes append value {{maxUses:1,demand:1,buy:{{id:emerald,count:1}},sell:{{""id"": ""minecraft:bundle"",""count"": 1,""components"": {{""minecraft:item_name"":'{{""text"":""$(name)""}}', ""minecraft:bundle_contents"": $(bundle_contents)}}}}}}
"},
        {@$"{Name.SnakeCase()}/function/umbrason_custom_album_shared/tick.mcfunction", @$"
execute as @e[type=minecraft:wandering_trader, tag=!album_trade_added] run function {Name.SnakeCase()}:umbrason_custom_album_shared/add_album_trade_self
"},
        {@$"{Name.SnakeCase()}/function/fill_scoreboard.mcfunction", @$"
$scoreboard players set _ albums.wandering_trader_offers.macro_count $(value)
"},
        {@$"{Name.SnakeCase()}/function/increment_album_count.mcfunction", @$"
scoreboard objectives add albums.wandering_trader_offers.macro_count dummy
function {Name.SnakeCase()}:fill_scoreboard with storage albums:wandering_trader_offers macro_count
execute store result storage albums:wandering_trader_offers macro_count.value int 1 run scoreboard players add _ albums.wandering_trader_offers.macro_count 1 
scoreboard objectives remove albums.wandering_trader_offers.macro_count 
"},
        {@$"{Name.SnakeCase()}/function/init.mcfunction", @$"
function {Name.SnakeCase()}:increment_album_count
data modify storage albums:wandering_trader_offers macros append value {{""name"":""{Name.SnakeCase()}"",""bundle_contents"":[{string.Join(',', Songs.Select(s => s.DiscItemJSON(Name.SnakeCase())))}]}}
"},
        //tags
        {@$"minecraft/tags/function/umbrason_custom_album_shared/tick.json", @$"
{{
    ""values"":[        
        ""{Name.SnakeCase()}:umbrason_custom_album_shared/tick""
    ]
}}
"},
        {@$"minecraft/tags/function/bootstrap.json", @$"
{{
    ""values"":[        
        ""{Name.SnakeCase()}:umbrason_custom_album_shared/bootstrap""
    ]
}}
"},
        {@$"minecraft/tags/function/load.json", @$"
{{
    ""values"":[        
        ""#minecraft:bootstrap"",
        ""{Name.SnakeCase()}:init""
    ]
}}
"},
        {@$"minecraft/tags/function/tick.json", @$"
{{
    ""values"":[        
        ""#minecraft:umbrason_custom_album_shared/tick""
    ]
}}
"},
    };
    }
}