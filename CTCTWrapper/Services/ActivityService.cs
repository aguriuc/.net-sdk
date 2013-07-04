using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTCT.Components.Activities;
using CTCT.Util;
using CTCT.Components;
using CTCT.Exceptions;
using System.Collections.Specialized;
using System.IO;

namespace CTCT.Services
{
    /// <summary>
    /// Performs all actions pertaining to scheduling Constant Contact Activities.
    /// </summary>
    public class ActivityService : BaseService, IActivityService
    {
        /// <summary>
        /// Adds or updates contacts to one or more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="addContacts">AddContacts object.</param>
        /// <returns>Returns an Activity object.</returns>
        public Activity AddContacts(string accessToken, string apiKey, AddContacts addContacts)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.AddContactsActivity);
            string json = addContacts.ToJSON();
            CUrlResponse response = RestClient.Post(url, accessToken, apiKey, json);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }

        /// <summary>
        /// Removes contacts from one ore more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="emailAddresses">List of email addresses.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        public Activity RemoveContactsFromLists(string accessToken, string apiKey, IList<string> emailAddresses, IList<string> lists)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.RemoveFromListsActivity);
            RemoveContact removeContact = new RemoveContact()
            {
                ImportData = new List<ImportEmailAddress>() { new ImportEmailAddress() { EmailAddresses = emailAddresses } },
                Lists = lists
            };

            string json = removeContact.ToJSON();
            CUrlResponse response = RestClient.Post(url, accessToken, apiKey, json);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }

        /// <summary>
        /// Clears all contacts from one or more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="lists">Array of list id's to be cleared.</param>
        /// <returns>Returns an Activity object.</returns>
        public Activity ClearLists(string accessToken, string apiKey, IList<string> lists)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.ClearListsActivity);

            ClearContactList clearContact = new ClearContactList() { Lists = lists };
            string json = clearContact.ToJSON();
            CUrlResponse response = RestClient.Post(url, accessToken, apiKey, json);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }

        /// <summary>
        /// Exports contacts from the specified contact list to a CSV file.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="exportContacts">Export contacts object.</param>
        /// <returns>Returns an Activity object.</returns>
        public Activity ExportContacts(string accessToken, string apiKey, ExportContacts exportContacts)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.ExportContactsActivity);
            string json = exportContacts.ToJSON();
            CUrlResponse response = RestClient.Post(url, accessToken, apiKey, json);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }

        /// <summary>
        /// Returns a list with the last 50 bulk activities.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>Returns the list of activities.</returns>
        public IList<Activity> GetLast50BulkActivities(string accessToken, string apiKey)
        {
            IList<Activity> activities = new List<Activity>();
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.Activities);
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activities = response.Get<IList<Activity>>();
            }

            return activities;
        }

        /// <summary>
        /// Get an activity.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="activityId">The activity identification.</param>
        /// <param name="status">The activity status.</param>
        /// <param name="type">The activity type.</param>
        /// <returns>Returns the activity identified by its id.</returns>
        public IList<Activity> GetDetailedReport(string accessToken, string apiKey, string activityId, ActivityStatus? status, ActivityType? type)
        {
            IList<Activity> activities = new List<Activity>();
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.Activities);
            if (status != null)
            {
                url = String.Concat(url, "?status=", status.ToString());
            }
            if (type != null)
            {
                url = String.Concat(url, (status != null) ? "&type=" : "?type=", type.ToString());
            }
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activities = response.Get<IList<Activity>>();
            }

            return activities;
        }

        /// <summary>
        /// Adds or updates contacts to one or more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="filename">Multipart file name.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        public Activity AddContactsMultipart(string accessToken, string apiKey, string filename, IList<string> lists)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.AddContactsActivity);
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("file_name", Path.GetFileName(filename));
            nvc.Add("lists", String.Join(",", lists.ToArray()));
            CUrlResponse response = RestClient.HttpPostMultipart(url, accessToken, apiKey, filename, nvc);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }

        /// <summary>
        /// Removes contacts from one ore more contact lists.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="filename">Multipart file name.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        public Activity RemoveContactsMultipart(string accessToken, string apiKey, string filename, IList<string> lists)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, Config.Endpoints.RemoveFromListsActivity);
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("file_name", Path.GetFileName(filename));
            nvc.Add("lists", String.Join(",", lists.ToArray()));
            CUrlResponse response = RestClient.HttpPostMultipart(url, accessToken, apiKey, filename, nvc);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }

        /// <summary>
        /// Returns the status of the specified activity.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="activityId">The activity id.</param>
        /// <returns></returns>
        public Activity GetActivity(string accessToken, string apiKey, string activityId)
        {
            Activity activity = null;
            string url = String.Concat(Config.Endpoints.BaseUrl, String.Format(Config.Endpoints.Activity, activityId));
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                activity = response.Get<Activity>();
            }

            return activity;
        }
    }

    /// <summary>
    /// Activity status.
    /// </summary>
    public enum ActivityStatus
    {
        /// <summary>
        /// Unconfirmed.
        /// </summary>
        UNCONFIRMED,
        /// <summary>
        /// Initial state for an activity after it is created.
        /// </summary>
        PENDING,
        /// <summary>
        /// The activity has been retrieved and is in the queue to be run.
        /// </summary>
        QUEUED,
        /// <summary>
        /// The activity has been picked up from the queue and is running.
        /// </summary>
        RUNNING,
        /// <summary>
        /// The activity has completed without errors or warnings.
        /// </summary>
        COMPLETE,
        /// <summary>
        /// There were error or warning conditions after the job was run.
        /// </summary>
        ERROR
    }

    /// <summary>
    /// Activity type.
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// Add the contacts to contact list(s) specified in the import file.
        /// </summary>
        ADD_CONTACTS,
        /// <summary>
        /// Remove the contacts from the list(s), all specified in the import file.
        /// </summary>
        REMOVE_CONTACTS_FROM_LISTS,
        /// <summary>
        /// Removes all contacts from the contactlist(s) specified in the import file.
        /// </summary>
        CLEAR_CONTACTS_FROM_LISTS,
        /// <summary>
        /// Export contacts to a supported file type.
        /// </summary>
        EXPORT_CONTACTS
    }
}
