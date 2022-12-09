using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace tools.Pipes
{
	// Token: 0x02000007 RID: 7
	public class Pipe
	{
		// Token: 0x0600001F RID: 31
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool WaitNamedPipe(string pipe, int timeout = 10);

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002680 File Offset: 0x00000880
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002688 File Offset: 0x00000888
		public string Name { get; set; }

		// Token: 0x06000022 RID: 34 RVA: 0x00002691 File Offset: 0x00000891
		public Pipe(string n)
		{
			this.Name = n;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026A4 File Offset: 0x000008A4
		public bool Exists()
		{
			return Pipe.WaitNamedPipe("\\\\.\\pipe\\" + this.Name, 10);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000026D0 File Offset: 0x000008D0
		public string Read()
		{
			bool flag = this.Name == null;
			if (flag)
			{
				throw new Exception("Pipe Name was not set.");
			}
			bool flag2 = this.Exists();
			if (flag2)
			{
				using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", this.Name, PipeDirection.InOut))
				{
					namedPipeClientStream.Connect();
					using (StreamReader streamReader = new StreamReader(namedPipeClientStream))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
			return "";
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000276C File Offset: 0x0000096C
		public bool Write(string content)
		{
			bool flag = this.Name == null;
			if (flag)
			{
				throw new Exception("Pipe Name was not set.");
			}
			bool flag2 = string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(content);
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = this.Exists();
				if (flag3)
				{
					using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", this.Name, PipeDirection.InOut))
					{
						namedPipeClientStream.Connect();
						using (StreamWriter streamWriter = new StreamWriter(namedPipeClientStream))
						{
							streamWriter.Write(content);
						}
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
