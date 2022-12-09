using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Evon
{
	// Token: 0x0200000B RID: 11
	public class Intro : Window, IComponentConnector
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002AC7 File Offset: 0x00000CC7
		public Intro()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002ADC File Offset: 0x00000CDC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/Evon;component/introscreen.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002B14 File Offset: 0x00000D14
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				((Intro)target).Loaded += this.Window_Loaded;
			}
		}

		// Token: 0x04000016 RID: 22
		private bool _contentLoaded;
	}
}
