using System;
using System.Diagnostics;
using System.Timers;

namespace tools.ProcessTools
{
	// Token: 0x02000005 RID: 5
	public class Watcher : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000010 RID: 16 RVA: 0x00002450 File Offset: 0x00000650
		// (remove) Token: 0x06000011 RID: 17 RVA: 0x00002488 File Offset: 0x00000688
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Watcher.ProcessDelegate OnProcessMade;

		// Token: 0x06000012 RID: 18 RVA: 0x000024C0 File Offset: 0x000006C0
		protected virtual void Dispose(bool disposing)
		{
			bool flag = !this.DisposedValue;
			if (flag)
			{
				if (disposing)
				{
					this._watcherTimer.Dispose();
				}
				this.DisposedValue = true;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024F5 File Offset: 0x000006F5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002508 File Offset: 0x00000708
		private void OnTick(object sender, ElapsedEventArgs e)
		{
			Process[] processesByName = Process.GetProcessesByName(this._processName);
			bool flag = processesByName.Length != 0;
			if (flag)
			{
				processesByName[0].Exited += delegate(object ee, EventArgs eee)
				{
					this._watcherTimer.Start();
				};
				processesByName[0].EnableRaisingEvents = true;
				this._watcherTimer.Stop();
				this.OnProcessMade();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002564 File Offset: 0x00000764
		public void Initialize(string procName)
		{
			bool flag = !this._inited;
			if (flag)
			{
				this._watcherTimer.Interval = 2000.0;
				this._watcherTimer.Elapsed += this.OnTick;
				this._processName = procName;
				this._inited = true;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025BC File Offset: 0x000007BC
		public void SwitchProcess(string name)
		{
			this._processName = name;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025C8 File Offset: 0x000007C8
		public void Start()
		{
			bool flag = this.OnProcessMade == null;
			if (flag)
			{
				throw new Exception("Expected 'onProcessMade' Delegate. None was given");
			}
			bool flag2 = !this._inited;
			if (flag2)
			{
				throw new Exception("Expected Processwatcher to be initialized");
			}
			this._watcherTimer.Start();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002614 File Offset: 0x00000814
		public void Stop()
		{
			bool flag = this.OnProcessMade == null;
			if (flag)
			{
				throw new Exception("Expected 'onProcessMade' Delegate. None was given");
			}
			bool flag2 = !this._inited;
			if (flag2)
			{
				throw new Exception("Expected Processwatcher to be initialized");
			}
			this._watcherTimer.Stop();
		}

		// Token: 0x0400000E RID: 14
		public bool DisposedValue;

		// Token: 0x0400000F RID: 15
		private Timer _watcherTimer = new Timer();

		// Token: 0x04000010 RID: 16
		private string _processName;

		// Token: 0x04000011 RID: 17
		public bool _inited;

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x0600001C RID: 28
		public delegate void ProcessDelegate();
	}
}
