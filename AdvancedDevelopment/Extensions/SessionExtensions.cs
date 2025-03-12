using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AdvancedDevelopment.Extensions
{
    /// <summary>
    /// Provides extension methods for session management with JSON serialization.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Sets an object as a JSON string in the session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="key">The key to store the object under.</param>
        /// <param name="value">The object to store.</param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Gets an object from the session, deserialized from JSON.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="key">The key the object is stored under.</param>
        /// <returns>The deserialized object, or the default value of <typeparamref name="T"/> if not found.</returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
