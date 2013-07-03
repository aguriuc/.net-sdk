using System.Collections.Generic;
using CTCT.Components.Activities;

namespace CTCT.Services
{
    /// <summary>
    /// Interface for ActivityService class.
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// Adds or updates contacts to one or more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="addContacts">AddContacts object.</param>
        /// <returns>Returns an Activity object.</returns>
        Activity AddContacts(string accessToken, string apiKey, AddContacts addContacts);
        /// <summary>
        /// Removes contacts from one ore more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="emailAddresses">List of email addresses.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        Activity RemoveContactsFromLists(string accessToken, string apiKey, IList<string> emailAddresses, IList<string> lists);
        /// <summary>
        /// Clears all contacts from one or more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="lists">Array of list id's to be cleared.</param>
        /// <returns>Returns an Activity object.</returns>
        Activity ClearLists(string accessToken, string apiKey, IList<string> lists);
        /// <summary>
        /// Exports contacts from the specified contact list to a CSV file.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="exportContacts">Export contacts object.</param>
        /// <returns>Returns an Activity object.</returns>
        Activity ExportContacts(string accessToken, string apiKey, ExportContacts exportContacts);
        /// <summary>
        /// Returns a list with the last 50 bulk activities.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <returns>Returns the list of activities.</returns>
        IList<Activity> GetLast50BulkActivities(string accessToken, string apiKey);
        /// <summary>
        /// Get an activity.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="activityId">The activity identification.</param>
        /// <param name="status">The activity status.</param>
        /// <param name="type">The activity type.</param>
        /// <returns>Returns the activity identified by its id.</returns>
        IList<Activity> GetDetailedReport(string accessToken, string apiKey, string activityId, ActivityStatus? status, ActivityType? type);
        /// <summary>
        /// Adds or updates contacts to one or more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="filename">Multipart file name.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        Activity AddContactsMultipart(string accessToken, string apiKey, string filename, IList<string> lists);
        /// <summary>
        /// Removes contacts from one ore more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="filename">Multipart file name.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        Activity RemoveContactsMultipart(string accessToken, string apiKey, string filename, IList<string> lists);
        /// <summary>
        /// Returns the status of the specified activity.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="activityId">The activity id.</param>
        /// <returns></returns>
        Activity GetActivity(string accessToken, string apiKey, string activityId);
    }
}
