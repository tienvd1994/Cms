namespace MyWeb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerPassword")]
    public partial class CustomerPassword
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Password { get; set; }

        public int PasswordFormatId { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        //public PasswordFormat PasswordFormat
        //{
        //    get { return (PasswordFormat)PasswordFormatId; }
        //    set { this.PasswordFormatId = (int)value; }
        //}

        public virtual Customer Customer { get; set; }
    }
}
