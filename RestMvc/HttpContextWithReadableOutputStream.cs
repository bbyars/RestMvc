using System;
using System.Collections;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Profile;

namespace RestMvc
{
    /// <summary>
    /// To handle HEAD requests, we need to capture the output of a GET
    /// without sending the result to the user.  The only way I could think
    /// of handling that was by proxying the HttpContext, and replacing
    /// the Response.Output.
    /// </summary>
    public class HttpContextWithReadableOutputStream : HttpContextBase, IDisposable
    {
        private readonly Controller controller;
        private readonly HttpContextBase proxiedContext;
        private readonly ResponseWithReadableOutputStream response;

        public HttpContextWithReadableOutputStream(Controller controller)
        {
            this.controller = controller;
            proxiedContext = controller.ControllerContext.HttpContext;
            response = new ResponseWithReadableOutputStream(proxiedContext.Response);
            controller.ControllerContext.HttpContext = this;
        }

        public virtual string GetResponseText()
        {
            return response.OutputText;
        }

        public override HttpResponseBase Response
        {
            get { return response; }
        }

        #region Proxied Methods
        public override void AddError(Exception errorInfo)
        {
            proxiedContext.AddError(errorInfo);
        }

        public override Exception[] AllErrors
        {
            get { return proxiedContext.AllErrors; }
        }

        public override HttpApplicationStateBase Application
        {
            get { return proxiedContext.Application; }
        }

        public override HttpApplication ApplicationInstance
        {
            get { return proxiedContext.ApplicationInstance; }
            set { proxiedContext.ApplicationInstance = value; }
        }

        public override Cache Cache
        {
            get { return proxiedContext.Cache; }
        }

        public override void ClearError()
        {
            proxiedContext.ClearError();
        }

        public override IHttpHandler CurrentHandler
        {
            get { return proxiedContext.CurrentHandler; }
        }

        public override RequestNotification CurrentNotification
        {
            get { return proxiedContext.CurrentNotification; }
        }

        public override Exception Error
        {
            get { return proxiedContext.Error; }
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey)
        {
            return proxiedContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
        {
            return proxiedContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey)
        {
            return proxiedContext.GetLocalResourceObject(virtualPath, resourceKey);
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture)
        {
            return proxiedContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
        }

        public override object GetSection(string sectionName)
        {
            return proxiedContext.GetSection(sectionName);
        }

        public override object GetService(Type serviceType)
        {
            return proxiedContext.GetService(serviceType);
        }

        public override IHttpHandler Handler
        {
            get { return proxiedContext.Handler; }
            set { proxiedContext.Handler = value; }
        }

        public override bool IsCustomErrorEnabled
        {
            get { return proxiedContext.IsCustomErrorEnabled; }
        }

        public override bool IsDebuggingEnabled
        {
            get { return proxiedContext.IsDebuggingEnabled; }
        }

        public override bool IsPostNotification
        {
            get { return proxiedContext.IsPostNotification; }
        }

        public override IDictionary Items
        {
            get { return proxiedContext.Items; }
        }

        public override IHttpHandler PreviousHandler
        {
            get { return proxiedContext.PreviousHandler; }
        }

        public override ProfileBase Profile
        {
            get { return proxiedContext.Profile; }
        }

        public override HttpRequestBase Request
        {
            get { return proxiedContext.Request; }
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString)
        {
            proxiedContext.RewritePath(filePath, pathInfo, queryString);
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
        {
            proxiedContext.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
        }

        public override void RewritePath(string path)
        {
            proxiedContext.RewritePath(path);
        }

        public override void RewritePath(string path, bool rebaseClientPath)
        {
            proxiedContext.RewritePath(path, rebaseClientPath);
        }

        public override HttpServerUtilityBase Server
        {
            get { return proxiedContext.Server; }
        }

        public override HttpSessionStateBase Session
        {
            get { return proxiedContext.Session; }
        }

        public override bool SkipAuthorization
        {
            get { return proxiedContext.SkipAuthorization; }
            set { proxiedContext.SkipAuthorization = value; }
        }

        public override DateTime Timestamp
        {
            get { return proxiedContext.Timestamp; }
        }

        public override TraceContext Trace
        {
            get { return proxiedContext.Trace; }
        }

        public override IPrincipal User
        {
            get { return proxiedContext.User; }
            set { proxiedContext.User = value; }
        }
        #endregion

        public void Dispose()
        {
            controller.ControllerContext.HttpContext = proxiedContext;
        }
    }
}
