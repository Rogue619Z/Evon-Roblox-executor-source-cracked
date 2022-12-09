using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FluxAPI;
using KrnlAPI;
using Oxygen;
using tools.Pipes;

namespace Evon.Classes.Apis
{
	// Token: 0x02000037 RID: 55
	public static class Apis
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000D07C File Offset: 0x0000B27C
		public static bool Execute(string script)
		{
			try
			{
				switch (Apis.selected)
				{
				case 0:
					switch (Execution.Execute(script))
					{
					case 0:
						return true;
					case 1:
						MessageBox.Show("Couldn't find the Oxygen U dll, please make sure your anti-virus's exclusions has had the EVON folder added.\nFirst time executing? Make sure you're injected.");
						return false;
					case 2:
						MessageBox.Show("Please inject EVON before trying to execute.");
						return false;
					case 3:
						MessageBox.Show("Something unexpected happened while attempting to execute your script. Sorry!");
						return false;
					default:
						return false;
					}
					break;
				case 1:
				{
					bool flag = Process.GetProcessesByName("RobloxPlayerBeta").Length < 1;
					if (flag)
					{
						MessageBox.Show("Please open ROBLOX before executing.");
						return false;
					}
					bool flag2 = !Apis.p.Exists();
					if (flag2)
					{
						MessageBox.Show("Please inject EVON.");
						return false;
					}
					Apis.p.Write(script);
					return true;
				}
				case 2:
				{
					bool flag3 = Process.GetProcessesByName("RobloxPlayerBeta").Length < 1;
					if (flag3)
					{
						MessageBox.Show("Please open ROBLOX before executing.");
						return false;
					}
					Apis.fAPI.Execute(script, true);
					return true;
				}
				case 3:
				{
					bool flag4 = !Apis.kAPI.IsInitialized();
					if (flag4)
					{
						MessageBox.Show("Please try injecting first before executing.");
						return false;
					}
					bool flag5 = Process.GetProcessesByName("RobloxPlayerBeta").Length < 1;
					if (flag5)
					{
						MessageBox.Show("Please open ROBLOX before executing.");
						return false;
					}
					return Apis.kAPI.Execute(script);
				}
				}
			}
			catch
			{
				return false;
			}
			return false;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000D23C File Offset: 0x0000B43C
		public static Task<bool> Inject()
		{
			Apis.<>c__DisplayClass7_0 CS$<>8__locals1 = new Apis.<>c__DisplayClass7_0();
			CS$<>8__locals1.result = new TaskCompletionSource<bool>();
			Task<bool> task;
			try
			{
				new Thread(delegate()
				{
					Apis.<>c__DisplayClass7_0.<<Inject>b__0>d <<Inject>b__0>d = new Apis.<>c__DisplayClass7_0.<<Inject>b__0>d();
					<<Inject>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
					<<Inject>b__0>d.<>4__this = CS$<>8__locals1;
					<<Inject>b__0>d.<>1__state = -1;
					<<Inject>b__0>d.<>t__builder.Start<Apis.<>c__DisplayClass7_0.<<Inject>b__0>d>(ref <<Inject>b__0>d);
				}).Start();
				task = CS$<>8__locals1.result.Task;
			}
			catch
			{
				CS$<>8__locals1.result.SetResult(false);
				task = CS$<>8__locals1.result.Task;
			}
			return task;
		}

		// Token: 0x040001F6 RID: 502
		public static Pipe p = new Pipe("EvonSakpot");

		// Token: 0x040001F7 RID: 503
		private static KrnlApi kAPI = new KrnlApi();

		// Token: 0x040001F8 RID: 504
		private static API fAPI = new API();

		// Token: 0x040001F9 RID: 505
		private static int injectionms = 0;

		// Token: 0x040001FA RID: 506
		private static int pipems = 0;

		// Token: 0x040001FB RID: 507
		public static int selected = 1;
	}
}
