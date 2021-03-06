﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIssueRestClient.cs" company="David Bevin">
//   Copyright (c) 2013 David Bevin.
// </copyright>
// // <summary>
//   https://bitbucket.org/dpbevin/jira-rest-client-dot-net
//   Licensed under the BSD 2-Clause License.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using JIRC.Domain;
using JIRC.Domain.Input;

using ServiceStack.ServiceClient.Web;

namespace JIRC
{
    /// <summary>
    /// An interface for handling Issues in JIRA.
    /// </summary>
    public interface IIssueRestClient
    {
        /// <summary>
        /// Creates an issue or a subtask.
        /// </summary>
        /// <param name="issue">Information about the issue to create. The fields that can be set on issue creation can be determined using the <see cref="GetCreateIssueMetadata()"/> or <see cref="GetCreateIssueMetadata(GetCreateIssueMetadataOptions)"/> methods.
        /// The <see cref="IssueInputBuilder"/> class can be used to help generate the appropriate fields.</param>
        /// <seealso cref="IssueInputBuilder"/>
        /// <returns>Details of the created issue.</returns>
        /// <exception cref="WebServiceException">The input is invalid (e.g. missing required fields, invalid field values, and so forth), or if the calling user does not have permission to create the issue.</exception>
        BasicIssue CreateIssue(IssueInput issue);

        /// <summary>
        /// Returns the meta data for creating issues.
        /// This includes the available projects, issue types and fields, including field types and whether or not those fields are required. Projects will not be returned if the user does not have permission to create issues in that project.</summary>
        /// <returns>A collection of fields for each project that corresponds with fields in the create screen for each project/issue type.</returns>
        /// <exception cref="WebServiceException">The caller is not logged in, or does not have permission to view any projects.</exception>
        IEnumerable<CimProject> GetCreateIssueMetadata();

        /// <summary>
        /// Returns the meta data for creating issues.
        /// This includes the available projects, issue types and fields, including field types and whether or not those fields are required. Projects will not be returned if the user does not have permission to create issues in that project.</summary>
        /// <param name="options">A set of options that allow the project/field/issue types to be constrained. The <see cref="GetCreateIssueMetadataOptionsBuilder"/> class can be used to help generate the appropriate request.</param>
        /// <seealso cref="GetCreateIssueMetadataOptionsBuilder"/>
        /// <returns>A collection of fields for each project that corresponds with fields in the create screen for each project/issue type.</returns>
        /// <exception cref="WebServiceException">The caller is not logged in, or does not have permission to view any of the requested projects.</exception>
        IEnumerable<CimProject> GetCreateIssueMetadata(GetCreateIssueMetadataOptions options);

        /// <summary>
        /// Creates many issue or subtasks in one bulk operation.
        /// </summary>
        /// <param name="issues">The collection of issues or subtasks to create.</param>
        /// <returns>Information about the created issue (including URIs), or error information.</returns>
        /// <exception cref="WebServiceException">The caller is not logged in, or does not have permission to create issues in the specified projects.</exception>
        BulkOperationResult<BasicIssue> CreateIssues(IList<IssueInput> issues);

        /// <summary>
        /// Gets the issue specified by the unique key (e.g. "AA-123").
        /// </summary>
        /// <param name="key">The unique key for the issue.</param>
        /// <returns>Information about the issue.</returns>
        /// <exception cref="WebServiceException">The requested issue is not found, or the user does not have permission to view it.</exception>
        Issue GetIssue(string key);

        /// <summary>
        /// Delete an issue.
        /// If the issue has subtasks you must set the parameter <see cref="deleteSubtasks"/> to delete the issue. You cannot delete an issue without its subtasks also being deleted.
        /// </summary>
        /// <param name="issueKey">The unique key of the issue to delete.</param>
        /// <param name="deleteSubtasks">Must be set to true if the issue has subtasks.</param>
        /// <exception cref="WebServiceException">The requested issue is not found, or the user does not have permission to delete it.</exception>
        void DeleteIssue(string issueKey, bool deleteSubtasks);

        /// <summary>
        /// Gets details about the users who are watching the specified issue.
        /// </summary>
        /// <param name="watchersUri">URI of the watchers resource for the selected issue. Usually obtained by getting the <see cref="BasicWatchers.Self"/> property on the <see cref="Issue"/>.</param>
        /// <returns>The list of watchers for the issue with the given URI.</returns>
        /// <exception cref="WebServiceException">The requested watcher URI is not found, or the user does not have permission to view it.</exception>
        Watchers GetWatchers(Uri watchersUri);

