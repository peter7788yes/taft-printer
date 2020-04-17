using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using 離線列印Client程式.L1_1O;
using 離線列印Client程式.Properties;

[GeneratedCode("System.Web.Services", "4.6.1087.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "L1_1OSoap", Namespace = "http://tempuri.org/")]
public class L1_1O : SoapHttpClientProtocol
{
	private SendOrPostCallback UploadDataOperationCompleted;

	private bool useDefaultCredentialsSetExplicitly;

	public new string Url
	{
		get
		{
			return base.Url;
		}
		set
		{
			if (IsLocalFileSystemWebService(base.Url) && !useDefaultCredentialsSetExplicitly && !IsLocalFileSystemWebService(value))
			{
				base.UseDefaultCredentials = false;
			}
			base.Url = value;
		}
	}

	public new bool UseDefaultCredentials
	{
		get
		{
			return base.UseDefaultCredentials;
		}
		set
		{
			base.UseDefaultCredentials = value;
			useDefaultCredentialsSetExplicitly = true;
		}
	}

	public event UploadDataCompletedEventHandler UploadDataCompleted;

	public L1_1O()
	{
		Url = Settings.Default.離線列印Client程式_L1_1O_L1_1O;
		if (IsLocalFileSystemWebService(Url))
		{
			UseDefaultCredentials = true;
			useDefaultCredentialsSetExplicitly = false;
		}
		else
		{
			useDefaultCredentialsSetExplicitly = true;
		}
	}

	[SoapDocumentMethod("http://tempuri.org/UploadData", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string UploadData(string xmlData)
	{
		return (string)Invoke("UploadData", new object[1]
		{
			xmlData
		})[0];
	}

	public void UploadDataAsync(string xmlData)
	{
		UploadDataAsync(xmlData, null);
	}

	public void UploadDataAsync(string xmlData, object userState)
	{
		if (UploadDataOperationCompleted == null)
		{
			UploadDataOperationCompleted = new SendOrPostCallback(OnUploadDataOperationCompleted);
		}
		InvokeAsync("UploadData", new object[1]
		{
			xmlData
		}, UploadDataOperationCompleted, userState);
	}

	private void OnUploadDataOperationCompleted(object arg)
	{
		if (this.UploadDataCompleted != null)
		{
			InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
			this.UploadDataCompleted(this, new UploadDataCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}

	private bool IsLocalFileSystemWebService(string url)
	{
		if (url == null || url == string.Empty)
		{
			return false;
		}
		Uri uri = new Uri(url);
		if (uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		return false;
	}
}
