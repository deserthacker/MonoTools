﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace MonoTools.Debugger {

	[Serializable]
	public class StartTask {
		public string SourcePath { get; set; }
		public int MonoDebuggerPort { get; set; }
		public MonoTools.Library.Frameworks Framework { get; set; }
		public bool NetFXBuild { get; set; }
	}

	[Serializable]
	public class StartProgramTask: StartTask {
		public string Program { get; set; }
		public string Arguments { get; set; }
		public string WorkingDir { get; set; }
		public string Display { get; set; }
		public NameValueCollection MonoOptions { get; set; }
	}

	[Serializable]
	public class StartWebTask: StartTask {
		public int XspPort { get; set; }
		public bool Ssl { get; set; }
		public string Path { get; set; }
		public string LogLevels { get; set; }
	}
}
