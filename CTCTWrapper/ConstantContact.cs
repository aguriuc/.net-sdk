﻿#region Using

using System;
using System.Collections.Generic;
using CTCT.Components.Contacts;
using CTCT.Exceptions;
using CTCT.Services;
using CTCT.Util;
using CTCT.Components.Activities;
using CTCT.Components.EmailCampaigns;
using CTCT.Components;
using CTCT.Components.Tracking;
using System.Configuration;
using System.IO;

#endregion

namespace CTCT
{
    /// <summary>
    /// Main class meant to be used by users to access Constant Contact API functionality.
    /// <example>
    /// ASPX page:
    /// <code>
    /// <![CDATA[
    ///  <%@Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    ///         CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default"%>
    ///  ...
    ///     <asp:TextBox ID="tbxEmail" runat="server"></asp:TextBox>
    ///     <asp:Button ID="btnJoin" runat="server" Text="Join" onclick="btnJoin_Click" />
    ///     <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    ///  ...
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Code behind:
    /// <code>
    /// <![CDATA[
    /// public partial class _Default : System.Web.UI.Page
    /// {
    ///    protected void Page_Load(object sender, EventArgs e)
    ///    {
    ///         ...
    ///    }
    ///
    ///    protected void btnJoin_Click(object sender, EventArgs e)
    ///    {
    ///        try
    ///        {
    ///            Contact contact = new Contact();
    ///            // Don't care about the id value
    ///            contact.Id = 1;
    ///            contact.EmailAddresses.Add(new EmailAddress() {
    ///                 EmailAddr = tbxEmail.Text,
    ///                 ConfirmStatus = ConfirmStatus.NoConfirmationRequired,
    ///                 Status = Status.Active });
    ///            contact.Lists.Add(new ContactList() {
    ///                 Id = 1,
    ///                 Status = Status.Active });
    ///
    ///            ConstantContact cc = new ConstantContact();
    ///            cc.AddContact(contact);
    ///            lblMessage.Text = "You have been added to my mailing list!";
    ///        }
    ///        catch (Exception ex) { lblMessage.Text = ex.ToString(); }
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Web.config entries:
    /// <code>
    /// <![CDATA[
    /// ...
    /// <appSettings>
    ///     <add key="APIKey" value="APIKey"/>
    ///     <add key="Password" value="password"/>
    ///     <add key="Username" value="username"/>
    ///     <add key="RedirectURL" value="http://somedomain"/>
    /// </appSettings>
    /// ...
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public class ConstantContact
    {
        #region Fields

        /// <summary>
        /// Access token.
        /// </summary>
        private string _accessToken;

        /// <summary>
        /// api_key field
        /// </summary>
        private string _apiKey;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Contact service.
        /// </summary>
        protected virtual IContactService ContactService { get; set; }

        /// <summary>
        /// Gets or sets the List service.
        /// </summary>
        protected virtual IListService ListService { get; set; }

        /// <summary>
        /// Gets or sets the Activity service.
        /// </summary>
        protected virtual IActivityService ActivityService { get; set; }

        /// <summary>
        /// Gets or sets the Campaign Schedule service.
        /// </summary>
        protected virtual ICampaignScheduleService CampaignScheduleService { get; set; }

        /// <summary>
        /// Gets or sets the Campaign Tracking service.
        /// </summary>
        protected virtual ICampaignTrackingService CampaignTrackingService { get; set; }

        /// <summary>
        /// Gets or sets the Contact Tracking service.
        /// </summary>
        protected virtual IContactTrackingService ContactTrackingService { get; set; }

        /// <summary>
        /// Gets or sets the Email Campaign service.
        /// </summary>
        protected virtual IEmailCampaignService EmailCampaignService { get; set; }

        /// <summary>
        /// Gets or sets the Account service
        /// </summary>
        protected virtual IAccountService AccountService { get; set; }

        /// <summary>
        /// Gets or sets the AccessToken
        /// </summary>
        private string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        /// <summary>
        /// Gets or sets the api_key
        /// </summary>
        private string APIKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ConstantContact object using provided apiKey and accessToken parameters
        /// </summary>
        /// <param name="apiKey">APIKey</param>
        /// <param name="accessToken">access token</param>
        public ConstantContact(string apiKey, string accessToken)
        {
            InitializeFields();

            AccessToken = accessToken;
            APIKey = apiKey;
        }

        #endregion

        #region Public methods

        #region Private methods

        private void InitializeFields()
        {
            ContactService = new ContactService();
            ListService = new ListService();
            ActivityService = new ActivityService();
            CampaignScheduleService = new CampaignScheduleService();
            CampaignTrackingService = new CampaignTrackingService();
            ContactTrackingService = new ContactTrackingService();
            EmailCampaignService = new EmailCampaignService();
            AccountService = new AccountService();
        }

        #endregion Private methods

        #region Contact service

        /// <summary>
        /// Get a set of contacts.
        /// </summary>
        /// <param name="email">Match the exact email address.</param>
        /// <param name="limit">Limit the number of returned values, default 500.</param>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns a list of contacts.</returns>
        public ResultSet<Contact> GetContacts(string email, int? limit, DateTime? modifiedSince)
        {
            return ContactService.GetContacts(AccessToken, APIKey, email, limit, modifiedSince);
        }

        /// <summary>
        /// Get a set of contacts.
        /// </summary>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns a list of contacts.</returns>
        public ResultSet<Contact> GetContacts(DateTime? modifiedSince)
        {
            return ContactService.GetContacts(AccessToken, APIKey, null, null, modifiedSince);
        }

        /// <summary>
        /// Get an array of contacts.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>Returns a list of contacts.</returns>
        public ResultSet<Contact> GetContacts(Pagination pag)
        {
            return ContactService.GetContacts(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get an individual contact.
        /// </summary>
        /// <param name="contactId">Id of the contact to retrieve</param>
        /// <returns>Returns a contact.</returns>
        public Contact GetContact(string contactId)
        {
            return ContactService.GetContact(AccessToken, APIKey, contactId);
        }

        /// <summary>
        /// Add a new contact to an account.
        /// </summary>
        /// <param name="contact">Contact to add.</param>
        /// <param name="actionByVisitor">Set to true if action by visitor.</param>
        /// <returns>Returns the newly created contact.</returns>
        public Contact AddContact(Contact contact, bool actionByVisitor)
        {
            return ContactService.AddContact(AccessToken, APIKey, contact, actionByVisitor);
        }

        /// <summary>
        /// Sets an individual contact to 'REMOVED' status.
        /// </summary>
        /// <param name="contact">Contact object.</param>
        /// <returns>Returns true if operation succeeded.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteContact(Contact contact)
        {
            if (contact == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }
            return DeleteContact(contact.Id);
        }

        /// <summary>
        /// Sets an individual contact to 'REMOVED' status.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Returns true if operation succeeded.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteContact(string contactId)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }

            return ContactService.DeleteContact(AccessToken, APIKey, contactId);
        }

        /// <summary>
        /// Delete a contact from all contact lists.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Returns true if operation succeeded.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteContactFromLists(string contactId)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }
            return ContactService.DeleteContactFromLists(AccessToken, APIKey, contactId);
        }

        /// <summary>
        /// Delete a contact from all contact lists.
        /// </summary>
        /// <param name="contact">Contact object.</param>
        /// <returns>Returns true if operation succeeded.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteContactFromLists(Contact contact)
        {
            if (contact == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }

            return ContactService.DeleteContactFromLists(AccessToken, APIKey, contact.Id);
        }


        /// <summary>
        /// Delete a contact from all contact lists.
        /// </summary>
        /// <param name="contact">Contact object.</param>
        /// <param name="list">List object.</param>
        /// <returns>Returns true if operation succeeded.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteContactFromList(Contact contact, ContactList list)
        {
            if (contact == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }
            if (list == null)
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ContactService.DeleteContactFromList(AccessToken, APIKey, contact.Id, list.Id);
        }

        /// <summary>
        /// Delete a contact from all contact lists.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="listId">List id.</param>
        /// <returns>Returns true if operation succeeded.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteContactFromList(string contactId, string listId)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }
            if (listId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ContactService.DeleteContactFromList(AccessToken, APIKey, contactId, listId);
        }

        /// <summary>
        /// Update an individual contact.
        /// </summary>
        /// <param name="contact">Contact to update.</param>
        /// <param name="actionByVisitor">Set to true if action by visitor.</param>
        /// <returns>Returns the updated contact.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Contact UpdateContact(Contact contact, bool actionByVisitor)
        {
            if (contact == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactOrId);
            }

            return ContactService.UpdateContact(AccessToken, APIKey, contact, actionByVisitor);
        }

