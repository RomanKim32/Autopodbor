using System;
using System.Globalization;
using System.Net.Mail;

namespace Autopodbor_312.Models
{
	public class PrivateInfoConfig
	{
		public const string PrivateInfo = "PrivateInfo";
		public string Email { get; set; } = String.Empty;
		public string Password { get; set; } = String.Empty;
		public string BotToken { get; set; } = String.Empty;
		public string ChatId { get; set; } = String.Empty;

	}
}
