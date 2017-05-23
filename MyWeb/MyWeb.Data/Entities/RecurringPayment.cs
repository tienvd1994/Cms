namespace MyWeb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RecurringPayment")]
    public partial class RecurringPayment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecurringPayment()
        {
            RecurringPaymentHistories = new HashSet<RecurringPaymentHistory>();
        }

        public int Id { get; set; }

        public int CycleLength { get; set; }

        public int CyclePeriodId { get; set; }

        public int TotalCycles { get; set; }

        public DateTime StartDateUtc { get; set; }

        public bool IsActive { get; set; }

        public bool LastPaymentFailed { get; set; }

        public bool Deleted { get; set; }

        public int InitialOrderId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public virtual Order Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecurringPaymentHistory> RecurringPaymentHistories { get; set; }
    }
}
