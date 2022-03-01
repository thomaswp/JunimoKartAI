using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;
using static StardewValley.Minigames.MineCart;

namespace JunimoKart
{
    public abstract class AI
    {
        public abstract bool IsPressed(MineCart game);
    }

    public class RandomAI : AI
    {
        public override bool IsPressed(MineCart game)
        {
            return Game1.random.Next(2) == 0;
        }
    }

    public class SimpleAI : AI
    {
        int jumpCount = 0;

        public override bool IsPressed(MineCart game)
        {
            if (game.gameState == GameStates.Title) return Game1.random.Next(2) == 0;

            if (jumpCount > 0)
            {
                jumpCount--;
                return true;
            }
            else if (!game.player.IsGrounded()) return false;

            Track current = game.GetTrackForXPosition(game.player.position.X);
            Track inFront = game.GetTrackForXPosition(game.player.position.X + 300);
            if (current != null && inFront != null && inFront.position.Y != current.position.Y)
            {
                jumpCount = 5 - (int)(inFront.position.Y - current.position.Y) / 5;
                Console.WriteLine(jumpCount);
                return true;
            }
            return false;
        }
    }

    public class PlayerAI : AI
    {
        public override bool IsPressed(MineCart game)
        {
            if (Game1.input.GetMouseState().LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            if (Game1.isOneOfTheseKeysDown(Game1.input.GetKeyboardState(), Keys.Space) || Game1.isOneOfTheseKeysDown(Game1.input.GetKeyboardState(), Keys.Enter) || Game1.input.GetKeyboardState().IsKeyDown(Keys.Space) || Game1.input.GetKeyboardState().IsKeyDown(Keys.LeftShift))
            {
                return true;
            }
            if (Game1.input.GetGamePadState().IsButtonDown(Buttons.A) || Game1.input.GetGamePadState().IsButtonDown(Buttons.B))
            {
                return true;
            }
            return false;
        }
    }

    class GameState
    {
        List<MineCart.Track> tracks;

    }
}
