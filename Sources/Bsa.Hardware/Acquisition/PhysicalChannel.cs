﻿//
// This file is part of Biological Signals Acquisition Framework (BSA-F).
// Copyright © Adriano Repetti 2016.
//
// BSA-F is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BSA-F is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with BSA-F.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bsa.Hardware.Acquisition
{
    /// <summary>
    /// Represents a physical channel as acquired by an <see cref="AcquisitionDevice"/>.
    /// </summary>
    [Serializable]
    public class PhysicalChannel : Sealable
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PhysicalChannel"/> with specified ID.
        /// </summary>
        /// <param name="id">Unique ID of this physical channel.</param>
        public PhysicalChannel(Guid id)
        {
            _id = id;
        }

        /// <summary>
        /// Gets the ID of this channel.
        /// </summary>
        /// <value>
        /// The unique ID of this channel. ID format and content is completely implementation defined
        /// and it may even be a <em>random</em> ID, only constraint is to be unique within the same
        /// <see cref="PhysicalChannelCollection{T}"/>.
        /// </value>
        /// <seealso cref="Name"/>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets/sets the name of this channel.
        /// </summary>
        /// <value>
        /// The display name of this channel. Display name is used to identify this channel using a friendly name,
        /// there aren't restrictions on its content but it has to be unique within the same <see cref="PhysicalChannelCollection{T}"/>.
        /// <c>Name</c> is not directly related to <see cref="Id"/> but they represent the same thing from two points of view:
        /// name is how all the other components will refer to this channel and ID is how hardware (and hardware only) will refer to
        /// this channel, this class is the junction point between these two views.
        /// </value>
        /// <exception cref="ArgumentException">
        /// If <see langword="value"/> is a <see langword="null"/> or blank string.
        /// </exception>
        /// <seealso cref="Id"/>
        public string Name
        {
            get { return _name; }
            set
            {
                ThrowIfSealed();

                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Channel name cannot be a null or blank string.");

                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the sampling rate for this channel.
        /// </summary>
        /// <value>
        /// The sampling rate for this channel. 0 means <em>sampled on change</em> if it's supported
        /// (see <see cref="AcquisitionDevice.SamplingOnValueChange"/> feature) otherwise it a placeholder for an invalid value.
        /// Usage of this property is implementation defined. If hardware supports acquisition of channels with multiple frequencies
        /// (see <see cref="AcquisitionDevice.Multifrequency"/> feature) then this value may be different for channels of the same set
        /// otherwise it must be the same for all channels. Default value is 0.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If value is less than zero.
        /// </exception>
        /// <seealso cref="Id"/>
        public double SamplingRate
        {
            get { return _samplingRate; }
            set
            {
                ThrowIfSealed();

                if (value < 0)
                    throw new ArgumentOutOfRangeException("Sampling rate cannot be a negative value");

                _samplingRate = value;
            }
        }

        protected override Sealable CreateNewInstance()
        {
            return new PhysicalChannel(Id);
        }

        protected override void CopyPropertiesTo(Sealable target)
        {
            PhysicalChannel other = (PhysicalChannel)target;
            other.Name = this.Name;
            other.SamplingRate = this.SamplingRate;
        }

        private readonly Guid _id;
        private string _name;
        private double _samplingRate;
    }
}