        /// <summary>
        /// Gets details about the users who have voted for the specified issue.
        /// </summary>
        /// <param name="votesUri">URI of the voters resource for the selected issue. Usually obtained by getting the <see cref="BasicVotes.Self"/> property on the <see cref="Issue"/>.</param>
        /// <returns>The list of voters for the issue with the given URI.</returns>
        /// <exception cref="WebServiceException">The requested voter URI is not found, or the user does not have permission to view it.</exception>
        Votes GetVotes(Uri votesUri);

        /// <summary>
        /// Get a list of the transitions possible for this issue by the current user, along with fields that are required and their types.
        /// </summary>
        /// <param name="transitionsUri">URI of transitions resource of selected issue. Usually obtained by getting the <see cref="Issue.TransitionsUri"/> property.</param>
        /// <returns>Transition information about the transitions available for the selected issue in its current state.</returns>
        /// <exception cref="WebServiceException">The requested transition URI is not found, or the user does not have permission to view it.</exception>
        IEnumerable<Transition> GetTransitions(Uri transitionsUri);

        /// <summary>
        /// Get a list of the transitions possible for this issue by the current user, along with fields that are required and their types.
        /// </summary>
        /// <param name="issue">The issue on which to obtain the available transitions for.</param>
        /// <returns>Transition information about the transitions available for the selected issue in its current state.</returns>
        /// <exception cref="WebServiceException">The requested issue is not found, or the user does not have permission to view it.</exception>
        IEnumerable<Transition> GetTransitions(Issue issue);

        /// <summary>
        /// Perform a transition on an issue. When performing the transition you can update or set other issue fields.
        /// </summary>
        /// <param name="transitionsUri">URI of transitions resource of selected issue. Usually obtained by getting the <see cref="Issue.TransitionsUri"/> property.</param>
        /// <param name="transitionInput">Information about the transition to perform.</param>
        /// <exception cref="WebServiceException">There is no transition specified, or the requested issue is not found, or the user does not have permission to view it.</exception>
        void Transition(Uri transitionsUri, TransitionInput transitionInput);

        /// <summary>
        /// Perform a transition on an issue. When performing the transition you can update or set other issue fields.
        /// </summary>
        /// <param name="issue">The issue on which to obtain the available transitions for.</param>
        /// <param name="transitionInput">Information about the transition to perform.</param>
        /// <exception cref="WebServiceException">There is no transition specified, or the requested issue is not found, or the user does not have permission to view it.</exception>
        void Transition(Issue issue, TransitionInput transitionInput);

        /// <summary>
        /// Retrieves a list of users who may be used as assignee when editing an issue. For a list of users when creating an issue, see <see cref="IProjectRestClient.GetAssignableUsers(string)"/>.
        /// </summary>
        /// <param name="issueKey">The unique key for the issue (e.g. "AA-123").</param>
        /// <returns>Returns a list of users who may be assigned to an issue during an edit.</returns>
        /// <exception cref="WebServiceException">The project is not found, or the calling user does not have permission to view it.</exception>
        IEnumerable<User> GetAssignableUsers(string issueKey);

        /// <summary>
        /// Retrieves a list of users who may be used as assignee when editing an issue. For a list of users when creating an issue, see <see cref="IProjectRestClient.GetAssignableUsers(string, int?, int?)"/>.
        /// </summary>
        /// <param name="issueKey">The unique key for the issue (e.g. "AA-123").</param>
        /// <param name="startAt">The index of the first user to return (0-based).</param>
        /// <param name="maxResults">The maximum number of users to return (defaults to 50). The maximum allowed value is 1000. If you specify a value that is higher than this number, your search results will be truncated.</param>
        /// <returns>Returns a list of users who may be assigned to an issue during an edit.</returns>
        IEnumerable<User> GetAssignableUsers(string issueKey, int? startAt, int? maxResults);

        /// <summary>
        /// Assigns an issue to a user.
        /// </summary>
        /// <param name="issue">The issue to make the assignment for.</param>
        /// <param name="user">The username of the person to assign the issue to.</param>
        void AssignTo(Issue issue, string user);

        /// <summary>
        /// Assigns an issue to the automatic assignee or removes the assignee entirely.
        /// </summary>
        /// <param name="issue">The issue to make the assignment for.</param>
        /// <param name="assignee">The special assignee type.</param>
        void AssignTo(Issue issue, SpecialAssignee assignee);

        /// <summary>
        /// Casts your vote on the selected issue. Casting a vote on already votes issue by the caller, causes the exception.
        /// </summary>
        /// <param name="votesUri">URI of the voters resource for the selected issue. Usually obtained by getting the <see cref="BasicVotes.Self"/> property on the <see cref="Issue"/>.</param>
        /// <exception cref="WebServiceException">If the user cannot vote for any reason. (The user is the reporter, the user does not have permission to vote, voting is disabled in the instance, the issue does not exist, etc.).</exception>
        void Vote(Uri votesUri);

