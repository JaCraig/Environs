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
using System.Runtime.Versioning;
using System.Text;

namespace Environs.LDAP
{
    /// <summary>
    /// LDAP entry
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    /// <remarks>Initializes a new instance of the <see cref="Entry"/> class.</remarks>
    /// <param name="directoryEntry">The directory entry.</param>
    /// <exception cref="System.ArgumentNullException">directoryEntry</exception>
    [SupportedOSPlatform("windows")]
    public class Entry(DirectoryEntry directoryEntry) : IDisposable
    {
        /// <summary>
        /// Gets or sets the cn.
        /// </summary>
        /// <value>The cn.</value>
        public string CN
        {
            get => (string)GetValue("cn"); set => SetValue("cn", value);
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            get => (string)GetValue("company"); set => SetValue("company", value);
        }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get => (string)GetValue("countrycode"); set => SetValue("countrycode", value);
        }

        /// <summary>
        /// Gets the directory entry.
        /// </summary>
        /// <value>The directory entry.</value>
        public DirectoryEntry DirectoryEntry { get; private set; } = directoryEntry ?? throw new ArgumentNullException(nameof(directoryEntry));

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get => (string)GetValue("displayname"); set => SetValue("displayname", value);
        }

        /// <summary>
        /// Gets or sets the name of the distinguished.
        /// </summary>
        /// <value>The name of the distinguished.</value>
        public string DistinguishedName
        {
            get => (string)GetValue("distinguishedname"); set => SetValue("distinguishedname", value);
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get => (string)GetValue("mail"); set => SetValue("mail", value);
        }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>The name of the given.</value>
        public string GivenName
        {
            get => (string)GetValue("givenname"); set => SetValue("givenname", value);
        }

        /// <summary>
        /// Gets or sets the initials.
        /// </summary>
        /// <value>The initials.</value>
        public string Initials
        {
            get => (string)GetValue("initials"); set => SetValue("initials", value);
        }

        /// <summary>
        /// Gets the member of.
        /// </summary>
        /// <value>The member of.</value>
        public IEnumerable<string> MemberOf
        {
            get
            {
                List<string> Values = [];
                PropertyValueCollection Collection = DirectoryEntry.Properties["memberof"];
                foreach (var Item in Collection)
                {
                    Values.Add((string)Item);
                }
                return Values;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get => (string)GetValue("name"); set => SetValue("name", value);
        }

        /// <summary>
        /// Gets or sets the office.
        /// </summary>
        /// <value>The office.</value>
        public string Office
        {
            get => (string)GetValue("physicaldeliveryofficename"); set => SetValue("physicaldeliveryofficename", value);
        }

        /// <summary>
        /// Gets or sets the name of the sam account.
        /// </summary>
        /// <value>The name of the sam account.</value>
        public string SamAccountName
        {
            get => (string)GetValue("samaccountname"); set => SetValue("samaccountname", value);
        }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>The telephone number.</value>
        public string TelephoneNumber
        {
            get => (string)GetValue("telephonenumber"); set => SetValue("telephonenumber", value);
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => (string)GetValue("title"); set => SetValue("title", value);
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
        /// Gets the value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public object GetValue(string property) => DirectoryEntry.Properties[property]?.Value;

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save() => DirectoryEntry.CommitChanges();

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string property, object value)
        {
            PropertyValueCollection Collection = DirectoryEntry.Properties[property];
            if (Collection == null)
                return;
            Collection.Value = value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public virtual void SetValue(string property, int index, object value)
        {
            PropertyValueCollection Collection = DirectoryEntry.Properties[property];
            if (Collection == null)
                return;
            Collection[index] = value;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (PropertyValueCollection Property in DirectoryEntry.Properties)
            {
                _ = Builder.AppendLine(Property.PropertyName).Append(" = ").AppendLine(Property.Value.ToString());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            DirectoryEntry?.Dispose();
            DirectoryEntry = null;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Entry"/> class.
        /// </summary>
        ~Entry()
        {
            Dispose(false);
        }
    }
}