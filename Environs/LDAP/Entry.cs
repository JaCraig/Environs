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
using System.Text;

namespace Environs.LDAP
{
    /// <summary>
    /// LDAP entry
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public class Entry : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        /// <param name="directoryEntry">The directory entry.</param>
        /// <exception cref="System.ArgumentNullException">directoryEntry</exception>
        public Entry(DirectoryEntry directoryEntry)
        {
            DirectoryEntry = directoryEntry ?? throw new ArgumentNullException(nameof(directoryEntry));
        }

        /// <summary>
        /// Gets or sets the cn.
        /// </summary>
        /// <value>The cn.</value>
        public string CN
        {
            get { return (string)GetValue("cn"); }
            set { SetValue("cn", value); }
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            get { return (string)GetValue("company"); }
            set { SetValue("company", value); }
        }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get { return (string)GetValue("countrycode"); }
            set { SetValue("countrycode", value); }
        }

        /// <summary>
        /// Gets the directory entry.
        /// </summary>
        /// <value>The directory entry.</value>
        public DirectoryEntry DirectoryEntry { get; private set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return (string)GetValue("displayname"); }
            set { SetValue("displayname", value); }
        }

        /// <summary>
        /// Gets or sets the name of the distinguished.
        /// </summary>
        /// <value>The name of the distinguished.</value>
        public string DistinguishedName
        {
            get { return (string)GetValue("distinguishedname"); }
            set { SetValue("distinguishedname", value); }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return (string)GetValue("mail"); }
            set { SetValue("mail", value); }
        }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>The name of the given.</value>
        public string GivenName
        {
            get { return (string)GetValue("givenname"); }
            set { SetValue("givenname", value); }
        }

        /// <summary>
        /// Gets or sets the initials.
        /// </summary>
        /// <value>The initials.</value>
        public string Initials
        {
            get { return (string)GetValue("initials"); }
            set { SetValue("initials", value); }
        }

        /// <summary>
        /// Gets the member of.
        /// </summary>
        /// <value>The member of.</value>
        public IEnumerable<string> MemberOf
        {
            get
            {
                List<string> Values = new List<string>();
                PropertyValueCollection Collection = DirectoryEntry.Properties["memberof"];
                foreach (object Item in Collection)
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
            get { return (string)GetValue("name"); }
            set { SetValue("name", value); }
        }

        /// <summary>
        /// Gets or sets the office.
        /// </summary>
        /// <value>The office.</value>
        public string Office
        {
            get { return (string)GetValue("physicaldeliveryofficename"); }
            set { SetValue("physicaldeliveryofficename", value); }
        }

        /// <summary>
        /// Gets or sets the name of the sam account.
        /// </summary>
        /// <value>The name of the sam account.</value>
        public string SamAccountName
        {
            get { return (string)GetValue("samaccountname"); }
            set { SetValue("samaccountname", value); }
        }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>The telephone number.</value>
        public string TelephoneNumber
        {
            get { return (string)GetValue("telephonenumber"); }
            set { SetValue("telephonenumber", value); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return (string)GetValue("title"); }
            set { SetValue("title", value); }
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
        public object GetValue(string property)
        {
            return DirectoryEntry.Properties[property]?.Value;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            DirectoryEntry.CommitChanges();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string property, object value)
        {
            PropertyValueCollection Collection = DirectoryEntry.Properties[property];
            if (Collection != null)
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
            if (Collection != null)
                Collection[index] = value;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            foreach (PropertyValueCollection Property in DirectoryEntry.Properties)
            {
                Builder.AppendLine(Property.PropertyName).Append(" = ").AppendLine(Property.Value.ToString());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="Disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool Disposing)
        {
            if (DirectoryEntry != null)
            {
                DirectoryEntry.Dispose();
                DirectoryEntry = null;
            }
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