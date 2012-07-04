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
namespace Probel.NDoctor.Domain.Test.Component
{
    using NUnit.Framework;

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Exceptions;
    using Probel.NDoctor.Domain.Test.Helpers;

    public class TestPatientDataComponent : TestBase<PatientDataComponent>
    {
        #region Methods

        [Test]
        public void CanFillData()
        {
            var c = new PatientSessionComponent(SQLiteDatabase.Scope.OpenSession());
            var patient = c.GetPatientLightById(3);
            Assert.NotNull(patient, "A patient with the id '3' should exist in the database");

            long id = patient.Id;
            var loadedPatient = this.Component.GetPatient(id);

            Assert.NotNull(loadedPatient, "The patient with id {0} should exist", id);
            Assert.NotNull(loadedPatient.Insurance, "Insurance should be loaded");
            Assert.NotNull(loadedPatient.Practice, "Practice should be loaded");
            Assert.NotNull(loadedPatient.Profession, "Profession should be loaded");
            Assert.NotNull(loadedPatient.Reputation, "Reputation should be loaded");
            Assert.NotNull(loadedPatient.Tag, "Tag should be loaded");
        }

        [Test]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void FailToLoadUnknownPatient()
        {
            long id = 123456789;
            var loadedPatient = this.Component.GetPatient(id);
        }

        /// <summary>
        /// Gets the component instance.
        /// </summary>
        /// <returns></returns>
        protected override PatientDataComponent GetComponentInstance()
        {
            return new PatientDataComponent(SQLiteDatabase.Scope.OpenSession());
        }

        #endregion Methods
    }
}