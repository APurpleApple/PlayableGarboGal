using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using PlayableGarboGal.Cards;

namespace PlayableGarboGal
{
    public class Mod : IModManifest, ISpriteManifest, ICharacterManifest, IDeckManifest, IAnimationManifest, ICardManifest, IStatusManifest, IGlossaryManifest, IArtifactManifest, IStoryManifest, IStartershipManifest, IShipManifest, IShipPartManifest
    {
        public IEnumerable<DependencyEntry> Dependencies { get { yield break; } }
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }

        public string Name => "PlayableGarboGirl";

        public static Dictionary<string, ExternalSprite> extSprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, Spr> sprites = new Dictionary<string, Spr>();
        public static Dictionary<string, ExternalStatus> extStatuses = new Dictionary<string, ExternalStatus>();
        public static Dictionary<string, Status> statuses = new Dictionary<string, Status>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        public static Dictionary<string, ExternalGlossary> glossaries = new Dictionary<string, ExternalGlossary>();
        public static Dictionary<string, ExternalAnimation> animations = new Dictionary<string, ExternalAnimation>();
        public static Dictionary<string, ExternalArtifact> artifacts = new Dictionary<string, ExternalArtifact>();
        public static Dictionary<string, ExternalCard> cards = new Dictionary<string, ExternalCard>();

        public static ExternalDeck? Deck { get; private set; }
        public static ExternalCharacter? Character { get; private set; }

        public static System.Drawing.Color DeckColor = System.Drawing.Color.FromArgb(210, 103, 198);
        public static System.Drawing.Color TitleColor = System.Drawing.Color.FromArgb(210, 103, 198);

        public void BootMod(IModLoaderContact contact)
        {
            Harmony harmony = new Harmony(Name);
            harmony.PatchAll();
        }


        private void RegisterSprite(string subfile, string fileName, string name, ISpriteRegistry artRegistry)
        {
            var file_name = Path.Combine(ModRootFolder?.FullName ?? "", "Sprites", subfile, fileName);
            var externalSprite = new ExternalSprite(Name + ".Sprite." + name, new FileInfo(file_name));
            artRegistry.RegisterArt(externalSprite);
            extSprites.Add(name, externalSprite);
            sprites.Add(name, (Spr)externalSprite.Id);
        }

        private void RegisterAnimation(string name, string tag, ExternalDeck deck, ExternalSprite[] sprites, IAnimationRegistry registry)
        {
            Console.WriteLine("Registering Anim " + name);

            ExternalAnimation anim = new ExternalAnimation(Name + ".Animation." + name, deck, tag, false, sprites);
            registry.RegisterAnimation(anim);
            animations.Add(name, anim);
        }

        private void RegisterCard(string name, Type cardType, ExternalSprite art, ExternalDeck? deck, string descName, ICardRegistry registry)
        {
            ExternalCard card = new ExternalCard(Name + ".Card." + name, cardType, art, deck);
            card.AddLocalisation(descName);
            registry.RegisterCard(card);
            cards.Add(name, card);
        }

        private void RegisterArtifact(string name, Type artifactType, ExternalSprite sprite, string descName, string desc, IArtifactRegistry registry)
        {
            ExternalArtifact artifact = new ExternalArtifact(Name + ".Artifact." + name, artifactType, sprite);
            artifact.AddLocalisation(descName, desc);
            registry.RegisterArtifact(artifact);
            artifacts.Add(name, artifact);
        }

        private void RegisterPart(string name, Part part, ExternalSprite on, ExternalSprite? off, IShipPartRegistry registry)
        {
            part.skin = "@mod_part:" + Name + ".Part." + name;
            ExternalPart newPart = new ExternalPart(Name + ".Part." + name, part, on, off);
            registry.RegisterPart(newPart);
            parts.Add(name, newPart);
        }

        private void RegisterGlossary(string name, ExternalGlossary.GlossayType type, ExternalSprite icon, string descName, string desc, string? altDesc, IGlossaryRegisty registry)
        {
            ExternalGlossary glossary = new ExternalGlossary(Name + ".Glossary." + name, name, false, type, icon);
            glossary.AddLocalisation("en", descName, desc, altDesc);
            registry.RegisterGlossary(glossary);
            glossaries.Add(name, glossary);
        }

        private void RegisterStatus(string name, bool isGood, System.Drawing.Color mainColor, System.Drawing.Color borderColor, ExternalSprite icon, bool affectedByTimestop, string descName, string desc, IStatusRegistry registry)
        {
            ExternalStatus status = new ExternalStatus(Name + ".Status." + name, isGood, mainColor, borderColor, icon, affectedByTimestop);
            status.AddLocalisation(descName, desc);
            registry.RegisterStatus(status);
            statuses.Add(name, (Status)status.Id);
            extStatuses.Add(name, status);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            RegisterSprite("Characters", "garbogirl_mini.png", "Mini", artRegistry);
            RegisterSprite("Characters", "garbogirl_neutral_0.png", "Neutral0", artRegistry);
            RegisterSprite("Characters", "garbogirl_neutral_1.png", "Neutral1", artRegistry);
            RegisterSprite("Characters", "garbogirl_neutral_2.png", "Neutral2", artRegistry);
            RegisterSprite("Characters", "garbogirl_neutral_3.png", "Neutral3", artRegistry);
            RegisterSprite("Characters", "garbogirl_neutral_4.png", "Neutral4", artRegistry);
            RegisterSprite("Characters", "panel.png", "Panel", artRegistry);

            RegisterSprite("Cards", "border_default.png", "BorderDef", artRegistry);
            RegisterSprite("Cards", "back_default.png", "BackDef", artRegistry);
        }

        public void LoadManifest(ICharacterRegistry registry)
        {
            Character = new ExternalCharacter(Name + ".Character", Deck, extSprites["Panel"], new Type[0], new Type[0], animations["Neutral"], animations["Mini"]);
            Character.AddNameLocalisation("GarboGirl");
            Character.AddDescLocalisation("TODO");
            registry.RegisterCharacter(Character);
        }

        public void LoadManifest(IDeckRegistry registry)
        {
            Deck = new ExternalDeck(Name + ".Deck", DeckColor, TitleColor, extSprites["BackDef"], extSprites["BorderDef"], null);
            registry.RegisterDeck(Deck);
        }

        public void LoadManifest(IAnimationRegistry registry)
        {
            RegisterAnimation("Mini", "mini", Deck, new ExternalSprite[] { extSprites["Mini"] }, registry);
            RegisterAnimation("Neutral", "neutral", Deck, new ExternalSprite[] {
                extSprites["Neutral0"],
                extSprites["Neutral1"],
                extSprites["Neutral2"],
                extSprites["Neutral3"],
                extSprites["Neutral4"]
            }, registry);
        }

        public void LoadManifest(ICardRegistry registry)
        {
            RegisterCard("TestCard", typeof(TestCard), extSprites["BackDef"], Deck, "Add Cannon", registry);
        }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {
        }

        public void LoadManifest(IGlossaryRegisty registry)
        {
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
        }

        public void LoadManifest(IStoryRegistry storyRegistry)
        {
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
        }
        public void LoadManifest(IStartershipRegistry registry)
        {
        }
        public void LoadManifest(IShipPartRegistry registry)
        {
            RegisterPart("TempCannon", new TempPart()
            {
                type = PType.cannon,
            },
            ExternalSprite.GetRaw((int)Spr.parts_cannon_rust),
            ExternalSprite.GetRaw((int)Spr.parts_cannon_rust),
            registry);
        }
    }
}
