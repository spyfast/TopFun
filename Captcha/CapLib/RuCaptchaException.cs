using System;

namespace TopFun.Captcha.CapLib
{
	public class RuCaptchaException : Exception
	{
		public RuCaptchaException() { }
		public RuCaptchaException(string message) 
			: base(message) { }
		public RuCaptchaException(string message, Exception inner) 
			: base(message, inner) { }
	}
}
