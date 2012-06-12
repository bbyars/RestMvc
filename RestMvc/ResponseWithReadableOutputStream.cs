using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace RestMvc
{
    /// <summary>
    /// Acts as a proxy to the real HttpResponse during HEAD requests.
    /// Used only to capture the output stream without actually sending
    /// it to the client.
    /// </summary>
    public class ResponseWithReadableOutputStream : HttpResponseBase
    {
        private readonly HttpResponseBase proxiedResponse;
        private readonly StringWriter output = new StringWriter();

        public ResponseWithReadableOutputStream(HttpResponseBase proxiedResponse)
        {
            this.proxiedResponse = proxiedResponse;
        }

        /// <summary>
        /// Returns the contents of the output stream converted to a string
        /// </summary>
        public virtual string OutputText
        {
            get { return output.ToString(); }
        }

        public override TextWriter Output
        {
            get { return output; }
        }

        // Sadly, HttpResponse does not use the encapsulation it provides for
        // writing to it's Output writer, so we're forced to change these methods as well
        public override void Write(string s)
        {
            Output.Write(s);
        }

        public override void Write(char ch)
        {
            Output.Write(ch);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Output.Write(buffer, index, count);
        }

        public override void Write(object obj)
        {
            Output.Write(obj);
        }

        #region Proxied Methods
        public override void AddCacheDependency(params CacheDependency[] dependencies)
        {
            proxiedResponse.AddCacheDependency(dependencies);
        }

        public override void AddCacheItemDependencies(string[] cacheKeys)
        {
            proxiedResponse.AddCacheItemDependencies(cacheKeys);
        }

        public override void AddCacheItemDependencies(ArrayList cacheKeys)
        {
            proxiedResponse.AddCacheItemDependencies(cacheKeys);
        }

        public override void AddCacheItemDependency(string cacheKey)
        {
            proxiedResponse.AddCacheItemDependency(cacheKey);
        }

        public override void AddFileDependencies(ArrayList filenames)
        {
            proxiedResponse.AddFileDependencies(filenames);
        }

        public override void AddFileDependencies(string[] filenames)
        {
            proxiedResponse.AddFileDependencies(filenames);
        }

        public override void AddFileDependency(string filename)
        {
            proxiedResponse.AddFileDependency(filename);
        }

        public override void AddHeader(string name, string value)
        {
            proxiedResponse.AddHeader(name, value);
        }

        public override void AppendCookie(HttpCookie cookie)
        {
            proxiedResponse.AppendCookie(cookie);
        }

        public override void AppendHeader(string name, string value)
        {
            proxiedResponse.AppendHeader(name, value);
        }

        public override void AppendToLog(string param)
        {
            proxiedResponse.AppendToLog(param);
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return proxiedResponse.ApplyAppPathModifier(virtualPath);
        }

        public override void BinaryWrite(byte[] buffer)
        {
            proxiedResponse.BinaryWrite(buffer);
        }

        public override bool Buffer
        {
            get { return proxiedResponse.Buffer; }
            set { proxiedResponse.Buffer = value; }
        }

        public override bool BufferOutput
        {
            get { return proxiedResponse.BufferOutput; }
            set { proxiedResponse.BufferOutput = value; }
        }

        public override HttpCachePolicyBase Cache
        {
            get { return proxiedResponse.Cache; }
        }

        public override string CacheControl
        {
            get { return proxiedResponse.CacheControl; }
            set { proxiedResponse.CacheControl = value; }
        }

        public override string Charset
        {
            get { return proxiedResponse.Charset; }
            set { proxiedResponse.Charset = value; }
        }

        public override void Clear()
        {
            proxiedResponse.Clear();
        }

        public override void ClearContent()
        {
            proxiedResponse.ClearContent();
        }

        public override void ClearHeaders()
        {
            proxiedResponse.ClearHeaders();
        }

        public override void Close()
        {
            proxiedResponse.Close();
        }

        public override Encoding ContentEncoding
        {
            get { return proxiedResponse.ContentEncoding; }
            set { proxiedResponse.ContentEncoding = value; }
        }

        public override string ContentType
        {
            get { return proxiedResponse.ContentType; }
            set { proxiedResponse.ContentType = value; }
        }

        public override HttpCookieCollection Cookies
        {
            get { return proxiedResponse.Cookies; }
        }

        public override void DisableKernelCache()
        {
            proxiedResponse.DisableKernelCache();
        }

        public override void End()
        {
            proxiedResponse.End();
        }

        public override int Expires
        {
            get { return proxiedResponse.Expires; }
            set { proxiedResponse.Expires = value; }
        }

        public override DateTime ExpiresAbsolute
        {
            get { return proxiedResponse.ExpiresAbsolute; }
            set { proxiedResponse.ExpiresAbsolute = value; }
        }

        public override Stream Filter
        {
            get { return proxiedResponse.Filter; }
            set { proxiedResponse.Filter = value; }
        }

        public override void Flush()
        {
            proxiedResponse.Flush();
        }

        public override Encoding HeaderEncoding
        {
            get { return proxiedResponse.HeaderEncoding; }
            set { proxiedResponse.HeaderEncoding = value; }
        }

        public override NameValueCollection Headers
        {
            get { return proxiedResponse.Headers; }
        }

        public override bool IsClientConnected
        {
            get { return proxiedResponse.IsClientConnected; }
        }

        public override bool IsRequestBeingRedirected
        {
            get { return proxiedResponse.IsRequestBeingRedirected; }
        }

        public override Stream OutputStream
        {
            get { return proxiedResponse.OutputStream; }
        }

        public override void Pics(string value)
        {
            proxiedResponse.Pics(value);
        }

        public override void Redirect(string url)
        {
            proxiedResponse.Redirect(url);
        }

        public override void Redirect(string url, bool endResponse)
        {
            proxiedResponse.Redirect(url, endResponse);
        }

        public override string RedirectLocation
        {
            get { return proxiedResponse.RedirectLocation; }
            set { proxiedResponse.RedirectLocation = value; }
        }

        public override void RemoveOutputCacheItem(string path)
        {
            proxiedResponse.RemoveOutputCacheItem(path);
        }

        public override void SetCookie(HttpCookie cookie)
        {
            proxiedResponse.SetCookie(cookie);
        }

        public override string Status
        {
            get { return proxiedResponse.Status; }
            set { proxiedResponse.Status = value; }
        }

        public override int StatusCode
        {
            get { return proxiedResponse.StatusCode; }
            set { proxiedResponse.StatusCode = value; }
        }

        public override string StatusDescription
        {
            get { return proxiedResponse.StatusDescription; }
            set { proxiedResponse.StatusDescription = value; }
        }

        public override int SubStatusCode
        {
            get { return proxiedResponse.SubStatusCode; }
            set { proxiedResponse.SubStatusCode = value; }
        }

        public override bool SuppressContent
        {
            get { return proxiedResponse.SuppressContent; }
            set { proxiedResponse.SuppressContent = value; }
        }

        public override void TransmitFile(string filename)
        {
            proxiedResponse.TransmitFile(filename);
        }

        public override void TransmitFile(string filename, long offset, long length)
        {
            proxiedResponse.TransmitFile(filename, offset, length);
        }

        public override bool TrySkipIisCustomErrors
        {
            get { return proxiedResponse.TrySkipIisCustomErrors; }
            set { proxiedResponse.TrySkipIisCustomErrors = value; }
        }

        public override void WriteFile(IntPtr fileHandle, long offset, long size)
        {
            proxiedResponse.WriteFile(fileHandle, offset, size);
        }

        public override void WriteFile(string filename)
        {
            proxiedResponse.WriteFile(filename);
        }

        public override void WriteFile(string filename, bool readIntoMemory)
        {
            proxiedResponse.WriteFile(filename, readIntoMemory);
        }

        public override void WriteFile(string filename, long offset, long size)
        {
            proxiedResponse.WriteFile(filename, offset, size);
        }

        public override void WriteSubstitution(HttpResponseSubstitutionCallback callback)
        {
            proxiedResponse.WriteSubstitution(callback);
        }
        #endregion
    }
}