        #endregion

        #region List service

        /// <summary>
        /// Get lists.
        /// </summary>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns the list of lists where contact belong to.</returns>
        public IList<ContactList> GetLists(DateTime? modifiedSince)
        {
            return ListService.GetLists(AccessToken, APIKey, modifiedSince);
        }

        /// <summary>
        /// Get an individual list.
        /// </summary>
        /// <param name="listId">Id of the list to retrieve</param>
        /// <returns>Returns contact list.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ContactList GetList(string listId)
        {
            if (listId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ListService.GetList(AccessToken, APIKey, listId);
        }

        /// <summary>
        /// Update a Contact List.
        /// </summary>
        /// <param name="list">ContactList to be updated</param>
        /// <returns>Contact list</returns>
        public ContactList UpdateList(ContactList list)
        {
            if (list == null)
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ListService.UpdateList(AccessToken, APIKey, list);
        }

        /// <summary>
        /// Add a new list to an account.
        /// </summary>
        /// <param name="list">List to add.</param>
        /// <returns>Returns the newly created list.</returns>
        public ContactList AddList(ContactList list)
        {
            return ListService.AddList(AccessToken, APIKey, list);
        }

        /// <summary>
        /// Delete a Contact List.
        /// </summary>
        /// <param name="listId">List id.</param>
        /// <returns>return true if list was deleted successfully, false otherwise</returns>
        public bool DeleteList(string listId)
        {
            if (string.IsNullOrEmpty(listId))
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ListService.DeleteList(AccessToken, APIKey, listId);
        }

        /// <summary>
        /// Get contact that belong to a specific list.
        /// </summary>
        /// <param name="list">Contact list object.</param>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns the list of contacts.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<Contact> GetContactsFromList(ContactList list, DateTime? modifiedSince)
        {
            return GetContactsFromList(list, modifiedSince);
        }

        /// <summary>
        /// Get contact that belong to a specific list.
        /// </summary>
        /// <param name="list">Contact list object.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns the list of contacts.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<Contact> GetContactsFromList(ContactList list, int? limit, DateTime? modifiedSince)
        {
            if (list == null)
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ListService.GetContactsFromList(AccessToken, APIKey, list.Id, limit, modifiedSince);
        }

        /// <summary>
        /// Get contact that belong to a specific list.
        /// </summary>
        /// <param name="listId">Contact list id.</param>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns a list of contacts.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<Contact> GetContactsFromList(string listId, DateTime? modifiedSince)
        {
            return GetContactsFromList(listId, null, modifiedSince);
        }

        /// <summary>
        /// Get contact that belong to a specific list.
        /// </summary>
        /// <param name="listId">Contact list id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="modifiedSince">limit contacts retrieved to contacts modified since the supplied date</param>
        /// <returns>Returns a list of contacts.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<Contact> GetContactsFromList(string listId, int? limit, DateTime? modifiedSince)
        {
            if (listId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ListOrId);
            }

            return ListService.GetContactsFromList(AccessToken, APIKey, listId, limit, modifiedSince);
        }

