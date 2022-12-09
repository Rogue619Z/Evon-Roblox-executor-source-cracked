using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using ColorPicker;
using Evon.Classes.Apis;
using Evon.Editor;
using Evon.UserControls;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Win32;
using Newtonsoft.Json;
using tools;
using tools.ProcessTools;

namespace Evon
{
	// Token: 0x0200000C RID: 12
	public partial class Executor : Window, IStyleConnector
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002B50 File Offset: 0x00000D50
		[DebuggerStepThrough]
		public void showNotif(string title, string text)
		{
			Executor.<showNotif>d__12 <showNotif>d__ = new Executor.<showNotif>d__12();
			<showNotif>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<showNotif>d__.<>4__this = this;
			<showNotif>d__.title = title;
			<showNotif>d__.text = text;
			<showNotif>d__.<>1__state = -1;
			<showNotif>d__.<>t__builder.Start<Executor.<showNotif>d__12>(ref <showNotif>d__);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B98 File Offset: 0x00000D98
		public static bool WebViewIsInstalled()
		{
			bool result;
			try
			{
				bool is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
				if (is64BitOperatingSystem)
				{
					using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\WOW6432Node\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}"))
					{
						bool flag = registryKey != null;
						if (flag)
						{
							object value = registryKey.GetValue("pv");
							bool flag2 = value != null;
							if (flag2)
							{
								return true;
							}
						}
						return false;
					}
				}
				using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}"))
				{
					bool flag3 = registryKey2 != null;
					if (flag3)
					{
						object value2 = registryKey2.GetValue("pv");
						bool flag4 = value2 != null;
						if (flag4)
						{
							return true;
						}
					}
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C78 File Offset: 0x00000E78
		public static bool isInstalled()
		{
			bool result;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\DevDiv\\VC\\Servicing\\14.0\\RuntimeMinimum", false);
				bool flag = registryKey == null;
				if (flag)
				{
					result = false;
				}
				else
				{
					object value = registryKey.GetValue("Version");
					result = ((string)value != null && ((string)value).StartsWith("14"));
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002CE8 File Offset: 0x00000EE8
		[DebuggerStepThrough]
		public static void doRedistDownload()
		{
			Executor.<doRedistDownload>d__15 <doRedistDownload>d__ = new Executor.<doRedistDownload>d__15();
			<doRedistDownload>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<doRedistDownload>d__.<>1__state = -1;
			<doRedistDownload>d__.<>t__builder.Start<Executor.<doRedistDownload>d__15>(ref <doRedistDownload>d__);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D1C File Offset: 0x00000F1C
		[DebuggerStepThrough]
		public static void doDownload()
		{
			Executor.<doDownload>d__16 <doDownload>d__ = new Executor.<doDownload>d__16();
			<doDownload>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<doDownload>d__.<>1__state = -1;
			<doDownload>d__.<>t__builder.Start<Executor.<doDownload>d__16>(ref <doDownload>d__);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002D4E File Offset: 0x00000F4E
		public static bool IsAdministrator
		{
			get
			{
				return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D64 File Offset: 0x00000F64
		public static bool IsValidURI(string uri)
		{
			bool flag = !Uri.IsWellFormedUriString(uri, UriKind.Absolute);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Uri uri2;
				bool flag2 = !Uri.TryCreate(uri, UriKind.Absolute, out uri2);
				result = (!flag2 && (uri2.Scheme == Uri.UriSchemeHttp || uri2.Scheme == Uri.UriSchemeHttps));
			}
			return result;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002DC1 File Offset: 0x00000FC1
		public void reloadScripts(object sender = null, FileSystemEventArgs e = null)
		{
			base.Dispatcher.Invoke(delegate()
			{
				IEnumerable<string> enumerable = Directory.EnumerateFiles("scripts", "*.*", SearchOption.AllDirectories);
				this.ScriptList.Items.Clear();
				bool flag = this.ListBoxSearch.Text != null;
				if (flag)
				{
					foreach (string path in from script in enumerable
					where script.ToLower().Contains(this.ListBoxSearch.Text.ToLower())
					select script)
					{
						this.ScriptList.Items.Add(Path.GetFileName(path));
					}
				}
				else
				{
					foreach (string path2 in enumerable)
					{
						this.ScriptList.Items.Add(Path.GetFileName(path2));
					}
				}
			});
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002DDC File Offset: 0x00000FDC
		private void reloadrScripts()
		{
			new Thread(delegate()
			{
				base.Dispatcher.Invoke(delegate()
				{
					this.ScriptList.Items.Clear();
					this.GameSys.Children.Clear();
				});
				try
				{
					bool flag = this.searchingsystem == "";
					if (flag)
					{
						this.searchingsystem = "a";
					}
					using (WebClient webClient = new WebClient())
					{
						object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
						object arg = JsonConvert.DeserializeObject(webClient.DownloadString("https://scriptblox.com/api/script/search?q=" + this.searchingsystem + "&mode=free&max=100&page=" + this.pagenum.ToString()));
						List<int> list = new List<int>();
						BrushConverter BrushConversion = new BrushConverter();
						if (Executor.<>o__21.<>p__29 == null)
						{
							Executor.<>o__21.<>p__29 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(Executor)));
						}
						Func<CallSite, object, IEnumerable> target = Executor.<>o__21.<>p__29.Target;
						CallSite <>p__ = Executor.<>o__21.<>p__29;
						if (Executor.<>o__21.<>p__1 == null)
						{
							Executor.<>o__21.<>p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "scripts", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target2 = Executor.<>o__21.<>p__1.Target;
						CallSite <>p__2 = Executor.<>o__21.<>p__1;
						if (Executor.<>o__21.<>p__0 == null)
						{
							Executor.<>o__21.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "result", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						using (IEnumerator enumerator = target(<>p__, target2(<>p__2, Executor.<>o__21.<>p__0.Target(Executor.<>o__21.<>p__0, arg))).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								dynamic script = enumerator.Current;
								RoutedEventHandler <>9__3;
								base.Dispatcher.Invoke(delegate()
								{
									if (Executor.<>o__21.<>p__5 == null)
									{
										Executor.<>o__21.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Executor)));
									}
									Func<CallSite, object, string> target3 = Executor.<>o__21.<>p__5.Target;
									CallSite <>p__3 = Executor.<>o__21.<>p__5;
									if (Executor.<>o__21.<>p__4 == null)
									{
										Executor.<>o__21.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target4 = Executor.<>o__21.<>p__4.Target;
									CallSite <>p__4 = Executor.<>o__21.<>p__4;
									if (Executor.<>o__21.<>p__3 == null)
									{
										Executor.<>o__21.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "imageUrl", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target5 = Executor.<>o__21.<>p__3.Target;
									CallSite <>p__5 = Executor.<>o__21.<>p__3;
									if (Executor.<>o__21.<>p__2 == null)
									{
										Executor.<>o__21.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "game", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									string text = target3(<>p__3, target4(<>p__4, target5(<>p__5, Executor.<>o__21.<>p__2.Target(Executor.<>o__21.<>p__2, script))));
									bool flag3 = !text.Contains("rbxcdn.com");
									if (flag3)
									{
										if (Executor.<>o__21.<>p__10 == null)
										{
											Executor.<>o__21.<>p__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Executor)));
										}
										Func<CallSite, object, string> target6 = Executor.<>o__21.<>p__10.Target;
										CallSite <>p__6 = Executor.<>o__21.<>p__10;
										if (Executor.<>o__21.<>p__9 == null)
										{
											Executor.<>o__21.<>p__9 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Executor), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
											}));
										}
										Func<CallSite, string, object, object> target7 = Executor.<>o__21.<>p__9.Target;
										CallSite <>p__7 = Executor.<>o__21.<>p__9;
										string arg2 = "https://scriptblox.com";
										if (Executor.<>o__21.<>p__8 == null)
										{
											Executor.<>o__21.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
											}));
										}
										Func<CallSite, object, object> target8 = Executor.<>o__21.<>p__8.Target;
										CallSite <>p__8 = Executor.<>o__21.<>p__8;
										if (Executor.<>o__21.<>p__7 == null)
										{
											Executor.<>o__21.<>p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "imageUrl", typeof(Executor), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
											}));
										}
										Func<CallSite, object, object> target9 = Executor.<>o__21.<>p__7.Target;
										CallSite <>p__9 = Executor.<>o__21.<>p__7;
										if (Executor.<>o__21.<>p__6 == null)
										{
											Executor.<>o__21.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "game", typeof(Executor), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
											}));
										}
										text = target6(<>p__6, target7(<>p__7, arg2, target8(<>p__8, target9(<>p__9, Executor.<>o__21.<>p__6.Target(Executor.<>o__21.<>p__6, script)))));
									}
									GameRectangle gameRectangle = new GameRectangle();
									gameRectangle.ScriptImage.ImageSource = (Executor.IsValidURI(text) ? new BitmapImage(new Uri(text)) : null);
									TextBlock scriptTitle = gameRectangle.ScriptTitle;
									if (Executor.<>o__21.<>p__13 == null)
									{
										Executor.<>o__21.<>p__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Executor)));
									}
									Func<CallSite, object, string> target10 = Executor.<>o__21.<>p__13.Target;
									CallSite <>p__10 = Executor.<>o__21.<>p__13;
									if (Executor.<>o__21.<>p__12 == null)
									{
										Executor.<>o__21.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target11 = Executor.<>o__21.<>p__12.Target;
									CallSite <>p__11 = Executor.<>o__21.<>p__12;
									if (Executor.<>o__21.<>p__11 == null)
									{
										Executor.<>o__21.<>p__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "title", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									scriptTitle.Text = target10(<>p__10, target11(<>p__11, Executor.<>o__21.<>p__11.Target(Executor.<>o__21.<>p__11, script)));
									GameRectangle gameRectangle2 = gameRectangle;
									gameRectangle2.items = this.GameSys;
									ButtonBase executeB = gameRectangle2.ExecuteB;
									RoutedEventHandler value;
									if ((value = <>9__3) == null)
									{
										value = (<>9__3 = delegate(object ssss, RoutedEventArgs eeee)
										{
											if (Executor.<>o__21.<>p__16 == null)
											{
												Executor.<>o__21.<>p__16 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", null, typeof(Executor), new CSharpArgumentInfo[]
												{
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
												}));
											}
											Action<CallSite, Type, object> target22 = Executor.<>o__21.<>p__16.Target;
											CallSite <>p__22 = Executor.<>o__21.<>p__16;
											Type typeFromHandle = typeof(Apis);
											if (Executor.<>o__21.<>p__15 == null)
											{
												Executor.<>o__21.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
												{
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
												}));
											}
											Func<CallSite, object, object> target23 = Executor.<>o__21.<>p__15.Target;
											CallSite <>p__23 = Executor.<>o__21.<>p__15;
											if (Executor.<>o__21.<>p__14 == null)
											{
												Executor.<>o__21.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "script", typeof(Executor), new CSharpArgumentInfo[]
												{
													CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
												}));
											}
											target22(<>p__22, typeFromHandle, target23(<>p__23, Executor.<>o__21.<>p__14.Target(Executor.<>o__21.<>p__14, script)));
										});
									}
									executeB.Click += value;
									object arg3 = JsonConvert.DeserializeObject(File.ReadAllText("./bin/theme.evon"));
									Control executeB2 = gameRectangle2.ExecuteB;
									if (Executor.<>o__21.<>p__22 == null)
									{
										Executor.<>o__21.<>p__22 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
									}
									Func<CallSite, object, Brush> target12 = Executor.<>o__21.<>p__22.Target;
									CallSite <>p__12 = Executor.<>o__21.<>p__22;
									if (Executor.<>o__21.<>p__21 == null)
									{
										Executor.<>o__21.<>p__21 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, BrushConverter, object, object> target13 = Executor.<>o__21.<>p__21.Target;
									CallSite <>p__13 = Executor.<>o__21.<>p__21;
									BrushConverter brushConversion = BrushConversion;
									if (Executor.<>o__21.<>p__20 == null)
									{
										Executor.<>o__21.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target14 = Executor.<>o__21.<>p__20.Target;
									CallSite <>p__14 = Executor.<>o__21.<>p__20;
									if (Executor.<>o__21.<>p__19 == null)
									{
										Executor.<>o__21.<>p__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target15 = Executor.<>o__21.<>p__19.Target;
									CallSite <>p__15 = Executor.<>o__21.<>p__19;
									if (Executor.<>o__21.<>p__18 == null)
									{
										Executor.<>o__21.<>p__18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
										}));
									}
									Func<CallSite, object, int, object> target16 = Executor.<>o__21.<>p__18.Target;
									CallSite <>p__16 = Executor.<>o__21.<>p__18;
									if (Executor.<>o__21.<>p__17 == null)
									{
										Executor.<>o__21.<>p__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									executeB2.Background = target12(<>p__12, target13(<>p__13, brushConversion, target14(<>p__14, target15(<>p__15, target16(<>p__16, Executor.<>o__21.<>p__17.Target(Executor.<>o__21.<>p__17, arg3), 0)))));
									DropShadowEffect buttonGlow = gameRectangle2.ButtonGlow;
									if (Executor.<>o__21.<>p__28 == null)
									{
										Executor.<>o__21.<>p__28 = CallSite<Func<CallSite, object, SolidColorBrush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(SolidColorBrush), typeof(Executor)));
									}
									Func<CallSite, object, SolidColorBrush> target17 = Executor.<>o__21.<>p__28.Target;
									CallSite <>p__17 = Executor.<>o__21.<>p__28;
									if (Executor.<>o__21.<>p__27 == null)
									{
										Executor.<>o__21.<>p__27 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, BrushConverter, object, object> target18 = Executor.<>o__21.<>p__27.Target;
									CallSite <>p__18 = Executor.<>o__21.<>p__27;
									BrushConverter brushConversion2 = BrushConversion;
									if (Executor.<>o__21.<>p__26 == null)
									{
										Executor.<>o__21.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target19 = Executor.<>o__21.<>p__26.Target;
									CallSite <>p__19 = Executor.<>o__21.<>p__26;
									if (Executor.<>o__21.<>p__25 == null)
									{
										Executor.<>o__21.<>p__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Func<CallSite, object, object> target20 = Executor.<>o__21.<>p__25.Target;
									CallSite <>p__20 = Executor.<>o__21.<>p__25;
									if (Executor.<>o__21.<>p__24 == null)
									{
										Executor.<>o__21.<>p__24 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
										}));
									}
									Func<CallSite, object, int, object> target21 = Executor.<>o__21.<>p__24.Target;
									CallSite <>p__21 = Executor.<>o__21.<>p__24;
									if (Executor.<>o__21.<>p__23 == null)
									{
										Executor.<>o__21.<>p__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									buttonGlow.Color = target17(<>p__17, target18(<>p__18, brushConversion2, target19(<>p__19, target20(<>p__20, target21(<>p__21, Executor.<>o__21.<>p__23.Target(Executor.<>o__21.<>p__23, arg3), 0))))).Color;
									this.scriptItems.Add(gameRectangle2);
									this.GameSys.Children.Add(gameRectangle2);
								});
							}
						}
						GC.Collect(2, GCCollectionMode.Forced);
					}
				}
				catch (Exception ex)
				{
					bool flag2 = MessageBox.Show("Something happened while attempting to load scripts from https://www.rbxscripts.xyz . Please make sure your connection is valid. Would you like to retry fetching scripts?", "EVON", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
					if (flag2)
					{
						this.reloadrScripts();
					}
				}
			}).Start();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public Executor()
		{
			bool flag = !Directory.Exists("bin");
			if (flag)
			{
				Directory.CreateDirectory("bin");
			}
			Directory.CreateDirectory("bin\\tabs");
			bool flag2 = !Directory.Exists("scripts");
			if (flag2)
			{
				Directory.CreateDirectory("scripts");
			}
			bool flag3 = !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon");
			if (flag3)
			{
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon", "{}");
			}
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows NT\\CurrentVersion");
			bool flag4 = registryKey.GetValue("ProductName").ToString().Contains("Windows 7");
			if (flag4)
			{
				MessageBox.Show("Windows 7 is not supported with EVON. We're terribly sorry for this inconvenience");
				base.Close();
			}
			registryKey.Dispose();
			this.InitializeComponent();
			this.editor.OnTextChanged += delegate(string text)
			{
				bool flag35 = this.tabs.SelectedItem != null;
				if (flag35)
				{
					TabItem key = (TabItem)this.tabs.SelectedItem;
					string text2;
					bool flag36 = this.Texts.TryGetValue(key, out text2);
					if (flag36)
					{
						this.Texts[key] = text;
					}
					else
					{
						this.Texts.Add(key, text);
					}
				}
			};
			base.Hide();
			this.w.OnProcessMade += delegate()
			{
				base.Dispatcher.Invoke<Task>(delegate()
				{
					Executor.<<-ctor>b__22_6>d <<-ctor>b__22_6>d = new Executor.<<-ctor>b__22_6>d();
					<<-ctor>b__22_6>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<-ctor>b__22_6>d.<>4__this = this;
					<<-ctor>b__22_6>d.<>1__state = -1;
					<<-ctor>b__22_6>d.<>t__builder.Start<Executor.<<-ctor>b__22_6>d>(ref <<-ctor>b__22_6>d);
					return <<-ctor>b__22_6>d.<>t__builder.Task;
				});
			};
			this.w.Initialize("RobloxPlayerBeta");
			this.setcolor();
			string b = null;
			bool flag5 = !Executor.WebViewIsInstalled();
			if (flag5)
			{
				Executor.doDownload();
			}
			bool flag6 = !Executor.isInstalled();
			if (flag6)
			{
				Executor.doRedistDownload();
			}
			bool flag7 = !File.Exists("C:\\Windows\\System32\\msvcp140.dll");
			if (flag7)
			{
				bool flag8 = !Executor.IsAdministrator;
				if (flag8)
				{
					MessageBox.Show("Please restart evon as an admin so we can download missing files.");
					Process.GetCurrentProcess().Kill();
				}
				try
				{
					using (WebClient webClient = new WebClient())
					{
						webClient.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/msvcp" + (Environment.Is64BitOperatingSystem ? "64" : "32") + ".dll", "C:\\Windows\\System32\\msvcp140.dll");
					}
				}
				catch
				{
					Clipboard.SetText("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/msvcp" + (Environment.Is64BitOperatingSystem ? "64" : "32") + ".dll");
					MessageBox.Show("Something happened while attempting to download msvcp140. We've copied the link to your clipboard.");
					base.Close();
				}
			}
			try
			{
				using (WebClient webClient2 = new WebClient())
				{
					object arg = JsonConvert.DeserializeObject(webClient2.DownloadString("https://clientsettingscdn.roblox.com/v2/client-version/WindowsPlayer"));
					if (Executor.<>o__22.<>p__2 == null)
					{
						Executor.<>o__22.<>p__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target = Executor.<>o__22.<>p__2.Target;
					CallSite <>p__ = Executor.<>o__22.<>p__2;
					if (Executor.<>o__22.<>p__1 == null)
					{
						Executor.<>o__22.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Executor), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, object, object> target2 = Executor.<>o__22.<>p__1.Target;
					CallSite <>p__2 = Executor.<>o__22.<>p__1;
					if (Executor.<>o__22.<>p__0 == null)
					{
						Executor.<>o__22.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "clientVersionUpload", typeof(Executor), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					bool flag9 = target(<>p__, target2(<>p__2, Executor.<>o__22.<>p__0.Target(Executor.<>o__22.<>p__0, arg), null));
					if (flag9)
					{
						if (Executor.<>o__22.<>p__5 == null)
						{
							Executor.<>o__22.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Executor)));
						}
						Func<CallSite, object, string> target3 = Executor.<>o__22.<>p__5.Target;
						CallSite <>p__3 = Executor.<>o__22.<>p__5;
						if (Executor.<>o__22.<>p__4 == null)
						{
							Executor.<>o__22.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target4 = Executor.<>o__22.<>p__4.Target;
						CallSite <>p__4 = Executor.<>o__22.<>p__4;
						if (Executor.<>o__22.<>p__3 == null)
						{
							Executor.<>o__22.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "clientVersionUpload", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						b = target3(<>p__3, target4(<>p__4, Executor.<>o__22.<>p__3.Target(Executor.<>o__22.<>p__3, arg)));
					}
				}
			}
			catch
			{
				MessageBox.Show("Failed to get the ROBLOX Version! Please make sure you have a valid internet connection and your connection to roblox.com is correct.");
				base.Close();
			}
			bool flag10 = !File.Exists("version.data");
			if (flag10)
			{
				bool flag11 = MessageBox.Show("Failed to find a vital file! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
				if (flag11)
				{
					try
					{
						Process.Start("https://sakpot.com/evon-executor");
					}
					catch
					{
						MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
						Clipboard.SetText("https://sakpot.com/evon-executor");
					}
				}
				Process.GetCurrentProcess().Kill();
			}
			bool flag12 = File.ReadAllText("version.data") != b;
			if (flag12)
			{
				bool flag13 = MessageBox.Show("The current version you're using is out of date, this could be because the exploit is currently patched, or you have downloaded an older verison of EVON. Please make sure you downloaded from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
				if (flag13)
				{
					try
					{
						Process.Start("https://sakpot.com/evon-executor");
					}
					catch
					{
						MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
						Clipboard.SetText("https://sakpot.com/evon-executor");
					}
				}
				Process.GetCurrentProcess().Kill();
			}
			bool flag14 = !File.Exists("Evon.dll");
			if (flag14)
			{
				bool flag15 = MessageBox.Show("Failed to find Evon.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
				if (flag15)
				{
					try
					{
						Process.Start("https://sakpot.com/evon-executor");
					}
					catch
					{
						MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
						Clipboard.SetText("https://sakpot.com/evon-executor");
					}
				}
				Process.GetCurrentProcess().Kill();
			}
			bool flag16 = !File.Exists("Oxygen API.dll");
			if (flag16)
			{
				bool flag17 = MessageBox.Show("Failed to find Oxygen API.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
				if (flag17)
				{
					try
					{
						Process.Start("https://sakpot.com/evon-executor");
					}
					catch
					{
						MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
						Clipboard.SetText("https://sakpot.com/evon-executor");
					}
				}
				Process.GetCurrentProcess().Kill();
			}
			bool flag18 = !File.Exists("KrnlAPI.dll");
			if (flag18)
			{
				bool flag19 = MessageBox.Show("Failed to find KrnlAPI.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
				if (flag19)
				{
					try
					{
						Process.Start("https://sakpot.com/evon-executor");
					}
					catch
					{
						MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
						Clipboard.SetText("https://sakpot.com/evon-executor");
					}
				}
				Process.GetCurrentProcess().Kill();
			}
			bool flag20 = !File.Exists("FluxAPI.dll");
			if (flag20)
			{
				bool flag21 = MessageBox.Show("Failed to find FluxAPI.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
				if (flag21)
				{
					try
					{
						Process.Start("https://sakpot.com/evon-executor");
					}
					catch
					{
						MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
						Clipboard.SetText("https://sakpot.com/evon-executor");
					}
				}
				Process.GetCurrentProcess().Kill();
			}
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher("scripts")
			{
				EnableRaisingEvents = true,
				IncludeSubdirectories = true,
				NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime)
			};
			fileSystemWatcher.Changed += delegate(object poop, FileSystemEventArgs pee)
			{
				this.reloadScripts(null, null);
			};
			fileSystemWatcher.Created += delegate(object poop, FileSystemEventArgs pee)
			{
				this.reloadScripts(null, null);
			};
			fileSystemWatcher.Deleted += delegate(object poop, FileSystemEventArgs pee)
			{
				this.reloadScripts(null, null);
			};
			fileSystemWatcher.Renamed += delegate(object poop, RenamedEventArgs pee)
			{
				this.reloadScripts(null, null);
			};
			object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
			if (Executor.<>o__22.<>p__8 == null)
			{
				Executor.<>o__22.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target5 = Executor.<>o__22.<>p__8.Target;
			CallSite <>p__5 = Executor.<>o__22.<>p__8;
			if (Executor.<>o__22.<>p__7 == null)
			{
				Executor.<>o__22.<>p__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target6 = Executor.<>o__22.<>p__7.Target;
			CallSite <>p__6 = Executor.<>o__22.<>p__7;
			if (Executor.<>o__22.<>p__6 == null)
			{
				Executor.<>o__22.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "topmost", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag22 = target5(<>p__5, target6(<>p__6, Executor.<>o__22.<>p__6.Target(Executor.<>o__22.<>p__6, obj), null));
			if (flag22)
			{
				if (Executor.<>o__22.<>p__9 == null)
				{
					Executor.<>o__22.<>p__9 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "topmost", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__9.Target(Executor.<>o__22.<>p__9, obj, false);
			}
			if (Executor.<>o__22.<>p__12 == null)
			{
				Executor.<>o__22.<>p__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target7 = Executor.<>o__22.<>p__12.Target;
			CallSite <>p__7 = Executor.<>o__22.<>p__12;
			if (Executor.<>o__22.<>p__11 == null)
			{
				Executor.<>o__22.<>p__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target8 = Executor.<>o__22.<>p__11.Target;
			CallSite <>p__8 = Executor.<>o__22.<>p__11;
			if (Executor.<>o__22.<>p__10 == null)
			{
				Executor.<>o__22.<>p__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "legacy", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag23 = target7(<>p__7, target8(<>p__8, Executor.<>o__22.<>p__10.Target(Executor.<>o__22.<>p__10, obj), null));
			if (flag23)
			{
				if (Executor.<>o__22.<>p__13 == null)
				{
					Executor.<>o__22.<>p__13 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "legacy", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__13.Target(Executor.<>o__22.<>p__13, obj, false);
			}
			if (Executor.<>o__22.<>p__16 == null)
			{
				Executor.<>o__22.<>p__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target9 = Executor.<>o__22.<>p__16.Target;
			CallSite <>p__9 = Executor.<>o__22.<>p__16;
			if (Executor.<>o__22.<>p__15 == null)
			{
				Executor.<>o__22.<>p__15 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target10 = Executor.<>o__22.<>p__15.Target;
			CallSite <>p__10 = Executor.<>o__22.<>p__15;
			if (Executor.<>o__22.<>p__14 == null)
			{
				Executor.<>o__22.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "antiskid", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag24 = target9(<>p__9, target10(<>p__10, Executor.<>o__22.<>p__14.Target(Executor.<>o__22.<>p__14, obj), null));
			if (flag24)
			{
				if (Executor.<>o__22.<>p__17 == null)
				{
					Executor.<>o__22.<>p__17 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "antiskid", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__17.Target(Executor.<>o__22.<>p__17, obj, false);
			}
			if (Executor.<>o__22.<>p__20 == null)
			{
				Executor.<>o__22.<>p__20 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target11 = Executor.<>o__22.<>p__20.Target;
			CallSite <>p__11 = Executor.<>o__22.<>p__20;
			if (Executor.<>o__22.<>p__19 == null)
			{
				Executor.<>o__22.<>p__19 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target12 = Executor.<>o__22.<>p__19.Target;
			CallSite <>p__12 = Executor.<>o__22.<>p__19;
			if (Executor.<>o__22.<>p__18 == null)
			{
				Executor.<>o__22.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minimap", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag25 = target11(<>p__11, target12(<>p__12, Executor.<>o__22.<>p__18.Target(Executor.<>o__22.<>p__18, obj), null));
			if (flag25)
			{
				if (Executor.<>o__22.<>p__21 == null)
				{
					Executor.<>o__22.<>p__21 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "minimap", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__21.Target(Executor.<>o__22.<>p__21, obj, false);
			}
			if (Executor.<>o__22.<>p__24 == null)
			{
				Executor.<>o__22.<>p__24 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target13 = Executor.<>o__22.<>p__24.Target;
			CallSite <>p__13 = Executor.<>o__22.<>p__24;
			if (Executor.<>o__22.<>p__23 == null)
			{
				Executor.<>o__22.<>p__23 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target14 = Executor.<>o__22.<>p__23.Target;
			CallSite <>p__14 = Executor.<>o__22.<>p__23;
			if (Executor.<>o__22.<>p__22 == null)
			{
				Executor.<>o__22.<>p__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "autoinject", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag26 = target13(<>p__13, target14(<>p__14, Executor.<>o__22.<>p__22.Target(Executor.<>o__22.<>p__22, obj), null));
			if (flag26)
			{
				if (Executor.<>o__22.<>p__25 == null)
				{
					Executor.<>o__22.<>p__25 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "autoinject", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__25.Target(Executor.<>o__22.<>p__25, obj, false);
			}
			if (Executor.<>o__22.<>p__28 == null)
			{
				Executor.<>o__22.<>p__28 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target15 = Executor.<>o__22.<>p__28.Target;
			CallSite <>p__15 = Executor.<>o__22.<>p__28;
			if (Executor.<>o__22.<>p__27 == null)
			{
				Executor.<>o__22.<>p__27 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target16 = Executor.<>o__22.<>p__27.Target;
			CallSite <>p__16 = Executor.<>o__22.<>p__27;
			if (Executor.<>o__22.<>p__26 == null)
			{
				Executor.<>o__22.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "unlockfps", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag27 = target15(<>p__15, target16(<>p__16, Executor.<>o__22.<>p__26.Target(Executor.<>o__22.<>p__26, obj), null));
			if (flag27)
			{
				if (Executor.<>o__22.<>p__29 == null)
				{
					Executor.<>o__22.<>p__29 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "unlockfps", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__29.Target(Executor.<>o__22.<>p__29, obj, false);
			}
			bool flag28 = !File.Exists("bin\\info.evon");
			if (flag28)
			{
				File.WriteAllText("bin\\info.evon", "true");
				bool flag29 = MessageBox.Show("Would you like to join our discord server for constant updates about EVON? It may seem annoying that we make this request but do not worry, it's only a one-time request", "Evon - Invite", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
				if (flag29)
				{
					try
					{
						Process.Start("https://discord.gg/YpXFb3xUqz");
					}
					catch
					{
						Clipboard.SetText("https://discord.gg/YpXFb3xUqz");
						MessageBox.Show("Couldn't open your default browser, please make sure that you have one set. For now, we've copied our cute invite to your clipboard!");
					}
				}
			}
			bool flag30 = !Directory.Exists("bin\\tabs");
			if (flag30)
			{
				Directory.CreateDirectory("bin\\tabs");
			}
			if (Executor.<>o__22.<>p__31 == null)
			{
				Executor.<>o__22.<>p__31 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(Executor)));
			}
			Func<CallSite, object, bool> target17 = Executor.<>o__22.<>p__31.Target;
			CallSite <>p__17 = Executor.<>o__22.<>p__31;
			if (Executor.<>o__22.<>p__30 == null)
			{
				Executor.<>o__22.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "topmost", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			base.Topmost = target17(<>p__17, Executor.<>o__22.<>p__30.Target(Executor.<>o__22.<>p__30, obj));
			ToggleButton topMostCheck = this.TopMostCheck;
			if (Executor.<>o__22.<>p__33 == null)
			{
				Executor.<>o__22.<>p__33 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(Executor)));
			}
			Func<CallSite, object, bool> target18 = Executor.<>o__22.<>p__33.Target;
			CallSite <>p__18 = Executor.<>o__22.<>p__33;
			if (Executor.<>o__22.<>p__32 == null)
			{
				Executor.<>o__22.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "topmost", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			topMostCheck.IsChecked = new bool?(target18(<>p__18, Executor.<>o__22.<>p__32.Target(Executor.<>o__22.<>p__32, obj)));
			ToggleButton toggleButton = this.antiskidcheck;
			if (Executor.<>o__22.<>p__35 == null)
			{
				Executor.<>o__22.<>p__35 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(Executor)));
			}
			Func<CallSite, object, bool> target19 = Executor.<>o__22.<>p__35.Target;
			CallSite <>p__19 = Executor.<>o__22.<>p__35;
			if (Executor.<>o__22.<>p__34 == null)
			{
				Executor.<>o__22.<>p__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "antiskid", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			toggleButton.IsChecked = new bool?(target19(<>p__19, Executor.<>o__22.<>p__34.Target(Executor.<>o__22.<>p__34, obj)));
			ToggleButton toggleButton2 = this.minimapCheck;
			if (Executor.<>o__22.<>p__37 == null)
			{
				Executor.<>o__22.<>p__37 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(Executor)));
			}
			Func<CallSite, object, bool> target20 = Executor.<>o__22.<>p__37.Target;
			CallSite <>p__20 = Executor.<>o__22.<>p__37;
			if (Executor.<>o__22.<>p__36 == null)
			{
				Executor.<>o__22.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minimap", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			toggleButton2.IsChecked = new bool?(target20(<>p__20, Executor.<>o__22.<>p__36.Target(Executor.<>o__22.<>p__36, obj)));
			ToggleButton toggleButton3 = this.autoinjectcheck;
			if (Executor.<>o__22.<>p__39 == null)
			{
				Executor.<>o__22.<>p__39 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(Executor)));
			}
			Func<CallSite, object, bool> target21 = Executor.<>o__22.<>p__39.Target;
			CallSite <>p__21 = Executor.<>o__22.<>p__39;
			if (Executor.<>o__22.<>p__38 == null)
			{
				Executor.<>o__22.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "autoinject", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			toggleButton3.IsChecked = new bool?(target21(<>p__21, Executor.<>o__22.<>p__38.Target(Executor.<>o__22.<>p__38, obj)));
			ToggleButton toggleButton4 = this.unlockfpscheck;
			if (Executor.<>o__22.<>p__41 == null)
			{
				Executor.<>o__22.<>p__41 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(Executor)));
			}
			Func<CallSite, object, bool> target22 = Executor.<>o__22.<>p__41.Target;
			CallSite <>p__22 = Executor.<>o__22.<>p__41;
			if (Executor.<>o__22.<>p__40 == null)
			{
				Executor.<>o__22.<>p__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "unlockfps", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			toggleButton4.IsChecked = new bool?(target22(<>p__22, Executor.<>o__22.<>p__40.Target(Executor.<>o__22.<>p__40, obj)));
			if (Executor.<>o__22.<>p__44 == null)
			{
				Executor.<>o__22.<>p__44 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target23 = Executor.<>o__22.<>p__44.Target;
			CallSite <>p__23 = Executor.<>o__22.<>p__44;
			if (Executor.<>o__22.<>p__43 == null)
			{
				Executor.<>o__22.<>p__43 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target24 = Executor.<>o__22.<>p__43.Target;
			CallSite <>p__24 = Executor.<>o__22.<>p__43;
			if (Executor.<>o__22.<>p__42 == null)
			{
				Executor.<>o__22.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag31 = target23(<>p__23, target24(<>p__24, Executor.<>o__22.<>p__42.Target(Executor.<>o__22.<>p__42, obj), null));
			if (flag31)
			{
				if (Executor.<>o__22.<>p__46 == null)
				{
					Executor.<>o__22.<>p__46 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Executor)));
				}
				Func<CallSite, object, int> target25 = Executor.<>o__22.<>p__46.Target;
				CallSite <>p__25 = Executor.<>o__22.<>p__46;
				if (Executor.<>o__22.<>p__45 == null)
				{
					Executor.<>o__22.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				switch (target25(<>p__25, Executor.<>o__22.<>p__45.Target(Executor.<>o__22.<>p__45, obj)))
				{
				case 0:
					this.api_oxygen.IsChecked = new bool?(true);
					break;
				case 1:
					this.api_evon.IsChecked = new bool?(true);
					break;
				case 2:
					this.api_fluxus.IsChecked = new bool?(true);
					break;
				case 3:
					this.api_krnl.IsChecked = new bool?(true);
					break;
				default:
					this.api_evon.IsChecked = new bool?(true);
					break;
				}
				if (Executor.<>o__22.<>p__48 == null)
				{
					Executor.<>o__22.<>p__48 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Executor)));
				}
				Func<CallSite, object, int> target26 = Executor.<>o__22.<>p__48.Target;
				CallSite <>p__26 = Executor.<>o__22.<>p__48;
				if (Executor.<>o__22.<>p__47 == null)
				{
					Executor.<>o__22.<>p__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Apis.selected = target26(<>p__26, Executor.<>o__22.<>p__47.Target(Executor.<>o__22.<>p__47, obj));
			}
			else
			{
				if (Executor.<>o__22.<>p__49 == null)
				{
					Executor.<>o__22.<>p__49 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Executor.<>o__22.<>p__49.Target(Executor.<>o__22.<>p__49, obj, 1);
				if (Executor.<>o__22.<>p__51 == null)
				{
					Executor.<>o__22.<>p__51 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(Executor)));
				}
				Func<CallSite, object, int> target27 = Executor.<>o__22.<>p__51.Target;
				CallSite <>p__27 = Executor.<>o__22.<>p__51;
				if (Executor.<>o__22.<>p__50 == null)
				{
					Executor.<>o__22.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Apis.selected = target27(<>p__27, Executor.<>o__22.<>p__50.Target(Executor.<>o__22.<>p__50, obj));
				this.api_evon.IsChecked = new bool?(true);
			}
			bool value = this.autoinjectcheck.IsChecked.Value;
			if (value)
			{
				this.w.Start();
			}
			else
			{
				this.w.Stop();
			}
			if (Executor.<>o__22.<>p__53 == null)
			{
				Executor.<>o__22.<>p__53 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Action<CallSite, Type, string, object> target28 = Executor.<>o__22.<>p__53.Target;
			CallSite <>p__28 = Executor.<>o__22.<>p__53;
			Type typeFromHandle = typeof(File);
			string arg2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
			if (Executor.<>o__22.<>p__52 == null)
			{
				Executor.<>o__22.<>p__52 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			target28(<>p__28, typeFromHandle, arg2, Executor.<>o__22.<>p__52.Target(Executor.<>o__22.<>p__52, typeof(JsonConvert), obj));
			base.Show();
			this.reloadrScripts();
			this.reloadScripts(null, null);
			bool flag32 = !Directory.Exists("runtimes");
			if (flag32)
			{
				using (WebClient webClient3 = new WebClient())
				{
					try
					{
						webClient3.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/runtimes.zip", Path.GetTempPath() + "\\runtimes.zip");
						ZipFile.ExtractToDirectory(Path.GetTempPath() + "\\runtimes.zip", "./");
						File.Delete(Path.GetTempPath() + "\\runtimes.zip");
					}
					catch
					{
						MessageBox.Show("Something happened while downloading files required for EVON. Please make sure your Firewall isnt blocking https://www.github.com .", "EVON", MessageBoxButton.OK, MessageBoxImage.Hand);
						Process.GetCurrentProcess().Kill();
					}
				}
			}
			bool flag33 = !File.Exists("bin\\Monaco.html");
			if (flag33)
			{
				using (WebClient webClient4 = new WebClient())
				{
					try
					{
						webClient4.DownloadFile("https://raw.githubusercontent.com/ahhh-ahhh/EVON-downloads/main/Monaco.html", "bin\\Monaco.html");
					}
					catch
					{
						MessageBox.Show("Something happened while downloading files required for EVON. Please make sure your Firewall isnt blocking https://www.github.com .", "EVON", MessageBoxButton.OK, MessageBoxImage.Hand);
						Process.GetCurrentProcess().Kill();
					}
				}
			}
			bool flag34 = !Directory.Exists("bin\\vs");
			if (flag34)
			{
				using (WebClient webClient5 = new WebClient())
				{
					try
					{
						webClient5.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/vs.zip", Path.GetTempPath() + "\\vs.zip");
						ZipFile.ExtractToDirectory(Path.GetTempPath() + "\\vs.zip", "bin");
						File.Delete(Path.GetTempPath() + "\\vz.zip");
					}
					catch
					{
						MessageBox.Show("Something happened while downloading files required for EVON. Please make sure your Firewall isnt blocking https://www.github.com .", "EVON", MessageBoxButton.OK, MessageBoxImage.Hand);
						Process.GetCurrentProcess().Kill();
					}
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000049B8 File Offset: 0x00002BB8
		private void onDrop(object sender, DragEventArgs e)
		{
			try
			{
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				string[] array2 = array;
				int i = 0;
				while (i < array2.Length)
				{
					string path = array2[i];
					bool flag = File.Exists(path);
					if (flag)
					{
						bool flag2 = Path.GetExtension(path) != ".lua" && Path.GetExtension(path) != ".txt";
						if (!flag2)
						{
							this.createTab(Path.GetFileName(path), File.ReadAllText(path));
						}
					}
					IL_73:
					i++;
					continue;
					goto IL_73;
				}
			}
			catch
			{
				MessageBox.Show("Something happened while attempting to handle your drag-drop.");
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004A68 File Offset: 0x00002C68
		private void createTab(string name = "New Tab", string script = "")
		{
			try
			{
				name = ((name == "New Tab") ? string.Format("New Tab {0}", this.tabs.Items.Count + 1) : name);
				TabItem tab = new TabItem
				{
					Style = (Style)base.TryFindResource("Tab"),
					BorderBrush = this.DefaultBrush,
					Header = new TextBox
					{
						Text = name,
						IsHitTestVisible = false,
						IsEnabled = false,
						TextWrapping = TextWrapping.NoWrap,
						Style = (Style)base.TryFindResource("InvisibleTextBox"),
						MaxLength = 26
					}
				};
				tab.Content = this.editor;
				tab.MouseLeftButtonDown += delegate(object a, MouseButtonEventArgs e)
				{
					((WebView)tab.Content).antiskid();
					((WebView)tab.Content).minimap();
				};
				tab.MouseRightButtonDown += delegate(object _, MouseButtonEventArgs __)
				{
					TextBox textBox = (TextBox)tab.Header;
					textBox.IsEnabled = true;
					textBox.Focus();
					textBox.SelectAll();
				};
				RoutedEventHandler <>9__3;
				tab.Loaded += delegate(object loaded, RoutedEventArgs yay)
				{
					TextBox tb = (TextBox)tab.Header;
					ButtonBase buttonBase = (Button)tab.Template.FindName("CloseButton", tab);
					RoutedEventHandler value;
					if ((value = <>9__3) == null)
					{
						value = (<>9__3 = delegate(object _, RoutedEventArgs __)
						{
							this.tabs.Items.Remove(tab);
						});
					}
					buttonBase.Click += value;
					tb.FocusableChanged += delegate(object _, DependencyPropertyChangedEventArgs e)
					{
						bool flag = !tb.IsFocused;
						if (flag)
						{
							tb.IsEnabled = false;
						}
					};
					tb.KeyDown += delegate(object _, KeyEventArgs e)
					{
						bool flag = e.Key == Key.Return;
						if (flag)
						{
							tb.IsEnabled = false;
						}
					};
				};
				this.tabs.Items.Add(tab);
				this.tabs.SelectedItem = tab;
				this.SetText(script);
			}
			catch
			{
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00004BE4 File Offset: 0x00002DE4
		private void addTab(object sender, RoutedEventArgs e)
		{
			this.createTab("New Tab", "");
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004BF8 File Offset: 0x00002DF8
		private void doExecute(object sender, RoutedEventArgs e)
		{
			bool flag = this.Texts[(TabItem)this.tabs.SelectedItem] != null;
			if (flag)
			{
				Apis.Execute(this.GetText());
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004C34 File Offset: 0x00002E34
		private void SetText(string text)
		{
			this.editor.SetText(text);
			TabItem key = (TabItem)this.tabs.SelectedItem;
			string text2;
			bool flag = this.Texts.TryGetValue(key, out text2);
			if (flag)
			{
				this.Texts[key] = text;
			}
			else
			{
				this.Texts.Add(key, text);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004C90 File Offset: 0x00002E90
		private string GetText()
		{
			TabItem key = (TabItem)this.tabs.SelectedItem;
			string text;
			bool flag = this.Texts.TryGetValue(key, out text);
			string result;
			if (flag)
			{
				result = text;
			}
			else
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004CD0 File Offset: 0x00002ED0
		private void doOpen(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = "EVON | Open Script",
				Filter = "Lua (*.lua) |*.lua|Text (*.txt) |*.txt",
				Multiselect = true
			};
			bool? flag = openFileDialog.ShowDialog();
			bool flag2 = true;
			bool flag3 = !(flag.GetValueOrDefault() == flag2 & flag != null);
			if (!flag3)
			{
				bool flag4 = openFileDialog.FileNames.Length > 1;
				if (flag4)
				{
					foreach (string path in openFileDialog.FileNames)
					{
						this.createTab(Path.GetFileName(path), File.ReadAllText(path));
					}
				}
				else
				{
					bool flag5 = this.tabs.SelectedItem != null && this.Texts[(TabItem)this.tabs.SelectedItem] != null;
					if (flag5)
					{
						this.SetText(File.ReadAllText(openFileDialog.FileName));
					}
					else
					{
						this.createTab(Path.GetFileName(openFileDialog.FileName), File.ReadAllText(openFileDialog.FileName));
					}
				}
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004DDC File Offset: 0x00002FDC
		private void doSave(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Title = "EVON | Save File",
				Filter = "Lua (*.lua) |*.lua | Text Files (*.txt) |*.txt"
			};
			bool? flag = saveFileDialog.ShowDialog();
			bool flag2 = true;
			bool flag3 = !(flag.GetValueOrDefault() == flag2 & flag != null);
			if (!flag3)
			{
				using (StreamWriter streamWriter = new StreamWriter(File.OpenWrite(saveFileDialog.FileName)))
				{
					bool flag4 = this.tabs.SelectedItem != null;
					if (flag4)
					{
						streamWriter.Write(this.Texts[(TabItem)this.tabs.SelectedItem]);
					}
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004E98 File Offset: 0x00003098
		private void doClear(object sender, RoutedEventArgs e)
		{
			this.SetText("");
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004EA8 File Offset: 0x000030A8
		[DebuggerStepThrough]
		private void doInject(object sender, RoutedEventArgs e)
		{
			Executor.<doInject>d__32 <doInject>d__ = new Executor.<doInject>d__32();
			<doInject>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<doInject>d__.<>4__this = this;
			<doInject>d__.sender = sender;
			<doInject>d__.e = e;
			<doInject>d__.<>1__state = -1;
			<doInject>d__.<>t__builder.Start<Executor.<doInject>d__32>(ref <doInject>d__);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004EF0 File Offset: 0x000030F0
		private void handleShortcuts(object sender, KeyEventArgs e)
		{
			bool flag = Keyboard.Modifiers == ModifierKeys.Control;
			if (flag)
			{
				Key key = e.Key;
				Key key2 = key;
				if (key2 != Key.I)
				{
					switch (key2)
					{
					case Key.N:
						this.addTab(sender, e);
						break;
					case Key.O:
						this.doOpen(sender, e);
						break;
					case Key.Q:
					{
						bool flag2 = MessageBox.Show("Are you sure you want to close EVON?", "EVON", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes;
						if (flag2)
						{
							base.Close();
						}
						break;
					}
					case Key.S:
						this.doSave(sender, e);
						break;
					}
				}
				else
				{
					this.doInject(sender, e);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004F90 File Offset: 0x00003190
		private void selectExecutor(object sender, RoutedEventArgs e)
		{
			((Storyboard)base.TryFindResource("selectExecution")).Begin();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004FA8 File Offset: 0x000031A8
		private void selectScripts(object sender, RoutedEventArgs e)
		{
			((Storyboard)base.TryFindResource("selectScripts")).Begin();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004FC0 File Offset: 0x000031C0
		private void showSettings(object sender, RoutedEventArgs e)
		{
			((Storyboard)base.TryFindResource("selectSettings")).Begin();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004FD8 File Offset: 0x000031D8
		private void doDrag(object sender, MouseButtonEventArgs e)
		{
			base.DragMove();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004FE1 File Offset: 0x000031E1
		private void minimizeUI(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004FEB File Offset: 0x000031EB
		private void closeUI(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004FF4 File Offset: 0x000031F4
		private void doScripts(object sender, RoutedEventArgs e)
		{
			this.showScripts = !this.showScripts;
			((Storyboard)base.TryFindResource(this.showScripts ? "showScripts" : "hideScripts")).Begin();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000502C File Offset: 0x0000322C
		private void doSearch(object sender, TextChangedEventArgs e)
		{
			this.ScriptList.Items.Clear();
			foreach (string path in from script in Directory.EnumerateFiles("scripts", "*.*", SearchOption.AllDirectories)
			where script.ToLower().Contains(this.ListBoxSearch.Text.ToLower())
			select script)
			{
				this.ScriptList.Items.Add(Path.GetFileName(path));
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000050B8 File Offset: 0x000032B8
		private void doScriptSelection(object sender, SelectionChangedEventArgs e)
		{
			string text = (string)this.ScriptList.SelectedItem;
			bool flag = File.Exists("scripts\\" + text);
			if (flag)
			{
				this.createTab(text, File.ReadAllText("scripts\\" + text));
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00005103 File Offset: 0x00003303
		private void doSearchScripts(object sender, TextChangedEventArgs e)
		{
			this.pagenum = 1;
			this.searchingsystem = this.search.Text;
			this.reloadrScripts();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private void GameRefreshB_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005128 File Offset: 0x00003328
		private void selectEvonAPI(object sender, RoutedEventArgs e)
		{
			Apis.selected = 1;
			IEnumerable<CheckBox> source = Extensions.FindVisualChildren<CheckBox>(this);
			Func<CheckBox, bool> <>9__0;
			Func<CheckBox, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((CheckBox b) => b != (CheckBox)sender && b.Name.Contains("api_")));
			}
			foreach (CheckBox checkBox in source.Where(predicate))
			{
				checkBox.IsChecked = new bool?(false);
			}
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__45.<>p__0 == null)
				{
					Executor.<>o__45.<>p__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__45.<>p__0.Target(Executor.<>o__45.<>p__0, obj, Apis.selected);
				if (Executor.<>o__45.<>p__2 == null)
				{
					Executor.<>o__45.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__45.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__45.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__45.<>p__1 == null)
				{
					Executor.<>o__45.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__45.<>p__1.Target(Executor.<>o__45.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005350 File Offset: 0x00003550
		private void selectOxygenU(object sender, RoutedEventArgs e)
		{
			Apis.selected = 0;
			IEnumerable<CheckBox> source = Extensions.FindVisualChildren<CheckBox>(this);
			Func<CheckBox, bool> <>9__0;
			Func<CheckBox, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((CheckBox b) => b != (CheckBox)sender && b.Name.Contains("api_")));
			}
			foreach (CheckBox checkBox in source.Where(predicate))
			{
				checkBox.IsChecked = new bool?(false);
			}
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__46.<>p__0 == null)
				{
					Executor.<>o__46.<>p__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__46.<>p__0.Target(Executor.<>o__46.<>p__0, obj, Apis.selected);
				if (Executor.<>o__46.<>p__2 == null)
				{
					Executor.<>o__46.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__46.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__46.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__46.<>p__1 == null)
				{
					Executor.<>o__46.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__46.<>p__1.Target(Executor.<>o__46.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005578 File Offset: 0x00003778
		private void selectFluxus(object sender, RoutedEventArgs e)
		{
			Apis.selected = 2;
			IEnumerable<CheckBox> source = Extensions.FindVisualChildren<CheckBox>(this);
			Func<CheckBox, bool> <>9__0;
			Func<CheckBox, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((CheckBox b) => b != (CheckBox)sender && b.Name.Contains("api_")));
			}
			foreach (CheckBox checkBox in source.Where(predicate))
			{
				checkBox.IsChecked = new bool?(false);
			}
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__47.<>p__0 == null)
				{
					Executor.<>o__47.<>p__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__47.<>p__0.Target(Executor.<>o__47.<>p__0, obj, Apis.selected);
				if (Executor.<>o__47.<>p__2 == null)
				{
					Executor.<>o__47.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__47.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__47.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__47.<>p__1 == null)
				{
					Executor.<>o__47.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__47.<>p__1.Target(Executor.<>o__47.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000057A0 File Offset: 0x000039A0
		private void selectKRNL(object sender, RoutedEventArgs e)
		{
			Apis.selected = 3;
			IEnumerable<CheckBox> source = Extensions.FindVisualChildren<CheckBox>(this);
			Func<CheckBox, bool> <>9__0;
			Func<CheckBox, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((CheckBox b) => b != (CheckBox)sender && b.Name.Contains("api_")));
			}
			foreach (CheckBox checkBox in source.Where(predicate))
			{
				checkBox.IsChecked = new bool?(false);
			}
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__48.<>p__0 == null)
				{
					Executor.<>o__48.<>p__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__48.<>p__0.Target(Executor.<>o__48.<>p__0, obj, Apis.selected);
				if (Executor.<>o__48.<>p__2 == null)
				{
					Executor.<>o__48.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__48.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__48.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__48.<>p__1 == null)
				{
					Executor.<>o__48.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__48.<>p__1.Target(Executor.<>o__48.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000059C8 File Offset: 0x00003BC8
		[DebuggerStepThrough]
		private Task<string> getVersion()
		{
			Executor.<getVersion>d__49 <getVersion>d__ = new Executor.<getVersion>d__49();
			<getVersion>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<getVersion>d__.<>4__this = this;
			<getVersion>d__.<>1__state = -1;
			<getVersion>d__.<>t__builder.Start<Executor.<getVersion>d__49>(ref <getVersion>d__);
			return <getVersion>d__.<>t__builder.Task;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005A0C File Offset: 0x00003C0C
		[DebuggerStepThrough]
		private void revert268(object sender, RoutedEventArgs e)
		{
			Executor.<revert268>d__50 <revert268>d__ = new Executor.<revert268>d__50();
			<revert268>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<revert268>d__.<>4__this = this;
			<revert268>d__.sender = sender;
			<revert268>d__.e = e;
			<revert268>d__.<>1__state = -1;
			<revert268>d__.<>t__builder.Start<Executor.<revert268>d__50>(ref <revert268>d__);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005A54 File Offset: 0x00003C54
		[DebuggerStepThrough]
		private void fix268(object sender, RoutedEventArgs e)
		{
			Executor.<fix268>d__51 <fix268>d__ = new Executor.<fix268>d__51();
			<fix268>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<fix268>d__.<>4__this = this;
			<fix268>d__.sender = sender;
			<fix268>d__.e = e;
			<fix268>d__.<>1__state = -1;
			<fix268>d__.<>t__builder.Start<Executor.<fix268>d__51>(ref <fix268>d__);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005A9C File Offset: 0x00003C9C
		private void killRoblox(object sender, RoutedEventArgs e)
		{
			foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
			{
				process.Kill();
			}
			this.showNotif("Wohoo!", "Closed all open Roblox processes.");
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005AE0 File Offset: 0x00003CE0
		private void doUnlockFPS(object sender, RoutedEventArgs e)
		{
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__53.<>p__0 == null)
				{
					Executor.<>o__53.<>p__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "unlockfps", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__53.<>p__0.Target(Executor.<>o__53.<>p__0, obj, this.unlockfpscheck.IsChecked);
				if (Executor.<>o__53.<>p__2 == null)
				{
					Executor.<>o__53.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__53.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__53.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__53.<>p__1 == null)
				{
					Executor.<>o__53.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__53.<>p__1.Target(Executor.<>o__53.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
			bool flag = Apis.p.Exists();
			if (flag)
			{
				Apis.p.Write("setfpscap(" + (this.unlockfpscheck.IsChecked.Value ? "999" : "60") + ")");
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005CCC File Offset: 0x00003ECC
		private void doAutoInject(object sender, RoutedEventArgs e)
		{
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__54.<>p__0 == null)
				{
					Executor.<>o__54.<>p__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "autoinject", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__54.<>p__0.Target(Executor.<>o__54.<>p__0, obj, this.autoinjectcheck.IsChecked);
				if (Executor.<>o__54.<>p__2 == null)
				{
					Executor.<>o__54.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__54.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__54.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__54.<>p__1 == null)
				{
					Executor.<>o__54.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__54.<>p__1.Target(Executor.<>o__54.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
			bool value = this.autoinjectcheck.IsChecked.Value;
			if (value)
			{
				this.w.Start();
			}
			else
			{
				this.w.Stop();
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005EA0 File Offset: 0x000040A0
		private void doMinimap(object sender, RoutedEventArgs e)
		{
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__55.<>p__0 == null)
				{
					Executor.<>o__55.<>p__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "minimap", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__55.<>p__0.Target(Executor.<>o__55.<>p__0, obj, this.minimapCheck.IsChecked);
				if (Executor.<>o__55.<>p__2 == null)
				{
					Executor.<>o__55.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__55.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__55.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__55.<>p__1 == null)
				{
					Executor.<>o__55.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__55.<>p__1.Target(Executor.<>o__55.<>p__1, typeof(JsonConvert), obj));
				foreach (object obj2 in ((IEnumerable)this.tabs.Items))
				{
					TabItem tabItem = (TabItem)obj2;
					((WebView)tabItem.Content).minimap();
				}
				WebView webView = this.editor;
				if (webView != null)
				{
					webView.minimap();
				}
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000060BC File Offset: 0x000042BC
		private void doAntiSkid(object sender, RoutedEventArgs e)
		{
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__56.<>p__0 == null)
				{
					Executor.<>o__56.<>p__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "antiskid", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__56.<>p__0.Target(Executor.<>o__56.<>p__0, obj, this.antiskidcheck.IsChecked);
				if (Executor.<>o__56.<>p__2 == null)
				{
					Executor.<>o__56.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__56.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__56.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__56.<>p__1 == null)
				{
					Executor.<>o__56.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__56.<>p__1.Target(Executor.<>o__56.<>p__1, typeof(JsonConvert), obj));
				foreach (object obj2 in ((IEnumerable)this.tabs.Items))
				{
					TabItem tabItem = (TabItem)obj2;
					((WebView)tabItem.Content).antiskid();
				}
				WebView webView = this.editor;
				if (webView != null)
				{
					webView.antiskid();
				}
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000062D8 File Offset: 0x000044D8
		private void doTopMost(object sender, RoutedEventArgs e)
		{
			try
			{
				object obj = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
				if (Executor.<>o__57.<>p__0 == null)
				{
					Executor.<>o__57.<>p__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "topmost", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Executor.<>o__57.<>p__0.Target(Executor.<>o__57.<>p__0, obj, this.TopMostCheck.IsChecked);
				base.Topmost = this.TopMostCheck.IsChecked.Value;
				if (Executor.<>o__57.<>p__2 == null)
				{
					Executor.<>o__57.<>p__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Action<CallSite, Type, string, object> target = Executor.<>o__57.<>p__2.Target;
				CallSite <>p__ = Executor.<>o__57.<>p__2;
				Type typeFromHandle = typeof(File);
				string arg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
				if (Executor.<>o__57.<>p__1 == null)
				{
					Executor.<>o__57.<>p__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				target(<>p__, typeFromHandle, arg, Executor.<>o__57.<>p__1.Target(Executor.<>o__57.<>p__1, typeof(JsonConvert), obj));
			}
			catch
			{
				MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00006494 File Offset: 0x00004694
		[DebuggerStepThrough]
		private void onLoaded(object sender, RoutedEventArgs e)
		{
			Executor.<onLoaded>d__58 <onLoaded>d__ = new Executor.<onLoaded>d__58();
			<onLoaded>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<onLoaded>d__.<>4__this = this;
			<onLoaded>d__.sender = sender;
			<onLoaded>d__.e = e;
			<onLoaded>d__.<>1__state = -1;
			<onLoaded>d__.<>t__builder.Start<Executor.<onLoaded>d__58>(ref <onLoaded>d__);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000064DC File Offset: 0x000046DC
		private void onClosing(object sender, CancelEventArgs e)
		{
			TabItem tabItem = (TabItem)this.tabs.SelectedItem;
			bool flag = tabItem != null;
			if (flag)
			{
				string text;
				bool flag2 = this.Texts.TryGetValue(tabItem, out text);
				if (flag2)
				{
					this.Texts[tabItem] = this.GetText();
				}
				else
				{
					this.Texts.Add(tabItem, this.GetText());
				}
			}
			foreach (KeyValuePair<TabItem, string> keyValuePair in this.Texts)
			{
				File.WriteAllText("./bin/tabs/" + ((TextBox)keyValuePair.Key.Header).Text, keyValuePair.Value);
			}
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private void sViewMouseMove(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000065BC File Offset: 0x000047BC
		private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = e.RemovedItems.Count > 0;
			if (flag)
			{
				foreach (object obj in e.RemovedItems)
				{
					TabItem key = (TabItem)obj;
					string text;
					bool flag2 = this.Texts.TryGetValue(key, out text);
					if (flag2)
					{
						this.Texts[key] = this.editor.Text;
					}
					else
					{
						this.Texts.Add(key, this.editor.Text);
					}
				}
			}
			e.Handled = true;
			bool flag3 = this.tabs.SelectedItem != null;
			if (flag3)
			{
				string text2;
				bool flag4 = this.Texts.TryGetValue((TabItem)this.tabs.SelectedItem, out text2);
				if (flag4)
				{
					this.editor.SetText(text2);
				}
				else
				{
					this.editor.SetText("");
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000066D8 File Offset: 0x000048D8
		private void docpl(object sender, RoutedEventArgs e)
		{
			bool flag = this.pagenum < 1;
			if (flag)
			{
				MessageBox.Show("You cannot go back any further.");
			}
			else
			{
				this.pagenum--;
				this.reloadrScripts();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006717 File Offset: 0x00004917
		private void docpr(object sender, RoutedEventArgs e)
		{
			this.pagenum++;
			this.reloadrScripts();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006730 File Offset: 0x00004930
		public static void SetColor(PortableColorPicker s, SolidColorBrush r)
		{
			s.Color.RGB_R = (double)r.Color.R;
			s.Color.RGB_G = (double)r.Color.G;
			s.Color.RGB_B = (double)r.Color.B;
			s.Color.A = 255.0;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000067A4 File Offset: 0x000049A4
		private void setcolor()
		{
			try
			{
				bool flag = !File.Exists("./bin/theme.evon");
				if (flag)
				{
					Executor.ThemeStrings themeStrings = new Executor.ThemeStrings
					{
						Theme = new List<Executor.ThemeStrings.ThemeSystem>
						{
							new Executor.ThemeStrings.ThemeSystem
							{
								ThemeManufacturer = "Evon",
								Color1 = "#FF9700FF",
								TextboxImage = ""
							}
						}
					};
					try
					{
						File.WriteAllText("./bin/theme.evon", JsonConvert.SerializeObject(themeStrings, 1));
					}
					catch
					{
					}
				}
				object arg = JsonConvert.DeserializeObject(File.ReadAllText("./bin/theme.evon"));
				BrushConverter brushConverter = new BrushConverter();
				Border shadowVibe = this.ShadowVibe;
				if (Executor.<>o__65.<>p__5 == null)
				{
					Executor.<>o__65.<>p__5 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
				}
				Func<CallSite, object, Brush> target = Executor.<>o__65.<>p__5.Target;
				CallSite <>p__ = Executor.<>o__65.<>p__5;
				if (Executor.<>o__65.<>p__4 == null)
				{
					Executor.<>o__65.<>p__4 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, BrushConverter, object, object> target2 = Executor.<>o__65.<>p__4.Target;
				CallSite <>p__2 = Executor.<>o__65.<>p__4;
				BrushConverter arg2 = brushConverter;
				if (Executor.<>o__65.<>p__3 == null)
				{
					Executor.<>o__65.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target3 = Executor.<>o__65.<>p__3.Target;
				CallSite <>p__3 = Executor.<>o__65.<>p__3;
				if (Executor.<>o__65.<>p__2 == null)
				{
					Executor.<>o__65.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target4 = Executor.<>o__65.<>p__2.Target;
				CallSite <>p__4 = Executor.<>o__65.<>p__2;
				if (Executor.<>o__65.<>p__1 == null)
				{
					Executor.<>o__65.<>p__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target5 = Executor.<>o__65.<>p__1.Target;
				CallSite <>p__5 = Executor.<>o__65.<>p__1;
				if (Executor.<>o__65.<>p__0 == null)
				{
					Executor.<>o__65.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				shadowVibe.Background = target(<>p__, target2(<>p__2, arg2, target3(<>p__3, target4(<>p__4, target5(<>p__5, Executor.<>o__65.<>p__0.Target(Executor.<>o__65.<>p__0, arg), 0)))));
				foreach (Border depObj in Extensions.FindVisualChildren<Border>(this.settingsViewWindow))
				{
					IEnumerable<CheckBox> enumerable = Extensions.FindVisualChildren<CheckBox>(depObj);
					foreach (CheckBox checkBox in enumerable)
					{
						Control control = checkBox;
						if (Executor.<>o__65.<>p__11 == null)
						{
							Executor.<>o__65.<>p__11 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
						}
						Func<CallSite, object, Brush> target6 = Executor.<>o__65.<>p__11.Target;
						CallSite <>p__6 = Executor.<>o__65.<>p__11;
						if (Executor.<>o__65.<>p__10 == null)
						{
							Executor.<>o__65.<>p__10 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, BrushConverter, object, object> target7 = Executor.<>o__65.<>p__10.Target;
						CallSite <>p__7 = Executor.<>o__65.<>p__10;
						BrushConverter arg3 = brushConverter;
						if (Executor.<>o__65.<>p__9 == null)
						{
							Executor.<>o__65.<>p__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target8 = Executor.<>o__65.<>p__9.Target;
						CallSite <>p__8 = Executor.<>o__65.<>p__9;
						if (Executor.<>o__65.<>p__8 == null)
						{
							Executor.<>o__65.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target9 = Executor.<>o__65.<>p__8.Target;
						CallSite <>p__9 = Executor.<>o__65.<>p__8;
						if (Executor.<>o__65.<>p__7 == null)
						{
							Executor.<>o__65.<>p__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						Func<CallSite, object, int, object> target10 = Executor.<>o__65.<>p__7.Target;
						CallSite <>p__10 = Executor.<>o__65.<>p__7;
						if (Executor.<>o__65.<>p__6 == null)
						{
							Executor.<>o__65.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						control.Background = target6(<>p__6, target7(<>p__7, arg3, target8(<>p__8, target9(<>p__9, target10(<>p__10, Executor.<>o__65.<>p__6.Target(Executor.<>o__65.<>p__6, arg), 0)))));
						Control control2 = checkBox;
						if (Executor.<>o__65.<>p__17 == null)
						{
							Executor.<>o__65.<>p__17 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
						}
						Func<CallSite, object, Brush> target11 = Executor.<>o__65.<>p__17.Target;
						CallSite <>p__11 = Executor.<>o__65.<>p__17;
						if (Executor.<>o__65.<>p__16 == null)
						{
							Executor.<>o__65.<>p__16 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, BrushConverter, object, object> target12 = Executor.<>o__65.<>p__16.Target;
						CallSite <>p__12 = Executor.<>o__65.<>p__16;
						BrushConverter arg4 = brushConverter;
						if (Executor.<>o__65.<>p__15 == null)
						{
							Executor.<>o__65.<>p__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target13 = Executor.<>o__65.<>p__15.Target;
						CallSite <>p__13 = Executor.<>o__65.<>p__15;
						if (Executor.<>o__65.<>p__14 == null)
						{
							Executor.<>o__65.<>p__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target14 = Executor.<>o__65.<>p__14.Target;
						CallSite <>p__14 = Executor.<>o__65.<>p__14;
						if (Executor.<>o__65.<>p__13 == null)
						{
							Executor.<>o__65.<>p__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						Func<CallSite, object, int, object> target15 = Executor.<>o__65.<>p__13.Target;
						CallSite <>p__15 = Executor.<>o__65.<>p__13;
						if (Executor.<>o__65.<>p__12 == null)
						{
							Executor.<>o__65.<>p__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						control2.BorderBrush = target11(<>p__11, target12(<>p__12, arg4, target13(<>p__13, target14(<>p__14, target15(<>p__15, Executor.<>o__65.<>p__12.Target(Executor.<>o__65.<>p__12, arg), 0)))));
					}
				}
				PortableColorPicker s = this.colr;
				if (Executor.<>o__65.<>p__23 == null)
				{
					Executor.<>o__65.<>p__23 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target16 = Executor.<>o__65.<>p__23.Target;
				CallSite <>p__16 = Executor.<>o__65.<>p__23;
				if (Executor.<>o__65.<>p__22 == null)
				{
					Executor.<>o__65.<>p__22 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target17 = Executor.<>o__65.<>p__22.Target;
				CallSite <>p__17 = Executor.<>o__65.<>p__22;
				Type typeFromHandle = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__21 == null)
				{
					Executor.<>o__65.<>p__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target18 = Executor.<>o__65.<>p__21.Target;
				CallSite <>p__18 = Executor.<>o__65.<>p__21;
				if (Executor.<>o__65.<>p__20 == null)
				{
					Executor.<>o__65.<>p__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target19 = Executor.<>o__65.<>p__20.Target;
				CallSite <>p__19 = Executor.<>o__65.<>p__20;
				if (Executor.<>o__65.<>p__19 == null)
				{
					Executor.<>o__65.<>p__19 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target20 = Executor.<>o__65.<>p__19.Target;
				CallSite <>p__20 = Executor.<>o__65.<>p__19;
				if (Executor.<>o__65.<>p__18 == null)
				{
					Executor.<>o__65.<>p__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Executor.SetColor(s, new SolidColorBrush(target16(<>p__16, target17(<>p__17, typeFromHandle, target18(<>p__18, target19(<>p__19, target20(<>p__20, Executor.<>o__65.<>p__18.Target(Executor.<>o__65.<>p__18, arg), 0)))))));
				foreach (Border depObj2 in Extensions.FindVisualChildren<Border>(this.ApiPanerl))
				{
					IEnumerable<CheckBox> enumerable2 = Extensions.FindVisualChildren<CheckBox>(depObj2);
					foreach (CheckBox checkBox2 in enumerable2)
					{
						Control control3 = checkBox2;
						if (Executor.<>o__65.<>p__29 == null)
						{
							Executor.<>o__65.<>p__29 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
						}
						Func<CallSite, object, Brush> target21 = Executor.<>o__65.<>p__29.Target;
						CallSite <>p__21 = Executor.<>o__65.<>p__29;
						if (Executor.<>o__65.<>p__28 == null)
						{
							Executor.<>o__65.<>p__28 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, BrushConverter, object, object> target22 = Executor.<>o__65.<>p__28.Target;
						CallSite <>p__22 = Executor.<>o__65.<>p__28;
						BrushConverter arg5 = brushConverter;
						if (Executor.<>o__65.<>p__27 == null)
						{
							Executor.<>o__65.<>p__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target23 = Executor.<>o__65.<>p__27.Target;
						CallSite <>p__23 = Executor.<>o__65.<>p__27;
						if (Executor.<>o__65.<>p__26 == null)
						{
							Executor.<>o__65.<>p__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target24 = Executor.<>o__65.<>p__26.Target;
						CallSite <>p__24 = Executor.<>o__65.<>p__26;
						if (Executor.<>o__65.<>p__25 == null)
						{
							Executor.<>o__65.<>p__25 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						Func<CallSite, object, int, object> target25 = Executor.<>o__65.<>p__25.Target;
						CallSite <>p__25 = Executor.<>o__65.<>p__25;
						if (Executor.<>o__65.<>p__24 == null)
						{
							Executor.<>o__65.<>p__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						control3.Background = target21(<>p__21, target22(<>p__22, arg5, target23(<>p__23, target24(<>p__24, target25(<>p__25, Executor.<>o__65.<>p__24.Target(Executor.<>o__65.<>p__24, arg), 0)))));
						Control control4 = checkBox2;
						if (Executor.<>o__65.<>p__35 == null)
						{
							Executor.<>o__65.<>p__35 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
						}
						Func<CallSite, object, Brush> target26 = Executor.<>o__65.<>p__35.Target;
						CallSite <>p__26 = Executor.<>o__65.<>p__35;
						if (Executor.<>o__65.<>p__34 == null)
						{
							Executor.<>o__65.<>p__34 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, BrushConverter, object, object> target27 = Executor.<>o__65.<>p__34.Target;
						CallSite <>p__27 = Executor.<>o__65.<>p__34;
						BrushConverter arg6 = brushConverter;
						if (Executor.<>o__65.<>p__33 == null)
						{
							Executor.<>o__65.<>p__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target28 = Executor.<>o__65.<>p__33.Target;
						CallSite <>p__28 = Executor.<>o__65.<>p__33;
						if (Executor.<>o__65.<>p__32 == null)
						{
							Executor.<>o__65.<>p__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target29 = Executor.<>o__65.<>p__32.Target;
						CallSite <>p__29 = Executor.<>o__65.<>p__32;
						if (Executor.<>o__65.<>p__31 == null)
						{
							Executor.<>o__65.<>p__31 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						Func<CallSite, object, int, object> target30 = Executor.<>o__65.<>p__31.Target;
						CallSite <>p__30 = Executor.<>o__65.<>p__31;
						if (Executor.<>o__65.<>p__30 == null)
						{
							Executor.<>o__65.<>p__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						control4.BorderBrush = target26(<>p__26, target27(<>p__27, arg6, target28(<>p__28, target29(<>p__29, target30(<>p__30, Executor.<>o__65.<>p__30.Target(Executor.<>o__65.<>p__30, arg), 0)))));
					}
				}
				DropShadowEffect 1e = this._1e;
				if (Executor.<>o__65.<>p__41 == null)
				{
					Executor.<>o__65.<>p__41 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target31 = Executor.<>o__65.<>p__41.Target;
				CallSite <>p__31 = Executor.<>o__65.<>p__41;
				if (Executor.<>o__65.<>p__40 == null)
				{
					Executor.<>o__65.<>p__40 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target32 = Executor.<>o__65.<>p__40.Target;
				CallSite <>p__32 = Executor.<>o__65.<>p__40;
				Type typeFromHandle2 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__39 == null)
				{
					Executor.<>o__65.<>p__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target33 = Executor.<>o__65.<>p__39.Target;
				CallSite <>p__33 = Executor.<>o__65.<>p__39;
				if (Executor.<>o__65.<>p__38 == null)
				{
					Executor.<>o__65.<>p__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target34 = Executor.<>o__65.<>p__38.Target;
				CallSite <>p__34 = Executor.<>o__65.<>p__38;
				if (Executor.<>o__65.<>p__37 == null)
				{
					Executor.<>o__65.<>p__37 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target35 = Executor.<>o__65.<>p__37.Target;
				CallSite <>p__35 = Executor.<>o__65.<>p__37;
				if (Executor.<>o__65.<>p__36 == null)
				{
					Executor.<>o__65.<>p__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				1e.Color = target31(<>p__31, target32(<>p__32, typeFromHandle2, target33(<>p__33, target34(<>p__34, target35(<>p__35, Executor.<>o__65.<>p__36.Target(Executor.<>o__65.<>p__36, arg), 0)))));
				DropShadowEffect 2e = this._2e;
				if (Executor.<>o__65.<>p__47 == null)
				{
					Executor.<>o__65.<>p__47 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target36 = Executor.<>o__65.<>p__47.Target;
				CallSite <>p__36 = Executor.<>o__65.<>p__47;
				if (Executor.<>o__65.<>p__46 == null)
				{
					Executor.<>o__65.<>p__46 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target37 = Executor.<>o__65.<>p__46.Target;
				CallSite <>p__37 = Executor.<>o__65.<>p__46;
				Type typeFromHandle3 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__45 == null)
				{
					Executor.<>o__65.<>p__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target38 = Executor.<>o__65.<>p__45.Target;
				CallSite <>p__38 = Executor.<>o__65.<>p__45;
				if (Executor.<>o__65.<>p__44 == null)
				{
					Executor.<>o__65.<>p__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target39 = Executor.<>o__65.<>p__44.Target;
				CallSite <>p__39 = Executor.<>o__65.<>p__44;
				if (Executor.<>o__65.<>p__43 == null)
				{
					Executor.<>o__65.<>p__43 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target40 = Executor.<>o__65.<>p__43.Target;
				CallSite <>p__40 = Executor.<>o__65.<>p__43;
				if (Executor.<>o__65.<>p__42 == null)
				{
					Executor.<>o__65.<>p__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				2e.Color = target36(<>p__36, target37(<>p__37, typeFromHandle3, target38(<>p__38, target39(<>p__39, target40(<>p__40, Executor.<>o__65.<>p__42.Target(Executor.<>o__65.<>p__42, arg), 0)))));
				DropShadowEffect 3e = this._3e;
				if (Executor.<>o__65.<>p__53 == null)
				{
					Executor.<>o__65.<>p__53 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target41 = Executor.<>o__65.<>p__53.Target;
				CallSite <>p__41 = Executor.<>o__65.<>p__53;
				if (Executor.<>o__65.<>p__52 == null)
				{
					Executor.<>o__65.<>p__52 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target42 = Executor.<>o__65.<>p__52.Target;
				CallSite <>p__42 = Executor.<>o__65.<>p__52;
				Type typeFromHandle4 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__51 == null)
				{
					Executor.<>o__65.<>p__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target43 = Executor.<>o__65.<>p__51.Target;
				CallSite <>p__43 = Executor.<>o__65.<>p__51;
				if (Executor.<>o__65.<>p__50 == null)
				{
					Executor.<>o__65.<>p__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target44 = Executor.<>o__65.<>p__50.Target;
				CallSite <>p__44 = Executor.<>o__65.<>p__50;
				if (Executor.<>o__65.<>p__49 == null)
				{
					Executor.<>o__65.<>p__49 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target45 = Executor.<>o__65.<>p__49.Target;
				CallSite <>p__45 = Executor.<>o__65.<>p__49;
				if (Executor.<>o__65.<>p__48 == null)
				{
					Executor.<>o__65.<>p__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				3e.Color = target41(<>p__41, target42(<>p__42, typeFromHandle4, target43(<>p__43, target44(<>p__44, target45(<>p__45, Executor.<>o__65.<>p__48.Target(Executor.<>o__65.<>p__48, arg), 0)))));
				DropShadowEffect 4e = this._4e;
				if (Executor.<>o__65.<>p__59 == null)
				{
					Executor.<>o__65.<>p__59 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target46 = Executor.<>o__65.<>p__59.Target;
				CallSite <>p__46 = Executor.<>o__65.<>p__59;
				if (Executor.<>o__65.<>p__58 == null)
				{
					Executor.<>o__65.<>p__58 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target47 = Executor.<>o__65.<>p__58.Target;
				CallSite <>p__47 = Executor.<>o__65.<>p__58;
				Type typeFromHandle5 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__57 == null)
				{
					Executor.<>o__65.<>p__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target48 = Executor.<>o__65.<>p__57.Target;
				CallSite <>p__48 = Executor.<>o__65.<>p__57;
				if (Executor.<>o__65.<>p__56 == null)
				{
					Executor.<>o__65.<>p__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target49 = Executor.<>o__65.<>p__56.Target;
				CallSite <>p__49 = Executor.<>o__65.<>p__56;
				if (Executor.<>o__65.<>p__55 == null)
				{
					Executor.<>o__65.<>p__55 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target50 = Executor.<>o__65.<>p__55.Target;
				CallSite <>p__50 = Executor.<>o__65.<>p__55;
				if (Executor.<>o__65.<>p__54 == null)
				{
					Executor.<>o__65.<>p__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				4e.Color = target46(<>p__46, target47(<>p__47, typeFromHandle5, target48(<>p__48, target49(<>p__49, target50(<>p__50, Executor.<>o__65.<>p__54.Target(Executor.<>o__65.<>p__54, arg), 0)))));
				DropShadowEffect shadowNotifColor = this.ShadowNotifColor;
				if (Executor.<>o__65.<>p__65 == null)
				{
					Executor.<>o__65.<>p__65 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target51 = Executor.<>o__65.<>p__65.Target;
				CallSite <>p__51 = Executor.<>o__65.<>p__65;
				if (Executor.<>o__65.<>p__64 == null)
				{
					Executor.<>o__65.<>p__64 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target52 = Executor.<>o__65.<>p__64.Target;
				CallSite <>p__52 = Executor.<>o__65.<>p__64;
				Type typeFromHandle6 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__63 == null)
				{
					Executor.<>o__65.<>p__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target53 = Executor.<>o__65.<>p__63.Target;
				CallSite <>p__53 = Executor.<>o__65.<>p__63;
				if (Executor.<>o__65.<>p__62 == null)
				{
					Executor.<>o__65.<>p__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target54 = Executor.<>o__65.<>p__62.Target;
				CallSite <>p__54 = Executor.<>o__65.<>p__62;
				if (Executor.<>o__65.<>p__61 == null)
				{
					Executor.<>o__65.<>p__61 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target55 = Executor.<>o__65.<>p__61.Target;
				CallSite <>p__55 = Executor.<>o__65.<>p__61;
				if (Executor.<>o__65.<>p__60 == null)
				{
					Executor.<>o__65.<>p__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				shadowNotifColor.Color = target51(<>p__51, target52(<>p__52, typeFromHandle6, target53(<>p__53, target54(<>p__54, target55(<>p__55, Executor.<>o__65.<>p__60.Target(Executor.<>o__65.<>p__60, arg), 0)))));
				Border border = this.notifBar;
				if (Executor.<>o__65.<>p__71 == null)
				{
					Executor.<>o__65.<>p__71 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target56 = Executor.<>o__65.<>p__71.Target;
				CallSite <>p__56 = Executor.<>o__65.<>p__71;
				if (Executor.<>o__65.<>p__70 == null)
				{
					Executor.<>o__65.<>p__70 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target57 = Executor.<>o__65.<>p__70.Target;
				CallSite <>p__57 = Executor.<>o__65.<>p__70;
				Type typeFromHandle7 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__69 == null)
				{
					Executor.<>o__65.<>p__69 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target58 = Executor.<>o__65.<>p__69.Target;
				CallSite <>p__58 = Executor.<>o__65.<>p__69;
				if (Executor.<>o__65.<>p__68 == null)
				{
					Executor.<>o__65.<>p__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target59 = Executor.<>o__65.<>p__68.Target;
				CallSite <>p__59 = Executor.<>o__65.<>p__68;
				if (Executor.<>o__65.<>p__67 == null)
				{
					Executor.<>o__65.<>p__67 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target60 = Executor.<>o__65.<>p__67.Target;
				CallSite <>p__60 = Executor.<>o__65.<>p__67;
				if (Executor.<>o__65.<>p__66 == null)
				{
					Executor.<>o__65.<>p__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				border.Background = new SolidColorBrush(target56(<>p__56, target57(<>p__57, typeFromHandle7, target58(<>p__58, target59(<>p__59, target60(<>p__60, Executor.<>o__65.<>p__66.Target(Executor.<>o__65.<>p__66, arg), 0))))));
				DropShadowEffect dsb = this.DSB1;
				if (Executor.<>o__65.<>p__77 == null)
				{
					Executor.<>o__65.<>p__77 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target61 = Executor.<>o__65.<>p__77.Target;
				CallSite <>p__61 = Executor.<>o__65.<>p__77;
				if (Executor.<>o__65.<>p__76 == null)
				{
					Executor.<>o__65.<>p__76 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target62 = Executor.<>o__65.<>p__76.Target;
				CallSite <>p__62 = Executor.<>o__65.<>p__76;
				Type typeFromHandle8 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__75 == null)
				{
					Executor.<>o__65.<>p__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target63 = Executor.<>o__65.<>p__75.Target;
				CallSite <>p__63 = Executor.<>o__65.<>p__75;
				if (Executor.<>o__65.<>p__74 == null)
				{
					Executor.<>o__65.<>p__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target64 = Executor.<>o__65.<>p__74.Target;
				CallSite <>p__64 = Executor.<>o__65.<>p__74;
				if (Executor.<>o__65.<>p__73 == null)
				{
					Executor.<>o__65.<>p__73 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target65 = Executor.<>o__65.<>p__73.Target;
				CallSite <>p__65 = Executor.<>o__65.<>p__73;
				if (Executor.<>o__65.<>p__72 == null)
				{
					Executor.<>o__65.<>p__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb.Color = target61(<>p__61, target62(<>p__62, typeFromHandle8, target63(<>p__63, target64(<>p__64, target65(<>p__65, Executor.<>o__65.<>p__72.Target(Executor.<>o__65.<>p__72, arg), 0)))));
				DropShadowEffect dsb2 = this.DSB2;
				if (Executor.<>o__65.<>p__83 == null)
				{
					Executor.<>o__65.<>p__83 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target66 = Executor.<>o__65.<>p__83.Target;
				CallSite <>p__66 = Executor.<>o__65.<>p__83;
				if (Executor.<>o__65.<>p__82 == null)
				{
					Executor.<>o__65.<>p__82 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target67 = Executor.<>o__65.<>p__82.Target;
				CallSite <>p__67 = Executor.<>o__65.<>p__82;
				Type typeFromHandle9 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__81 == null)
				{
					Executor.<>o__65.<>p__81 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target68 = Executor.<>o__65.<>p__81.Target;
				CallSite <>p__68 = Executor.<>o__65.<>p__81;
				if (Executor.<>o__65.<>p__80 == null)
				{
					Executor.<>o__65.<>p__80 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target69 = Executor.<>o__65.<>p__80.Target;
				CallSite <>p__69 = Executor.<>o__65.<>p__80;
				if (Executor.<>o__65.<>p__79 == null)
				{
					Executor.<>o__65.<>p__79 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target70 = Executor.<>o__65.<>p__79.Target;
				CallSite <>p__70 = Executor.<>o__65.<>p__79;
				if (Executor.<>o__65.<>p__78 == null)
				{
					Executor.<>o__65.<>p__78 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb2.Color = target66(<>p__66, target67(<>p__67, typeFromHandle9, target68(<>p__68, target69(<>p__69, target70(<>p__70, Executor.<>o__65.<>p__78.Target(Executor.<>o__65.<>p__78, arg), 0)))));
				DropShadowEffect dsb3 = this.DSB3;
				if (Executor.<>o__65.<>p__89 == null)
				{
					Executor.<>o__65.<>p__89 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target71 = Executor.<>o__65.<>p__89.Target;
				CallSite <>p__71 = Executor.<>o__65.<>p__89;
				if (Executor.<>o__65.<>p__88 == null)
				{
					Executor.<>o__65.<>p__88 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target72 = Executor.<>o__65.<>p__88.Target;
				CallSite <>p__72 = Executor.<>o__65.<>p__88;
				Type typeFromHandle10 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__87 == null)
				{
					Executor.<>o__65.<>p__87 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target73 = Executor.<>o__65.<>p__87.Target;
				CallSite <>p__73 = Executor.<>o__65.<>p__87;
				if (Executor.<>o__65.<>p__86 == null)
				{
					Executor.<>o__65.<>p__86 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target74 = Executor.<>o__65.<>p__86.Target;
				CallSite <>p__74 = Executor.<>o__65.<>p__86;
				if (Executor.<>o__65.<>p__85 == null)
				{
					Executor.<>o__65.<>p__85 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target75 = Executor.<>o__65.<>p__85.Target;
				CallSite <>p__75 = Executor.<>o__65.<>p__85;
				if (Executor.<>o__65.<>p__84 == null)
				{
					Executor.<>o__65.<>p__84 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb3.Color = target71(<>p__71, target72(<>p__72, typeFromHandle10, target73(<>p__73, target74(<>p__74, target75(<>p__75, Executor.<>o__65.<>p__84.Target(Executor.<>o__65.<>p__84, arg), 0)))));
				DropShadowEffect dsb4 = this.DSB4;
				if (Executor.<>o__65.<>p__95 == null)
				{
					Executor.<>o__65.<>p__95 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target76 = Executor.<>o__65.<>p__95.Target;
				CallSite <>p__76 = Executor.<>o__65.<>p__95;
				if (Executor.<>o__65.<>p__94 == null)
				{
					Executor.<>o__65.<>p__94 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target77 = Executor.<>o__65.<>p__94.Target;
				CallSite <>p__77 = Executor.<>o__65.<>p__94;
				Type typeFromHandle11 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__93 == null)
				{
					Executor.<>o__65.<>p__93 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target78 = Executor.<>o__65.<>p__93.Target;
				CallSite <>p__78 = Executor.<>o__65.<>p__93;
				if (Executor.<>o__65.<>p__92 == null)
				{
					Executor.<>o__65.<>p__92 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target79 = Executor.<>o__65.<>p__92.Target;
				CallSite <>p__79 = Executor.<>o__65.<>p__92;
				if (Executor.<>o__65.<>p__91 == null)
				{
					Executor.<>o__65.<>p__91 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target80 = Executor.<>o__65.<>p__91.Target;
				CallSite <>p__80 = Executor.<>o__65.<>p__91;
				if (Executor.<>o__65.<>p__90 == null)
				{
					Executor.<>o__65.<>p__90 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb4.Color = target76(<>p__76, target77(<>p__77, typeFromHandle11, target78(<>p__78, target79(<>p__79, target80(<>p__80, Executor.<>o__65.<>p__90.Target(Executor.<>o__65.<>p__90, arg), 0)))));
				DropShadowEffect dsb5 = this.DSB5;
				if (Executor.<>o__65.<>p__101 == null)
				{
					Executor.<>o__65.<>p__101 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target81 = Executor.<>o__65.<>p__101.Target;
				CallSite <>p__81 = Executor.<>o__65.<>p__101;
				if (Executor.<>o__65.<>p__100 == null)
				{
					Executor.<>o__65.<>p__100 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target82 = Executor.<>o__65.<>p__100.Target;
				CallSite <>p__82 = Executor.<>o__65.<>p__100;
				Type typeFromHandle12 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__99 == null)
				{
					Executor.<>o__65.<>p__99 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target83 = Executor.<>o__65.<>p__99.Target;
				CallSite <>p__83 = Executor.<>o__65.<>p__99;
				if (Executor.<>o__65.<>p__98 == null)
				{
					Executor.<>o__65.<>p__98 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target84 = Executor.<>o__65.<>p__98.Target;
				CallSite <>p__84 = Executor.<>o__65.<>p__98;
				if (Executor.<>o__65.<>p__97 == null)
				{
					Executor.<>o__65.<>p__97 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target85 = Executor.<>o__65.<>p__97.Target;
				CallSite <>p__85 = Executor.<>o__65.<>p__97;
				if (Executor.<>o__65.<>p__96 == null)
				{
					Executor.<>o__65.<>p__96 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb5.Color = target81(<>p__81, target82(<>p__82, typeFromHandle12, target83(<>p__83, target84(<>p__84, target85(<>p__85, Executor.<>o__65.<>p__96.Target(Executor.<>o__65.<>p__96, arg), 0)))));
				DropShadowEffect dsb6 = this.DSB6;
				if (Executor.<>o__65.<>p__107 == null)
				{
					Executor.<>o__65.<>p__107 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target86 = Executor.<>o__65.<>p__107.Target;
				CallSite <>p__86 = Executor.<>o__65.<>p__107;
				if (Executor.<>o__65.<>p__106 == null)
				{
					Executor.<>o__65.<>p__106 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target87 = Executor.<>o__65.<>p__106.Target;
				CallSite <>p__87 = Executor.<>o__65.<>p__106;
				Type typeFromHandle13 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__105 == null)
				{
					Executor.<>o__65.<>p__105 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target88 = Executor.<>o__65.<>p__105.Target;
				CallSite <>p__88 = Executor.<>o__65.<>p__105;
				if (Executor.<>o__65.<>p__104 == null)
				{
					Executor.<>o__65.<>p__104 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target89 = Executor.<>o__65.<>p__104.Target;
				CallSite <>p__89 = Executor.<>o__65.<>p__104;
				if (Executor.<>o__65.<>p__103 == null)
				{
					Executor.<>o__65.<>p__103 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target90 = Executor.<>o__65.<>p__103.Target;
				CallSite <>p__90 = Executor.<>o__65.<>p__103;
				if (Executor.<>o__65.<>p__102 == null)
				{
					Executor.<>o__65.<>p__102 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb6.Color = target86(<>p__86, target87(<>p__87, typeFromHandle13, target88(<>p__88, target89(<>p__89, target90(<>p__90, Executor.<>o__65.<>p__102.Target(Executor.<>o__65.<>p__102, arg), 0)))));
				DropShadowEffect dsb7 = this.DSB7;
				if (Executor.<>o__65.<>p__113 == null)
				{
					Executor.<>o__65.<>p__113 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target91 = Executor.<>o__65.<>p__113.Target;
				CallSite <>p__91 = Executor.<>o__65.<>p__113;
				if (Executor.<>o__65.<>p__112 == null)
				{
					Executor.<>o__65.<>p__112 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target92 = Executor.<>o__65.<>p__112.Target;
				CallSite <>p__92 = Executor.<>o__65.<>p__112;
				Type typeFromHandle14 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__111 == null)
				{
					Executor.<>o__65.<>p__111 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target93 = Executor.<>o__65.<>p__111.Target;
				CallSite <>p__93 = Executor.<>o__65.<>p__111;
				if (Executor.<>o__65.<>p__110 == null)
				{
					Executor.<>o__65.<>p__110 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target94 = Executor.<>o__65.<>p__110.Target;
				CallSite <>p__94 = Executor.<>o__65.<>p__110;
				if (Executor.<>o__65.<>p__109 == null)
				{
					Executor.<>o__65.<>p__109 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target95 = Executor.<>o__65.<>p__109.Target;
				CallSite <>p__95 = Executor.<>o__65.<>p__109;
				if (Executor.<>o__65.<>p__108 == null)
				{
					Executor.<>o__65.<>p__108 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb7.Color = target91(<>p__91, target92(<>p__92, typeFromHandle14, target93(<>p__93, target94(<>p__94, target95(<>p__95, Executor.<>o__65.<>p__108.Target(Executor.<>o__65.<>p__108, arg), 0)))));
				DropShadowEffect dsb8 = this.DSB8;
				if (Executor.<>o__65.<>p__119 == null)
				{
					Executor.<>o__65.<>p__119 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target96 = Executor.<>o__65.<>p__119.Target;
				CallSite <>p__96 = Executor.<>o__65.<>p__119;
				if (Executor.<>o__65.<>p__118 == null)
				{
					Executor.<>o__65.<>p__118 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target97 = Executor.<>o__65.<>p__118.Target;
				CallSite <>p__97 = Executor.<>o__65.<>p__118;
				Type typeFromHandle15 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__117 == null)
				{
					Executor.<>o__65.<>p__117 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target98 = Executor.<>o__65.<>p__117.Target;
				CallSite <>p__98 = Executor.<>o__65.<>p__117;
				if (Executor.<>o__65.<>p__116 == null)
				{
					Executor.<>o__65.<>p__116 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target99 = Executor.<>o__65.<>p__116.Target;
				CallSite <>p__99 = Executor.<>o__65.<>p__116;
				if (Executor.<>o__65.<>p__115 == null)
				{
					Executor.<>o__65.<>p__115 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target100 = Executor.<>o__65.<>p__115.Target;
				CallSite <>p__100 = Executor.<>o__65.<>p__115;
				if (Executor.<>o__65.<>p__114 == null)
				{
					Executor.<>o__65.<>p__114 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb8.Color = target96(<>p__96, target97(<>p__97, typeFromHandle15, target98(<>p__98, target99(<>p__99, target100(<>p__100, Executor.<>o__65.<>p__114.Target(Executor.<>o__65.<>p__114, arg), 0)))));
				DropShadowEffect dsb9 = this.DSB9;
				if (Executor.<>o__65.<>p__125 == null)
				{
					Executor.<>o__65.<>p__125 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target101 = Executor.<>o__65.<>p__125.Target;
				CallSite <>p__101 = Executor.<>o__65.<>p__125;
				if (Executor.<>o__65.<>p__124 == null)
				{
					Executor.<>o__65.<>p__124 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target102 = Executor.<>o__65.<>p__124.Target;
				CallSite <>p__102 = Executor.<>o__65.<>p__124;
				Type typeFromHandle16 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__123 == null)
				{
					Executor.<>o__65.<>p__123 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target103 = Executor.<>o__65.<>p__123.Target;
				CallSite <>p__103 = Executor.<>o__65.<>p__123;
				if (Executor.<>o__65.<>p__122 == null)
				{
					Executor.<>o__65.<>p__122 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target104 = Executor.<>o__65.<>p__122.Target;
				CallSite <>p__104 = Executor.<>o__65.<>p__122;
				if (Executor.<>o__65.<>p__121 == null)
				{
					Executor.<>o__65.<>p__121 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target105 = Executor.<>o__65.<>p__121.Target;
				CallSite <>p__105 = Executor.<>o__65.<>p__121;
				if (Executor.<>o__65.<>p__120 == null)
				{
					Executor.<>o__65.<>p__120 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				dsb9.Color = target101(<>p__101, target102(<>p__102, typeFromHandle16, target103(<>p__103, target104(<>p__104, target105(<>p__105, Executor.<>o__65.<>p__120.Target(Executor.<>o__65.<>p__120, arg), 0)))));
				TextBoxBase listBoxSearch = this.ListBoxSearch;
				if (Executor.<>o__65.<>p__131 == null)
				{
					Executor.<>o__65.<>p__131 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
				}
				Func<CallSite, object, Brush> target106 = Executor.<>o__65.<>p__131.Target;
				CallSite <>p__106 = Executor.<>o__65.<>p__131;
				if (Executor.<>o__65.<>p__130 == null)
				{
					Executor.<>o__65.<>p__130 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, BrushConverter, object, object> target107 = Executor.<>o__65.<>p__130.Target;
				CallSite <>p__107 = Executor.<>o__65.<>p__130;
				BrushConverter arg7 = brushConverter;
				if (Executor.<>o__65.<>p__129 == null)
				{
					Executor.<>o__65.<>p__129 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target108 = Executor.<>o__65.<>p__129.Target;
				CallSite <>p__108 = Executor.<>o__65.<>p__129;
				if (Executor.<>o__65.<>p__128 == null)
				{
					Executor.<>o__65.<>p__128 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target109 = Executor.<>o__65.<>p__128.Target;
				CallSite <>p__109 = Executor.<>o__65.<>p__128;
				if (Executor.<>o__65.<>p__127 == null)
				{
					Executor.<>o__65.<>p__127 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target110 = Executor.<>o__65.<>p__127.Target;
				CallSite <>p__110 = Executor.<>o__65.<>p__127;
				if (Executor.<>o__65.<>p__126 == null)
				{
					Executor.<>o__65.<>p__126 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				listBoxSearch.CaretBrush = target106(<>p__106, target107(<>p__107, arg7, target108(<>p__108, target109(<>p__109, target110(<>p__110, Executor.<>o__65.<>p__126.Target(Executor.<>o__65.<>p__126, arg), 0)))));
				this.search.CaretBrush = this.ListBoxSearch.CaretBrush;
				this.search.BorderBrush = this.ListBoxSearch.CaretBrush;
				if (Executor.<>o__65.<>p__137 == null)
				{
					Executor.<>o__65.<>p__137 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Color), typeof(Executor)));
				}
				Func<CallSite, object, Color> target111 = Executor.<>o__65.<>p__137.Target;
				CallSite <>p__111 = Executor.<>o__65.<>p__137;
				if (Executor.<>o__65.<>p__136 == null)
				{
					Executor.<>o__65.<>p__136 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object> target112 = Executor.<>o__65.<>p__136.Target;
				CallSite <>p__112 = Executor.<>o__65.<>p__136;
				Type typeFromHandle17 = typeof(ColorConverter);
				if (Executor.<>o__65.<>p__135 == null)
				{
					Executor.<>o__65.<>p__135 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target113 = Executor.<>o__65.<>p__135.Target;
				CallSite <>p__113 = Executor.<>o__65.<>p__135;
				if (Executor.<>o__65.<>p__134 == null)
				{
					Executor.<>o__65.<>p__134 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target114 = Executor.<>o__65.<>p__134.Target;
				CallSite <>p__114 = Executor.<>o__65.<>p__134;
				if (Executor.<>o__65.<>p__133 == null)
				{
					Executor.<>o__65.<>p__133 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target115 = Executor.<>o__65.<>p__133.Target;
				CallSite <>p__115 = Executor.<>o__65.<>p__133;
				if (Executor.<>o__65.<>p__132 == null)
				{
					Executor.<>o__65.<>p__132 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				this.DefaultBrush = new SolidColorBrush(target111(<>p__111, target112(<>p__112, typeFromHandle17, target113(<>p__113, target114(<>p__114, target115(<>p__115, Executor.<>o__65.<>p__132.Target(Executor.<>o__65.<>p__132, arg), 0))))));
				foreach (object obj in ((IEnumerable)this.tabs.Items))
				{
					TabItem tabItem = (TabItem)obj;
					tabItem.BorderBrush = this.DefaultBrush;
				}
				this.reloadrScripts();
				Control uisettingsTab = this.UISettingsTab;
				if (Executor.<>o__65.<>p__143 == null)
				{
					Executor.<>o__65.<>p__143 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
				}
				Func<CallSite, object, Brush> target116 = Executor.<>o__65.<>p__143.Target;
				CallSite <>p__116 = Executor.<>o__65.<>p__143;
				if (Executor.<>o__65.<>p__142 == null)
				{
					Executor.<>o__65.<>p__142 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, BrushConverter, object, object> target117 = Executor.<>o__65.<>p__142.Target;
				CallSite <>p__117 = Executor.<>o__65.<>p__142;
				BrushConverter arg8 = brushConverter;
				if (Executor.<>o__65.<>p__141 == null)
				{
					Executor.<>o__65.<>p__141 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target118 = Executor.<>o__65.<>p__141.Target;
				CallSite <>p__118 = Executor.<>o__65.<>p__141;
				if (Executor.<>o__65.<>p__140 == null)
				{
					Executor.<>o__65.<>p__140 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target119 = Executor.<>o__65.<>p__140.Target;
				CallSite <>p__119 = Executor.<>o__65.<>p__140;
				if (Executor.<>o__65.<>p__139 == null)
				{
					Executor.<>o__65.<>p__139 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target120 = Executor.<>o__65.<>p__139.Target;
				CallSite <>p__120 = Executor.<>o__65.<>p__139;
				if (Executor.<>o__65.<>p__138 == null)
				{
					Executor.<>o__65.<>p__138 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				uisettingsTab.BorderBrush = target116(<>p__116, target117(<>p__117, arg8, target118(<>p__118, target119(<>p__119, target120(<>p__120, Executor.<>o__65.<>p__138.Target(Executor.<>o__65.<>p__138, arg), 0)))));
				Control apiTab = this.ApiTab;
				if (Executor.<>o__65.<>p__149 == null)
				{
					Executor.<>o__65.<>p__149 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Brush), typeof(Executor)));
				}
				Func<CallSite, object, Brush> target121 = Executor.<>o__65.<>p__149.Target;
				CallSite <>p__121 = Executor.<>o__65.<>p__149;
				if (Executor.<>o__65.<>p__148 == null)
				{
					Executor.<>o__65.<>p__148 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, BrushConverter, object, object> target122 = Executor.<>o__65.<>p__148.Target;
				CallSite <>p__122 = Executor.<>o__65.<>p__148;
				BrushConverter arg9 = brushConverter;
				if (Executor.<>o__65.<>p__147 == null)
				{
					Executor.<>o__65.<>p__147 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target123 = Executor.<>o__65.<>p__147.Target;
				CallSite <>p__123 = Executor.<>o__65.<>p__147;
				if (Executor.<>o__65.<>p__146 == null)
				{
					Executor.<>o__65.<>p__146 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target124 = Executor.<>o__65.<>p__146.Target;
				CallSite <>p__124 = Executor.<>o__65.<>p__146;
				if (Executor.<>o__65.<>p__145 == null)
				{
					Executor.<>o__65.<>p__145 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target125 = Executor.<>o__65.<>p__145.Target;
				CallSite <>p__125 = Executor.<>o__65.<>p__145;
				if (Executor.<>o__65.<>p__144 == null)
				{
					Executor.<>o__65.<>p__144 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof(Executor), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				apiTab.BorderBrush = target121(<>p__121, target122(<>p__122, arg9, target123(<>p__123, target124(<>p__124, target125(<>p__125, Executor.<>o__65.<>p__144.Target(Executor.<>o__65.<>p__144, arg), 0)))));
			}
			catch
			{
				File.Delete("./bin/theme.evon");
				this.setcolor();
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000099EC File Offset: 0x00007BEC
		private void colorbtnc(object sender, RoutedEventArgs e)
		{
			this.colrp(sender, null);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000099F8 File Offset: 0x00007BF8
		private void colrp(object sender, ContextMenuEventArgs e)
		{
			Executor.ThemeStrings themeStrings = new Executor.ThemeStrings
			{
				Theme = new List<Executor.ThemeStrings.ThemeSystem>
				{
					new Executor.ThemeStrings.ThemeSystem
					{
						ThemeManufacturer = "Evon",
						Color1 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)this.colr.Color.RGB_R, (byte)this.colr.Color.RGB_G, (byte)this.colr.Color.RGB_B)).ToString(),
						TextboxImage = ""
					}
				}
			};
			try
			{
				File.WriteAllText("./bin/theme.evon", JsonConvert.SerializeObject(themeStrings, 1));
			}
			catch
			{
			}
			object obj = JsonConvert.DeserializeObject(File.ReadAllText("./bin/theme.evon"));
			obj = JsonConvert.DeserializeObject(File.ReadAllText("./bin/theme.evon"));
			this.setcolor();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000A524 File Offset: 0x00008724
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				((Button)target).Click += this.addTab;
			}
		}

		// Token: 0x04000017 RID: 23
		private bool showNotiff = false;

		// Token: 0x04000018 RID: 24
		private Dictionary<TabItem, string> Texts = new Dictionary<TabItem, string>();

		// Token: 0x04000019 RID: 25
		private string searchingsystem = "";

		// Token: 0x0400001A RID: 26
		private int pagenum = 1;

		// Token: 0x0400001B RID: 27
		private bool showScripts;

		// Token: 0x0400001C RID: 28
		private int pipeDelay;

		// Token: 0x0400001D RID: 29
		private bool doingDownload;

		// Token: 0x0400001E RID: 30
		private Brush DefaultBrush = new SolidColorBrush(Color.FromRgb(153, 0, byte.MaxValue));

		// Token: 0x0400001F RID: 31
		private WebView editor = new WebView("");

		// Token: 0x04000020 RID: 32
		private Watcher w = new Watcher();

		// Token: 0x04000021 RID: 33
		public List<GameRectangle> scriptItems = new List<GameRectangle>();

		// Token: 0x0200000D RID: 13
		public class ThemeStrings
		{
			// Token: 0x0400006E RID: 110
			public List<Executor.ThemeStrings.ThemeSystem> Theme;

			// Token: 0x0200000E RID: 14
			public class ThemeSystem
			{
				// Token: 0x17000005 RID: 5
				// (get) Token: 0x0600007A RID: 122 RVA: 0x0000A9E3 File Offset: 0x00008BE3
				// (set) Token: 0x0600007B RID: 123 RVA: 0x0000A9EB File Offset: 0x00008BEB
				public string ThemeManufacturer { get; set; }

				// Token: 0x17000006 RID: 6
				// (get) Token: 0x0600007C RID: 124 RVA: 0x0000A9F4 File Offset: 0x00008BF4
				// (set) Token: 0x0600007D RID: 125 RVA: 0x0000A9FC File Offset: 0x00008BFC
				public string Color1 { get; set; }

				// Token: 0x17000007 RID: 7
				// (get) Token: 0x0600007E RID: 126 RVA: 0x0000AA05 File Offset: 0x00008C05
				// (set) Token: 0x0600007F RID: 127 RVA: 0x0000AA0D File Offset: 0x00008C0D
				public string TextboxImage { get; set; }
			}
		}
	}
}
