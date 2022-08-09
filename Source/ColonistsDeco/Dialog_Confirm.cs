using System;
using UnityEngine;
using Verse;

namespace ColonistsDeco
{
	public class Dialog_Confirm : Dialog_MessageBox
	{
		private const float TitleHeight = 42f;

		private const float DialogWidth = 500f;

		private const float DialogHeight = 300f;

		public override Vector2 InitialSize
		{
			get
			{
				float num = DialogHeight;
				if (title != null)
				{
					num += TitleHeight;
				}
				return new Vector2(DialogWidth, num);
			}
		}

		public Dialog_Confirm(string text, Action confirmedAct = null, bool destructive = false, string title = null)
			: base(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive)
		{
			closeOnCancel = false;
			closeOnAccept = false;
		}

		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (Event.current.type != EventType.KeyDown)
			{
				return;
			}
			if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
			{
				if (buttonAAction != null)
				{
					buttonAAction();
				}
				Close();
			}
			else if (Event.current.keyCode == KeyCode.Escape)
			{
				if (buttonBAction != null)
				{
					buttonBAction();
				}
				Close();
			}
		}
	}
}