        /// <summary>
        ///  Removes your vote from the selected issue. Removing a vote from the issue without your vote causes the exception.
        /// </summary>
        /// <param name="votesUri">URI of the voters resource for the selected issue. Usually obtained by getting the <see cref="BasicVotes.Self"/> property on the <see cref="Issue"/>.</param>
        /// <exception cref="WebServiceException">If the user cannot remove a vote for any reason. (The user did not vote on the issue, the user is the reporter, voting is disabled, the issue does not exist, etc.).</exception>
        void Unvote(Uri votesUri);

        /// <summary>
        /// Starts watching selected issue.
        /// </summary>
        /// <param name="watchersUri">URI of the watchers resource for the selected issue. Usually obtained by getting the <see cref="BasicWatchers.Self"/> property on the <see cref="Issue"/>.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to watch the issue.</exception>
        void Watch(Uri watchersUri);

        /// <summary>
        /// Stops watching selected issue.
        /// </summary>
        /// <param name="watchersUri">URI of the watchers resource for the selected issue. Usually obtained by getting the <see cref="BasicWatchers.Self"/> property on the <see cref="Issue"/>.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to watch the issue.</exception>
        void Unwatch(Uri watchersUri);

        /// <summary>
        /// Adds selected person as a watcher for selected issue.
        /// </summary>
        /// <param name="watchersUri">URI of the watchers resource for the selected issue. Usually obtained by getting the <see cref="BasicWatchers.Self"/> property on the <see cref="Issue"/>.</param>
        /// <param name="username">The user to add as a watcher.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to modifier watchers of the issue.</exception>
        void AddWatcher(Uri watchersUri, string username);

        /// <summary>
        /// Removes selected person as a watcher for selected issue.
        /// </summary>
        /// <param name="watchersUri">URI of the watchers resource for the selected issue. Usually obtained by getting the <see cref="BasicWatchers.Self"/> property on the <see cref="Issue"/>.</param>
        /// <param name="username">The user to remove as a watcher.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to modifier watchers of the issue.</exception>
        void RemoveWatcher(Uri watchersUri, string username);

        /// <summary>
        /// Creates link between two issues and adds a comment (optional) to the source issues.
        /// </summary>
        /// <param name="linkIssuesInput">Details for the link and the comment (optional) to be created.</param>
        /// <exception cref="WebServiceException">If there was a problem linking the issues, or the the calling user does not have permission to link issues.</exception>
        void LinkIssue(LinkIssuesInput linkIssuesInput);

        /// <summary>
        /// Adds an attachment to an issue.
        /// </summary>
        /// <param name="attachmentsUri">The URI of the attachment resource for a given issue.</param>
        /// <param name="filename">The name of the file to attach.</param>
        void AddAttachment(Uri attachmentsUri, string filename);

        /// <summary>
        /// Adds a comment to the specified issue.
        /// </summary>
        /// <param name="issue">The  issue to add the comment to.</param>
        /// <param name="comment">The comment to add to the issue.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to comment on the issue.</exception>
        void AddComment(BasicIssue issue, Comment comment);

        /// <summary>
        /// Adds a comment to the specified issue.
        /// </summary>
        /// <param name="commentsUri">The URI for the issue to add the comment to.</param>
        /// <param name="comment">The comment to add to the issue.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to comment on the issue.</exception>
        void AddComment(Uri commentsUri, Comment comment);

        /// <summary>
        /// Adds a new work log entry to issue.
        /// </summary>
        /// <param name="worklogUri">The URI for the work log resource for the selected issue.</param>
        /// <param name="worklogInput">The work log information to add to the issue.</param>
        /// <exception cref="WebServiceException">If the issue does not exist, or the the calling user does not have permission to add work log information to the issue.</exception>
        void AddWorklog(Uri worklogUri, WorklogInput worklogInput);

        /// <summary>
        /// Adds a label to an issue.
        /// </summary>
        /// <param name="issue">The issue to add the label to.</param>
        /// <param name="label">The label to add.</param>
        void AddLabel(Issue issue, string label);

        /// <summary>
        /// Removes a label from an issue.
        /// </summary>
        /// <param name="issue">The issue to remove the label from.</param>
        /// <param name="label">The label to remove.</param>
        void RemoveLabel(Issue issue, string label);

        /// <summary>
        /// Edits a standard field on an issue.
        /// </summary>
        /// <param name="issue">The issue to edit the field on.</param>
        /// <param name="id">The id of the standard field to edit.</param>
        /// <param name="value">The new value of the field.</param>
        void EditField(Issue issue, IssueFieldId id, object value);

        /// <summary>
        /// Edits a custom field on an issue.
        /// </summary>
        /// <param name="issue">The issue to edit the field on.</param>
        /// <param name="name">The name of the custom field to edit.</param>
        /// <param name="value">The new value of the field.</param>
        void EditField(Issue issue, string name, object value);
    }
}
