using System;
using System.Management.Automation;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Http;

namespace PoShLog.Sinks.Http.CmdLets
{
	/// <summary>
	/// <para type="synopsis">Writes log events into given server over the network using HTTP</para>
	/// <para type="description">Adds a non-durable sink that sends log events using HTTP POST over the network.</para>
	/// <para type="description">A non-durable sink will lose data after a system or process restart.</para>
	/// </summary>
	/// <example>
	/// <code>PS> New-Logger | Add-SinkHttp -RequestUri 'https://requestbin.net/r/6y06j5z8' | Start-Logger</code>
	/// </example>
	[Cmdlet(VerbsCommon.Add, "SinkHttp")]
	[OutputType(typeof(LoggerConfiguration))]
	public class AddSinkHttp : PSCmdlet
	{
		/// <summary>
		/// <param type="description">Instance of LoggerConfiguration.</param>
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
		public LoggerConfiguration LoggerConfig { get; set; }

		/// <summary>
		/// <param type="description">The URI the request is sent to.</param>
		/// </summary>
		[Parameter(Mandatory = true)]
		public string RequestUri { get; set; }

		/// <summary>
		/// <param type="description">The maximum number of events to post in a single batch. Default value is 1000.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public int BatchPostingLimit { get; set; } = 1000;

		/// <summary>
		/// <param type="description">The maximum number of events stored in the queue in memory, waiting to be posted over the network. Default value is infinitely.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public int? QueueLimit { get; set; }

		/// <summary>
		/// <param type="description">The time to wait between checking for event batches. Default value is 2 seconds.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public TimeSpan? Period { get; set; }

		/// <summary>
		/// <param type="description">he formatter rendering individual log events into text, for example JSON. Default value is <see cref="T:Serilog.Sinks.Http.TextFormatters.NormalRenderedTextFormatter" />.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public ITextFormatter TextFormatter { get; set; }

		/// <summary>
		/// <param type="description">The formatter batching multiple log events into a payload that can be sent over the network. Default value is <see cref="T:Serilog.Sinks.Http.BatchFormatters.DefaultBatchFormatter" />.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public IBatchFormatter BatchFormatter { get; set; }

		/// <summary>
		/// <param type="description">The minimum level for events passed through the sink. Default value is <see cref="F:Serilog.Events.LevelAlias.Minimum" />.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public LogEventLevel RestrictedToMinimumLevel { get; set; }

		/// <summary>
		/// <param type="description">A custom <see cref="T:Serilog.Sinks.Http.IHttpClient" /> implementation. Default value is <see cref="T:System.Net.Http.HttpClient" />.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public IHttpClient HttpClient { get; set; }

		/// <summary>
		/// <param type="description">Configuration passed to HttpClient. Parameter is either manually specified when configuring the sink in source code or automatically passed in when configuring the sink using <see href="https://www.nuget.org/packages/Serilog.Settings.Configuration">Serilog.Settings.Configuration</see>.</param>
		/// </summary>
		[Parameter(Mandatory = false)]
		public IConfiguration Configuration { get; set; }

		/// <inheritdoc />
		protected override void ProcessRecord()
		{
			WriteObject(LoggerConfig.WriteTo.Http(RequestUri, BatchPostingLimit, QueueLimit, Period, TextFormatter, BatchFormatter, RestrictedToMinimumLevel, HttpClient, Configuration));
		}
	}
}
