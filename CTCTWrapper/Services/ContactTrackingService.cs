﻿using System;
using CTCT.Components;
using CTCT.Components.Tracking;
using CTCT.Util;
using CTCT.Exceptions;

namespace CTCT.Services
{
    /// <summary>
    /// Performs all actions pertaining to Contact Tracking.
    /// </summary>
    public class ContactTrackingService : BaseService, IContactTrackingService
    {
        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        public ResultSet<BounceActivity> GetBounces(string accessToken, string apiKey, string contactId, int? limit)
        {
            return GetBounces(accessToken, apiKey, contactId, limit, null);
        }

        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        public ResultSet<BounceActivity> GetBounces(string accessToken, string apiKey, Pagination pag)
        {
            return GetBounces(accessToken, apiKey, null, null, pag);
        }

        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        private ResultSet<BounceActivity> GetBounces(string accessToken, string apiKey, string contactId, int? limit, Pagination pag)
        {
            ResultSet<BounceActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingBounces, new object[] { contactId }, new object[] { "limit", limit }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<BounceActivity>>(response.Body);
            }

            return results;
        }

        /// <summary>
        /// Get clicks for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity</returns>
        public ResultSet<ClickActivity> GetClicks(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince)
        {
            return GetClicks(accessToken, apiKey, contactId, limit, createdSince, null);
        }

        /// <summary>
        /// Get clicks for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity</returns>
        public ResultSet<ClickActivity> GetClicks(string accessToken, string apiKey, Pagination pag)
        {
            return GetClicks(accessToken, apiKey, null, null, null, pag);
        }

        /// <summary>
        /// Get clicks for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity</returns>
        private ResultSet<ClickActivity> GetClicks(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince, Pagination pag)
        {
            ResultSet<ClickActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingClicks, new object[] { contactId }, new object[] { "limit", limit, "created_since", Extensions.ToISO8601String(createdSince) }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<ClickActivity>>(response.Body);
            }

            return results;
        }

        /// <summary>
        /// Get forwards for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        public ResultSet<ForwardActivity> GetForwards(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince)
        {
            return GetForwards(accessToken, apiKey, contactId, limit, createdSince, null);
        }

        /// <summary>
        /// Get forwards for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        public ResultSet<ForwardActivity> GetForwards(string accessToken, string apiKey, Pagination pag)
        {
            return GetForwards(accessToken, apiKey, null, null, null, pag);
        }

        /// <summary>
        /// Get forwards for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        private ResultSet<ForwardActivity> GetForwards(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince, Pagination pag)
        {
            ResultSet<ForwardActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingForwards, new object[] { contactId }, new object[] { "limit", limit, "created_since", Extensions.ToISO8601String(createdSince) }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<ForwardActivity>>(response.Body);
            }

            return results;
        }

        /// <summary>
        /// Get opens for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        public ResultSet<OpenActivity> GetOpens(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince)
        {
            return GetOpens(accessToken, apiKey, contactId, limit, createdSince, null);
        }

        /// <summary>
        /// Get opens for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        public ResultSet<OpenActivity> GetOpens(string accessToken, string apiKey, Pagination pag)
        {
            return GetOpens(accessToken, apiKey, null, null, null, pag);
        }

        /// <summary>
        /// Get opens for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        private ResultSet<OpenActivity> GetOpens(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince, Pagination pag)
        {
            ResultSet<OpenActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingOpens, new object[] { contactId }, new object[] { "limit", limit, "created_since", Extensions.ToISO8601String(createdSince) }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<OpenActivity>>(response.Body);
            }

            return results;
        }

        /// <summary>
        /// Get sends for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link SendActivity.</returns>
        public ResultSet<SendActivity> GetSends(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince)
        {
            return GetSends(accessToken, apiKey, contactId, limit, createdSince, null);
        }

        /// <summary>
        /// Get sends for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link SendActivity.</returns>
        public ResultSet<SendActivity> GetSends(string accessToken, string apiKey, Pagination pag)
        {
            return GetSends(accessToken, apiKey, null, null, null, pag);
        }

        /// <summary>
        /// Get sends for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link SendActivity.</returns>
        private ResultSet<SendActivity> GetSends(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince, Pagination pag)
        {
            ResultSet<SendActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingSends, new object[] { contactId }, new object[] { "limit", limit, "created_since", Extensions.ToISO8601String(createdSince) }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<SendActivity>>(response.Body);
            }

            return results;
        }

        /// <summary>
        /// Get opt outs for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        public ResultSet<UnsubscribeActivity> GetUnsubscribes(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince)
        {
            return GetUnsubscribes(accessToken, apiKey, contactId, limit, createdSince, null);
        }

        /// <summary>
        /// Get opt outs for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        public ResultSet<UnsubscribeActivity> GetUnsubscribes(string accessToken, string apiKey, Pagination pag)
        {
            return GetUnsubscribes(accessToken, apiKey, null, null, null, pag);
        }

        /// <summary>
        /// Get opt outs for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        private ResultSet<UnsubscribeActivity> GetUnsubscribes(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince, Pagination pag)
        {
            ResultSet<UnsubscribeActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingUnsubscribes, new object[] { contactId }, new object[] { "limit", limit, "created_since", Extensions.ToISO8601String(createdSince) }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<UnsubscribeActivity>>(response.Body);
            }

            return results;
        }

        /// <summary>
        /// Get a summary of reporting data for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Tracking summary.</returns>
        public TrackingSummary GetSummary(string accessToken, string apiKey, string contactId)
        {
            TrackingSummary summary = null;
            string url = Config.ConstructUrl(Config.Endpoints.ContactTrackingSummary, new object[] { contactId }, null);
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                summary = Component.FromJSON<TrackingSummary>(response.Body);
            }

            return summary;
        }

        /// <summary>
        /// Get a summary of reporting data for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Tracking summary.</returns>
        public TrackingSummaryByEmailCampaign GetSummaryByEmailCampaign(string accessToken, string apiKey, string contactId)
        {
            TrackingSummaryByEmailCampaign summary = null;
            string url = Config.ConstructUrl(Config.Endpoints.ContactTrackingSummaryByCampaign, new object[] { contactId }, null);
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                summary = Component.FromJSON<TrackingSummaryByEmailCampaign>(response.Body);
            }

            return summary;
        }

        /// <summary>
        /// Get all activities for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        public ResultSet<AllActivity> GetAllActivities(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince)
        {
            return GetAllActivities(accessToken, apiKey, contactId, limit, createdSince, null);
        }

        /// <summary>
        /// Get all activities for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        public ResultSet<AllActivity> GetAllActivities(string accessToken, string apiKey, Pagination pag)
        {
            return GetAllActivities(accessToken, apiKey, null, null, null, pag);
        }

        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="pag">Pagination object.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        private ResultSet<AllActivity> GetAllActivities(string accessToken, string apiKey, string contactId, int? limit, DateTime? createdSince, Pagination pag)
        {
            ResultSet<AllActivity> results = null;
            string url = (pag == null) ? Config.ConstructUrl(Config.Endpoints.ContactTrackingBounces, new object[] { contactId }, new object[] { "limit", limit, "created_since", Extensions.ToISO8601String(createdSince) }) : pag.GetNextUrl();
            CUrlResponse response = RestClient.Get(url, accessToken, apiKey);

            if (response.IsError)
            {
                throw new CtctException(response.GetErrorMessage());
            }

            if (response.HasData)
            {
                results = Component.FromJSON<ResultSet<AllActivity>>(response.Body);
            }

            return results;
        }
    }
}
