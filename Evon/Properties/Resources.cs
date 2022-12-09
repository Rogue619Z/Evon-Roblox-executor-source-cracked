using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Evon.Properties
{
	// Token: 0x0200002D RID: 45
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		internal Resources()
		{
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000C6C8 File Offset: 0x0000A8C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("Evon.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x0000C710 File Offset: 0x0000A910
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x0000C727 File Offset: 0x0000A927
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x040001CB RID: 459
		private static ResourceManager resourceMan;

		// Token: 0x040001CC RID: 460
		private static CultureInfo resourceCulture;
	}
}
