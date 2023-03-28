using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Integration.Abstract.Helpers
{
    /// <summary>
    /// This helper method provides a way to set contextual data that flows with any forward calls. This provides iPaaS with some critical functionality, 
    /// but may be of limited use to other developers. The primary use of this class for external developers will be in the UniversalCancellationToken 
    /// that is stored in the CallContext. Under certain conditions, iPaaS may choose to terminate a long-running external process. So where possible 
    /// (e.g. on an async call to an external API) developers should utilize the given cancellation token to allow us to gracefully terminate the process.
    /// 
    /// This code was based on the article here: https://www.cazzulino.com/callcontext-netstandard-netcore.html
    /// </summary>
    public static class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, object data) =>
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
        public static object GetData(string name) =>
            state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;


        /// <summary>
        /// The cancellation token that will persist throughout the lifetime of a given transfer request. Any async calls could use this
        /// token to allow iPaaS to gracefully exit from any excessively long transfers.
        /// </summary>
        public static CancellationToken? UniversalCancellationToken
        {
            get
            {
                var cancellationToken_raw = GetData("CancellationToken");
                //If there is no cancellation token set, just use the normal sleep process
                if (cancellationToken_raw == null)
                    return null;

                return (CancellationToken)cancellationToken_raw;
            }

            set
            {
                SetData("CancellationToken", value);
            }
        }

        /// <summary>
        /// Determines if the current hook was sent in DebugMode or not. DebugMode is triggered by sending a normal scope with /debug appended to it.
        /// E.g. sending product/updated/debug instead of product/updated as the scope will cause the transfer to run in debugmode. External integration dlls may 
        /// want to trigger additional logging or handle logging routines differently in debugmode, so we expose that setting here.
        /// </summary>
        public static bool DebugMode
        {
            get
            {
                var callContextData = GetData("DebugMode");
                if (callContextData == null)
                    return false;
                return (bool)callContextData;
            }
            set
            {
                SetData("DebugMode", value);
            }
        }
    }
}
