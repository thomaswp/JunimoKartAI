using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;

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
            if (game.gameState == MineCart.GameStates.Title) return Game1.random.Next(2) == 0;

            if (jumpCount > 0)
            {
                jumpCount--;
                return true;
            }
            else if (!game.player.IsGrounded()) return false;

            MineCart.Track current = game.GetTrackForXPosition(game.player.position.X);
            MineCart.Track inFront = game.GetTrackForXPosition(game.player.position.X + 300);
            if (current != null && inFront != null && inFront.position.Y != current.position.Y)
            {
                jumpCount = 5 - (int)(inFront.position.Y - current.position.Y) / 5;
                Console.WriteLine(jumpCount);
                return true;
            }
            return false;
        }
    }

    public class JumpAI : AI
    {
        int jumpCount = 0;
        Vector2 goal;
        MineCart.Track goalTrack;

        class Jump
        {
            internal float dx, dy, error;
            internal int jumpCount;

            public double similarity(Jump j)
            {
                return Math.Pow(dx - j.dx, 2) + Math.Pow(dy - j.dy, 2);
            }

            public override string ToString()
            {
                return String.Format("[{0:F2},{1:F2}]", dx, dy);
            }
        }

        List<Jump> goodJumps = new List<Jump>();
        Jump lastJump;
        MineCart.Track lastHighlight;

        public override bool IsPressed(MineCart game)
        {
            if (game.gameState == MineCart.GameStates.Title) return Game1.random.Next(2) == 0;

            var player = game.player;

            if (jumpCount > 0)
            {
                jumpCount--;
                return true;
            }

            // Died
            if (game.player.position.Y == -1000f)
            {
                //recordError(player, game.screenHeight);
                return false;
            }

            if (!player.IsGrounded())
            {
                // Passed goal in air
                if (player.position.X >= goal.X)
                {
                    //recordError(player, player.position.Y);
                }
                return false;
            }

            // Is on a track
            //recordError(player);

            MineCart.Track current = game.GetTrackForXPosition(game.player.position.X);
            if (current == goalTrack && lastJump != null)
            {
                lastJump.error = Math.Abs(goalTrack.position.X + 8 - player.position.X);
                goodJumps.Add(lastJump);
                Console.WriteLine("Success!");
                lastJump = null;
            }


            MineCart.Track inFront = game.GetTrackForXPosition(game.player.position.X + 120);
            if (current != null && inFront != null)
            {
                if (lastHighlight != null) lastHighlight.highlighted = false;
                inFront.highlight();
                lastHighlight = inFront;
                float dx = inFront.position.X - current.position.X;
                float dy = inFront.position.Y - current.position.Y;
                dx *= SCALE;
                dy *= SCALE;
                goal = inFront.position;
                goalTrack = inFront;
                lastJump = new Jump() { dx = dx, dy = dy };
                jumpCount = getJumpCount(dx, dy);
                lastJump.jumpCount = jumpCount;
                //Console.WriteLine(jumpCount);
                return true;
            }
            return false;
        }

        private void recordError(MineCart.MineCartCharacter player)
        {
            recordError(player, player.position.Y);
        }

        const float SCALE = 0.01f;
        const float MAX_ERROR = 0.1f;

        private void recordError(MineCart.MineCartCharacter player, float y)
        {
            if (lastJump == null) return;
            float ex = goal.X + 8 - player.position.X;
            float ey = y - goal.Y;
            ex *= SCALE;
            ey *= SCALE;
            float error = Math.Abs(ex) + Math.Abs(ey);
            if (Math.Abs(error) < MAX_ERROR)
            {
                Console.WriteLine(string.Format("Error {0}: {1} + {2} => {3}", 
                    lastJump, ex, ey, error));
                lastJump.error = error;
                goodJumps.Add(lastJump);
            }
            lastJump = null;
        }

        private int getJumpCount(float dx, float dy)
        {
            //double delta = 1;
            //double wdx = 25, wdy = 2;
            //foreach (Jump jump in jumps)
            //{
            //    wdx += jump.dx * jump.error * delta / jumps.Count;
            //    wdy += jump.dy * jump.error * delta / jumps.Count;
            //}
            //int jumpCount = (int)Math.Round(wdx * dx + wdy * dy);


            int rand = Game1.random.Next(1, 120);
            if (Game1.random.Next(0, 10) == 0) return rand;
            if (goodJumps.Count == 0) return rand;

            Jump best = goodJumps[0];
            double bestDis = double.MaxValue;
            double bestError = double.MaxValue;
            for (int i = 1; i < goodJumps.Count; i++)
            {
                Jump candidate = goodJumps[i];
                double distance = candidate.similarity(lastJump);
                float error = Math.Abs(candidate.error);
                if (distance < bestDis || (distance == bestDis && error < bestError))
                {
                    bestDis = distance;
                    bestError = error;
                    best = candidate;
                    if (distance == 0 && error == 0) break;
                }
            }
            Console.WriteLine(string.Format("For {0} found {1} at dis {2}", 
                lastJump, best.jumpCount, bestDis));
            if (bestDis > 0.05) return rand;

            return best.jumpCount;
            //Console.WriteLine($"Weights: {wdx:F2}*{dx:F2} + {wdy:F2}*{dy:F2} => {jumpCount}");
            //return jumpCount;
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
