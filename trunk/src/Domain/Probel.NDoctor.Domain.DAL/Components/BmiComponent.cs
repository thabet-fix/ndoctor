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
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.Domain.DAL.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate;
    using NHibernate.Linq;

    using Probel.Helpers.Conversions;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// 
    /// </summary>
    public class BmiComponent : BaseComponent, IBmiComponent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiComponent"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public BmiComponent(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiComponent"/> class.
        /// </summary>
        public BmiComponent()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds a bmi entry to the specified patient.
        /// </summary>
        /// <param name="bmi">The bmi.</param>
        /// <param name="patient">The patient.</param>
        public void CreateBmi(BmiDto bmi, LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);

            if (entity != null) entity.BmiHistory.Add(Mapper.Map<BmiDto, Bmi>(bmi));
            else throw new EntityNotFoundException(typeof(Bmi));
        }

        /// <summary>
        /// Finds the bmi history.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public IList<BmiDto> FindBmiHistory(LightPatientDto patient, DateTime start, DateTime end)
        {
            var entity = this.Session.Get<Patient>(patient.Id);

            var result = (from b in entity.BmiHistory
                          where b.Date >= start
                             && b.Date <= end
                          select b).ToList();

            return Mapper.Map<IList<Bmi>, IList<BmiDto>>(result);
        }

        /// <summary>
        /// Gets all bmi history for the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// A list of BMI
        /// </returns>
        public IList<BmiDto> GetAllBmiHistory(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);

            var result = (from b in entity.BmiHistory
                          select b).ToList();

            return Mapper.Map<IList<Bmi>, IList<BmiDto>>(result);
        }

        /// <summary>
        /// Gets the bmi history of the specified patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public PatientBmiDto GetPatientWithBmiHistory(LightPatientDto patient)
        {
            //var patientDto = Mapper.Map<LightPatientDto, PatientBmiDto>(patient);
            var historyDto = Mapper.Map<IList<Bmi>, IList<BmiDto>>(this.Session.Get<Patient>(patient.Id).BmiHistory);

            historyDto.OrderBy(e => e.Date);
            var patientDto = new PatientBmiDto(historyDto);
            Mapper.Map<LightPatientDto, PatientBmiDto>(patient, patientDto);
            return patientDto;
        }

        /// <summary>
        /// Removes the specified BMI entry from the specified patient.
        /// </summary>
        /// <param name="from">The patient.</param>
        /// <param name="dto">The dto to remove.</param>
        public void Remove(BmiDto bmi, LightPatientDto from)
        {
            var ebmi = this.Session.Get<Bmi>(bmi.Id);
            this.Session.Delete(ebmi);

            var patient = this.Session.Get<Patient>(from.Id);
            this.Session.Update(patient);
        }

        /// <summary>
        /// Deletes the bmi with the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        public void RemoveBmiWithDate(LightPatientDto patient, DateTime date)
        {
            var current = (from p in this.Session.Query<Patient>()
                           where p.Id == patient.Id
                           select p).FirstOrDefault();

            var deletedCount = 0;
            for (int i = 0; i < current.BmiHistory.Count; i++)
            {
                if (current.BmiHistory[i].Date.Date == date.Date)
                {
                    current.BmiHistory.Remove(current.BmiHistory[i]);
                    deletedCount++;
                    i--; //I've deleted one item, I step back and continue to check for deletion
                }
            }
            this.Session.Update(current);
        }

        /// <summary>
        /// Updates the specified light patient. This method should be used to update 
        /// first/last name height and or Gender
        /// </summary>
        /// <param name="patient">The patient.</param>
        public void Update(LightPatientDto patient)
        {
            var entity = this.Session.Get<Patient>(patient.Id);
            Mapper.Map<LightPatientDto, Patient>(patient, entity);

            this.Session.Update(entity);
        }

        #endregion Methods
    }
}