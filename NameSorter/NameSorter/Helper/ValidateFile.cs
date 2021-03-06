﻿using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace NameSorter.Helper
{
    /// <summary>
    /// Author: Francis Decena 
    /// Date: 15/8/2020
    /// Description: This will validate the file uploaded if its .txt extension file name. 
    /// This will be used in TextFileModel as extedned validation attribute 
    /// </summary>
    public class ValidateFile : ValidationAttribute
    {
        //Properties
        private readonly string[] _Extensions;

        //Constructor
        public ValidateFile(string[] Extensions)
        {
            _Extensions = Extensions;
        }

        /// <summary>
        /// Description: This will override the class ValidationResult where the value will assigned as IFormFile. 
        /// If the validation is correct return ValidationResult as Success to TextFileModel
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if(file != null)
            {
                //Get the extension file of the specfic path including the "."
                var extension = Path.GetExtension(file.FileName);

                if(!(file == null))
                {
                    if (!_Extensions.Contains(extension.ToLower()))
                    {
                        // return a error message if the file uploaded has wrong filename extension
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Description: Return an error message to client if the file has wrong extension filename
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return $"Error uploading file, Make sure the extension file is .txt";
        }
    }
}
