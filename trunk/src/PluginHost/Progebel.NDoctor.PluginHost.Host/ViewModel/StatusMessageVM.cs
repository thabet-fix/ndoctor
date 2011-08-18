﻿/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Progebel.NDoctor.PluginHost.Host.ViewModel
{
    using Progebel.NDoctor.PluginHost.Core;
    using Progebel.NDoctor.PluginHost.Host.Model;

    public class StatusMessageVM : ViewModelBase
    {
        #region Fields

        private StatusMessage status;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusMessageVM"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        public StatusMessageVM(StatusMessage status)
        {
            this.status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusMessageVM"/> class.
        /// </summary>
        public StatusMessageVM()
        {
            this.status = new StatusMessage();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return this.status.Message; }
            set
            {
                this.status.Message = value;
                this.OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public StatusType Type
        {
            get
            {
                return this.status.Type;
            }
            set
            {
                this.status.Type = value;
                this.OnPropertyChanged("Type");
            }
        }

        #endregion Properties
    }
}