using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Evon.UserControls
{
	// Token: 0x0200002F RID: 47
	public partial class GameRectangle : UserControl
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000C766 File Offset: 0x0000A966
		public GameRectangle()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private void doRightClick(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x040001CE RID: 462
		public WrapPanel items;

		// Token: 0x040001CF RID: 463
		public int id;
	}
}
