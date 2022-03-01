using JunimoKart;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StardewValley
{
	public class Utility
	{
		public static float Clamp(float value, float min, float max)
		{
			if (max < min)
			{
				float num = min;
				min = max;
				max = num;
			}
			if (value < min)
			{
				value = min;
			}
			if (value > max)
			{
				value = max;
			}
			return value;
		}

		public static Color MakeCompletelyOpaque(Color color)
		{
			if (color.A >= byte.MaxValue)
			{
				return color;
			}
			color.A = byte.MaxValue;
			return color;
		}

		public static int Clamp(int value, int min, int max)
		{
			if (max < min)
			{
				int num = min;
				min = max;
				max = num;
			}
			if (value < min)
			{
				value = min;
			}
			if (value > max)
			{
				value = max;
			}
			return value;
		}


		public static float Lerp(float a, float b, float t)
		{
			return a + t * (b - a);
		}

		public static float MoveTowards(float from, float to, float delta)
		{
			if (Math.Abs(to - from) <= delta)
			{
				return to;
			}
			return from + (float)Math.Sign(to - from) * delta;
		}

		public static float RandomFloat(float min, float max, Random random = null)
		{
			if (random == null)
			{
				random = Game1.random;
			}
			return Lerp(min, max, (float)random.NextDouble());
		}

		public static Point Vector2ToPoint(Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		public static Vector2 PointToVector2(Point p)
		{
			return new Vector2(p.X, p.Y);
		}

		public static Color[] PRISMATIC_COLORS = new Color[6]
		{
			Color.Red,
			new Color(255, 120, 0),
			new Color(255, 217, 0),
			Color.Lime,
			Color.Cyan,
			Color.Violet
		};

		public static Color GetPrismaticColor(int offset = 0, float speedMultiplier = 1f)
		{
			float interval = 1500f;
			int current_index = ((int)((float)Game1.currentGameTime.TotalGameTime.TotalMilliseconds * speedMultiplier / interval) + offset) % PRISMATIC_COLORS.Length;
			int next_index = (current_index + 1) % PRISMATIC_COLORS.Length;
			float position = (float)Game1.currentGameTime.TotalGameTime.TotalMilliseconds * speedMultiplier / interval % 1f;
			Color prismatic_color = default(Color);
			prismatic_color.R = (byte)(Lerp((float)(int)PRISMATIC_COLORS[current_index].R / 255f, (float)(int)PRISMATIC_COLORS[next_index].R / 255f, position) * 255f);
			prismatic_color.G = (byte)(Lerp((float)(int)PRISMATIC_COLORS[current_index].G / 255f, (float)(int)PRISMATIC_COLORS[next_index].G / 255f, position) * 255f);
			prismatic_color.B = (byte)(Lerp((float)(int)PRISMATIC_COLORS[current_index].B / 255f, (float)(int)PRISMATIC_COLORS[next_index].B / 255f, position) * 255f);
			prismatic_color.A = (byte)(Lerp((float)(int)PRISMATIC_COLORS[current_index].A / 255f, (float)(int)PRISMATIC_COLORS[next_index].A / 255f, position) * 255f);
			return prismatic_color;
		}

		public static Vector2 getRandomPositionInThisRectangle(Microsoft.Xna.Framework.Rectangle r, Random random)
		{
			return new Vector2(random.Next(r.X, r.X + r.Width), random.Next(r.Y, r.Y + r.Height));
		}

		public static readonly RasterizerState ScissorEnabled = new RasterizerState
		{
			ScissorTestEnable = true
		};

		public static Microsoft.Xna.Framework.Rectangle ConstrainScissorRectToScreen(Microsoft.Xna.Framework.Rectangle scissor_rect)
		{
			int amount_to_trim5 = 0;
			if (scissor_rect.Top < 0)
			{
				amount_to_trim5 = -scissor_rect.Top;
				scissor_rect.Height -= amount_to_trim5;
				scissor_rect.Y += amount_to_trim5;
			}
			if (scissor_rect.Bottom > Game1.viewport.Height)
			{
				amount_to_trim5 = scissor_rect.Bottom - Game1.viewport.Height;
				scissor_rect.Height -= amount_to_trim5;
			}
			if (scissor_rect.Left < 0)
			{
				amount_to_trim5 = -scissor_rect.Left;
				scissor_rect.Width -= amount_to_trim5;
				scissor_rect.X += amount_to_trim5;
			}
			if (scissor_rect.Right > Game1.viewport.Width)
			{
				amount_to_trim5 = scissor_rect.Right - Game1.viewport.Width;
				scissor_rect.Width -= amount_to_trim5;
			}
			return scissor_rect;
		}

        internal static void CollectGarbage()
        {
            
        }

		public static T GetRandom<T>(List<T> list, Random random = null)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			if (random == null)
			{
				random = Game1.random;
			}
			return list[random.Next(list.Count)];
		}
	}
}
