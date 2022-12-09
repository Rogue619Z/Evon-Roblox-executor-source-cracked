using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Costura
{
	// Token: 0x0200003A RID: 58
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x0000DAAC File Offset: 0x0000BCAC
		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return "";
			}
			return culture.Name;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = currentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000DB30 File Offset: 0x0000BD30
		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000DB64 File Offset: 0x0000BD64
		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
					{
						MemoryStream memoryStream = new MemoryStream();
						AssemblyLoader.CopyTo(deflateStream, memoryStream);
						memoryStream.Position = 0L;
						return memoryStream;
					}
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			string fullName;
			if (resourceNames.TryGetValue(name, out fullName))
			{
				return AssemblyLoader.LoadStream(fullName);
			}
			return null;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000DC08 File Offset: 0x0000BE08
		private static byte[] ReadStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000DC30 File Offset: 0x0000BE30
		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string text = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				text = requestedAssemblyName.CultureInfo.Name + "." + text;
			}
			byte[] rawAssembly;
			using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, text))
			{
				if (stream == null)
				{
					return null;
				}
				rawAssembly = AssemblyLoader.ReadStream(stream);
			}
			using (Stream stream2 = AssemblyLoader.LoadStream(symbolNames, text))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = AssemblyLoader.ReadStream(stream2);
					return Assembly.Load(rawAssembly, rawSymbolStore);
				}
			}
			return Assembly.Load(rawAssembly);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			object obj = AssemblyLoader.nullCacheLock;
			lock (obj)
			{
				if (AssemblyLoader.nullCache.ContainsKey(e.Name))
				{
					return null;
				}
			}
			AssemblyName assemblyName = new AssemblyName(e.Name);
			Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
			if (assembly == null)
			{
				object obj2 = AssemblyLoader.nullCacheLock;
				lock (obj2)
				{
					AssemblyLoader.nullCache[e.Name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		// Note: this type is marked as 'beforefieldinit'.
		static AssemblyLoader()
		{
			AssemblyLoader.assemblyNames.Add("colorpicker", "costura.colorpicker.dll.compressed");
			AssemblyLoader.assemblyNames.Add("costura", "costura.costura.dll.compressed");
			AssemblyLoader.symbolNames.Add("costura", "costura.costura.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("fluxapi", "costura.fluxapi.dll.compressed");
			AssemblyLoader.assemblyNames.Add("krnlapi", "costura.krnlapi.dll.compressed");
			AssemblyLoader.assemblyNames.Add("materialdesigncolors", "costura.materialdesigncolors.dll.compressed");
			AssemblyLoader.symbolNames.Add("materialdesigncolors", "costura.materialdesigncolors.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("materialdesignthemes.wpf", "costura.materialdesignthemes.wpf.dll.compressed");
			AssemblyLoader.symbolNames.Add("materialdesignthemes.wpf", "costura.materialdesignthemes.wpf.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("microsoft.web.webview2.core", "costura.microsoft.web.webview2.core.dll.compressed");
			AssemblyLoader.assemblyNames.Add("microsoft.web.webview2.winforms", "costura.microsoft.web.webview2.winforms.dll.compressed");
			AssemblyLoader.assemblyNames.Add("microsoft.web.webview2.wpf", "costura.microsoft.web.webview2.wpf.dll.compressed");
			AssemblyLoader.assemblyNames.Add("microsoft.xaml.behaviors", "costura.microsoft.xaml.behaviors.dll.compressed");
			AssemblyLoader.symbolNames.Add("microsoft.xaml.behaviors", "costura.microsoft.xaml.behaviors.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
			AssemblyLoader.assemblyNames.Add("oxygen api", "costura.oxygen api.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.diagnostics.diagnosticsource", "costura.system.diagnostics.diagnosticsource.dll.compressed");
			AssemblyLoader.assemblyNames.Add("wearedevs_api", "costura.wearedevs_api.dll.compressed");
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000DF68 File Offset: 0x0000C168
		public static void Attach()
		{
			if (Interlocked.Exchange(ref AssemblyLoader.isAttached, 1) == 1)
			{
				return;
			}
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyResolve += AssemblyLoader.ResolveAssembly;
		}

		// Token: 0x0400020E RID: 526
		private static object nullCacheLock = new object();

		// Token: 0x0400020F RID: 527
		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		// Token: 0x04000210 RID: 528
		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		// Token: 0x04000211 RID: 529
		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		// Token: 0x04000212 RID: 530
		private static int isAttached;
	}
}