        /// <summary>
        /// Get contacts after pagination object received.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>Returns a list of contacts.</returns>
        public ResultSet<Contact> GetContactsFromList(Pagination pag)
        {
            return ListService.GetContactsFromList(AccessToken, APIKey, pag);
        }

        #endregion

        #region Activity service

        /// <summary>
        /// Adds or updates contacts to one or more contact lists.
        /// </summary>
        /// <param name="addContacts">AddContacts object.</param>
        /// <returns>Returns an Activity object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity AddContacts(AddContacts addContacts)
        {
            if (addContacts == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }

            return ActivityService.AddContacts(AccessToken, APIKey, addContacts);
        }

        /// <summary>
        /// Removes contacts from one ore more contact lists.
        /// </summary>
        /// <param name="emailAddresses">List of email addresses.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity RemoveContactsFromLists(IList<string> emailAddresses, IList<string> lists)
        {
            if (emailAddresses == null || lists == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }

            return ActivityService.RemoveContactsFromLists(AccessToken, APIKey, emailAddresses, lists);
        }

        /// <summary>
        /// Clears all contacts from one or more contact lists.
        /// </summary>
        /// <param name="lists">Array of list id's to be cleared.</param>
        /// <returns>Returns an Activity object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity ClearLists(IList<string> lists)
        {
            if (lists == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }

            return ActivityService.ClearLists(AccessToken, APIKey, lists);
        }

