﻿using System.ComponentModel.DataAnnotations;
using InstantDelivery.Domain.Entities;

namespace InstantDelivery.Model.Vehicles
{
    public class AddVehicleModelDto : ValidationBase
    {
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Model { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Wartość musi być liczbą całkowitą")]
        public double Payload { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        public double AvailableSpaceX { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        public double AvailableSpaceY { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        public double AvailableSpaceZ { get; set; }
    }
}