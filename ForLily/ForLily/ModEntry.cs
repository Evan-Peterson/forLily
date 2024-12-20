using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Locations;

namespace ForLily
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {

        private List<DateTime> clicks = new List<DateTime>();
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            var existingCueDef = Game1.soundBank.GetCueDefinition("cat");
            var purrCue = new CueDefinition();
            purrCue.name = "purr";
            purrCue.instanceLimit = 1;
            purrCue.limitBehavior = CueDefinition.LimitBehavior.ReplaceOldest;

            SoundEffect purrAudio;
            string purrPath = Path.Combine(this.Helper.DirectoryPath, "cutPurr.wav");
            using (var stream = new System.IO.FileStream(purrPath, System.IO.FileMode.Open))
            {
                purrAudio = SoundEffect.FromStream(stream);
            }

            purrCue.SetSound(purrAudio, Game1.audioEngine.GetCategoryIndex("Sound"), false);

            Game1.soundBank.AddCue(purrCue);

            SoundEffect audio;
            string filePathCombined = Path.Combine(this.Helper.DirectoryPath, "cutMeow.wav");
            using (var stream = new System.IO.FileStream(filePathCombined, System.IO.FileMode.Open))
            {
                audio = SoundEffect.FromStream(stream);
            }

            existingCueDef.SetSound(audio, Game1.audioEngine.GetCategoryIndex("Sound"), false);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;


            if(e.Button == SButton.MouseRight)
            {

                this.Monitor.Log($"Cursor Tile: {this.Helper.Input.GetCursorPosition().GrabTile}", LogLevel.Debug);
                Vector2 pos = this.Helper.Input.GetCursorPosition().ScreenPixels;
                Vector2 playerTile = this.Helper.Input.GetCursorPosition().GrabTile;

                foreach (Pet character in Utility.getAllPets())
                {
                    this.Monitor.Log($"Pet Tile: {character.Tile}", LogLevel.Debug);

                    
                    if(character.petType.Equals(new NetString("Cat")) && character.whichBreed.Equals(new NetString("0")))
                    {
                        this.Monitor.Log($"CAT DETECTED", LogLevel.Debug);
                        Vector2 catTile = character.Tile;

                        if(Math.Abs(catTile.X - playerTile.X) <= 2 && Math.Abs(catTile.Y - playerTile.Y) <= 2)
                        {
                            this.Monitor.Log($"HIT DETECTED", LogLevel.Debug);

                            DateTime currentTime = DateTime.Now;

                            for (int i = clicks.Count - 1; i >= 0; i--)
                            {
                                TimeSpan age = currentTime.Subtract(clicks[i]);
                                if (age.Seconds >= 5)
                                {
                                    clicks.RemoveAt(i);
                                }
                            }

                            clicks.Add(DateTime.Now);

                            this.Monitor.Log($"Length: {clicks.Count}", LogLevel.Debug);


                            if (clicks.Count >= 3)
                            {
                                Game1.playSound("purr");
                            }

                        }
                    }

                }
            }
        }
    }
}