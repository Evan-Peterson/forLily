using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            var existingCueDef = Game1.soundBank.GetCueDefinition("cat");

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

            // print button presses to the console window
            //this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);

            if(e.Button == SButton.MouseRight)
            {
                this.Monitor.Log($"MOUSE RIGHT DETECTED", LogLevel.Debug);
                this.Monitor.Log($"{this.Helper.Input.GetCursorPosition().GrabTile}", LogLevel.Debug);
                this.Monitor.Log($"{this.Helper.Input.GetCursorPosition().ScreenPixels}", LogLevel.Debug);

                foreach(NPC character in Utility.getAllPets())
                {
                    this.Monitor.Log($"{character.currentLocation}", LogLevel.Debug);
                    this.Monitor.Log($"{character.lastPosition}", LogLevel.Debug);
                }
            }
        }
    }
}