using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;

namespace Evon.Editor
{
	// Token: 0x02000030 RID: 48
	public class WebView : WebView2
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000BB RID: 187 RVA: 0x0000C860 File Offset: 0x0000AA60
		// (remove) Token: 0x060000BC RID: 188 RVA: 0x0000C898 File Offset: 0x0000AA98
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event WebView.TextChanged OnTextChanged;

		// Token: 0x060000BD RID: 189 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		public WebView(string script = "")
		{
			WebView.<>c__DisplayClass7_0 CS$<>8__locals1 = new WebView.<>c__DisplayClass7_0();
			CS$<>8__locals1.script = script;
			base..ctor();
			CS$<>8__locals1.<>4__this = this;
			this.scr = CS$<>8__locals1.script;
			this.BeginInit();
			base.Source = new Uri(Directory.GetCurrentDirectory() + "\\bin\\Monaco.html");
			base.DefaultBackgroundColor = Color.FromArgb(20, 20, 20);
			base.EnsureCoreWebView2Async(null);
			base.CoreWebView2InitializationCompleted += delegate(object s, CoreWebView2InitializationCompletedEventArgs e)
			{
				CoreWebView2 coreWebView = CS$<>8__locals1.<>4__this.CoreWebView2;
				EventHandler<CoreWebView2WebMessageReceivedEventArgs> eventHandler;
				if ((eventHandler = CS$<>8__locals1.<>9__1) == null)
				{
					eventHandler = (CS$<>8__locals1.<>9__1 = delegate(object __, CoreWebView2WebMessageReceivedEventArgs args)
					{
						CS$<>8__locals1.<>4__this.Text = args.TryGetWebMessageAsString();
						WebView.TextChanged onTextChanged = CS$<>8__locals1.<>4__this.OnTextChanged;
						if (onTextChanged != null)
						{
							onTextChanged(CS$<>8__locals1.<>4__this.Text);
						}
					});
				}
				coreWebView.WebMessageReceived += eventHandler;
				CoreWebView2 coreWebView2 = CS$<>8__locals1.<>4__this.CoreWebView2;
				EventHandler<CoreWebView2DOMContentLoadedEventArgs> eventHandler2;
				if ((eventHandler2 = CS$<>8__locals1.<>9__2) == null)
				{
					eventHandler2 = (CS$<>8__locals1.<>9__2 = delegate(object sender, CoreWebView2DOMContentLoadedEventArgs args)
					{
						WebView.<>c__DisplayClass7_0.<<-ctor>b__2>d <<-ctor>b__2>d = new WebView.<>c__DisplayClass7_0.<<-ctor>b__2>d();
						<<-ctor>b__2>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<-ctor>b__2>d.<>4__this = CS$<>8__locals1;
						<<-ctor>b__2>d.sender = sender;
						<<-ctor>b__2>d.args = args;
						<<-ctor>b__2>d.<>1__state = -1;
						<<-ctor>b__2>d.<>t__builder.Start<WebView.<>c__DisplayClass7_0.<<-ctor>b__2>d>(ref <<-ctor>b__2>d);
					});
				}
				coreWebView2.DOMContentLoaded += eventHandler2;
				CS$<>8__locals1.<>4__this.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
				CS$<>8__locals1.<>4__this.CoreWebView2.Settings.AreDevToolsEnabled = true;
			};
			this.EndInit();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000C968 File Offset: 0x0000AB68
		public void minimap()
		{
			object arg = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
			if (WebView.<>o__8.<>p__2 == null)
			{
				WebView.<>o__8.<>p__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebView), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target = WebView.<>o__8.<>p__2.Target;
			CallSite <>p__ = WebView.<>o__8.<>p__2;
			if (WebView.<>o__8.<>p__1 == null)
			{
				WebView.<>o__8.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(WebView), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target2 = WebView.<>o__8.<>p__1.Target;
			CallSite <>p__2 = WebView.<>o__8.<>p__1;
			if (WebView.<>o__8.<>p__0 == null)
			{
				WebView.<>o__8.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minimap", typeof(WebView), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag = target(<>p__, target2(<>p__2, WebView.<>o__8.<>p__0.Target(WebView.<>o__8.<>p__0, arg), null));
			if (flag)
			{
				if (WebView.<>o__8.<>p__5 == null)
				{
					WebView.<>o__8.<>p__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebView), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target3 = WebView.<>o__8.<>p__5.Target;
				CallSite <>p__3 = WebView.<>o__8.<>p__5;
				if (WebView.<>o__8.<>p__4 == null)
				{
					WebView.<>o__8.<>p__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(WebView), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, bool, object> target4 = WebView.<>o__8.<>p__4.Target;
				CallSite <>p__4 = WebView.<>o__8.<>p__4;
				if (WebView.<>o__8.<>p__3 == null)
				{
					WebView.<>o__8.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minimap", typeof(WebView), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				bool flag2 = target3(<>p__3, target4(<>p__4, WebView.<>o__8.<>p__3.Target(WebView.<>o__8.<>p__3, arg), true));
				if (flag2)
				{
					base.CoreWebView2.ExecuteScriptAsync("ShowMinimap()");
				}
				else
				{
					base.CoreWebView2.ExecuteScriptAsync("HideMinimap()");
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		public void antiskid()
		{
			object arg = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
			if (WebView.<>o__9.<>p__2 == null)
			{
				WebView.<>o__9.<>p__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebView), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target = WebView.<>o__9.<>p__2.Target;
			CallSite <>p__ = WebView.<>o__9.<>p__2;
			if (WebView.<>o__9.<>p__1 == null)
			{
				WebView.<>o__9.<>p__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(WebView), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			Func<CallSite, object, object, object> target2 = WebView.<>o__9.<>p__1.Target;
			CallSite <>p__2 = WebView.<>o__9.<>p__1;
			if (WebView.<>o__9.<>p__0 == null)
			{
				WebView.<>o__9.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "antiskid", typeof(WebView), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag = target(<>p__, target2(<>p__2, WebView.<>o__9.<>p__0.Target(WebView.<>o__9.<>p__0, arg), null));
			if (flag)
			{
				if (WebView.<>o__9.<>p__5 == null)
				{
					WebView.<>o__9.<>p__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(WebView), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target3 = WebView.<>o__9.<>p__5.Target;
				CallSite <>p__3 = WebView.<>o__9.<>p__5;
				if (WebView.<>o__9.<>p__4 == null)
				{
					WebView.<>o__9.<>p__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(WebView), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, bool, object> target4 = WebView.<>o__9.<>p__4.Target;
				CallSite <>p__4 = WebView.<>o__9.<>p__4;
				if (WebView.<>o__9.<>p__3 == null)
				{
					WebView.<>o__9.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "antiskid", typeof(WebView), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				bool flag2 = target3(<>p__3, target4(<>p__4, WebView.<>o__9.<>p__3.Target(WebView.<>o__9.<>p__3, arg), true));
				if (flag2)
				{
					base.CoreWebView2.ExecuteScriptAsync("enableAntiSkid()");
				}
				else
				{
					base.CoreWebView2.ExecuteScriptAsync("disableAntiSkid()");
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000CDD0 File Offset: 0x0000AFD0
		public void Undo()
		{
			bool isLoaded = this.IsLoaded;
			if (isLoaded)
			{
				base.CoreWebView2.ExecuteScriptAsync("Undo()");
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000CDFC File Offset: 0x0000AFFC
		public void SetText(string text)
		{
			bool isLoaded = this.IsLoaded;
			if (isLoaded)
			{
				base.CoreWebView2.ExecuteScriptAsync("SetText(\"" + HttpUtility.JavaScriptStringEncode(text) + "\")");
			}
		}

		// Token: 0x040001D7 RID: 471
		public bool IsLoaded;

		// Token: 0x040001D9 RID: 473
		public string Text = "";

		// Token: 0x040001DA RID: 474
		public string scr;

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x060000C3 RID: 195
		public delegate void TextChanged(string Text);
	}
}