        /// <summary>
        /// Exports contacts from the specified contact list to a CSV file.
        /// </summary>
        /// <param name="exportContacts">Export contacts object.</param>
        /// <returns>Returns an Activity object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity ExportContacts(ExportContacts exportContacts)
        {
            if (exportContacts == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }

            return ActivityService.ExportContacts(AccessToken, APIKey, exportContacts);
        }

        /// <summary>
        /// Returns a list with the last 50 bulk activities.
        /// </summary>
        /// <returns>Returns the list of activities.</returns>
        public IList<Activity> GetLast50BulkActivities()
        {
            return ActivityService.GetLast50BulkActivities(AccessToken, APIKey);
        }

        /// <summary>
        /// Get an activity.
        /// </summary>
        /// <param name="activityId">The activity identification.</param>
        /// <param name="status">The activity status.</param>
        /// <param name="type">The activity type.</param>
        /// <returns>Returns the activity identified by its id.</returns>
        public IList<Activity> GetDetailedReport(string activityId, ActivityStatus? status, ActivityType? type)
        {
            return ActivityService.GetDetailedReport(AccessToken, APIKey, activityId, status, type);
        }

        /// <summary>
        /// Adds or updates contacts to one or more contact lists.
        /// </summary>
        /// <remarks>
        /// The currently supported file types are: text, csv, xls, xlsx.
        /// 
        /// The import file is constructed of columns containing contact properties, with rows for each contact. It needs to contain at least the email address for each contact.
        /// The following is a list of the contact properties (column headings in an .xls spreadsheet) that can be included in the import file (these are not case sensitive):
        ///     EMAIL (also E-MAIL, EMAIL ADDRESS, E-MAIL ADDRESS)
        ///     FIRST NAME
        ///     MIDDLE NAME
        ///     LAST NAME
        ///     JOB TITLE
        ///     COMPANY NAME
        ///     WORK PHONE
        ///     HOME PHONE
        ///     ADDRESS LINE 1 (to 3)
        ///     CITY
        ///     STATE
        ///     US STATE/CA PROVINCE
        ///     COUNTRY
        ///     ZIP/POSTAL CODE
        ///     SUB ZIP/POSTAL CODE
        ///     CUSTOM FIELD 1 (to 15)
        /// The import file should contain in the first row the name of the columns which are about to be imported, this is not neccessary when only the email addresses are contained by the file.
        /// </remarks>
        /// <param name="filename">Multipart file name.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity AddContactsMultipart(string filename, IList<string> lists)
        {
            if (lists == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }
            if (String.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                throw new IllegalArgumentException(Config.Errors.FileMultipart);
            }

            return ActivityService.AddContactsMultipart(AccessToken, APIKey, filename, lists);
        }

