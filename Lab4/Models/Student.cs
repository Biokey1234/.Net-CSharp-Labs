using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Student
    {

        public required string FirstName
        {
            get; set;
        }

        public required string LastName
        {
            get; set;
        }

        public required int StudentId
        {
            get; set;
        }

        public required string EmailAddress
        {
            get; set;
        }

        public required string Password
        {

            get; set;
        }

        public required string Description
        {
            get; set;
        }
    }


}
