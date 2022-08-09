using UnityEngine;
using Verse;

namespace ColonistsDeco
{
	public class Dialog_Inspect : Window
	{
		private readonly TaggedString _text;

		private readonly Texture2D _image;

		private readonly string _title;

		private Vector2 _scrollPosition = Vector2.zero;

		private const float TitleHeight = 42f;

		private const float DialogWidth = 500f;

		private const float DialogHeight = 600f;

		public override Vector2 InitialSize
		{
			get
			{
				float num = DialogHeight;
				if (_title != null)
				{
					num += TitleHeight;
				}
				return new Vector2(DialogWidth, num);
			}
		}

		public Dialog_Inspect(string text, Texture2D image, string title = null)
		{
			_text = text;
			_image = image;
			_title = title;
			if (text.NullOrEmpty())
			{
				_text = "Null";
			}
			forcePause = false;
			draggable = true;
			resizeable = true;
			preventCameraMotion = false;
			absorbInputAroundWindow = false;
			doCloseX = true;
			closeOnClickedOutside = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!_title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, TitleHeight), _title);
				num += TitleHeight;
			}

			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 42f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(_text, width));
			Widgets.BeginScrollView(outRect, ref _scrollPosition, viewRect);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), _text);
			Widgets.EndScrollView();

			Widgets.DrawTextureFitted(outRect, _image, 1f);
		}
	}
}