using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Evon.Properties
{
	// Token: 0x0200002E RID: 46
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000C730 File Offset: 0x0000A930
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040001CD RID: 461
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