        /// <summary>
        /// Removes contacts from one ore more contact lists.
        /// </summary>
        /// <remarks>
        /// The currently supported file types are: text, csv, xls, xlsx.
        /// 
        /// The import file is constructed of columns containing contact properties, with rows for each contact. It needs to contain at least the email address for each contact.
        /// The following is a list of the contact properties (column headings in an .xls spreadsheet) that can be included in the import file (these are not case sensitive):
        ///     EMAIL (also E-MAIL, EMAIL ADDRESS, E-MAIL ADDRESS)
        ///     FIRST NAME
        ///     MIDDLE NAME
        ///     LAST NAME
        ///     JOB TITLE
        ///     COMPANY NAME
        ///     WORK PHONE
        ///     HOME PHONE
        ///     ADDRESS LINE 1 (to 3)
        ///     CITY
        ///     STATE
        ///     US STATE/CA PROVINCE
        ///     COUNTRY
        ///     ZIP/POSTAL CODE
        ///     SUB ZIP/POSTAL CODE
        ///     CUSTOM FIELD 1 (to 15)
        /// The import file should contain in the first row the name of the columns which are about to be imported, this is not neccessary when only the email addresses are contained by the file.
        /// </remarks>
        /// <param name="filename">Multipart file name.</param>
        /// <param name="lists">List of id's.</param>
        /// <returns>Returns an Activity object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity RemoveContactsMultipart(string filename, IList<string> lists)
        {
            if (lists == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }
            if (String.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                throw new IllegalArgumentException(Config.Errors.FileMultipart);
            }

            return ActivityService.RemoveContactsMultipart(AccessToken, APIKey, filename, lists);
        }

        /// <summary>
        /// Returns the status of the specified activity.
        /// </summary>
        /// <param name="activityId">The activity id.</param>
        /// <returns>Returns the activity identified by its id.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Activity GetActivity(string activityId)
        {
            if (activityId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ActivityOrId);
            }

