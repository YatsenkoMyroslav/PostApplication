//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PostApplication.Models
{
    using System;
    
    public partial class GetTodaysSendings_Result
    {
        public int TTH { get; set; }
        public int From_office_number { get; set; }
        public string Sender { get; set; }
        public int To_office_number { get; set; }
        public string Recipient { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Date_of_sending { get; set; }
        public string Sent_by { get; set; }
    }
}
