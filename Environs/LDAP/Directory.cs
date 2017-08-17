/*
Copyright 2017 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;

namespace Environs.LDAP
{
    /// <summary>
    /// LDAP Directory helper
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public class Directory : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Directory"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="path">The path.</param>
        public Directory(string userName, string password, string path)
        {
            Entry = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
            Searcher = new DirectorySearcher(Entry)
            {
                PageSize = 1000
            };
        }

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated => !string.IsNullOrEmpty(Entry.Guid.ToString().Trim());

        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        private DirectoryEntry Entry { get; set; }

        /// <summary>
        /// Gets or sets the searcher.
        /// </summary>
        /// <value>The searcher.</value>
        private DirectorySearcher Searcher { get; set; }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Entry.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finds the active group members.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindActiveGroupMembers(string groupName, bool recursive = false)
        {
            try
            {
                Entry Group = FindGroup(groupName);
                ICollection<Entry> Entries = FindActiveUsersAndGroups("memberOf=" + Group.DistinguishedName);
                if (recursive)
                {
                    List<Entry> ReturnValue = new List<Entry>();
                    foreach (Entry Entry in Entries)
                    {
                        Entry TempEntry = FindGroup(Entry.CN);
                        if (TempEntry == null)
                            ReturnValue.Add(Entry);
                        else
                            ReturnValue.AddRange(FindActiveGroupMembers(TempEntry.CN, true));
                    }
                    return ReturnValue;
                }
                else
                {
                    return Entries;
                }
            }
            catch
            {
                return new List<Entry>();
            }
        }

        /// <summary>
        /// Finds the active groups.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindActiveGroups(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", filter);
            return FindGroups(filter);
        }

        /// <summary>
        /// Finds the active users.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindActiveUsers(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", filter);
            return FindUsers(filter);
        }

        /// <summary>
        /// Finds the active users and groups.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindActiveUsersAndGroups(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", filter);
            return FindUsersAndGroups(filter);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>All entries</returns>
        public ICollection<Entry> FindAll()
        {
            List<Entry> ReturnedResults = new List<Entry>();
            using (SearchResultCollection Results = Searcher.FindAll())
            {
                foreach (SearchResult Result in Results)
                    ReturnedResults.Add(new Entry(Result.GetDirectoryEntry()));
            }
            return ReturnedResults;
        }

        /// <summary>
        /// Finds the computers.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindComputers(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&(objectClass=computer)({0}))", filter);
            Searcher.Filter = filter;
            return FindAll();
        }

        /// <summary>
        /// Finds the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>The result</returns>
        public Entry FindGroup(string groupName)
        {
            return FindGroups("cn=" + groupName).FirstOrDefault();
        }

        /// <summary>
        /// Finds the group members.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindGroupMembers(string groupName, bool recursive = false)
        {
            try
            {
                Entry Group = FindGroup(groupName);
                ICollection<Entry> Entries = FindUsersAndGroups("memberOf=" + Group.DistinguishedName);
                if (recursive)
                {
                    List<Entry> ReturnValue = new List<Entry>();
                    foreach (Entry Entry in Entries)
                    {
                        Entry TempEntry = FindGroup(Entry.CN);
                        if (TempEntry == null)
                            ReturnValue.Add(Entry);
                        else
                            ReturnValue.AddRange(FindActiveGroupMembers(TempEntry.CN, true));
                    }
                    return ReturnValue;
                }
                else
                {
                    return Entries;
                }
            }
            catch
            {
                return new List<Entry>();
            }
        }

        /// <summary>
        /// Finds the groups.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The results</returns>
        public ICollection<Entry> FindGroups(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&(objectClass=Group)(objectCategory=Group)({0}))", filter);
            Searcher.Filter = filter;
            return FindAll();
        }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <returns>The one.</returns>
        public Entry FindOne()
        {
            return new Entry(Searcher.FindOne().GetDirectoryEntry());
        }

        /// <summary>
        /// Finds the name of the user by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The result</returns>
        /// <exception cref="System.ArgumentNullException">userName</exception>
        public Entry FindUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            return FindUsers("samAccountName=" + userName).FirstOrDefault();
        }

        /// <summary>
        /// Finds the users.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public ICollection<Entry> FindUsers(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&(objectClass=User)(objectCategory=Person)({0}))", filter);
            Searcher.Filter = filter;
            return FindAll();
        }

        /// <summary>
        /// Finds the users and groups.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The results of the query</returns>
        public ICollection<Entry> FindUsersAndGroups(string filter, params object[] args)
        {
            filter = string.Format(CultureInfo.InvariantCulture, filter, args);
            filter = string.Format(CultureInfo.InvariantCulture, "(&(|(&(objectClass=Group)(objectCategory=Group))(&(objectClass=User)(objectCategory=Person)))({0}))", filter);
            Searcher.Filter = filter;
            return FindAll();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected void Dispose(bool disposing)
        {
            if (Entry != null)
            {
                Entry.Close();
                Entry.Dispose();
                Entry = null;
            }
            if (Searcher != null)
            {
                Searcher.Dispose();
                Searcher = null;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Directory"/> class.
        /// </summary>
        ~Directory()
        {
            Dispose(false);
        }
    }
}