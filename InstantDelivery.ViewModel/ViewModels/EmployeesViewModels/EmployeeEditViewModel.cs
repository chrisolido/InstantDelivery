﻿using Caliburn.Micro;
using InstantDelivery.Domain.Entities;

namespace InstantDelivery.ViewModel
{
    /// <summary>
    /// Model widoku edycji pracownika
    /// </summary>
    public class EmployeeEditViewModel : Screen
    {
        /// <summary>
        /// Aktualnie edytowany pracownik.
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Zapisuje zmiany dokonane w widoku.
        /// </summary>
        public void Save()
        {
            TryClose(true);
        }

        /// <summary>
        /// Anuluje zmiany dokonane w widoku.
        /// </summary>
        public void Cancel()
        {
            TryClose(false);
        }
    }
}