            return ActivityService.GetActivity(AccessToken, APIKey, activityId);
        }

        #endregion

        #region CampaignSchedule service

        /// <summary>
        /// Create a new schedule for a campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id to be scheduled.</param>
        /// <param name="schedule">Schedule to be created.</param>
        /// <returns>Returns the scheduled object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Schedule AddSchedule(string campaignId, Schedule schedule)
        {
            if (campaignId == null || schedule == null)
            {
                throw new IllegalArgumentException(Config.Errors.ScheduleOrId);
            }

            return CampaignScheduleService.AddSchedule(AccessToken, APIKey, campaignId, schedule);
        }

        /// <summary>
        /// Get a list of schedules for a campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id to be scheduled.</param>
        /// <returns>Returns the list of schedules for specified campaign.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public IList<Schedule> GetSchedules(string campaignId)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ScheduleOrId);
            }

            return CampaignScheduleService.GetSchedules(AccessToken, APIKey, campaignId);
        }

        /// <summary>
        /// Get a specific schedule for a campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id to be get a schedule for.</param>
        /// <param name="scheduleId">Schedule id to retrieve.</param>
        /// <returns>Returns the schedule object for the requested campaign.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Schedule GetSchedule(string campaignId, string scheduleId)
        {
            if (campaignId == null || scheduleId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ScheduleOrId);
            }

            return CampaignScheduleService.GetSchedule(AccessToken, APIKey, campaignId, scheduleId);
        }

        /// <summary>
        /// Update a specific schedule for a campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id to be scheduled.</param>
        /// <param name="schedule">Schedule to retrieve.</param>
        /// <returns>Returns the updated schedule object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public Schedule UpdateSchedule(string campaignId, Schedule schedule)
        {
            if (campaignId == null || schedule == null)
            {
                throw new IllegalArgumentException(Config.Errors.ScheduleOrId);
            }

            return CampaignScheduleService.AddSchedule(AccessToken, APIKey, campaignId, schedule);
        }

        /// <summary>
        /// Get a specific schedule for a campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="scheduleId">Schedule id to delete.</param>
        /// <returns>Returns true if schedule was successfully deleted.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteSchedule(string campaignId, string scheduleId)
        {
            if (campaignId == null || scheduleId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ScheduleOrId);
            }

            return CampaignScheduleService.DeleteSchedule(AccessToken, APIKey, campaignId, scheduleId);
        }

        /// <summary>
        /// Send a test send of a campaign.
        /// </summary>
        /// <param name="campaignId">Id of campaign to send test of.</param>
        /// <param name="testSend">Test send details.</param>
        /// <returns>Returns the sent object.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public TestSend SendTest(string campaignId, TestSend testSend)
        {
            if (campaignId == null || testSend == null)
            {
                throw new IllegalArgumentException(Config.Errors.ScheduleOrId);
            }

            return CampaignScheduleService.SendTest(AccessToken, APIKey, campaignId, testSend);
        }

        #endregion

        #region CampaignTracking service

        /// <summary>
        /// Get a result set of bounces for a given campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity.</returns>
        public ResultSet<BounceActivity> GetCampaignTrackingBounces(string campaignId, int? limit)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetBounces(AccessToken, APIKey, campaignId, limit);
        }

        /// <summary>
        /// Get a result set of bounces for a given campaign.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity.</returns>
        public ResultSet<BounceActivity> GetCampaignTrackingBounces(Pagination pag)
        {
            return CampaignTrackingService.GetBounces(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get clicks for a given campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="linkId">Specifies the link in the email campaign to retrieve click data for.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity.</returns>
        public ResultSet<ClickActivity> GetCampaignTrackingClicks(string campaignId, string linkId, int? limit,
                                                                  DateTime? createdSince)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetClicks(AccessToken, APIKey, campaignId, linkId, limit, createdSince);
        }

        /// <summary>
        /// Get clicks for a given campaign.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity.</returns>
        public ResultSet<ClickActivity> GetClicks(Pagination pag)
        {
            return CampaignTrackingService.GetClicks(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get forwards for a given campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        public ResultSet<ForwardActivity> GetCampaignTrackingForwards(string campaignId, int? limit,
                                                                      DateTime? createdSince)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetForwards(AccessToken, APIKey, campaignId, limit, createdSince);
        }

        /// <summary>
        /// Get forwards for a given campaign.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        public ResultSet<ForwardActivity> GetCampaignTrackingForwards(Pagination pag)
        {
            return CampaignTrackingService.GetForwards(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get opens for a given campaign.
        /// </summary>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        public ResultSet<OpenActivity> GetCampaignTrackingOpens(string campaignId, int? limit, DateTime? createdSince)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetOpens(AccessToken, APIKey, campaignId, limit, createdSince);
        }

        /// <summary>
        /// Get opens for a given campaign.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        public ResultSet<OpenActivity> GetCampaignTrackingOpens(Pagination pag)
        {
            return CampaignTrackingService.GetOpens(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get sends for a given campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link SendActivity</returns>
        public ResultSet<SendActivity> GetCampaignTrackingSends(string campaignId, int? limit, DateTime? createdSince)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetSends(AccessToken, APIKey, campaignId, limit, createdSince);
        }

        /// <summary>
        /// Get sends for a given campaign.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link SendActivity</returns>
        public ResultSet<SendActivity> GetCampaignTrackingSends(Pagination pag)
        {
            return CampaignTrackingService.GetSends(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get opt outs for a given campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        public ResultSet<UnsubscribeActivity> GetCampaignTrackingOptOuts(string campaignId, int? limit,
                                                                    DateTime? createdSince)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetOptOuts(AccessToken, APIKey, campaignId, limit, createdSince);
        }

        /// <summary>
        /// Get opt outs for a given campaign.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        public ResultSet<UnsubscribeActivity> GetCampaignTrackingOptOuts(Pagination pag)
        {
            return CampaignTrackingService.GetOptOuts(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get a summary of reporting data for a given campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <returns>Tracking summary.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public TrackingSummary GetCampaignTrackingSummary(string campaignId)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.CampaignTrackingOrId);
            }

            return CampaignTrackingService.GetSummary(AccessToken, APIKey, campaignId);
        }

        #endregion

        #region ContractTracking service

        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<BounceActivity> GetContactTrackingBounces(string contactId)
        {
            return GetContactTrackingBounces(contactId, null);
        }

        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<BounceActivity> GetContactTrackingBounces(string contactId, int? limit)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetBounces(AccessToken, APIKey, contactId, limit);
        }

        /// <summary>
        /// Get bounces for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link BounceActivity</returns>
        public ResultSet<BounceActivity> GetContactTrackingBounces(Pagination pag)
        {
            return ContactTrackingService.GetBounces(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get clicks for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<ClickActivity> GetContactTrackingClicks(string contactId, DateTime? createdSince)
        {
            return GetContactTrackingClicks(contactId, null, createdSince);
        }

        /// <summary>
        /// Get clicks for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<ClickActivity> GetContactTrackingClicks(string contactId, int? limit, DateTime? createdSince)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetClicks(AccessToken, APIKey, contactId, limit, createdSince);
        }

        /// <summary>
        /// Get clicks for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ClickActivity</returns>
        public ResultSet<ClickActivity> GetContactTrackingClicks(Pagination pag)
        {
            return ContactTrackingService.GetClicks(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get forwards for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<ForwardActivity> GetContactTrackingForwards(string contactId, DateTime? createdSince)
        {
            return GetContactTrackingForwards(contactId, null, createdSince);
        }

        /// <summary>
        /// Get forwards for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<ForwardActivity> GetContactTrackingForwards(string contactId, int? limit,
                                                                     DateTime? createdSince)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetForwards(AccessToken, APIKey, contactId, limit, createdSince);
        }

        /// <summary>
        /// Get forwards for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link ForwardActivity.</returns>
        public ResultSet<ForwardActivity> GetContactTrackingForwards(Pagination pag)
        {
            return ContactTrackingService.GetForwards(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get opens for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<OpenActivity> GetContactTrackingOpens(string contactId, DateTime? createdSince)
        {
            return GetContactTrackingOpens(contactId, null, createdSince);
        }

        /// <summary>
        /// Get opens for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<OpenActivity> GetContactTrackingOpens(string contactId, int? limit, DateTime? createdSince)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetOpens(AccessToken, APIKey, contactId, limit, createdSince);
        }

        /// <summary>
        /// Get opens for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link OpenActivity.</returns>
        public ResultSet<OpenActivity> GetContactTrackingOpens(Pagination pag)
        {
            return ContactTrackingService.GetOpens(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get sends for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link SendActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<SendActivity> GetContactTrackingSends(string contactId, DateTime? createdSince)
        {
            return GetContactTrackingSends(contactId, null, createdSince);
        }

        /// <summary>
        /// Get sends for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link SendActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<SendActivity> GetContactTrackingSends(string contactId, int? limit, DateTime? createdSince)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetSends(AccessToken, APIKey, contactId, limit, createdSince);
        }

        /// <summary>
        /// Get sends for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link SendActivity.</returns>
        public ResultSet<SendActivity> GetContactTrackingSends(Pagination pag)
        {
            return ContactTrackingService.GetSends(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get opt outs for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<UnsubscribeActivity> GetContactTrackingUnsubscribes(string contactId, DateTime? createdSince)
        {
            return GetContactTrackingUnsubscribes(contactId, null, createdSince);
        }

        /// <summary>
        /// Get opt outs for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<UnsubscribeActivity> GetContactTrackingUnsubscribes(string contactId, int? limit, DateTime? createdSince)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetUnsubscribes(AccessToken, APIKey, contactId, limit, createdSince);
        }

        /// <summary>
        /// Get opt outs for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link UnsubscribeActivity.</returns>
        public ResultSet<UnsubscribeActivity> GetContactTrackingUnsubscribes(Pagination pag)
        {
            return ContactTrackingService.GetUnsubscribes(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get all activities for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link AllActivity</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<AllActivity> GetContactTrackingAll(string contactId, DateTime? createdSince)
        {
            return GetContactTrackingAll(contactId, null, createdSince);
        }

        /// <summary>
        /// Get all activities for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="createdSince">filter for activities created since the supplied date in the collection</param>
        /// <returns>ResultSet containing a results array of @link AllActivity</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public ResultSet<AllActivity> GetContactTrackingAll(string contactId, int? limit, DateTime? createdSince)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetAllActivities(AccessToken, APIKey, contactId, limit, createdSince);
        }

        /// <summary>
        /// Get all activities for a given contact.
        /// </summary>
        /// <param name="pag">Pagination object.</param>
        /// <returns>ResultSet containing a results array of @link AllActivity</returns>
        public ResultSet<AllActivity> GetContactTrackingAll(Pagination pag)
        {
            return ContactTrackingService.GetAllActivities(AccessToken, APIKey, pag);
        }

        /// <summary>
        /// Get a summary of reporting data for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Tracking summary.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public TrackingSummary GetContactTrackingSummary(string contactId)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetSummary(AccessToken, APIKey, contactId);
        }

        /// <summary>
        /// Get a summary of reporting data for a given contact.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Tracking summary.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public TrackingSummaryByEmailCampaign GetContactTrackingSummaryEmailCampaign(string contactId)
        {
            if (contactId == null)
            {
                throw new IllegalArgumentException(Config.Errors.ContactTrackingOrId);
            }

            return ContactTrackingService.GetSummaryByEmailCampaign(AccessToken, APIKey, contactId);
        }

        #endregion

        #region EmailCampaign service

        /// <summary>
        /// Get a set of campaigns.
        /// </summary>
        /// <param name="modifiedSince">limit campaigns to campaigns modified since the supplied date</param>
        /// <returns>Returns a list of campaigns.</returns>
        public IList<EmailCampaign> GetCampaigns(DateTime? modifiedSince)
        {
            return GetCampaigns(null, null, modifiedSince);
        }

        /// <summary>
        /// Get a set of campaigns.
        /// </summary>
        /// <param name="status">Returns list of email campaigns with specified status.</param>
        /// <param name="modifiedSince">limit campaigns to campaigns modified since the supplied date</param>
        /// <returns>Returns a list of campaigns.</returns>
        public IList<EmailCampaign> GetCampaigns(CampaignStatus status, DateTime? modifiedSince)
        {
            return GetCampaigns(status, null, modifiedSince);
        }

        /// <summary>
        /// Get a set of campaigns.
        /// </summary>
        /// <param name="status">Returns list of email campaigns with specified status.</param>
        /// <param name="limit">Specifies the number of results per page in the output, from 1 - 500, default = 500.</param>
        /// <param name="modifiedSince">limit campaigns to campaigns modified since the supplied date</param>
        /// <returns>Returns a list of campaigns.</returns>
        public IList<EmailCampaign> GetCampaigns(CampaignStatus? status, int? limit, DateTime? modifiedSince)
        {
            return EmailCampaignService.GetCampaigns(AccessToken, APIKey, status, limit, modifiedSince);
        }

        /// <summary>
        /// Get campaign details for a specific campaign.
        /// </summary>
        /// <param name="campaignId">Campaign id.</param>
        /// <returns>Returns a campaign.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public EmailCampaign GetCampaign(string campaignId)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.EmailCampaignOrId);
            }

            return EmailCampaignService.GetCampaign(AccessToken, APIKey, campaignId);
        }

        /// <summary>
        /// Create a new campaign.
        /// </summary>
        /// <param name="campaign">Campign to be created</param>
        /// <returns>Returns a campaign.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public EmailCampaign AddCampaign(EmailCampaign campaign)
        {
            if (campaign == null)
            {
                throw new IllegalArgumentException(Config.Errors.EmailCampaignOrId);
            }

            return EmailCampaignService.AddCampaign(AccessToken, APIKey, campaign);
        }

        /// <summary>
        /// Delete an email campaign.
        /// </summary>
        /// <param name="campaignId">Valid campaign id.</param>
        /// <returns>Returns true if successful.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public bool DeleteCampaign(string campaignId)
        {
            if (campaignId == null)
            {
                throw new IllegalArgumentException(Config.Errors.EmailCampaignOrId);
            }

            return EmailCampaignService.DeleteCampaign(AccessToken, APIKey, campaignId);
        }

        /// <summary>
        /// Update a specific email campaign.
        /// </summary>
        /// <param name="campaign">Campaign to be updated.</param>
        /// <returns>Returns a campaign.</returns>
        /// <exception cref="IllegalArgumentException">IllegalArgumentException</exception>
        public EmailCampaign UpdateCampaign(EmailCampaign campaign)
        {
            if (campaign == null)
            {
                throw new IllegalArgumentException(Config.Errors.EmailCampaignOrId);
            }

            return EmailCampaignService.UpdateCampaign(AccessToken, APIKey, campaign);
        }

        #endregion

        #region Account service

        /// <summary>
        /// Retrieve a list of all the account owner's email addresses.
        /// </summary>
        /// <returns>list of all verified account owner's email addresses.</returns>
        public IList<VerifiedEmailAddress> GetVerifiedEmailAddress()
        {
            return AccountService.GetVerifiedEmailAddress(AccessToken, APIKey);
        }

        #endregion Account service

        #endregion
    }
}
